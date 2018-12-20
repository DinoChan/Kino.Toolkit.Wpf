using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public sealed class DataGridHeaderBorder : Border
    {
        private enum AeroFreezables
        {
            NormalBevel,
            NormalBackground,
            PressedBackground,
            HoveredBackground,
            SortedBackground,
            PressedTop,
            NormalSides,
            PressedSides,
            HoveredSides,
            SortedSides,
            PressedBevel,
            NormalBottom,
            PressedOrHoveredBottom,
            SortedBottom,
            ArrowBorder,
            ArrowFill,
            ArrowFillScale,
            ArrowUpGeometry,
            ArrowDownGeometry,
            NumFreezables
        }

        public static readonly DependencyProperty IsHoveredProperty;

        public static readonly DependencyProperty IsPressedProperty;

        public static readonly DependencyProperty IsClickableProperty;

        public static readonly DependencyProperty SortDirectionProperty;

        public static readonly DependencyProperty IsSelectedProperty;

        public static readonly DependencyProperty OrientationProperty;

        public static readonly DependencyProperty SeparatorBrushProperty;

        public static readonly DependencyProperty SeparatorVisibilityProperty;

        private static List<Freezable> _freezableCache;

        private static readonly object _cacheAccess;

        public bool IsHovered
        {
            get
            {
                return (bool)base.GetValue(IsHoveredProperty);
            }

            set
            {
                base.SetValue(IsHoveredProperty, value);
            }
        }

        public bool IsPressed
        {
            get
            {
                return (bool)base.GetValue(IsPressedProperty);
            }

            set
            {
                base.SetValue(IsPressedProperty, value);
            }
        }

        public bool IsClickable
        {
            get
            {
                return (bool)base.GetValue(IsClickableProperty);
            }

            set
            {
                base.SetValue(IsClickableProperty, value);
            }
        }

        public ListSortDirection? SortDirection
        {
            get
            {
                return (ListSortDirection?)base.GetValue(SortDirectionProperty);
            }

            set
            {
                base.SetValue(SortDirectionProperty, value);
            }
        }

        public bool IsSelected
        {
            get
            {
                return (bool)base.GetValue(IsSelectedProperty);
            }

            set
            {
                base.SetValue(IsSelectedProperty, value);
            }
        }

        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(OrientationProperty);
            }

            set
            {
                base.SetValue(OrientationProperty, value);
            }
        }

        private bool UsingBorderImplementation
        {
            get
            {
                if (base.Background == null)
                {
                    return base.BorderBrush != null;
                }

                return true;
            }
        }

        public Brush SeparatorBrush
        {
            get
            {
                return (Brush)base.GetValue(SeparatorBrushProperty);
            }

            set
            {
                base.SetValue(SeparatorBrushProperty, value);
            }
        }

        public Visibility SeparatorVisibility
        {
            get
            {
                return (Visibility)base.GetValue(SeparatorVisibilityProperty);
            }

            set
            {
                base.SetValue(SeparatorVisibilityProperty, value);
            }
        }

        private Thickness DefaultPadding
        {
            get
            {
                Thickness result = new Thickness(3.0);
                Thickness? themeDefaultPadding = ThemeDefaultPadding;
                if (!themeDefaultPadding.HasValue)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        result.Right = 15.0;
                    }
                }
                else
                {
                    result = themeDefaultPadding.Value;
                }

                if (IsPressed && IsClickable)
                {
                    result.Left += 1.0;
                    result.Top += 1.0;
                    result.Right -= 1.0;
                    result.Bottom -= 1.0;
                }

                return result;
            }
        }

        private Thickness? ThemeDefaultPadding
        {
            get
            {
                if (Orientation == Orientation.Vertical)
                {
                    return new Thickness(5.0, 4.0, 5.0, 4.0);
                }
                return null;
            }
        }

        static DataGridHeaderBorder()
        {
            IsHoveredProperty = DependencyProperty.Register("IsHovered", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
            IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
            IsClickableProperty = DependencyProperty.Register("IsClickable", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
            SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsRender));
            SeparatorBrushProperty = DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(null));
            SeparatorVisibilityProperty = DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(Visibility.Visible));
            _cacheAccess = new object();
            UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridHeaderBorder), new FrameworkPropertyMetadata(true));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (UsingBorderImplementation)
            {
                return base.MeasureOverride(constraint);
            }

            UIElement child = Child;
            if (child != null)
            {
                Thickness thickness = base.Padding;
                if (thickness.Equals(default(Thickness)))
                {
                    thickness = DefaultPadding;
                }

                double num = constraint.Width;
                double num2 = constraint.Height;
                if (!double.IsInfinity(num))
                {
                    num = Math.Max(0.0, num - thickness.Left - thickness.Right);
                }

                if (!double.IsInfinity(num2))
                {
                    num2 = Math.Max(0.0, num2 - thickness.Top - thickness.Bottom);
                }

                child.Measure(new Size(num, num2));
                Size desiredSize = child.DesiredSize;
                return new Size(desiredSize.Width + thickness.Left + thickness.Right, desiredSize.Height + thickness.Top + thickness.Bottom);
            }

            return default(Size);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (UsingBorderImplementation)
            {
                return base.ArrangeOverride(arrangeSize);
            }

            UIElement child = Child;
            if (child != null)
            {
                Thickness thickness = base.Padding;
                if (thickness.Equals(default(Thickness)))
                {
                    thickness = DefaultPadding;
                }
                double width = Math.Max(0.0, arrangeSize.Width - thickness.Left - thickness.Right);
                double height = Math.Max(0.0, arrangeSize.Height - thickness.Top - thickness.Bottom);
                child.Arrange(new Rect(thickness.Left, thickness.Top, width, height));
            }

            return arrangeSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (UsingBorderImplementation)
            {
                base.OnRender(dc);
            }
            else
            {
                RenderTheme(dc);
            }
        }

        private static double Max0(double d)
        {
            return Math.Max(0.0, d);
        }

        private static void EnsureCache(int size)
        {
            if (_freezableCache == null)
            {
                lock (_cacheAccess)
                {
                    if (_freezableCache == null)
                    {
                        _freezableCache = new List<Freezable>(size);
                        for (int i = 0; i < size; i++)
                        {
                            _freezableCache.Add(null);
                        }
                    }
                }
            }
        }

        private static void ReleaseCache()
        {
            if (_freezableCache != null)
            {
                lock (_cacheAccess)
                {
                    _freezableCache = null;
                }
            }
        }

        private static Freezable GetCachedFreezable(int index)
        {
            lock (_cacheAccess)
            {
                return _freezableCache[index];
            }
        }

        private static void CacheFreezable(Freezable freezable, int index)
        {
            lock (_cacheAccess)
            {
                if (_freezableCache[index] != null)
                {
                    _freezableCache[index] = freezable;
                }
            }
        }

        private void RenderTheme(DrawingContext dc)
        {
            Size renderSize = base.RenderSize;
            bool flag = Orientation == Orientation.Horizontal;
            bool flag2 = IsClickable && base.IsEnabled;
            bool flag3 = flag2 && IsHovered;
            bool flag4 = flag2 && IsPressed;
            ListSortDirection? sortDirection = SortDirection;
            bool hasValue = sortDirection.HasValue;
            bool isSelected = IsSelected;
            bool flag5 = !flag3 && !flag4 && !hasValue && !isSelected;
            EnsureCache(19);
            if (flag)
            {
                Matrix trans = default(Matrix);
                trans.RotateAt(-90.0, 0.0, 0.0);
                Matrix trans2 = default(Matrix);
                trans2.Translate(0.0, renderSize.Height);
                MatrixTransform matrixTransform = new MatrixTransform(trans * trans2);
                matrixTransform.Freeze();
                dc.PushTransform(matrixTransform);
                double width = renderSize.Width;
                renderSize.Width = renderSize.Height;
                renderSize.Height = width;
            }

            if (flag5)
            {
                LinearGradientBrush linearGradientBrush = (LinearGradientBrush)GetCachedFreezable(0);
                if (linearGradientBrush == null)
                {
                    linearGradientBrush = new LinearGradientBrush
                    {
                        StartPoint = default(Point),
                        EndPoint = new Point(0.0, 1.0)
                    };
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0.0));
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0.4));
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 252, 252, 253), 0.4));
                    linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 251, 252, 252), 1.0));
                    linearGradientBrush.Freeze();
                    CacheFreezable(linearGradientBrush, 0);
                }

                dc.DrawRectangle(linearGradientBrush, null, new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
            }

            AeroFreezables aeroFreezables = AeroFreezables.NormalBackground;
            if (flag4)
            {
                aeroFreezables = AeroFreezables.PressedBackground;
            }
            else if (flag3)
            {
                aeroFreezables = AeroFreezables.HoveredBackground;
            }
            else if (hasValue | isSelected)
            {
                aeroFreezables = AeroFreezables.SortedBackground;
            }

            LinearGradientBrush linearGradientBrush2 = (LinearGradientBrush)GetCachedFreezable((int)aeroFreezables);
            if (linearGradientBrush2 == null)
            {
                linearGradientBrush2 = new LinearGradientBrush
                {
                    StartPoint = default(Point),
                    EndPoint = new Point(0.0, 1.0)
                };
                switch (aeroFreezables)
                {
                    case AeroFreezables.NormalBackground:
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0.0));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 247, 248, 250), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 241, 242, 244), 1.0));
                        break;
                    case AeroFreezables.PressedBackground:
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 188, 228, 249), 0.0));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 188, 228, 249), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 141, 214, 247), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 138, 209, 245), 1.0));
                        break;
                    case AeroFreezables.HoveredBackground:
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 227, 247, byte.MaxValue), 0.0));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 227, 247, byte.MaxValue), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 189, 237, byte.MaxValue), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 183, 231, 251), 1.0));
                        break;
                    case AeroFreezables.SortedBackground:
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 242, 249, 252), 0.0));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 242, 249, 252), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 225, 241, 249), 0.4));
                        linearGradientBrush2.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 216, 236, 246), 1.0));
                        break;
                }

                linearGradientBrush2.Freeze();
                CacheFreezable(linearGradientBrush2, (int)aeroFreezables);
            }

            dc.DrawRectangle(linearGradientBrush2, null, new Rect(0.0, 0.0, renderSize.Width, renderSize.Height));
            if (renderSize.Width >= 2.0)
            {
                AeroFreezables aeroFreezables2 = AeroFreezables.NormalSides;
                if (flag4)
                {
                    aeroFreezables2 = AeroFreezables.PressedSides;
                }
                else if (flag3)
                {
                    aeroFreezables2 = AeroFreezables.HoveredSides;
                }
                else if (hasValue | isSelected)
                {
                    aeroFreezables2 = AeroFreezables.SortedSides;
                }

                if (SeparatorVisibility == Visibility.Visible)
                {
                    Brush brush;
                    if (SeparatorBrush != null)
                    {
                        brush = SeparatorBrush;
                    }
                    else
                    {
                        brush = (Brush)GetCachedFreezable((int)aeroFreezables2);
                        if (brush == null)
                        {
                            LinearGradientBrush linearGradientBrush3 = null;
                            if (aeroFreezables2 != AeroFreezables.SortedSides)
                            {
                                linearGradientBrush3 = new LinearGradientBrush
                                {
                                    StartPoint = default(Point),
                                    EndPoint = new Point(0.0, 1.0)
                                };
                                brush = linearGradientBrush3;
                            }

                            switch (aeroFreezables2)
                            {
                                case AeroFreezables.NormalSides:
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 242, 242, 242), 0.0));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 239, 239, 239), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 231, 232, 234), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 222, 223, 225), 1.0));
                                    break;
                                case AeroFreezables.PressedSides:
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 122, 158, 177), 0.0));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 122, 158, 177), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 80, 145, 175), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 77, 141, 173), 1.0));
                                    break;
                                case AeroFreezables.HoveredSides:
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 136, 203, 235), 0.0));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 136, 203, 235), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 105, 187, 227), 0.4));
                                    linearGradientBrush3.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 105, 187, 227), 1.0));
                                    break;
                                case AeroFreezables.SortedSides:
                                    brush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 150, 217, 249));
                                    break;
                            }
                            brush.Freeze();
                            CacheFreezable(brush, (int)aeroFreezables2);
                        }
                    }
                    dc.DrawRectangle(brush, null, new Rect(0.0, 0.0, 1.0, Max0(renderSize.Height - 0.95)));
                    dc.DrawRectangle(brush, null, new Rect(renderSize.Width - 1.0, 0.0, 1.0, Max0(renderSize.Height - 0.95)));
                }
            }

            if (flag4 && renderSize.Width >= 4.0 && renderSize.Height >= 4.0)
            {
                LinearGradientBrush linearGradientBrush4 = (LinearGradientBrush)GetCachedFreezable(5);
                if (linearGradientBrush4 == null)
                {
                    linearGradientBrush4 = new LinearGradientBrush
                    {
                        StartPoint = default(Point),
                        EndPoint = new Point(0.0, 1.0)
                    };
                    linearGradientBrush4.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 134, 163, 178), 0.0));
                    linearGradientBrush4.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 134, 163, 178), 0.1));
                    linearGradientBrush4.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 170, 206, 225), 0.9));
                    linearGradientBrush4.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 170, 206, 225), 1.0));
                    linearGradientBrush4.Freeze();
                    CacheFreezable(linearGradientBrush4, 5);
                }

                dc.DrawRectangle(linearGradientBrush4, null, new Rect(0.0, 0.0, renderSize.Width, 2.0));
                LinearGradientBrush linearGradientBrush5 = (LinearGradientBrush)GetCachedFreezable(10);
                if (linearGradientBrush5 == null)
                {
                    linearGradientBrush5 = new LinearGradientBrush
                    {
                        StartPoint = default(Point),
                        EndPoint = new Point(0.0, 1.0)
                    };
                    linearGradientBrush5.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 162, 203, 224), 0.0));
                    linearGradientBrush5.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 162, 203, 224), 0.4));
                    linearGradientBrush5.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 114, 188, 223), 0.4));
                    linearGradientBrush5.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 110, 184, 220), 1.0));
                    linearGradientBrush5.Freeze();
                    CacheFreezable(linearGradientBrush5, 10);
                }

                dc.DrawRectangle(linearGradientBrush5, null, new Rect(1.0, 0.0, 1.0, renderSize.Height - 0.95));
                dc.DrawRectangle(linearGradientBrush5, null, new Rect(renderSize.Width - 2.0, 0.0, 1.0, renderSize.Height - 0.95));
            }

            if (renderSize.Height >= 2.0)
            {
                AeroFreezables aeroFreezables3 = AeroFreezables.NormalBottom;
                if (flag4)
                {
                    aeroFreezables3 = AeroFreezables.PressedOrHoveredBottom;
                }
                else if (flag3)
                {
                    aeroFreezables3 = AeroFreezables.PressedOrHoveredBottom;
                }
                else if (hasValue | isSelected)
                {
                    aeroFreezables3 = AeroFreezables.SortedBottom;
                }

                SolidColorBrush solidColorBrush = (SolidColorBrush)GetCachedFreezable((int)aeroFreezables3);
                if (solidColorBrush == null)
                {
                    switch (aeroFreezables3)
                    {
                        case AeroFreezables.NormalBottom:
                            solidColorBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 213, 213, 213));
                            break;
                        case AeroFreezables.PressedOrHoveredBottom:
                            solidColorBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 147, 201, 227));
                            break;
                        case AeroFreezables.SortedBottom:
                            solidColorBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 150, 217, 249));
                            break;
                    }

                    solidColorBrush.Freeze();
                    CacheFreezable(solidColorBrush, (int)aeroFreezables3);
                }

                dc.DrawRectangle(solidColorBrush, null, new Rect(0.0, renderSize.Height - 1.0, renderSize.Width, 1.0));
            }

            if (hasValue && renderSize.Width > 14.0 && renderSize.Height > 10.0)
            {
                TranslateTransform translateTransform = new TranslateTransform((renderSize.Width - 8.0) * 0.5, 1.0);
                translateTransform.Freeze();
                dc.PushTransform(translateTransform);
                bool flag6 = sortDirection == ListSortDirection.Ascending;
                PathGeometry pathGeometry = (PathGeometry)GetCachedFreezable(flag6 ? 17 : 18);
                if (pathGeometry == null)
                {
                    pathGeometry = new PathGeometry();
                    PathFigure pathFigure = new PathFigure();
                    if (flag6)
                    {
                        pathFigure.StartPoint = new Point(0.0, 4.0);
                        LineSegment lineSegment = new LineSegment(new Point(4.0, 0.0), false);
                        lineSegment.Freeze();
                        pathFigure.Segments.Add(lineSegment);
                        lineSegment = new LineSegment(new Point(8.0, 4.0), false);
                        lineSegment.Freeze();
                        pathFigure.Segments.Add(lineSegment);
                    }
                    else
                    {
                        pathFigure.StartPoint = new Point(0.0, 0.0);
                        LineSegment lineSegment2 = new LineSegment(new Point(8.0, 0.0), false);
                        lineSegment2.Freeze();
                        pathFigure.Segments.Add(lineSegment2);
                        lineSegment2 = new LineSegment(new Point(4.0, 4.0), false);
                        lineSegment2.Freeze();
                        pathFigure.Segments.Add(lineSegment2);
                    }
                    pathFigure.IsClosed = true;
                    pathFigure.Freeze();
                    pathGeometry.Figures.Add(pathFigure);
                    pathGeometry.Freeze();
                    CacheFreezable(pathGeometry, flag6 ? 17 : 18);
                }

                LinearGradientBrush linearGradientBrush6 = (LinearGradientBrush)GetCachedFreezable(14);
                if (linearGradientBrush6 == null)
                {
                    linearGradientBrush6 = new LinearGradientBrush
                    {
                        StartPoint = default(Point),
                        EndPoint = new Point(1.0, 1.0)
                    };
                    linearGradientBrush6.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 60, 94, 114), 0.0));
                    linearGradientBrush6.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 60, 94, 114), 0.1));
                    linearGradientBrush6.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 195, 228, 245), 1.0));
                    linearGradientBrush6.Freeze();
                    CacheFreezable(linearGradientBrush6, 14);
                }

                dc.DrawGeometry(linearGradientBrush6, null, pathGeometry);
                LinearGradientBrush linearGradientBrush7 = (LinearGradientBrush)GetCachedFreezable(15);
                if (linearGradientBrush7 == null)
                {
                    linearGradientBrush7 = new LinearGradientBrush
                    {
                        StartPoint = default(Point),
                        EndPoint = new Point(1.0, 1.0)
                    };
                    linearGradientBrush7.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 97, 150, 182), 0.0));
                    linearGradientBrush7.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 97, 150, 182), 0.1));
                    linearGradientBrush7.GradientStops.Add(new GradientStop(Color.FromArgb(byte.MaxValue, 202, 230, 245), 1.0));
                    linearGradientBrush7.Freeze();
                    CacheFreezable(linearGradientBrush7, 15);
                }

                ScaleTransform scaleTransform = (ScaleTransform)GetCachedFreezable(16);
                if (scaleTransform == null)
                {
                    scaleTransform = new ScaleTransform(0.75, 0.75, 3.5, 4.0);
                    scaleTransform.Freeze();
                    CacheFreezable(scaleTransform, 16);
                }

                dc.PushTransform(scaleTransform);
                dc.DrawGeometry(linearGradientBrush7, null, pathGeometry);
                dc.Pop();
                dc.Pop();
            }
            if (flag)
            {
                dc.Pop();
            }
        }
    }
}
