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
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = NoneStateName)]
    [TemplateVisualState(GroupName = ProgressStatesGroupName, Name = OtherStateName)]
    public class StateIndicator : ContentControl
    {
        /// <summary>
        /// 标识 State 依赖属性。
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(ProgressState), typeof(StateIndicator), new PropertyMetadata(ProgressState.None, OnStateChanged));

        private const string ProgressStatesGroupName = "ProgressStates";
        private const string IdleStateName = "Idle";
        private const string BusyStateName = "Busy";
        private const string CompletedStateName = "Completed";
        private const string FaultedStateName = "Faulted";
        private const string NoneStateName = "None";
        private const string OtherStateName = "Other";

        public StateIndicator()
        {
            DefaultStyleKey = typeof(StateIndicator);
        }

        /// <summary>
        /// 获取或设置State的值
        /// </summary>
        public ProgressState State
        {
            get => (ProgressState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
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

        protected virtual void UpdateVisualStates(bool useTransitions)
        {
            string progressState;
            switch (State)
            {
                case ProgressState.None:
                    progressState = NoneStateName;
                    break;
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
                case ProgressState.Other:
                    progressState = OtherStateName;
                    break;
                default:
                    progressState = NoneStateName;
                    break;
            }

            VisualStateManager.GoToState(this, progressState, useTransitions);
        }

        private static void OnStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as StateIndicator;
            ProgressState oldValue = (ProgressState)args.OldValue;
            ProgressState newValue = (ProgressState)args.NewValue;
            if (oldValue != newValue)
            {
                target.OnStateChanged(oldValue, newValue);
            }
        }
    }
}
