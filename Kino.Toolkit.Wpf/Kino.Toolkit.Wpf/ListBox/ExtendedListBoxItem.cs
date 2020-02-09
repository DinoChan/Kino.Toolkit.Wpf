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
    public class ExtendedListBoxItem : ListBoxItem
    {
        private const string MultiSelectEnabledStateName = "MultiSelectEnabled";
        private const string MultiSelectDisabledStateName = "MultiSelectDisabled";
        private const string MultiSelectStatesGroupName = "MultiSelectStates";


        public ExtendedListBoxItem()
        {
            DefaultStyleKey = typeof(ExtendedListBoxItem);
        }

        /// <summary>
        /// 获取或设置Owner的值
        /// </summary>
        public ExtendedListBox Owner
        {
            get => (ExtendedListBox)GetValue(OwnerProperty);
            set => SetValue(OwnerProperty, value);
        }

        /// <summary>
        /// 标识 Owner 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.Register(nameof(Owner), typeof(ExtendedListBox), typeof(ExtendedListBoxItem), new PropertyMetadata(default(ExtendedListBox), OnOwnerChanged));

        private static void OnOwnerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (ExtendedListBox)args.OldValue;
            var newValue = (ExtendedListBox)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as ExtendedListBoxItem;
            target?.OnOwnerChanged(oldValue, newValue);
        }

        /// <summary>
        /// Owner 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Owner 属性的旧值。</param>
        /// <param name="newValue">Owner 属性的新值。</param>
        protected virtual void OnOwnerChanged(ExtendedListBox oldValue, ExtendedListBox newValue)
        {
            if (oldValue != null)
            {
                var descriptor = DependencyPropertyDescriptor.FromProperty(ListBox.SelectionModeProperty, typeof(ExtendedListBox));
                descriptor.RemoveValueChanged(newValue, OnSelectionModeChanged);

                descriptor = DependencyPropertyDescriptor.FromProperty(ExtendedListBox.IsMultiSelectCheckBoxEnabledProperty, typeof(ExtendedListBox));
                descriptor.RemoveValueChanged(newValue, OnIsMultiSelectCheckBoxEnabledChanged);
            }
            if (newValue != null)
            {
                var descriptor = DependencyPropertyDescriptor.FromProperty(ListBox.SelectionModeProperty, typeof(ExtendedListBox));
                descriptor.AddValueChanged(newValue, OnSelectionModeChanged);

                descriptor = DependencyPropertyDescriptor.FromProperty(ExtendedListBox.IsMultiSelectCheckBoxEnabledProperty, typeof(ExtendedListBox));
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
