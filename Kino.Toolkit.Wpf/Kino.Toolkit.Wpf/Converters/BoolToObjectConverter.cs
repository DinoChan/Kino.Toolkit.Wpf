using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Kino.Toolkit.Wpf
{
    public class BoolToObjectConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// 标识 TrueValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TrueValueProperty =
            DependencyProperty.Register(nameof(TrueValue), typeof(object), typeof(BoolToObjectConverter), new PropertyMetadata());

        /// <summary>
        /// 标识 FalseValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FalseValueProperty =
            DependencyProperty.Register(nameof(FalseValue), typeof(object), typeof(BoolToObjectConverter), new PropertyMetadata());

        /// <summary>
        /// 获取或设置TrueValue的值
        /// </summary>
        public object TrueValue
        {
            get => (object)GetValue(TrueValueProperty);
            set => SetValue(TrueValueProperty, value);
        }

        /// <summary>
        /// 获取或设置FalseValue的值
        /// </summary>
        public object FalseValue
        {
            get => (object)GetValue(FalseValueProperty);
            set => SetValue(FalseValueProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value is bool && (bool)value;

            return boolValue ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
