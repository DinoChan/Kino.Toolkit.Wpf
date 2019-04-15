using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public sealed class SystemDropShadowChrome : Decorator
    {
        public static readonly DependencyProperty ColorProperty;

        public static readonly DependencyProperty CornerRadiusProperty;

        private const double ShadowDepth = 5;

        private const int TopLeft = 0;

        private const int Top = 1;

        private const int TopRight = 2;

        private const int Left = 3;

        private const int Center = 4;

        private const int Right = 5;

        private const int BottomLeft = 6;

        private const int Bottom = 7;

        private const int BottomRight = 8;

        private static Brush[] _commonBrushes;

        private static CornerRadius _commonCornerRadius;

        private static object _resourceAccess;

        private Brush[] _brushes;

        public Color Color
        {
            get => (Color)base.GetValue(SystemDropShadowChrome.ColorProperty);
            set => base.SetValue(SystemDropShadowChrome.ColorProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)base.GetValue(SystemDropShadowChrome.CornerRadiusProperty);
            set
            {
                base.SetValue(SystemDropShadowChrome.CornerRadiusProperty, value);
            }
        }

        static SystemDropShadowChrome()
        {
            SystemDropShadowChrome.ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(SystemDropShadowChrome), new FrameworkPropertyMetadata((object)Color.FromArgb(113, 0, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(SystemDropShadowChrome.ClearBrushes)));
            Type type = typeof(CornerRadius);
            Type type1 = typeof(SystemDropShadowChrome);
            CornerRadius cornerRadiu = new CornerRadius();
            SystemDropShadowChrome.CornerRadiusProperty = DependencyProperty.Register("CornerRadius", type, type1, new FrameworkPropertyMetadata((object)cornerRadiu, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(SystemDropShadowChrome.ClearBrushes)), new ValidateValueCallback(SystemDropShadowChrome.IsCornerRadiusValid));
            SystemDropShadowChrome._resourceAccess = new object();
        }

        public SystemDropShadowChrome()
        {
        }

        private static void ClearBrushes(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((SystemDropShadowChrome)o)._brushes = null;
        }

        private static Brush[] CreateBrushes(Color c, CornerRadius cornerRadius)
        {
            GradientStopCollection gradientStopCollections;
            GradientStopCollection gradientStopCollections1;
            GradientStopCollection gradientStopCollections2;
            GradientStopCollection gradientStopCollections3;
            Brush[] solidColorBrush = new Brush[] { null, null, null, null, new SolidColorBrush(c), null, null, null, null };
            solidColorBrush[4].Freeze();
            GradientStopCollection gradientStopCollections4 = SystemDropShadowChrome.CreateStops(c, 0);
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStopCollections4, new Point(0, 1), new Point(0, 0));
            linearGradientBrush.Freeze();
            solidColorBrush[1] = linearGradientBrush;
            LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush(gradientStopCollections4, new Point(1, 0), new Point(0, 0));
            linearGradientBrush1.Freeze();
            solidColorBrush[3] = linearGradientBrush1;
            LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(gradientStopCollections4, new Point(0, 0), new Point(1, 0));
            linearGradientBrush2.Freeze();
            solidColorBrush[5] = linearGradientBrush2;
            LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush(gradientStopCollections4, new Point(0, 0), new Point(0, 1));
            linearGradientBrush3.Freeze();
            solidColorBrush[7] = linearGradientBrush3;
            gradientStopCollections = (cornerRadius.TopLeft != 0 ? SystemDropShadowChrome.CreateStops(c, cornerRadius.TopLeft) : gradientStopCollections4);
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush(gradientStopCollections)
            {
                RadiusX = 1,
                RadiusY = 1,
                Center = new Point(1, 1),
                GradientOrigin = new Point(1, 1)
            };
            radialGradientBrush.Freeze();
            solidColorBrush[0] = radialGradientBrush;
            if (cornerRadius.TopRight != 0)
            {
                gradientStopCollections1 = (cornerRadius.TopRight != cornerRadius.TopLeft ? SystemDropShadowChrome.CreateStops(c, cornerRadius.TopRight) : gradientStopCollections);
            }
            else
            {
                gradientStopCollections1 = gradientStopCollections4;
            }
            RadialGradientBrush radialGradientBrush1 = new RadialGradientBrush(gradientStopCollections1)
            {
                RadiusX = 1,
                RadiusY = 1,
                Center = new Point(0, 1),
                GradientOrigin = new Point(0, 1)
            };
            radialGradientBrush1.Freeze();
            solidColorBrush[2] = radialGradientBrush1;
            if (cornerRadius.BottomLeft == 0)
            {
                gradientStopCollections2 = gradientStopCollections4;
            }
            else if (cornerRadius.BottomLeft != cornerRadius.TopLeft)
            {
                gradientStopCollections2 = (cornerRadius.BottomLeft != cornerRadius.TopRight ? SystemDropShadowChrome.CreateStops(c, cornerRadius.BottomLeft) : gradientStopCollections1);
            }
            else
            {
                gradientStopCollections2 = gradientStopCollections;
            }
            RadialGradientBrush radialGradientBrush2 = new RadialGradientBrush(gradientStopCollections2)
            {
                RadiusX = 1,
                RadiusY = 1,
                Center = new Point(1, 0),
                GradientOrigin = new Point(1, 0)
            };
            radialGradientBrush2.Freeze();
            solidColorBrush[6] = radialGradientBrush2;
            if (cornerRadius.BottomRight == 0)
            {
                gradientStopCollections3 = gradientStopCollections4;
            }
            else if (cornerRadius.BottomRight == cornerRadius.TopLeft)
            {
                gradientStopCollections3 = gradientStopCollections;
            }
            else if (cornerRadius.BottomRight != cornerRadius.TopRight)
            {
                gradientStopCollections3 = (cornerRadius.BottomRight != cornerRadius.BottomLeft ? SystemDropShadowChrome.CreateStops(c, cornerRadius.BottomRight) : gradientStopCollections2);
            }
            else
            {
                gradientStopCollections3 = gradientStopCollections1;
            }
            RadialGradientBrush radialGradientBrush3 = new RadialGradientBrush(gradientStopCollections3)
            {
                RadiusX = 1,
                RadiusY = 1,
                Center = new Point(0, 0),
                GradientOrigin = new Point(0, 0)
            };
            radialGradientBrush3.Freeze();
            solidColorBrush[8] = radialGradientBrush3;
            return solidColorBrush;
        }

        private static GradientStopCollection CreateStops(Color c, double cornerRadius)
        {
            double num = 1 / (cornerRadius + 5);
            GradientStopCollection gradientStopCollections = new GradientStopCollection()
            {
                new GradientStop(c, (0.5 + cornerRadius) * num)
            };
            Color a = c;
            a.A = (byte)(0.74336 * (double)c.A);
            gradientStopCollections.Add(new GradientStop(a, (1.5 + cornerRadius) * num));
            a.A = (byte)(0.38053 * (double)c.A);
            gradientStopCollections.Add(new GradientStop(a, (2.5 + cornerRadius) * num));
            a.A = (byte)(0.12389 * (double)c.A);
            gradientStopCollections.Add(new GradientStop(a, (3.5 + cornerRadius) * num));
            a.A = (byte)(0.02654 * (double)c.A);
            gradientStopCollections.Add(new GradientStop(a, (4.5 + cornerRadius) * num));
            a.A = 0;
            gradientStopCollections.Add(new GradientStop(a, (5 + cornerRadius) * num));
            gradientStopCollections.Freeze();
            return gradientStopCollections;
        }

        private Brush[] GetBrushes(Color c, CornerRadius cornerRadius)
        {
            if (SystemDropShadowChrome._commonBrushes == null)
            {
                lock (SystemDropShadowChrome._resourceAccess)
                {
                    if (SystemDropShadowChrome._commonBrushes == null)
                    {
                        SystemDropShadowChrome._commonBrushes = SystemDropShadowChrome.CreateBrushes(c, cornerRadius);
                        SystemDropShadowChrome._commonCornerRadius = cornerRadius;
                    }
                }
            }
            if (c == ((SolidColorBrush)SystemDropShadowChrome._commonBrushes[4]).Color && cornerRadius == SystemDropShadowChrome._commonCornerRadius)
            {
                this._brushes = null;
                return SystemDropShadowChrome._commonBrushes;
            }
            if (this._brushes == null)
            {
                this._brushes = SystemDropShadowChrome.CreateBrushes(c, cornerRadius);
            }
            return this._brushes;
        }

        private static bool IsCornerRadiusValid(object value)
        {
            CornerRadius cornerRadiu = (CornerRadius)value;
            if (cornerRadiu.TopLeft < 0 || cornerRadiu.TopRight < 0 || cornerRadiu.BottomLeft < 0 || cornerRadiu.BottomRight < 0 || double.IsNaN(cornerRadiu.TopLeft) || double.IsNaN(cornerRadiu.TopRight) || double.IsNaN(cornerRadiu.BottomLeft) || double.IsNaN(cornerRadiu.BottomRight) || double.IsInfinity(cornerRadiu.TopLeft) || double.IsInfinity(cornerRadiu.TopRight) || double.IsInfinity(cornerRadiu.BottomLeft))
            {
                return false;
            }
            return !double.IsInfinity(cornerRadiu.BottomRight);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            CornerRadius cornerRadius = this.CornerRadius;
            Point point = new Point(5, 5);
            double width = RenderSize.Width;
            Size renderSize = base.RenderSize;
            Rect rect = new Rect(point, new Size(width, renderSize.Height));
            Color color = this.Color;
            if (rect.Width > 0 && rect.Height > 0 && color.A > 0)
            {
                double right = rect.Right - rect.Left - 10;
                double bottom = rect.Bottom - rect.Top - 10;
                double num = Math.Min(right * 0.5, bottom * 0.5);
                cornerRadius.TopLeft = Math.Min(cornerRadius.TopLeft, num);
                cornerRadius.TopRight = Math.Min(cornerRadius.TopRight, num);
                cornerRadius.BottomLeft = Math.Min(cornerRadius.BottomLeft, num);
                cornerRadius.BottomRight = Math.Min(cornerRadius.BottomRight, num);
                Brush[] brushes = this.GetBrushes(color, cornerRadius);
                double top = rect.Top + 5;
                double left = rect.Left + 5;
                double right1 = rect.Right - 5;
                double bottom1 = rect.Bottom - 5;
                double[] topLeft = new double[] { left, left + cornerRadius.TopLeft, right1 - cornerRadius.TopRight, left + cornerRadius.BottomLeft, right1 - cornerRadius.BottomRight, right1 };
                double[] numArray = new double[] { top, top + cornerRadius.TopLeft, top + cornerRadius.TopRight, bottom1 - cornerRadius.BottomLeft, bottom1 - cornerRadius.BottomRight, bottom1 };
                drawingContext.PushGuidelineSet(new GuidelineSet(topLeft, numArray));
                cornerRadius.TopLeft = cornerRadius.TopLeft + 5;
                cornerRadius.TopRight = cornerRadius.TopRight + 5;
                cornerRadius.BottomLeft = cornerRadius.BottomLeft + 5;
                cornerRadius.BottomRight = cornerRadius.BottomRight + 5;
                Rect rect1 = new Rect(rect.Left, rect.Top, cornerRadius.TopLeft, cornerRadius.TopLeft);
                drawingContext.DrawRectangle(brushes[0], null, rect1);
                double num1 = topLeft[2] - topLeft[1];
                if (num1 > 0)
                {
                    Rect rect2 = new Rect(topLeft[1], rect.Top, num1, 5);
                    drawingContext.DrawRectangle(brushes[1], null, rect2);
                }
                Rect rect3 = new Rect(topLeft[2], rect.Top, cornerRadius.TopRight, cornerRadius.TopRight);
                drawingContext.DrawRectangle(brushes[2], null, rect3);
                double num2 = numArray[3] - numArray[1];
                if (num2 > 0)
                {
                    Rect rect4 = new Rect(rect.Left, numArray[1], 5, num2);
                    drawingContext.DrawRectangle(brushes[3], null, rect4);
                }

                double num3 = numArray[4] - numArray[2];
                if (num3 > 0)
                {
                    Rect rect5 = new Rect(topLeft[5], numArray[2], 5, num3);
                    drawingContext.DrawRectangle(brushes[5], null, rect5);
                }

                Rect rect6 = new Rect(rect.Left, numArray[3], cornerRadius.BottomLeft, cornerRadius.BottomLeft);
                drawingContext.DrawRectangle(brushes[6], null, rect6);
                double num4 = topLeft[4] - topLeft[3];
                if (num4 > 0)
                {
                    Rect rect7 = new Rect(topLeft[3], numArray[5], num4, 5);
                    drawingContext.DrawRectangle(brushes[7], null, rect7);
                }

                Rect rect8 = new Rect(topLeft[4], numArray[4], cornerRadius.BottomRight, cornerRadius.BottomRight);
                drawingContext.DrawRectangle(brushes[8], null, rect8);
                if (cornerRadius.TopLeft != 5 || cornerRadius.TopLeft != cornerRadius.TopRight || cornerRadius.TopLeft != cornerRadius.BottomLeft || cornerRadius.TopLeft != cornerRadius.BottomRight)
                {
                    PathFigure pathFigure = new PathFigure();
                    if (cornerRadius.TopLeft <= 5)
                    {
                        pathFigure.StartPoint = new Point(topLeft[0], numArray[0]);
                    }
                    else
                    {
                        pathFigure.StartPoint = new Point(topLeft[1], numArray[0]);
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[1], numArray[1]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[0], numArray[1]), true));
                    }

                    if (cornerRadius.BottomLeft <= 5)
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[0], numArray[5]), true));
                    }
                    else
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[0], numArray[3]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[3], numArray[3]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[3], numArray[5]), true));
                    }

                    if (cornerRadius.BottomRight <= 5)
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[5], numArray[5]), true));
                    }
                    else
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[4], numArray[5]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[4], numArray[4]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[5], numArray[4]), true));
                    }

                    if (cornerRadius.TopRight <= 5)
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[5], numArray[0]), true));
                    }
                    else
                    {
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[5], numArray[2]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[2], numArray[2]), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(topLeft[2], numArray[0]), true));
                    }

                    pathFigure.IsClosed = true;
                    pathFigure.Freeze();
                    PathGeometry pathGeometry = new PathGeometry();
                    pathGeometry.Figures.Add(pathFigure);
                    pathGeometry.Freeze();
                    drawingContext.DrawGeometry(brushes[4], null, pathGeometry);
                }
                else
                {
                    Rect rect9 = new Rect(topLeft[0], numArray[0], right, bottom);
                    drawingContext.DrawRectangle(brushes[4], null, rect9);
                }
                drawingContext.Pop();
            }
        }
    }
}
