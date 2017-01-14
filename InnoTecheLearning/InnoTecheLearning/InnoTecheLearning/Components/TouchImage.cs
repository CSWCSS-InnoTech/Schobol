using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
#if __ANDROID__
using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using BindableProperty = Xamarin.Forms.BindableProperty;
using ExportRenderer = Xamarin.Forms.ExportRendererAttribute;
#elif __IOS__
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PointF = CoreGraphics.CGPoint;
using RectangleF = CoreGraphics.CGRect;
#elif NETFX_CORE
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
#if WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#endif
#endif
using XColor = Xamarin.Forms.Color;
using Image = Xamarin.Forms.Image;
using TouchImage = InnoTecheLearning.Utils.TouchImage;

[assembly: ExportRenderer(typeof(TouchImage), typeof(TouchImage.Renderer))]

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class TouchImage : Image
        {
            public static readonly BindableProperty CurrentLineColorProperty =
                BindableProperty.Create("CurrentLineColor", typeof(XColor), typeof(TouchImage), XColor.Default);

            public XColor CurrentLineColor
            {
                get
                {
                    return (XColor)GetValue(CurrentLineColorProperty);
                }
                set
                {
                    SetValue(CurrentLineColorProperty, value);
                }
            }
#if __ANDROID__

            public class Renderer : ViewRenderer<TouchImage, DrawView>
            {
                protected override void OnElementChanged(ElementChangedEventArgs<TouchImage> e)
                {
                    base.OnElementChanged(e);

                    if (e.OldElement == null)
                    {
                        SetNativeControl(new DrawView(Context));
                    }
                }

                protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    base.OnElementPropertyChanged(sender, e);

                    if (e.PropertyName == TouchImage.CurrentLineColorProperty.PropertyName)
                    {
                        UpdateControl();
                    }
                }

                private void UpdateControl()
                {
                    Control.CurrentLineColor = Element.CurrentLineColor.ToAndroid();
                }
            }

            // Original Source: http://csharp-tricks-en.blogspot.com/2014/05/android-draw-on-screen-by-finger.html
            public class DrawView : View
            {
                public DrawView(Context context)
                    : base(context)
                {
                    Start();
                }

                public Color CurrentLineColor { get; set; }

                public float PenWidth { get; set; }

                private Path DrawPath;
                private Paint DrawPaint;
                private Paint CanvasPaint;
                private Canvas DrawCanvas;
                private Bitmap CanvasBitmap;

                private void Start()
                {
                    CurrentLineColor = Color.Black;
                    PenWidth = 5.0f;

                    DrawPath = new Path();
                    DrawPaint = new Paint
                    {
                        Color = CurrentLineColor,
                        AntiAlias = true,
                        StrokeWidth = PenWidth
                    };

                    DrawPaint.SetStyle(Paint.Style.Stroke);
                    DrawPaint.StrokeJoin = Paint.Join.Round;
                    DrawPaint.StrokeCap = Paint.Cap.Round;

                    CanvasPaint = new Paint
                    {
                        Dither = true
                    };
                }

                protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
                {
                    base.OnSizeChanged(w, h, oldw, oldh);

                    CanvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
                    DrawCanvas = new Canvas(CanvasBitmap);
                }

                protected override void OnDraw(Canvas canvas)
                {
                    base.OnDraw(canvas);

                    DrawPaint.Color = CurrentLineColor;
                    canvas.DrawBitmap(CanvasBitmap, 0, 0, CanvasPaint);
                    canvas.DrawPath(DrawPath, DrawPaint);
                }

                public override bool OnTouchEvent(MotionEvent e)
                {
                    var touchX = e.GetX();
                    var touchY = e.GetY();

                    switch (e.Action)
                    {
                        case MotionEventActions.Down:
                            DrawPath.MoveTo(touchX, touchY);
                            break;
                        case MotionEventActions.Move:
                            DrawPath.LineTo(touchX, touchY);
                            break;
                        case MotionEventActions.Up:
                            DrawCanvas.DrawPath(DrawPath, DrawPaint);
                            DrawPath.Reset();
                            break;
                        default:
                            return false;
                    }

                    Invalidate();

                    return true;
                }
            }
#elif __IOS__
            public class Renderer : ViewRenderer<TouchImage, DrawView>
            {
                protected override void OnElementChanged(ElementChangedEventArgs<TouchImage> e)
                {
                    base.OnElementChanged(e);

                    SetNativeControl(new DrawView(RectangleF.Empty));
                }

                protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    base.OnElementPropertyChanged(sender, e);

                    if (e.PropertyName == TouchImage.CurrentLineColorProperty.PropertyName)
                    {
                        UpdateControl();
                    }
                }

                private void UpdateControl()
                {
                    Control.CurrentLineColor = Element.CurrentLineColor.ToUIColor();
                }
            }
            // Original Source: http://stackoverflow.com/questions/21029440/xamarin-ios-drawing-onto-image-after-scaling-it
            public class DrawView : UIView
            {
                public DrawView(RectangleF frame) : base(frame)
                {
                    DrawPath = new CGPath();
                    CurrentLineColor = UIColor.Black;
                    PenWidth = 5.0f;
                    Lines = new List<VESLine>();
                }

                private PointF PreviousPoint;
                private CGPath DrawPath;
                private byte IndexCount;
                private UIBezierPath CurrentPath;
                private List<VESLine> Lines;

                public UIColor CurrentLineColor { get; set; }
                public float PenWidth { get; set; }

                public void Clear()
                {
                    DrawPath.Dispose();
                    DrawPath = new CGPath();
                    SetNeedsDisplay();
                }

                public override void TouchesBegan(NSSet touches, UIEvent evt)
                {
                    IndexCount++;

                    var path = new UIBezierPath
                    {
                        LineWidth = PenWidth
                    };

                    var touch = (UITouch)touches.AnyObject;
                    PreviousPoint = touch.PreviousLocationInView(this);

                    var newPoint = touch.LocationInView(this);
                    path.MoveTo(newPoint);

                    InvokeOnMainThread(SetNeedsDisplay);

                    CurrentPath = path;

                    var line = new VESLine
                    {
                        Path = CurrentPath,
                        Color = CurrentLineColor,
                        Index = IndexCount
                    };

                    Lines.Add(line);
                }

                public override void TouchesMoved(NSSet touches, UIEvent evt)
                {
                    var touch = (UITouch)touches.AnyObject;
                    var currentPoint = touch.LocationInView(this);

                    if (Math.Abs(currentPoint.X - PreviousPoint.X) >= 4 ||
                        Math.Abs(currentPoint.Y - PreviousPoint.Y) >= 4)
                    {

                        var newPoint = new PointF((currentPoint.X + PreviousPoint.X) / 2, (currentPoint.Y + PreviousPoint.Y) / 2);

                        CurrentPath.AddQuadCurveToPoint(newPoint, PreviousPoint);
                        PreviousPoint = currentPoint;
                    }
                    else
                    {
                        CurrentPath.AddLineTo(currentPoint);
                    }

                    InvokeOnMainThread(SetNeedsDisplay);
                }

                public override void TouchesEnded(NSSet touches, UIEvent evt)
                {
                    InvokeOnMainThread(SetNeedsDisplay);
                }

                public override void TouchesCancelled(NSSet touches, UIEvent evt)
                {
                    InvokeOnMainThread(SetNeedsDisplay);
                }

                public override void Draw(RectangleF rect)
                {
                    foreach (var line in Lines)
                    {
                        line.Color.SetStroke();
                        line.Path.Stroke();
                    }
                }
            }
            public class VESLine
            {
                public UIBezierPath Path
                {
                    get;
                    set;
                }

                public UIColor Color
                {
                    get;
                    set;
                }

                public byte Index
                {
                    get;
                    set;
                }
            }
#elif NETFX_CORE
            public class Renderer : ViewRenderer<TouchImage, DrawView>
            {
                protected override void OnElementChanged(ElementChangedEventArgs<TouchImage> e)
                {
                    base.OnElementChanged(e);

                    SetNativeControl(DrawView.Create());
                }

                protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    base.OnElementPropertyChanged(sender, e);

                    if (e.PropertyName == TouchImage.CurrentLineColorProperty.PropertyName)
                    {
                        UpdateControl();
                    }
                }

                private void UpdateControl()
                {
                    var converter = new ColorConverter();

                    Control.CurrentBrush =
                        (SolidColorBrush)
                            converter.Convert(Element.CurrentLineColor, null, null, null);
                }
            }    // Original Source: http://www.geekchamp.com/tips/drawing-in-wp7-2-drawing-shapes-with-finger
            public partial class DrawView : UserControl
            {
                private DrawView() { }

                public Brush CurrentBrush { get; set; }

                public float PenWidth { get; set; }
                private Point CurrentPoint;
                private Point PreviousPoint;

                public static DrawView Create()
                {
                    DrawView Return = (DrawView)global::Windows.UI.Xaml.Markup.XamlReader.Load(
@"<UserControl x:Class=""InnoTecheLearning.Utils.TouchImage.DrawView""
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
    mc:Ignorable=""d""
    FontFamily=""{StaticResource PhoneFontFamilyNormal}""
    FontSize=""{StaticResource PhoneFontSizeNormal}""
    Foreground=""{StaticResource PhoneForegroundBrush}""
    d:DesignHeight=""480"" d:DesignWidth=""480"">

    <Canvas x:Name=""ContentPanelCanvas"" Background=""Cornsilk"" HorizontalAlignment=""Stretch""
          VerticalAlignment=""Stretch"" />
</UserControl>");
                    Canvas ContentPanelCanvas = (Canvas)VisualTreeHelper.GetChild(Return, 0);

                    Return.CurrentBrush = new SolidColorBrush(Colors.Black);
                    Return.PenWidth = 5.0f;

                    ContentPanelCanvas.PointerMoved += (object sender, PointerRoutedEventArgs e) =>
                    {
                        var Point = e.GetCurrentPoint(Return).Position;
                        Return.CurrentPoint = new Point(Point.X, Point.Y);

                        var line = new Line
                        {
                            X1 = Return.CurrentPoint.X,
                            Y1 = Return.CurrentPoint.Y,
                            X2 = Return.PreviousPoint.X,
                            Y2 = Return.PreviousPoint.Y,
                            Stroke = Return.CurrentBrush,
                            StrokeThickness = Return.PenWidth
                        };

                        ContentPanelCanvas.Children.Add(line);

                        Return.PreviousPoint = Return.CurrentPoint;
                    };
                    ContentPanelCanvas.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
                    {
                        var Point = e.GetCurrentPoint(Return).Position;
                        Return.CurrentPoint = new Point(Point.X, Point.Y);
                        Return.PreviousPoint = Return.CurrentPoint;
                    };
                    return Return;
                }
            }
#endif

        }
    }
}
