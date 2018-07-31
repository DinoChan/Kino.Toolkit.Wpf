using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Kino.Toolkit.Wpf
{
    public class FrameworkElementService
    {

        /// <summary>
        /// 从指定元素获取 IsAutoFocus 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static bool GetIsAutoFocus(DependencyObject obj) => (bool)obj.GetValue(IsAutoFocusProperty);

        /// <summary>
        /// 将 IsAutoFocus 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetIsAutoFocus(DependencyObject obj, bool value) => obj.SetValue(IsAutoFocusProperty, value);

        /// <summary>
        /// 标识 IsAutoFocus 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsAutoFocusProperty =
            DependencyProperty.RegisterAttached("IsAutoFocus", typeof(bool), typeof(FrameworkElementService), new PropertyMetadata(default(bool), OnIsAutoFocusChanged));


        private static void OnIsAutoFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as FrameworkElement;
            if (target != null)
            {
                target.Loaded -= OnTargetLoaded;
                if (newValue)
                    target.Loaded += OnTargetLoaded;
            }
        }

        private static void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            var elemnt = sender as FrameworkElement;
            if (elemnt.Focus())
                return;

            foreach (var item in elemnt.GetLogicalChildren())
            {
                if (item.Focus())
                    return;
            }

            foreach (var item in elemnt.GetVisualDescendants().OfType<FrameworkElement>())
            {
                if (item.Focus())
                    return;
            } 
        }
    }
}
