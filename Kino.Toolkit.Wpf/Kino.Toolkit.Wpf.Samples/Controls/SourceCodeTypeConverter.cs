using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kino.Toolkit.Wpf.Samples
{
    public class SourceCodeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           var sourceCodeType= (SourceCodeType)value;
            switch (sourceCodeType)
            {
                case SourceCodeType.Xaml:
                    return "XAML";
                case SourceCodeType.CSharp:
                    return "C#";
                default:
                    break;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
