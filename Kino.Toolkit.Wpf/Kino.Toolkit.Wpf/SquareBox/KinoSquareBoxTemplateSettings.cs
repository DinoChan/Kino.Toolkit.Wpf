using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kino.Toolkit.Wpf
{
    public class KinoSquareBoxTemplateSettings : System.Windows.DependencyObject
    {
        public static readonly DependencyProperty MaxSideLengthProperty =
              DependencyProperty.Register("MaxSideLength", typeof(double), typeof(KinoSquareBoxTemplateSettings), new PropertyMetadata(0D));

        public KinoSquareBoxTemplateSettings(double width)
        {
            MaxSideLength = width;
        }

        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            set { SetValue(MaxSideLengthProperty, value); }
        }
    }
}
