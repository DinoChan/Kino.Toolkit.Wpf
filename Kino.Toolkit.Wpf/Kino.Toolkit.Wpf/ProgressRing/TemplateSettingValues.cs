// Thanks: http://briandunnington.github.io/progressring-wp8.html

using System.Windows;

namespace Kino.Toolkit.Wpf
{
    public partial class ProgressRing
    {
        public class TemplateSettingValues : System.Windows.DependencyObject
        {
            // Using a DependencyProperty as the backing store for MaxSideLength.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty MaxSideLengthProperty =
                DependencyProperty.Register("MaxSideLength", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

            // Using a DependencyProperty as the backing store for EllipseDiameter.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty EllipseDiameterProperty =
                DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

            // Using a DependencyProperty as the backing store for EllipseOffset.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty EllipseOffsetProperty =
                DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(TemplateSettingValues), new PropertyMetadata(default(Thickness)));

            public TemplateSettingValues(double width)
            {


                if (width <= 40)
                {
                    EllipseDiameter = (width / 10) + 1;
                }
                else
                {
                    EllipseDiameter = width / 10;
                }
                MaxSideLength = width - EllipseDiameter;
                EllipseOffset = new System.Windows.Thickness(0, EllipseDiameter * 2.5, 0, 0);
            }

            public double MaxSideLength
            {
                get { return (double)GetValue(MaxSideLengthProperty); }
                set { SetValue(MaxSideLengthProperty, value); }
            }

            public double EllipseDiameter
            {
                get { return (double)GetValue(EllipseDiameterProperty); }
                set { SetValue(EllipseDiameterProperty, value); }
            }

            public Thickness EllipseOffset
            {
                get { return (Thickness)GetValue(EllipseOffsetProperty); }
                set { SetValue(EllipseOffsetProperty, value); }
            }
        }
    }
}
