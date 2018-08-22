using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoValidationTooltip : Control
    {
        public KinoValidationTooltip()
        {
            DefaultStyleKey = typeof(KinoValidationTooltip);
        }


        /// <summary>
        /// 获取或设置AdornedElementPlaceholder的值
        /// </summary>  
        public AdornedElementPlaceholder AdornedElementPlaceholder
        {
            get => (AdornedElementPlaceholder)GetValue(AdornedElementPlaceholderProperty);
            set => SetValue(AdornedElementPlaceholderProperty, value);
        }

        /// <summary>
        /// 标识 AdornedElementPlaceholder 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AdornedElementPlaceholderProperty =
            DependencyProperty.Register(nameof(AdornedElementPlaceholder), typeof(AdornedElementPlaceholder), typeof(KinoValidationTooltip), new PropertyMetadata(default(AdornedElementPlaceholder), OnAdornedElementPlaceholderChanged));

        private static void OnAdornedElementPlaceholderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (AdornedElementPlaceholder)args.OldValue;
            var newValue = (AdornedElementPlaceholder)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoValidationTooltip;
            target?.OnAdornedElementPlaceholderChanged(oldValue, newValue);
        }

        /// <summary>
        /// AdornedElementPlaceholder 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">AdornedElementPlaceholder 属性的旧值。</param>
        /// <param name="newValue">AdornedElementPlaceholder 属性的新值。</param>
        protected virtual void OnAdornedElementPlaceholderChanged(AdornedElementPlaceholder oldValue, AdornedElementPlaceholder newValue)
        {
            if (newValue != null && newValue.AdornedElement != null)
            {
                newValue.AdornedElement.GotFocus += OnAdornedElementGotFocus;
                newValue.AdornedElement.LostFocus += OnAdornedElementLostFocus;
                newValue.Loaded += OnPlaceholderLoaded;
            }
        }

        private void OnPlaceholderLoaded(object sender, RoutedEventArgs e)
        {
            UpdateVisualState();
        }


        private void OnAdornedElementLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateVisualState();
        }

        private void OnAdornedElementGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            if (AdornedElementPlaceholder != null && AdornedElementPlaceholder.AdornedElement != null)
            {
                VisualStateManager.GoToState(this, AdornedElementPlaceholder.AdornedElement.IsFocused ? VisualStates.StateInvalidFocused : VisualStates.StateInvalidUnfocused, false);
            }
        }
    }
}
