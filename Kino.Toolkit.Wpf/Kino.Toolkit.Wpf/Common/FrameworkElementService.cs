using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public class FrameworkElementService
    {
        /// <summary>
        /// 标识 Resources 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ResourcesProperty =
            DependencyProperty.RegisterAttached("Resources", typeof(ResourceDictionary), typeof(FrameworkElementService), new PropertyMetadata(default(ResourceDictionary), OnResourcesChanged));

        /// <summary>
        /// 从指定元素获取 Resources 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static ResourceDictionary GetResources(DependencyObject obj) => (ResourceDictionary)obj.GetValue(ResourcesProperty);

        /// <summary>
        /// 将 Resources 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetResources(DependencyObject obj, ResourceDictionary value) => obj.SetValue(ResourcesProperty, value);

        private static void OnResourcesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = args.OldValue as ResourceDictionary;
            var newValue = args.NewValue as ResourceDictionary;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as FrameworkElement;
            target.Resources = newValue;
        }
    }
}
