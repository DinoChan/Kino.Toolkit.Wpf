using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    [TemplateVisualState(GroupName = MultiSelectStatesGroupName, Name = MultiSelectDisabledStateName)]
    [TemplateVisualState(GroupName = MultiSelectStatesGroupName, Name = MultiSelectEnabledStateName)]
    public class KinoListBoxItem : ListBoxItem
    {
        private const string MultiSelectEnabledStateName = "MultiSelectEnabled";
        private const string MultiSelectDisabledStateName = "MultiSelectDisabled";
        private const string MultiSelectStatesGroupName = "MultiSelectStates";


        public KinoListBoxItem()
        {
            DefaultStyleKey = typeof(KinoListBoxItem);
        }

        /// <summary>
        /// 获取或设置Owner的值
        /// </summary>
        public KinoListBox Owner
        {
            get => (KinoListBox)GetValue(OwnerProperty);
            set => SetValue(OwnerProperty, value);
        }

        /// <summary>
        /// 标识 Owner 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.Register(nameof(Owner), typeof(KinoListBox), typeof(KinoListBoxItem), new PropertyMetadata(default(KinoListBox), OnOwnerChanged));

        private static void OnOwnerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (KinoListBox)args.OldValue;
            var newValue = (KinoListBox)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoListBoxItem;
            target?.OnOwnerChanged(oldValue, newValue);
        }

        /// <summary>
        /// Owner 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Owner 属性的旧值。</param>
        /// <param name="newValue">Owner 属性的新值。</param>
        protected virtual void OnOwnerChanged(KinoListBox oldValue, KinoListBox newValue)
        {
            if (oldValue != null)
            {
                var descriptor = DependencyPropertyDescriptor.FromProperty(ListBox.SelectionModeProperty, typeof(KinoListBox));
                descriptor.RemoveValueChanged(newValue, OnSelectionModeChanged);

                descriptor = DependencyPropertyDescriptor.FromProperty(KinoListBox.IsMultiSelectCheckBoxEnabledProperty, typeof(KinoListBox));
                descriptor.RemoveValueChanged(newValue, OnIsMultiSelectCheckBoxEnabledChanged);
            }
            if (newValue != null)
            {
                var descriptor = DependencyPropertyDescriptor.FromProperty(ListBox.SelectionModeProperty, typeof(KinoListBox));
                descriptor.AddValueChanged(newValue, OnSelectionModeChanged);

                descriptor = DependencyPropertyDescriptor.FromProperty(KinoListBox.IsMultiSelectCheckBoxEnabledProperty, typeof(KinoListBox));
                descriptor.AddValueChanged(newValue, OnIsMultiSelectCheckBoxEnabledChanged);
            }
        }

        private void OnSelectionModeChanged(object sender, EventArgs args)
        {
            UpdateVisualStates(true);
        }

        private void OnIsMultiSelectCheckBoxEnabledChanged(object sender, EventArgs args)
        {
            UpdateVisualStates(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualStates(false);
        }

        public void UpdateVisualStates(bool useTransitions = true)
        {
            var isMultiSelectEnabled = Owner != null && Owner.SelectionMode != SelectionMode.Single && Owner.IsMultiSelectCheckBoxEnabled;
            VisualStateManager.GoToState(this, isMultiSelectEnabled ? MultiSelectEnabledStateName : MultiSelectDisabledStateName, useTransitions);
        }
    }
}
