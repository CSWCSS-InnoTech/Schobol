using System;
using System.ComponentModel;
using Xamarin.Forms;
#if __ANDROID__
using Android.Content;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Point = Xamarin.Forms.Point;
using View = Android.Views.View;
using MotionEvent = Android.Views.MotionEvent;
using MotionEventActions = Android.Views.MotionEventActions;
#elif __IOS__
using CoreGraphics;
using Foundation;
using UIKit;
using System.Collections.Generic;
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
            public TouchImage() : base() {
                BackgroundColor = DefaultColor; CurrentLineColor = XColor.Black; Ready += () => IsReady = true;
            }
            public static readonly BindableProperty CurrentLineColorProperty =
                BindableProperty.Create("CurrentLineColor", typeof(XColor), typeof(TouchImage), XColor.Black);
            public static XColor DefaultColor = XColor.Transparent;

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

            public void Clear() => WaitExecute(() => ClearEvent?.Invoke());
            public void WaitExecute(Action Task) { if(IsReady) Task(); else Ready += () => Task(); }
            protected internal event Action Ready;
            protected internal bool IsReady;
            protected internal event Action ClearEvent;

            public event EventHandler<PointerEventArgs> PointerEvent;
            public class PointerEventArgs : EventArgs
            {
                public enum PointerEventType : byte { Down, Up, Move, Cancel, /*Enter, Exit*/ }
                public PointerEventType Type { get; }
                public Point Previous { get; }
                public Point Current { get; }
                public bool PointerDown { get; }
                public PointerEventArgs(PointerEventType Type, Point Previous, Point Current, bool Down) : base()
                { this.Type = Type; this.Previous = Previous; this.Current = Current; PointerDown = Down; }
            }
            /*protected internal delegate void TextDelegate(string Text, NamedSize Size, XColor Color, Point Location);
            protected internal event TextDelegate DrawTextEvent;

            public void DrawText(string Text) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, NamedSize.Medium, XColor.Black, new Point()));
            public void DrawText(string Text, Point Location) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, NamedSize.Medium, XColor.Black, Location));
            public void DrawText(string Text, NamedSize Size) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, Size, XColor.Black, new Point()));
            public void DrawText(string Text, NamedSize Size, Point Location) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, Size, XColor.Black, Location));
            public void DrawText(string Text, XColor Color) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, NamedSize.Medium, Color, new Point()));
            public void DrawText(string Text, XColor Color, Point Location) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, NamedSize.Medium, Color, Location));
            public void DrawText(string Text, NamedSize Size, XColor Color) => 
                WaitExecute(() => DrawTextEvent?.Invoke(Text, Size, Color, new Point()));
            public void DrawText(string Text, NamedSize Size, XColor Color, Point Location) =>
                WaitExecute(() => DrawTextEvent?.Invoke(Text, Size, Color, Location));
*/

            public class Renderer : ViewRenderer<TouchImage, DrawView>
            {
                protected override void OnElementChanged(ElementChangedEventArgs<TouchImage> e)
                {
                    base.OnElementChanged(e);
                    try
                    {
                        var Draw = DrawView.Create(new Size(e.NewElement.Width, e.NewElement.Height));
                        //e.NewElement.DrawTextEvent = Draw.DrawText;
                        e.NewElement.ClearEvent = Draw.Clear;
                        Draw.PointerEvent = e.NewElement.PointerEvent;
                        e.NewElement.Ready();
                        SetNativeControl(Draw);
                    } catch (NullReferenceException) { }
                }

                protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    base.OnElementPropertyChanged(sender, e);

                    if (e.PropertyName == CurrentLineColorProperty.PropertyName)
                    Control.CurrentLineColor = Element.CurrentLineColor.
#if __IOS__
                        ToUIColor
#elif __ANDROID__
                        ToAndroid
#elif NETFX_CORE
                        ToWindows
#endif
                            ();
                    else if(e.PropertyName == BackgroundColorProperty.PropertyName)
                        Control.
#if __IOS__
                            BackgroundColor
#else
                            Background
#endif
                             =
#if __IOS__
                            Element.BackgroundColor.ToUIColor()
#elif __ANDROID__
                            new Android.Graphics.Drawables.ColorDrawable(Element.BackgroundColor.ToAndroid())
#else
                            new SolidColorBrush(Element.BackgroundColor.ToWindows())
#endif
                             ;
                }
            }
#if __ANDROID__
            // Original Source: http://csharp-tricks-en.blogspot.com/2014/05/android-draw-on-screen-by-finger.html
            public class DrawView : View
            {
                public static DrawView Create(Size Size)
                {
                    var Return = new DrawView(Forms.Context)
                    {
                        _Size = Size,
                        CanvasBitmap = Bitmap.CreateBitmap(Size.Width < 1 ? 1 : (int)Size.Width,
                        Size.Height < 1 ? 1 : (int)Size.Height, Bitmap.Config.Argb8888)
                    };
                    Return.DrawCanvas = new Canvas(Return.CanvasBitmap);
                    return Return;
                }
                public DrawView(Context context) : base(context) { Start(); }

                public Color CurrentLineColor { get; set; }

                public float PenWidth { get; set; }

                private Path DrawPath;
                private Paint DrawPaint;
                private Paint CanvasPaint;
                private Canvas DrawCanvas;
                private Bitmap CanvasBitmap;/* 
                private string Text;
                private NamedSize NamedSize;
                private XColor XColor;
                private Point Location;
                private bool HasText;*/
                private Size _Size;
                public Size Size
                {
                    get { return _Size; }
                    set
                    {
                        OnSizeChanged((int)value.Width, (int)value.Height,
                        (int)_Size.Width, (int)_Size.Height);
                        _Size = value;
                    }
                }

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
                        Dither = true,
                        Color = CurrentLineColor
                    };
                }

                protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
                {
                    base.OnSizeChanged(w, h, oldw, oldh);
                    if (w < 1 || h < 1) return;
                    CanvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
                    DrawCanvas = new Canvas(CanvasBitmap);
                }

                protected override void OnDraw(Canvas canvas)
                {
                    base.OnDraw(canvas);

                    DrawPaint.Color = CurrentLineColor;
                    canvas.DrawBitmap(CanvasBitmap, 0, 0, CanvasPaint);
                    /*if(HasText) canvas.DrawText(Text, (float)Location.X, (float)Location.Y,
                    new Paint
                    {
                        TextSize = (float)Device.GetNamedSize(NamedSize, typeof(Android.Widget.TextView)),
                        Color = XColor.ToAndroid()
                    });*/
                    canvas.DrawPath(DrawPath, DrawPaint);
                }

                bool PointerDown;
                internal EventHandler<PointerEventArgs> PointerEvent;
                public override bool OnTouchEvent(MotionEvent e)
                {
                    
                    var touchX = e.GetX();
                    var touchY = e.GetY();
                    PathMeasure pm = new PathMeasure(DrawPath, false);
                    //coordinates will be here
                    float[] PrevCoords = { 0f, 0f };
                    //get coordinates of the middle point
                    pm.GetPosTan(pm.Length, PrevCoords, null);
                    switch (e.Action)
                    {
                        case MotionEventActions.Down:
                            DrawPath.MoveTo(touchX, touchY);
                            PointerDown = true;
                            try { Invalidate(); } catch (ObjectDisposedException) { return false; }
                            PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                                 PointerEventType.Down, new Point(PrevCoords[0], PrevCoords[1]),
                                 new Point(touchX,touchY), PointerDown));
                            break;
                        case MotionEventActions.Move:
                            DrawPath.LineTo(touchX, touchY);
                            try { Invalidate(); } catch (ObjectDisposedException) { return false; }
                            PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                                 PointerEventType.Move, new Point(PrevCoords[0], PrevCoords[1]),
                                 new Point(touchX, touchY), PointerDown));
                            break;
                        case MotionEventActions.Up:
                            DrawCanvas.DrawPath(DrawPath, DrawPaint);
                            DrawPath.Reset();
                            PointerDown = false;
                            try { Invalidate(); } catch (ObjectDisposedException) { return false; }
                            PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                                 PointerEventType.Up, new Point(PrevCoords[0], PrevCoords[1]),
                                 new Point(touchX, touchY), PointerDown));
                            break;
                        case MotionEventActions.Cancel:
                            DrawCanvas.DrawPath(DrawPath, DrawPaint);
                            DrawPath.Reset();
                            PointerDown = false;
                            try { Invalidate(); } catch (ObjectDisposedException) { return false; }
                            PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                                 PointerEventType.Cancel, new Point(PrevCoords[0], PrevCoords[1]),
                                 new Point(touchX, touchY), PointerDown));
                            break;
                        default:
                            return false;
                    }

                    return true;
                }
                
                public void Clear()
                {
                    DrawPath.Reset();/*
                    this.HasText = false;
                    this.Text = string.Empty;
                    this.NamedSize = NamedSize.Default;
                    this.XColor = XColor.Default;
                    this.Location = Point.Zero;*/
                    DrawCanvas = new Canvas(CanvasBitmap =
                        Bitmap.CreateBitmap(CanvasBitmap.Width, CanvasBitmap.Height, Bitmap.Config.Argb8888));
                    IgnoreEx(Invalidate, typeof(ObjectDisposedException));
                }
/*
                public void DrawText(string Text, NamedSize Size, XColor Color, Point Location)
                {
                    this.HasText = true;
                    this.Text = Text;
                    this.NamedSize = Size;
                    this.XColor = Color;
                    this.Location = Location;
                    Invalidate();
                }*/
            }
#elif __IOS__
            // Original Source: http://stackoverflow.com/questions/21029440/xamarin-ios-drawing-onto-image-after-scaling-it
            public class DrawView : UIView
            {
                public static DrawView Create(Size Size) => new DrawView(new RectangleF(PointF.Empty, Size.ToSizeF()));

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
                
                public event DrawDelegate DrawEvent;
                public delegate void DrawDelegate(RectangleF rect);
                /*public void DrawText(string Text, NamedSize Size, XColor Color, Point Location)
                { DrawEvent = DrawText;
                    InnerText = Text;
                    InnerSize = Size;
                    InnerColor = Color;
                }
                private string InnerText;
                private NamedSize InnerSize;
                private XColor InnerColor;
                private Point InnerLocation;
                private void DrawText(RectangleF rect)
                { 
                    InnerColor.ToUIColor().SetStroke();
                    using (var Label = new UILabel(rect)
                    {
                        Text = InnerText,
                        Font = Font.SystemFontOfSize(InnerSize).ToUIFont(),
                        Center = new PointF(InnerLocation.X, InnerLocation.Y)
                    }) 
                        Label.DrawText(rect); }*/

                public void Clear()
                {
                    //DrawEvent -= DrawText;
                    DrawPath.Dispose();
                    DrawPath = new CGPath();
                    SetNeedsDisplay();
                }

                bool PointerDown;
                internal EventHandler<PointerEventArgs> PointerEvent;
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
                    PointerDown = true;
                    PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                         PointerEventType.Down, PreviousPoint.ToPoint(), newPoint.ToPoint(), PointerDown));
                }

                public override void TouchesMoved(NSSet touches, UIEvent evt)
                {
                    var touch = (UITouch)touches.AnyObject;
                    var currentPoint = touch.LocationInView(this);

                    if (Math.Abs(currentPoint.X - PreviousPoint.X) >= 4 ||
                        Math.Abs(currentPoint.Y - PreviousPoint.Y) >= 4)
                    {

                        var newPoint = new PointF((currentPoint.X + PreviousPoint.X) / 2,
                            (currentPoint.Y + PreviousPoint.Y) / 2);

                        CurrentPath.AddQuadCurveToPoint(newPoint, PreviousPoint);
                        PreviousPoint = currentPoint;
                        InvokeOnMainThread(SetNeedsDisplay);
                        PointerEvent?.Invoke(this, new PointerEventArgs(PointerEventArgs.
                             PointerEventType.Move, PreviousPoint.ToPoint(), newPoint.ToPoint(), PointerDown));
                    }
                    else
                    {
                        CurrentPath.AddLineTo(currentPoint);
                        InvokeOnMainThread(SetNeedsDisplay);
                    }

                }

                public override void TouchesEnded(NSSet touches, UIEvent evt)
                {
                    RaisePointerEvent(touches, PointerEventArgs.PointerEventType.Up, PointerDown = false);
                }

                public override void TouchesCancelled(NSSet touches, UIEvent evt)
                {
                    RaisePointerEvent(touches, PointerEventArgs.PointerEventType.Cancel, PointerDown = false);
                }

                public void RaisePointerEvent(NSSet touches, PointerEventArgs.PointerEventType Type, bool PointerDown)
                {
                    var touch = (UITouch)touches.AnyObject;
                    PreviousPoint = touch.PreviousLocationInView(this);

                    var newPoint = touch.LocationInView(this);
                    InvokeOnMainThread(SetNeedsDisplay);
                    PointerEvent?.Invoke(this, new PointerEventArgs(Type,
                        PreviousPoint.ToPoint(), newPoint.ToPoint(), PointerDown));
                }

                public override void Draw(RectangleF rect)
                {
                    foreach (var line in Lines)
                    {
                        line.Color.SetStroke();
                        line.Path.Stroke();
                    }
                    DrawEvent(rect);
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
            // Original Source: http://www.geekchamp.com/tips/drawing-in-wp7-2-drawing-shapes-with-finger
            public class DrawView : InnoTecheLearning.DrawView
            {
                private DrawView() : base() { }


                public global::Windows.UI.Color CurrentLineColor
                {
                    get
                    {
                        return CurrentBrush.Color;
                    }
                    set
                    {
                        CurrentBrush.Color = value;
                    }
                }

                public new Brush Background
                {
                    get
                    {
                        return VisualTreeHelper.GetChild(this, 0).Cast<Canvas>().Background;
                    }
                    set
                    {
                        base.Background = value;
                        VisualTreeHelper.GetChild(this, 0).Cast<Canvas>().Background = value;
                    }
                }
                public global::Windows.UI.Color BackgroundColor
                {
                    get
                    {
                        return Background.Cast<SolidColorBrush>().Color;
                    }
                    set
                    {
                        base.Background.Cast<SolidColorBrush>().Color = value;
                        Background.Cast<SolidColorBrush>().Color = value;
                    }
                }
                public SolidColorBrush CurrentBrush { get; set; }

                public float PenWidth { get; set; }
                private Point CurrentPoint;
                private Point PreviousPoint;
                internal EventHandler<PointerEventArgs> PointerEvent;

                public static DrawView Create(Size Rect)
                {
                    DrawView Return = new DrawView();
                    
                    Canvas ContentPanelCanvas = (Canvas)VisualTreeHelper.GetChild(Return, 0);

                    ContentPanelCanvas.Background = new SolidColorBrush(DefaultColor.ToWindows());
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

                        if(Return.PointerDown) ContentPanelCanvas.Children.Add(line);

                        Return.PreviousPoint = Return.CurrentPoint;
                        Return.PointerEvent?.Invoke(Return, new PointerEventArgs(PointerEventArgs.
                             PointerEventType.Move, Return.PreviousPoint, Return.CurrentPoint, Return.PointerDown));
                    };
                    ContentPanelCanvas.PointerPressed += (object sender, PointerRoutedEventArgs e) =>
                    Handle(Return, e, PointerEventArgs.PointerEventType.Down, true);
                    ContentPanelCanvas.PointerReleased += (object sender, PointerRoutedEventArgs e) =>
                    Handle(Return, e, PointerEventArgs.PointerEventType.Up, false);
                    ContentPanelCanvas.PointerCanceled += (object sender, PointerRoutedEventArgs e) =>
                    Handle(Return, e, PointerEventArgs.PointerEventType.Cancel, false);

                    Return.ClearEvent = ContentPanelCanvas.Children.Clear;
                    /*Return.TextEvent = (Text, Size, Color, Location) => {
                        ContentPanelCanvas.Children.Add(new TextBlock { Text = Text,
                        FontSize = Device.GetNamedSize(Size, typeof(TextBlock)),
                        Foreground = new SolidColorBrush(Color.ToWindows()),
                        RenderTransform = new TranslateTransform { X = Location.X, Y = Location.Y }
                        }); };*/
                    return Return;
                }
                private static void Handle(DrawView Return, PointerRoutedEventArgs e,
                    PointerEventArgs.PointerEventType Type, bool PointerDown)
                    {
                        Return.PointerDown = PointerDown;
                        var Point = e.GetCurrentPoint(Return).Position;
                        Return.CurrentPoint = new Point(Point.X, Point.Y);
                        Return.PreviousPoint = Return.CurrentPoint;
                        Return.PointerEvent?.Invoke(Return, new PointerEventArgs(Type,
                            Return.PreviousPoint, Return.CurrentPoint, PointerDown));
                    }
                public bool PointerDown { get; set; }
                private event Action ClearEvent;
                public void Clear()
                { ClearEvent(); }
                /*private event TextDelegate TextEvent;
                public void DrawText(string Text, NamedSize Size, XColor Color, Point Location)
                { TextEvent(Text, Size, Color, Location); }*/
            }
#endif

        }
    }
}