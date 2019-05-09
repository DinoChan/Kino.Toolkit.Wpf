using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    [TemplateVisualState(GroupName = BusyStatesName, Name = BusyStateName)]
    [TemplateVisualState(GroupName = BusyStatesName, Name = NormalStateName)]
    public class SkeletonScreen : ContentControl
    {
        /// <summary>
        /// 标识 IsBusy 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(SkeletonScreen), new PropertyMetadata(true, OnIsBusyChanged));

        private const string NormalStateName = "Normal";
        private const string BusyStateName = "Busy";
        private const string BusyStatesName = "BusyStates";

        static SkeletonScreen()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SkeletonScreen), new FrameworkPropertyMetadata(typeof(SkeletonScreen)));
        }

        /// <summary>
        /// 获取或设置IsBusy的值
        /// </summary>
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        /// <summary>
        /// IsBusy 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IsBusy 属性的旧值。</param>
        /// <param name="newValue">IsBusy 属性的新值。</param>
        protected virtual void OnIsBusyChanged(bool oldValue, bool newValue)
        {
            UpdateVisualState();
        }

        private static void OnIsBusyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SkeletonScreen;
            target?.OnIsBusyChanged(oldValue, newValue);
        }

        private void UpdateVisualState(bool useTransitions = true)
        {
            VisualStateManager.GoToState(this, IsBusy ? BusyStateName : NormalStateName, useTransitions);
        }
    }
}
