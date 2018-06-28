using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Kino.Toolkit.Wpf
{
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = IdleStateName)]
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = BusyStateName)]
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = CompletedStateName)]
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = FaultedStateName)]
    [ContentProperty(nameof(IdleContent))]
    public class KinoStateIndicator : Control
    {
        private const string ProgressStatesGroupName = "ProgressStates";
        private const string IdleStateName = "Idle";
        private const string BusyStateName = "Busy";
        private const string CompletedStateName = "Completed";
        private const string FaultedStateName = "Faulted";


        public KinoStateIndicator()
        {
            this.DefaultStyleKey = typeof(KinoStateIndicator);
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
            DependencyProperty.Register("State", typeof(ProgressState), typeof(KinoStateIndicator), new PropertyMetadata(ProgressState.Idle, OnStateChanged));

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as KinoStateIndicator;
            ProgressState oldValue = (ProgressState)args.OldValue;
            ProgressState newValue = (ProgressState)args.NewValue;
            if (oldValue != newValue)
                target.OnStateChanged(oldValue, newValue);
        }

        /// <summary>
        /// 获取或设置IdleContent的值
        /// </summary>  
        public object IdleContent
        {
            get => (object)GetValue(IdleContentProperty);
            set => SetValue(IdleContentProperty, value);
        }

        /// <summary>
        /// 标识 IdleContent 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IdleContentProperty =
            DependencyProperty.Register(nameof(IdleContent), typeof(object), typeof(KinoStateIndicator), new PropertyMetadata(null, OnIdleContentChanged));

        private static void OnIdleContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (object)args.OldValue;
            var newValue = (object)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoStateIndicator;
            target?.OnIdleContentChanged(oldValue, newValue);
        }

        /// <summary>
        /// IdleContent 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">IdleContent 属性的旧值。</param>
        /// <param name="newValue">IdleContent 属性的新值。</param>
        protected virtual void OnIdleContentChanged(object oldValue, object newValue)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualStates(false);
        }

        protected virtual void OnStateChanged(ProgressState oldValue, ProgressState newValue)
        {
            UpdateVisualStates(true);
        }

        private void UpdateVisualStates(bool useTransitions)
        {
            string progressState;
            switch (State)
            {
                case ProgressState.Idle:
                    progressState = IdleStateName;
                    break;
                case ProgressState.Busy:
                    progressState = BusyStateName;
                    break;
                case ProgressState.Completed:
                    progressState = CompletedStateName;
                    break;
                case ProgressState.Faulted:
                    progressState = FaultedStateName;
                    break;
                default:
                    progressState = IdleStateName;
                    break;
            }
            VisualStateManager.GoToState(this, progressState, useTransitions);
        }
    }
}
