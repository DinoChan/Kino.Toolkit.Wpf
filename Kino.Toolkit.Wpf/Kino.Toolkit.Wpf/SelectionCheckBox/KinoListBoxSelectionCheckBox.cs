using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoListBoxSelectionCheckBox : SelectionCheckBox
    {
        /// <summary>
        /// 标识 RelativeListBox 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeListBoxProperty =
            DependencyProperty.Register("RelativeListBox", typeof(ListBox), typeof(KinoListBoxSelectionCheckBox), new PropertyMetadata(null, OnRelativeListBoxChanged));

        /// <summary>
        /// 获取或设置RelativeListBox的值
        /// </summary>
        public ListBox RelativeListBox
        {
            get { return (ListBox)GetValue(RelativeListBoxProperty); }
            set { SetValue(RelativeListBoxProperty, value); }
        }

        protected override IList SelectedItems
        {
            get
            {
                if (RelativeListBox == null)
                {
                    return new List<object>();
                }
                else
                {
                    return RelativeListBox.SelectedItems;
                }
            }
        }

        protected virtual void OnRelativeListBoxChanged(ListBox oldValue, ListBox newValue)
        {
            Selector = newValue;
        }

        protected override void SelectAll()
        {
            if (RelativeListBox != null && RelativeListBox.SelectionMode != SelectionMode.Single)
            {
                RelativeListBox.SelectAll();
            }
        }

        protected override void UnselectAll()
        {
            if (RelativeListBox != null && RelativeListBox.SelectionMode != SelectionMode.Single)
            {
                RelativeListBox.UnselectAll();
            }
        }

        private static void OnRelativeListBoxChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as KinoListBoxSelectionCheckBox;
            var oldValue = (ListBox)args.OldValue;
            var newValue = (ListBox)args.NewValue;
            if (oldValue != newValue)
            {
                target.OnRelativeListBoxChanged(oldValue, newValue);
            }
        }
    }
}
