using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Kino.Toolkit.Wpf
{
    public abstract class SelectionCheckBox : CheckBox
    {
        private bool _isUpdatingIsChecked;

        private Selector _selector;

        /// <summary>
        /// 获取或设置 Selector 的值
        /// </summary>
        protected Selector Selector
        {
            get => _selector;
            set
            {
                if (_selector == value)
                {
                    return;
                }

                var oldValue = _selector;
                _selector = value;
                OnSelectorChanged(oldValue, value);
            }
        }

        protected abstract IList SelectedItems { get; }

        protected virtual void OnSelectorChanged(Selector oldValue, Selector newValue)
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

            IsEnabled = Selector != null && Selector.Items.Count > 0;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (_isUpdatingIsChecked || Selector == null)
            {
                return;
            }

            SelectAll();
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            if (_isUpdatingIsChecked || Selector == null)
            {
                return;
            }

            UnselectAll();
        }

        protected abstract void SelectAll();

        protected abstract void UnselectAll();

        private void OnRelativeListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Selector == null)
            {
                return;
            }

            _isUpdatingIsChecked = true;
            try
            {
                if (SelectedItems.Count == 0)
                {
                    IsChecked = false;
                }
                else if (SelectedItems.Count == Selector.Items.Count)
                {
                    IsChecked = true;
                }
                else
                {
                    IsChecked = null;
                }
            }
            finally
            {
                _isUpdatingIsChecked = false;
            }
        }

        private void OnRelativeListBoxItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Selector == null)
            {
                return;
            }

            IsEnabled = Selector.Items.Count > 0;
        }
    }
}
