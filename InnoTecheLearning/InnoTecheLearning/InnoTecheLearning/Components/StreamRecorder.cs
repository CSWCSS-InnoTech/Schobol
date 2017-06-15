using System;
using System.Collections.Generic;
using System.Text;
using Android.Graphics;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
#if __ANDROID__
        public class Camera : Android.Views.TextureView, Android.Views.TextureView.ISurfaceTextureListener
        {
            public Camera() : base(Forms.Context) => SurfaceTextureListener = this;
#pragma warning disable 618
            //Reason: Need Android 4 suport
            Android.Hardware.Camera cam;
            public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
            {
                cam = Android.Hardware.Camera.Open();
#pragma warning restore 618
                LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(w, h);

                try
                {
                    cam.SetPreviewTexture(surface);
                    cam.StartPreview();

                }
                catch (Java.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
            {
                cam.StopPreview();
                cam.Release();

                return true;
            }

            public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height) { }
            public void OnSurfaceTextureUpdated(SurfaceTexture surface) { }
        }
#endif
    }
}