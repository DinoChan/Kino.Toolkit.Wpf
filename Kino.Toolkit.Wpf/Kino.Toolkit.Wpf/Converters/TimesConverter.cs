using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kino.Toolkit.Wpf
{
    public class TimesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double times = 1;
            if (parameter is string parameterString)
            {
                if (double.TryParse(parameterString, out times) == false)
                {
                    return 0;
                }
            }

            if (value is double length)
            {
                return length * times;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
