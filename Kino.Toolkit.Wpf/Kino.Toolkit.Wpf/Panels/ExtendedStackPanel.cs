/*
https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Controls/StackPanel.cs


The MIT License (MIT)

Copyright (c) 2014 Steven Kirk

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public partial class StackPanel : Panel
    {

        #region Orientation DependencyProperty

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation",
                typeof(Orientation),
                typeof(StackPanel),
                new PropertyMetadata(Orientation.Vertical, (s, e) => ((StackPanel)s)?.OnOrientationChanged(e))
            );


        private void OnOrientationChanged(DependencyPropertyChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion


        /// <summary>
        /// Gets or sets a uniform distance (in pixels) between stacked items. It is applied in the direction of the StackPanel's Orientation.
        /// </summary>
        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                name: "Spacing",
                propertyType: typeof(double),
                ownerType: typeof(StackPanel),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: 0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure
                ));
    }

    partial class StackPanel
    {
        

        protected override Size MeasureOverride(Size availableSize)
        {
          

            var desiredSize = default(Size);
            var isHorizontal = Orientation == Orientation.Horizontal;
            var slotSize = availableSize;

            if (isHorizontal)
            {
                slotSize.Width = float.PositiveInfinity;
            }
            else
            {
                slotSize.Height = float.PositiveInfinity;
            }

            // Shadow variables for evaluation performance
            var spacing = Spacing;
            var count = Children.Count;

            for (int i = 0; i < count; i++)
            {
                var view = Children[i];

                view.Measure(slotSize);
                var measuredSize = view.DesiredSize;
                var addSpacing = i != count - 1;

                if (isHorizontal)
                {
                    desiredSize.Width += measuredSize.Width;
                    desiredSize.Height = Math.Max(desiredSize.Height, measuredSize.Height);

                    if (addSpacing)
                    {
                        desiredSize.Width += spacing;
                    }
                }
                else
                {
                    desiredSize.Width = Math.Max(desiredSize.Width, measuredSize.Width);
                    desiredSize.Height += measuredSize.Height;

                    if (addSpacing)
                    {
                        desiredSize.Height += spacing;
                    }
                }
            }

         

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {

            var childRectangle = new Rect(arrangeSize);


            var isHorizontal = Orientation == Orientation.Horizontal;
            var previousChildSize = 0.0;


            // Shadow variables for evaluation performance
            var spacing = Spacing;
            var count = Children.Count;

            for (int i = 0; i < count; i++)
            {
                var view = Children[i];
                var desiredChildSize = view.DesiredSize ;
                var addSpacing = i != 0;

                if (isHorizontal)
                {
                    childRectangle.X += previousChildSize;

                    if (addSpacing)
                    {
                        childRectangle.X += spacing;
                    }

                    previousChildSize = desiredChildSize.Width;
                    childRectangle.Width = desiredChildSize.Width;
                    childRectangle.Height = Math.Max(arrangeSize.Height, desiredChildSize.Height);
                }
                else
                {
                    childRectangle.Y += previousChildSize;

                    if (addSpacing)
                    {
                        childRectangle.Y += spacing;
                    }

                    previousChildSize = desiredChildSize.Height;
                    childRectangle.Height = desiredChildSize.Height;
                    childRectangle.Width = Math.Max(arrangeSize.Width, desiredChildSize.Width);
                }

                var adjustedRectangle = childRectangle;
                view.Arrange(adjustedRectangle);
            }

            return arrangeSize;
        }
    }
}
