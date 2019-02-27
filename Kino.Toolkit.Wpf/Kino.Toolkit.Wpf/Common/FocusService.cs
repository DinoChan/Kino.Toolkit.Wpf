using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kino.Toolkit.Wpf
{
    public class FocusService
    {
        /// <summary>
        /// 标识 IsAutoFocus 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsAutoFocusProperty =
            DependencyProperty.RegisterAttached("IsAutoFocus", typeof(bool), typeof(FocusService), new PropertyMetadata(default(bool), OnIsAutoFocusChanged));

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

        private static void OnIsAutoFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            if (obj is FrameworkElement target)
            {
                target.Loaded -= OnTargetLoaded;
                if (newValue)
                {
                    target.Loaded += OnTargetLoaded;
                }
            }
        }

        private static void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(element))
                return;

            var request = new TraversalRequest(FocusNavigationDirection.Next);
            element.MoveFocus(request);
        }
    }
}
