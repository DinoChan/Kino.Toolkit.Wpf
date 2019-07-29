using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf.Primitives
{
    public class ExpandableContentControl : ContentControl
    {
        /// <summary>
        /// 标识 Pentage 依赖属性。
        /// </summary>
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage),
                                        typeof(double),
                                        typeof(ExpandableContentControl),
                                        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 获取或设置Pentage的值
        /// </summary>
        public double Percentage
        {
            get => (double)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            int count = VisualChildrenCount;
            Size childConstraint = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            UIElement child = (count > 0) ? GetVisualChild(0) as UIElement : null;
            var result = new Size();
            if (child != null)
            {
                child.Measure(childConstraint);
                result = child.DesiredSize;
            }

            return new Size(result.Width * Percentage, result.Height * Percentage);
        }

        //protected override Size ArrangeOverride(Size arrangeBounds)
        //{
        //    int count = VisualChildrenCount;
        //    UIElement child = (count > 0) ? GetVisualChild(0) as UIElement : null;
        //    if (child != null)
        //    {
        //        Size childArrangeBounds = arrangeBounds;
        //        childArrangeBounds.Width = Math.Max(arrangeBounds.Width, child.DesiredSize.Width);
        //        childArrangeBounds.Height = Math.Max(arrangeBounds.Height, child.DesiredSize.Height);
        //        child.Arrange(new Rect(new Point(0, 0), childArrangeBounds));
        //    }

        //    return arrangeBounds;
        //}

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            if (ClipToBounds)
                return new RectangleGeometry(new Rect(RenderSize));
            else
                return null;
        }
    }
}
