using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public class KinoFormItem : HeaderedContentControl
    {
        /// <summary>
        /// 标识 IsRequired 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(KinoFormItem), new PropertyMetadata(default(bool)));

        public KinoFormItem()
        {
            DefaultStyleKey = typeof(KinoFormItem);
        }

        /// <summary>
        /// 获取或设置Description的值
        /// </summary>
        public object Description
        {
            get => (object)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// 标识 Description 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(KinoFormItem), new PropertyMetadata(default(object)));

        /// <summary>
        /// 获取或设置IsRequired的值
        /// </summary>
        public bool IsRequired
        {
            get => (bool)GetValue(IsRequiredProperty);
            set => SetValue(IsRequiredProperty, value);
        }
    }
}
