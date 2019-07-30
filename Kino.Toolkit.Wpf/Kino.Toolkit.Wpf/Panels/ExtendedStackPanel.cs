using System;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public partial class ExtendedStackPanel : Panel
    {

        #region  DependencyProperty

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation",
                typeof(Orientation),
                typeof(ExtendedStackPanel),
                new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure)
            );


        private void OnOrientationChanged(DependencyPropertyChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                name: "Spacing",
                propertyType: typeof(double),
                ownerType: typeof(ExtendedStackPanel),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: 0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure
                ));

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredSize = default(Size);
            var isHorizontal = Orientation == Orientation.Horizontal;

            if (isHorizontal)
                availableSize.Width = float.PositiveInfinity;
            else
                availableSize.Height = float.PositiveInfinity;

            var spacing = Spacing;
            var count = Children.Count;

            for (int i = 0; i < count; i++)
            {
                UIElement view = Children[i];
                view.Measure(availableSize);
                Size measuredSize = view.DesiredSize;
                var addSpacing = i != count - 1;

                if (isHorizontal)
                {
                    desiredSize.Width += measuredSize.Width;
                    desiredSize.Height = Math.Max(desiredSize.Height, measuredSize.Height);

                    if (addSpacing)
                        desiredSize.Width += spacing;
                }
                else
                {
                    desiredSize.Width = Math.Max(desiredSize.Width, measuredSize.Width);
                    desiredSize.Height += measuredSize.Height;

                    if (addSpacing)
                        desiredSize.Height += spacing;
                }
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var childRectangle = new Rect(arrangeSize);
            var isHorizontal = Orientation == Orientation.Horizontal;
            var previousChildSize = 0.0;

            var spacing = Spacing;
            var count = Children.Count;

            for (int i = 0; i < count; i++)
            {
                UIElement view = Children[i];
                Size desiredChildSize = view.DesiredSize;
                var addSpacing = i != 0;

                if (isHorizontal)
                {
                    childRectangle.X += previousChildSize;

                    if (addSpacing)
                        childRectangle.X += spacing;

                    previousChildSize = desiredChildSize.Width;
                    childRectangle.Width = desiredChildSize.Width;
                    childRectangle.Height = Math.Max(arrangeSize.Height, desiredChildSize.Height);
                }
                else
                {
                    childRectangle.Y += previousChildSize;

                    if (addSpacing)
                        childRectangle.Y += spacing;

                    previousChildSize = desiredChildSize.Height;
                    childRectangle.Height = desiredChildSize.Height;
                    childRectangle.Width = Math.Max(arrangeSize.Width, desiredChildSize.Width);
                }

                Rect adjustedRectangle = childRectangle;
                view.Arrange(adjustedRectangle);
            }

            return arrangeSize;
        }
    }
}
