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
    public class EmptyObjectToObjectConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// 标识 EmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyValueProperty =
            DependencyProperty.Register(nameof(EmptyValue), typeof(object), typeof(EmptyObjectToObjectConverter), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标识 NotEmptyValue 依赖属性。
        /// </summary>
        public static readonly DependencyProperty NotEmptyValueProperty =
            DependencyProperty.Register(nameof(NotEmptyValue), typeof(object), typeof(EmptyObjectToObjectConverter), new PropertyMetadata(default(object)));

        /// <summary>
        /// 获取或设置EmptyValue的值
        /// </summary>
        public object EmptyValue
        {
            get => (object)GetValue(EmptyValueProperty);
            set => SetValue(EmptyValueProperty, value);
        }

        /// <summary>
        /// 获取或设置NotEmptyValue的值
        /// </summary>
        public object NotEmptyValue
        {
            get => (object)GetValue(NotEmptyValueProperty);
            set => SetValue(NotEmptyValueProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return EmptyValue;
            else
                return NotEmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
