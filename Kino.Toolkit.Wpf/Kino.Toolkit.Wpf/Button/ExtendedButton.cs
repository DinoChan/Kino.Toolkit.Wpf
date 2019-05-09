using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class ExtendedButton : Button
    {
        /// <summary>
        /// 标识 Icon 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(object), typeof(ExtendedButton), new PropertyMetadata(null, OnIconChanged));

        /// <summary>
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(ProgressState), typeof(ExtendedButton), new PropertyMetadata(ProgressState.None, OnStateChanged));

        public ExtendedButton()
        {
            DefaultStyleKey = typeof(ExtendedButton);
        }

        /// <summary>
        /// 获取或设置Icon的值
        /// </summary>
        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// 获取或设置State的值
        /// </summary>
        public ProgressState State
        {
            get => (ProgressState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// Icon 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Icon 属性的旧值。</param>
        /// <param name="newValue">Icon 属性的新值。</param>
        protected virtual void OnIconChanged(object oldValue, object newValue)
        {
        }

        protected virtual void OnStateChanged(ProgressState oldValue, ProgressState newValue)
        {
        }

        private static void OnIconChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = args.OldValue;
            var newValue = args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as ExtendedButton;
            target?.OnIconChanged(oldValue, newValue);
        }

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as ExtendedButton;
            var oldValue = (ProgressState)args.OldValue;
            var newValue = (ProgressState)args.NewValue;
            if (oldValue != newValue)
                target.OnStateChanged(oldValue, newValue);
        }
    }
}
