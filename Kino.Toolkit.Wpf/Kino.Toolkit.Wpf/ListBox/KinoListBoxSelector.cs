using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoListBoxSelector : CheckBox
    {
        private bool _isUpdatingIsChecked;

        public KinoListBoxSelector()
        {
        }

        /// <summary>
        /// 获取或设置RelativeListBox的值
        /// </summary>  
        public ListBox RelativeListBox
        {
            get { return (ListBox)GetValue(RelativeListBoxProperty); }
            set { SetValue(RelativeListBoxProperty, value); }
        }

        /// <summary>
        /// 标识 RelativeListBox 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeListBoxProperty =
            DependencyProperty.Register("RelativeListBox", typeof(ListBox), typeof(KinoListBoxSelector), new PropertyMetadata(null, OnRelativeListBoxChanged));

        private static void OnRelativeListBoxChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as KinoListBoxSelector;
            var oldValue = (ListBox)args.OldValue;
            var newValue = (ListBox)args.NewValue;
            if (oldValue != newValue)
                target.OnRelativeListBoxChanged(oldValue, newValue);
        }

        protected virtual void OnRelativeListBoxChanged(ListBox oldValue, ListBox newValue)
        {
            if (oldValue != null)
            {
                oldValue.SelectionChanged -= OnRelativeListBoxSelectionChanged;
                (oldValue.Items as INotifyCollectionChanged).CollectionChanged -= OnRelativeListBoxItemsChanged;
            }

            if (newValue != null)
            {
                newValue.SelectionChanged += OnRelativeListBoxSelectionChanged;
                (newValue.Items as INotifyCollectionChanged).CollectionChanged += OnRelativeListBoxItemsChanged;
            }

            IsEnabled = RelativeListBox != null && RelativeListBox.Items.Count > 0;
        }


        private void OnRelativeListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RelativeListBox == null)
                return;

            _isUpdatingIsChecked = true;
            try
            {
                if (RelativeListBox.SelectedItems.Count == 0)
                    IsChecked = false;
                else if (RelativeListBox.SelectedItems.Count == RelativeListBox.Items.Count)
                    IsChecked = true;
                else
                    IsChecked = null;
            }
            finally
            {
                _isUpdatingIsChecked = false;
            }
        }

        private void OnRelativeListBoxItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (RelativeListBox == null)
                return;

            IsEnabled = RelativeListBox.Items.Count > 0;
        }


        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (_isUpdatingIsChecked || RelativeListBox == null)
                return;
            
            RelativeListBox.SelectAll();
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            if (_isUpdatingIsChecked || RelativeListBox == null)
                return;

            RelativeListBox.UnselectAll();
        }


    }
}
