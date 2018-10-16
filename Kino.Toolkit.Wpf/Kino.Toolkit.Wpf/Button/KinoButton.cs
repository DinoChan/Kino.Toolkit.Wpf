using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoButton : Button
    {
        public KinoButton()
        {
            DefaultStyleKey = typeof(KinoButton);
        }


        /// <summary>
        /// 获取或设置Icon的值
        /// </summary>
        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// 标识 Icon 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(object), typeof(KinoButton), new PropertyMetadata(null, OnIconChanged));

        private static void OnIconChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (object)args.OldValue;
            var newValue = (object)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as KinoButton;
            target?.OnIconChanged(oldValue, newValue);
        }

        /// <summary>
        /// Icon 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Icon 属性的旧值。</param>
        /// <param name="newValue">Icon 属性的新值。</param>
        protected virtual void OnIconChanged(object oldValue, object newValue)
        {
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
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(ProgressState), typeof(KinoButton), new PropertyMetadata(ProgressState.None, OnStateChanged));

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as KinoButton;
            var oldValue = (ProgressState)args.OldValue;
            var newValue = (ProgressState)args.NewValue;
            if (oldValue != newValue)
                target.OnStateChanged(oldValue, newValue);
        }

        protected virtual void OnStateChanged(ProgressState oldValue, ProgressState newValue)
        {
        }
    }
}
