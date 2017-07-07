using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class CameraEventArgs : EventArgs
        {
            public Bitmap PreviewFrameJPEG { get; }
            public IEnumerable<RectangleF> DetectedFaces { get; }
            public CameraEventArgs(Bitmap PreviewFrameJPEG, IEnumerable<RectangleF> DetectedFaces)
            { this.PreviewFrameJPEG = PreviewFrameJPEG; this.DetectedFaces = DetectedFaces; }
            public static implicit operator CameraEventArgs((Image I, IEnumerable<RectangleF> R) T) =>
                new CameraEventArgs(T.I as Bitmap, T.R);
        }
#if __ANDROID__
        public class Camera : Android.Views.TextureView, Android.Views.TextureView.ISurfaceTextureListener
        {
            public Camera() : base(Xamarin.Forms.Forms.Context)
            {
                SurfaceTextureListener = this;
                if (IsAvailable) OnSurfaceTextureAvailable(SurfaceTexture, Width, Height);
            }
            public event EventHandler<CameraEventArgs> ProcessingPreview = delegate { };
#pragma warning disable 618 //Reason: Need Android 4 support
            private System.IO.Stream ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera camera)
            {
                var cameraParameters = camera.GetParameters();
                var width = cameraParameters.PreviewSize.Width;
                var height = cameraParameters.PreviewSize.Height;
                var yuv = new Android.Graphics.YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null);
                var quality = 100;   // adjust this as needed, default: 80
                var ms = new System.IO.MemoryStream();
                yuv.CompressToJpeg(new Android.Graphics.Rect(0, 0, width, height), quality, ms);
                return ms;
            }
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
                        ProcessingPreview(this, (Image.FromStream(ConvertYuvToJpeg(data, camera)), Faces))));
                    cam.FaceDetection += (sender, e) => Faces = e.Faces
                        .Select(x => new RectangleF(x.Rect.Left, x.Rect.Top, x.Rect.Right - x.Rect.Left, x.Rect.Bottom - x.Rect.Top));
                    cam.StartFaceDetection();
                }
                catch (Java.IO.IOException ex)
                {
                    Log(ex);
                }
            }
            IEnumerable<RectangleF> Faces = Enumerable.Empty<RectangleF>();
            public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
            {
                cam?.StopFaceDetection();
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
            public event EventHandler<CameraEventArgs> ProcessingPreview = delegate { };
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
                    var ex = new Foundation.NSErrorException(error);
                    // Handle the error appropriately.
                    Log(ex);//@"ERROR: trying to open camera: %@", 
                    throw ex;
                }
                AVFoundation.AVCaptureVideoDataOutput output = new AVFoundation.AVCaptureVideoDataOutput();
                CoreFoundation.DispatchQueue queue = new CoreFoundation.DispatchQueue("edu.cswcss.eLearning.Camera.CtorQueue");
                output.SetSampleBufferDelegateQueue(new BufferDelegate(
                    (captureOutput, sampleBuffer, connection) =>
                    {
                        // Get a CMSampleBuffer's Core Video image buffer for the media data
                        // Create a UIImage from sample buffer data
                        CoreImage.CIImage image = new CoreImage.CIImage(sampleBuffer.GetImageBuffer());
                        CoreImage.CIContext context = new CoreImage.CIContext(new CoreImage.CIContextOptions());
                        CoreImage.CIDetector detector = CoreImage.CIDetector.CreateFaceDetector(context, true);
                        ProcessingPreview(this, 
                            (Bitmap.FromStream(new UIKit.UIImage(image).AsJPEG().AsStream()), 
                            detector.FeaturesInImage(image).Select(x => 
                            new RectangleF((float)x.Bounds.X, (float)x.Bounds.Y, (float)x.Bounds.Width, (float)x.Bounds.Height))));
                        //< Add your code here that uses the image >;
                    }), queue);
                session.AddInput(input);
                session.AddOutput(output);
                session.StartRunning();
            }
            protected override void Dispose(bool disposing)
            {
                session.StopRunning();
                base.Dispose(disposing);
                session.Dispose();
                device.Dispose();
            }
            class BufferDelegate : Foundation.NSObject, AVFoundation.IAVCaptureVideoDataOutputSampleBufferDelegate
            {
                Action<AVFoundation.AVCaptureOutput, CoreMedia.CMSampleBuffer, AVFoundation.AVCaptureConnection> Handler;
                public BufferDelegate(Action<AVFoundation.AVCaptureOutput, CoreMedia.CMSampleBuffer,
                    AVFoundation.AVCaptureConnection> Handler) => this.Handler = Handler;
                // Delegate routine that is called when a sample buffer was written
                void DidOutputSampleBuffer(AVFoundation.AVCaptureOutput captureOutput, CoreMedia.CMSampleBuffer sampleBuffer,
                    AVFoundation.AVCaptureConnection connection) => Handler?.Invoke(captureOutput, sampleBuffer, connection);

            }
        }
#elif WINDOWS_UWP
        public class Camera : Windows.UI.Xaml.Controls.UserControl, IDisposable
        {
            public event EventHandler<CameraEventArgs> ProcessingPreview = delegate { };
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
                if (_mediaCapture != null)
                    _mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
                try
                {

                    PreviewControl.Source = _mediaCapture;
                    await _mediaCapture?.StartPreviewAsync();
                    _isPreviewing = true;

                    using(var TempFrame = await _mediaCapture?.GetPreviewFrameAsync())
                    Xamarin.Forms.Device.StartTimer(TempFrame?.Duration ?? TimeSpan.FromMilliseconds(40), () => {
                        var ms = new System.IO.MemoryStream();

                        var encoder = Windows.Graphics.Imaging.BitmapEncoder.CreateAsync
                            (Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId,
                            System.IO.WindowsRuntimeStreamExtensions.AsRandomAccessStream(ms))
                            .AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                        
                        using (var b = _mediaCapture.GetPreviewFrameAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            encoder.SetSoftwareBitmap(b.SoftwareBitmap);
                            try { encoder.FlushAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult(); }
                            catch { ms.Dispose(); return _isPreviewing; }

                            var detector = Windows.Media.FaceAnalysis.FaceDetector.CreateAsync()
                                .AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                            ProcessingPreview(this, (Bitmap.FromStream(ms), 
                                detector.DetectFacesAsync(b.SoftwareBitmap).AsTask().ConfigureAwait(false).GetAwaiter().GetResult()
                                    .Select(x => new RectangleF(x.FaceBox.X, x.FaceBox.Y, x.FaceBox.Width, x.FaceBox.Height))));
                            return _isPreviewing;
                        }
                    });
                }
                catch (System.IO.FileLoadException)
                {
                    // _mediaCapture.CaptureDeviceExclusiveControlStatusChanged += _mediaCapture_CaptureDeviceExclusiveControlStatusChanged;
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