using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kino.Toolkit.Wpf
{
    public class TextBox : System.Windows.Controls.TextBox
    {

        /// <summary>
        /// 获取或设置Header的值
        /// </summary>
        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 标识 Header 依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(TextBox), new PropertyMetadata(default(object)));

      
    }
}
