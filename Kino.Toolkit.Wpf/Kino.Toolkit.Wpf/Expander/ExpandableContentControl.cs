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
        /// 标识 IsExpand 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsExpandProperty =
            DependencyProperty.Register(nameof(IsExpand), typeof(bool), typeof(ExpandableContentControl), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 获取或设置IsExpand的值
        /// </summary>
        public bool IsExpand
        {
            get => (bool)GetValue(IsExpandProperty);
            set => SetValue(IsExpandProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (IsExpand)
                return base.MeasureOverride(constraint);

            int count = VisualChildrenCount;
            Size childConstraint = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            UIElement child = (count > 0) ? GetVisualChild(0) as UIElement : null;
            if (child != null)
                child.Measure(childConstraint);

            return default;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            int count = VisualChildrenCount;
            UIElement child = (count > 0) ? GetVisualChild(0) as UIElement : null;
            if (child != null)
            {
                var childArrangeBounds = new Rect(arrangeBounds);
                arrangeBounds.Width = Math.Max(arrangeBounds.Width, child.DesiredSize.Width);
                arrangeBounds.Height = Math.Max(arrangeBounds.Height, child.DesiredSize.Height);
                child.Arrange(new Rect(new Point(0, 0), arrangeBounds));
            }

            return arrangeBounds;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            if (ClipToBounds)
                return new RectangleGeometry(new Rect(RenderSize));
            else
                return null;
        }
    }
}
