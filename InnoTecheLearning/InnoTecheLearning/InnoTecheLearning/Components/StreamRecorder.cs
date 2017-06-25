using System;
using System.Collections.Generic;
using System.Text;
using Android.Hardware;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
#if __ANDROID__
        public class Camera : Android.Views.TextureView, Android.Views.TextureView.ISurfaceTextureListener
        {
            public Camera() : base(Forms.Context)
            {
                SurfaceTextureListener = this;
                if (IsAvailable) OnSurfaceTextureAvailable(SurfaceTexture, Width, Height);
            }
#pragma warning disable 618 //Reason: Need Android 4 support
            class Callback : Java.Lang.Object, Android.Hardware.Camera.IPreviewCallback
            {
                public Callback(Action<byte[], Android.Hardware.Camera> callback) => this.callback = callback;
                Action<byte[], Android.Hardware.Camera> callback;
                public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera) => callback(data, camera);
            }
            Android.Hardware.Camera cam;
            public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
            {
                Log("OnSurfaceTextureAvailable");
                cam = Android.Hardware.Camera.Open();
#pragma warning restore 618
                switch (Android.Runtime.Extensions.JavaCast<Android.Views.IWindowManager>
                            (Context.GetSystemService(Android.Content.Context.WindowService))
                        .DefaultDisplay.Rotation)
                {
                    case Android.Views.SurfaceOrientation.Rotation0:
                        cam.SetDisplayOrientation(90);
                        break;
                    case Android.Views.SurfaceOrientation.Rotation180:
                        cam.SetDisplayOrientation(270);
                        break;
                    case Android.Views.SurfaceOrientation.Rotation270:
                        cam.SetDisplayOrientation(180);
                        break;
                    case Android.Views.SurfaceOrientation.Rotation90:
                        cam.SetDisplayOrientation(0);
                        break;
                    default:
                        break;
                }
                LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(w, h);
                
                try
                {
                    cam.SetPreviewTexture(surface);
                    cam.StartPreview();
                    cam.SetPreviewCallback(new Callback((data, camera) => 
                    {
                        new Accord.Vision.Detection.HaarObjectDetector(new Accord.Vision.Detection.Cascades.FaceHaarCascade(), 30);
                    }));
                }
                catch (Java.IO.IOException ex)
                {
                    Log(ex);
                }
            }
            public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
            {
                cam?.StopPreview();
                cam?.Release();

                return true;
            }

            public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height) { }
            public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface) { }
        }

#elif __IOS__
        public class Camera : UIKit.UIImageView
        {
            AVFoundation.AVCaptureSession session = new AVFoundation.AVCaptureSession()
            {
                SessionPreset = AVFoundation.AVCaptureSession.PresetMedium
            };
            AVFoundation.AVCaptureDevice device = AVFoundation.AVCaptureDevice.GetDefaultDevice(AVFoundation.AVMediaTypes.Video);
            public Camera() : base()
            {
                AVFoundation.AVCaptureVideoPreviewLayer captureVideoPreviewLayer = new AVFoundation.AVCaptureVideoPreviewLayer(session)
                {
                    Frame = Bounds
                };
                Layer.AddSublayer(captureVideoPreviewLayer);


                AVFoundation.AVCaptureDeviceInput input = new AVFoundation.AVCaptureDeviceInput(device, out Foundation.NSError error);
                if (input == null)
                {
                    // Handle the error appropriately.
                    Log(new Foundation.NSErrorException(error));//@"ERROR: trying to open camera: %@", 
                }
                session.AddInput(input);

                session.StartRunning();
            }
            protected override void Dispose(bool disposing)
            {
                session.StopRunning();
                base.Dispose(disposing);
                session.Dispose();
                device.Dispose();
            }
        }
#elif WINDOWS_UWP
        public class Camera : Windows.UI.Xaml.Controls.UserControl, IDisposable
        {
            Windows.UI.Xaml.Controls.CaptureElement PreviewControl = new Windows.UI.Xaml.Controls.CaptureElement();
            Windows.Media.Capture.MediaCapture _mediaCapture;
            bool _isPreviewing;
            Windows.System.Display.DisplayRequest _displayRequest = new Windows.System.Display.DisplayRequest();
            public Camera() => StartPreviewAsync().ConfigureAwait(false).GetAwaiter().GetResult().Ignore();
            private async System.Threading.Tasks.ValueTask<Unit> StartPreviewAsync()
            {
                try
                {
                    Content = PreviewControl;
                    _mediaCapture = new Windows.Media.Capture.MediaCapture();
                    await _mediaCapture.InitializeAsync();

                    _displayRequest.RequestActive();
                    Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = 
                        Windows.Graphics.Display.DisplayOrientations.Landscape;
                }
                catch (UnauthorizedAccessException)
                {
                    // This will be thrown if the user denied access to the camera in privacy settings
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, 
                        async () => await new Windows.UI.Popups.MessageDialog("The app was denied access to the camera").ShowAsync());
                    return Unit.Default;
                }

                try
                {
                    PreviewControl.Source = _mediaCapture;
                    await _mediaCapture.StartPreviewAsync();
                    _isPreviewing = true;
                }
                catch (System.IO.FileLoadException)
                {
                    _mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
                }
                return Unit.Default;
            }
            private async void _mediaCapture_CaptureDeviceExclusiveControlStatusChanged
                (Windows.Media.Capture.MediaCapture sender, 
                Windows.Media.Capture.MediaCaptureDeviceExclusiveControlStatusChangedEventArgs args)
            {
                if (args.Status == Windows.Media.Capture.MediaCaptureDeviceExclusiveControlStatus.SharedReadOnlyAvailable)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                       await new Windows.UI.Popups.MessageDialog
                        ("The camera preview can't be displayed because another app has exclusive access").ShowAsync());
                }
                else if (args.Status == Windows.Media.Capture.MediaCaptureDeviceExclusiveControlStatus.ExclusiveControlAvailable &&
                    !_isPreviewing)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        await StartPreviewAsync();
                    });
                }
            }
            public void Dispose() => CleanupCameraAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            private async System.Threading.Tasks.ValueTask<Unit> CleanupCameraAsync()
            {
                if (_mediaCapture != null)
                {
                    if (_isPreviewing)
                    {
                        await _mediaCapture.StopPreviewAsync();
                    }

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        PreviewControl.Source = null;
                        _displayRequest?.RequestRelease();

                        _mediaCapture.Dispose();
                        _mediaCapture = null;
                    });
                }
                return Unit.Default;
            }
        }
#endif
    }
}