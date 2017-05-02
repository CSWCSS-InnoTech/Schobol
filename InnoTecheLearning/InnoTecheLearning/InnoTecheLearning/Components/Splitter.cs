using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using static InnoTecheLearning.Utils;
#if __IOS__
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.View;
using Android.Views;
using View = Xamarin.Forms.View;
#elif NETFX_CORE
using Xamarin.Forms.Platform.UWP;
#endif
[assembly: ExportRendererAttribute(typeof(GridSplitter), typeof(GridSplitter.Renderer))]
namespace InnoTecheLearning
{
    partial class Utils
    {
        public class GridSplitter : ContentView
        {
#region ControlTemplateProperty

            public static new readonly BindableProperty ControlTemplateProperty =
                BindableProperty.Create(nameof(ControlTemplate), typeof(DataTemplate), typeof(GridSplitter),
                    default(DataTemplate), BindingMode.Default, null, (bo, oldCT, newCT) =>
                    {
                        (bo as GridSplitter).Content = (View)(newCT as DataTemplate).CreateContent();
                    });

            public new DataTemplate ControlTemplate
            {
                get
                {
                    return (DataTemplate)GetValue(ControlTemplateProperty);
                }
                set
                {
                    SetValue(ControlTemplateProperty, value);
                }
            }

            public GridSplitter(DataTemplate ControlTemplate) => this.ControlTemplate = ControlTemplate;
            public GridSplitter(Color BackColor = default(Color), Color HandleColor = default(Color)) =>
                ControlTemplate = new DataTemplate(
                    () => new Grid {
                        BackgroundColor = BackColor == default(Color) ? new Color(0, 0, 0, double.Epsilon) : BackColor,
                        WidthRequest = 20,
                        Children = {
                            new StackLayout {
                                HeightRequest = 10, Orientation = StackOrientation.Vertical, Padding = 5,
                                VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center,
                                Spacing = 2, Children = {
                                    new BoxView { HeightRequest = 3,
                                        BackgroundColor = HandleColor == default(Color) ? Color.Gray : HandleColor },
                                    new BoxView { HeightRequest = 3,
                                        BackgroundColor = HandleColor == default(Color) ? Color.Gray : HandleColor }
                                }
                            }
                        }
                    }
                );
#endregion

            public void UpdateGrid(double dragOffsetX, double dragOffsetY)
            {
                if (Parent as Grid == null)
                {
                    return;
                }

                if (IsRowSplitter())
                {
                    UpdateRow(dragOffsetY);
                }
                else
                    UpdateColumn(dragOffsetX);
            }

            private bool IsRowSplitter()
            {
                return HorizontalOptions.Alignment == LayoutAlignment.Fill;
            }

            private void UpdateRow(double offsetY)
            {
                if (offsetY == 0)
                {
                    return;
                }

                var grid = Parent as Grid;
                var row = Grid.GetRow(this);
                int rowCount = grid.RowDefinitions.Count();
                if (rowCount <= 1 ||
                    row == 0 ||
                    row == rowCount - 1 ||
                    row + Grid.GetRowSpan(this) >= rowCount)
                {
                    return;
                }

                RowDefinition rowAbove = grid.RowDefinitions[row - 1];
                var actualHeight = GetRowDefinitionActualHeight(rowAbove) + offsetY;
                if (actualHeight < 0)
                {
                    actualHeight = 0;
                }

                rowAbove.Height = new GridLength(actualHeight);
            }

            private void UpdateColumn(double offsetX)
            {
                if (offsetX == 0)
                {
                    return;
                }

                var grid = Parent as Grid;
                var column = Grid.GetColumn(this);
                int columnCount = grid.ColumnDefinitions.Count();
                if (columnCount <= 1 ||
                    column == 0 ||
                    column == columnCount - 1 ||
                    column + Grid.GetColumnSpan(this) >= columnCount)
                {
                    return;
                }

                ColumnDefinition columnLeft = grid.ColumnDefinitions[column - 1];
                var actualWidth = GetColumnDefinitionActualWidth(columnLeft) + offsetX;
                if (actualWidth < 0)
                {
                    actualWidth = 0;
                }

                columnLeft.Width = new GridLength(actualWidth);
            }

            static private double GetRowDefinitionActualHeight(RowDefinition row)
            {
                double actualHeight;
                if (row.Height.IsAbsolute)
                {
                    actualHeight = row.Height.Value;
                }
                else
                {
                    var property = row.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualHeight");
                    actualHeight = (double)property.GetValue(row);
                }
                return actualHeight;
            }

            static private double GetColumnDefinitionActualWidth(ColumnDefinition column)
            {
                double actualWidth;
                if (column.Width.IsAbsolute)
                {
                    actualWidth = column.Width.Value;
                }
                else
                {
                    var property = column.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualWidth");
                    actualWidth = (double)property.GetValue(column);
                }
                return actualWidth;
            }
#if __IOS__
            public class Renderer : VisualElementRenderer<GridSplitter>
            {
                private UIPanGestureRecognizer _panGestureRecognizer;
                private CGPoint? _oldPt;

                protected override void OnElementChanged(ElementChangedEventArgs<GridSplitter> e)
                {
                    base.OnElementChanged(e);

                    _panGestureRecognizer = new UIPanGestureRecognizer(OnPanGesture)
                        { MaximumNumberOfTouches = 1, MinimumNumberOfTouches = 1 };

                    if (e.NewElement == null)
                    {
                        if (_panGestureRecognizer != null)
                        {
                            this.RemoveGestureRecognizer(_panGestureRecognizer);
                        }
                    }

                    if (e.OldElement == null)
                    {
                        this.AddGestureRecognizer(_panGestureRecognizer);
                        this.UserInteractionEnabled = true;
                    }
                }

                public override void TouchesBegan(NSSet touches, UIEvent evt)
                {
                    base.TouchesBegan(touches, evt);
                    _oldPt = (touches.First() as UITouch).LocationInView(UIApplication.SharedApplication.KeyWindow);
                }

                void OnPanGesture()
                {
                    Point ptOffset = Point.Zero;
                    CGPoint pt = _panGestureRecognizer.LocationInView(UIApplication.SharedApplication.KeyWindow);

                    if (_oldPt != null)
                    {
                        ptOffset = new Point(pt.X - _oldPt.Value.X, pt.Y - _oldPt.Value.Y);
                    }
                    _oldPt = pt;

                    Element.UpdateGrid(ptOffset.X, ptOffset.Y);
                }
            }
#elif __ANDROID__
            public class Renderer : VisualElementRenderer<GridSplitter>
            {
                private Point _lastPoint;

                public override bool OnTouchEvent(MotionEvent e)
                {
                    int action = MotionEventCompat.GetActionMasked(e);

                    switch (action)
                    {
                        case (int)MotionEventActions.Down:
                            {
                                _lastPoint = new Point(e.RawX, e.RawY);
                                break;
                            }

                        case (int)MotionEventActions.Move:
                            {
                                Element.UpdateGrid(Context.FromPixels(e.RawX - _lastPoint.X), Context.FromPixels(e.RawY - _lastPoint.Y));
                                _lastPoint = new Point(e.RawX, e.RawY);
                                break;
                            }
                    }
                    return base.OnTouchEvent(e);
                }
            }
#elif NETFX_CORE
            public class Renderer : ViewRenderer<GridSplitter, Windows.UI.Xaml.FrameworkElement>
            {
                private Windows.Foundation.Point? _lastPt;

                protected override void OnElementChanged(ElementChangedEventArgs<GridSplitter> e)
                {
                    base.OnElementChanged(e);

                    if (e.OldElement != null)
                    {
                        this.PointerPressed -= GridSplitterRenderer_PointerPressed;
                        this.PointerReleased -= GridSplitterRenderer_PointerReleased;
                        this.PointerMoved -= GridSplitterRenderer_PointerMoved;
                    }

                    if (e.NewElement != null)
                    {
                        this.PointerPressed += GridSplitterRenderer_PointerPressed;
                        this.PointerReleased += GridSplitterRenderer_PointerReleased;
                        this.PointerMoved += GridSplitterRenderer_PointerMoved;
                    }
                }

                void GridSplitterRenderer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
                {
                    _lastPt = e.GetCurrentPoint(null).Position;
                    this.CapturePointer(e.Pointer);
                }

                void GridSplitterRenderer_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
                {
                    if (_lastPt != null)
                    {
                        var pt = e.GetCurrentPoint(null).Position;
                        (Element as GridSplitter).UpdateGrid(pt.X - _lastPt.Value.X, pt.Y - _lastPt.Value.Y);
                        _lastPt = pt;
                    }
                }

                void GridSplitterRenderer_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
                {
                    _lastPt = null;
                    this.ReleasePointerCapture(e.Pointer);
                }
            }
#endif
        }
    }
}