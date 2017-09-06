#undef FEATURE_CAMERA_PREVIEWJPEG //To use less memory

using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class CameraEventArgs : EventArgs
        {
#if FEATURE_CAMERA_PREVIEWJPEG
            public byte[] PreviewFrameJPEG { get; }
#endif
            public Rectangle[] DetectedFaces { get; }
            internal CameraEventArgs(
#if FEATURE_CAMERA_PREVIEWJPEG
                byte[] PreviewFrameJPEG, 
#endif
                Rectangle[] DetectedFaces)
            {
#if FEATURE_CAMERA_PREVIEWJPEG
                this.PreviewFrameJPEG = PreviewFrameJPEG;
#endif
                this.DetectedFaces = DetectedFaces;
            }
        }
#if __ANDROID__
        public class Camera : Android.Views.TextureView, Android.Views.TextureView.ISurfaceTextureListener, IDisposable
        {
            public Camera() : base(Xamarin.Forms.Forms.Context)
            {
                SurfaceTextureListener = this;
                if (IsAvailable) OnSurfaceTextureAvailable(SurfaceTexture, Width, Height);
            }
            public event EventHandler<CameraEventArgs> ProcessingPreview = delegate { };
#pragma warning disable 618 //Reason: Need Android 4 support
#if FEATURE_CAMERA_PREVIEWJPEG
            private void PreviewHandler(byte[] data, Android.Hardware.Camera camera)
            {
                 ProcessingPreview(this, new CameraEventArgs(
                     ConvertYuvToJpeg(data, param),
                     Faces.ToArray()));
            }

            private static byte[] ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera.Parameters cameraParameters)
            {
                var width = cameraParameters.PreviewSize.Width;
                var height = cameraParameters.PreviewSize.Height;
                var quality = 90;   // adjust this as needed, default: 80
                using (var ms = new MemoryStream())
                {
                    using (var yuv = new Android.Graphics.YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null))
                        yuv.CompressToJpeg(new Android.Graphics.Rect(0, 0, width, height), quality, ms);
                    return ms.ToArray();
                }
            }

            class Callback : Java.Lang.Object, Android.Hardware.Camera.IPreviewCallback
            {
                public Callback(Action<byte[], Android.Hardware.Camera> callback) => this.callback = callback;
                Action<byte[], Android.Hardware.Camera> callback;
                public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera) => callback(data, camera);
            }
#endif
            Android.Hardware.Camera cam;
            Android.Hardware.Camera.Parameters param;
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
                    param = cam.GetParameters();
#if FEATURE_CAMERA_PREVIEWJPEG
                    cam.SetPreviewCallback(new Callback(PreviewHandler));
#endif
                    cam.FaceDetection += (sender, e) =>
                    {
                        var Metrics = Xamarin.Forms.Forms.Context.Resources.DisplayMetrics;
                        var FacesArray = new Rectangle[e.Faces.Length];
                        for (int i = 0; i < FacesArray.Length; i++)
                        {
                            var Rect = e.Faces[i].Rect;
                            FacesArray[i] = new Rectangle(Rect.Left - Metrics.WidthPixels / 2, Rect.Top - Metrics.HeightPixels / 2, Rect.Width(), Rect.Height());
                        }
#if FEATURE_CAMERA_PREVIEWJPEG
                        Faces = FacesArray;
#else
                        ProcessingPreview(this, new CameraEventArgs(FacesArray));
#endif
                    };
                    cam.StartFaceDetection();
                }
                catch (Java.IO.IOException ex)
                {
                    Log(ex);
                }
            }
#if FEATURE_CAMERA_PREVIEWJPEG
            Rectangle[] Faces = Array.Empty<Rectangle>();
#endif
            public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
            {
#if FEATURE_CAMERA_PREVIEWJPEG
                cam?.SetPreviewCallback(null);
#endif
                cam?.StopFaceDetection();
                cam?.StopPreview();
                cam?.Release();

                return true;
            }

            public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height) { }
            public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface) { }

                        #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }
                        #endregion
        }

#elif __IOS__
        public class Camera : UIKit.UIImageView
        {
            public event EventHandler<CameraEventArgs> ProcessingPreview = delegate { };
            AVFoundation.AVCaptureSession session = new AVFoundation.AVCaptureSession()
            {
                SessionPreset = AVFoundation.AVCaptureSession.PresetMedium
            };
            AVFoundation.AVCaptureDevice device = AVFoundation.AVCaptureDevice.DefaultDeviceWithMediaType(AVFoundation.AVMediaType.Video);
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
                        
                        var Features = detector.FeaturesInImage(image);
                        var FacesArray = new Rectangle[Features.Length];
                        for (int i = 0; i < FacesArray.Length; i++)
                        {
                            var Bounds = Features[i].Bounds;
                            FacesArray[i] = new Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
                        }
                        ProcessingPreview(this, new CameraEventArgs(
#if FEATURE_CAMERA_PREVIEWJPEG
                            new UIKit.UIImage(image).AsJPEG().ToArray(), 
#endif
                            FacesArray));
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
            public Camera() => StartPreviewAsync().Ignore();
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
                    StartTimer(TempFrame?.Duration ?? TimeSpan.FromMilliseconds(40), async () => {
                        using (var b = await _mediaCapture.GetPreviewFrameAsync())
                        {
                            var detector = await Windows.Media.FaceAnalysis.FaceDetector.CreateAsync();
                            var Faces = await detector.DetectFacesAsync(b.SoftwareBitmap);
                            var FacesArray = new Rectangle[Faces.Count];
                            for (int i = 0; i < FacesArray.Length; i++)
                            {
                                var Box = Faces[i].FaceBox;
                                FacesArray[i] = new Rectangle((int)Box.X, (int)Box.Y, (int)Box.Width, (int)Box.Height);
                            }
#if FEATURE_CAMERA_PREVIEWJPEG
                            using (var ms = new MemoryStream())
                            {
                                var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync
                                    (Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, ms.AsRandomAccessStream());
                                encoder.SetSoftwareBitmap(b.SoftwareBitmap);
                                ProcessingPreview(this, new CameraEventArgs(ms.ToArray(), FacesArray));
                                try { await encoder.FlushAsync(); } catch { return _isPreviewing; }
                            }
#else
                            ProcessingPreview(this, new CameraEventArgs(FacesArray));
#endif
                            return _isPreviewing;
                        }
                    });
                }
                catch (FileLoadException)
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