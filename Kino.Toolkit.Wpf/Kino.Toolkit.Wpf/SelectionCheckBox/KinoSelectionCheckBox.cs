using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Kino.Toolkit.Wpf.Primitives;

namespace Kino.Toolkit.Wpf.Primitives
{
    public class SelectionCheckBox : CheckBox
    {
        // Using a DependencyProperty as the backing store for Selector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectorProperty =
            DependencyProperty.Register("Selector", typeof(IMultiSelector), typeof(SelectionCheckBox), new PropertyMetadata(null, OnSelectorPropertyChanged));

        private bool _isUpdatingIsChecked;

        public IMultiSelector Selector
        {
            get { return (IMultiSelector)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }

        protected IList SelectedItems
        {
            get
            {
                if (Selector == null)
                    return new List<object>();
                else
                    return Selector.SelectedItems;
            }
        }

        protected virtual void OnSelectorChanged(IMultiSelector oldValue, IMultiSelector newValue)
        {
            if (oldValue != null)
            {
                oldValue.SelectionChanged -= OnSelectorSelectionChanged;
                (oldValue.Items as INotifyCollectionChanged).CollectionChanged -= OnItemsChanged;
            }

            if (newValue != null)
            {
                newValue.SelectionChanged += OnSelectorSelectionChanged;
                (newValue.Items as INotifyCollectionChanged).CollectionChanged += OnItemsChanged;
            }

            IsEnabled = Selector != null && Selector.Items.Count > 0;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (_isUpdatingIsChecked || Selector == null)
                return;

            Selector.SelectAll();
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            if (_isUpdatingIsChecked || Selector == null)
                return;

            Selector.UnselectAll();
        }

        private static void OnSelectorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as SelectionCheckBox;
            target.OnSelectorChanged(e.OldValue as IMultiSelector, e.NewValue as IMultiSelector);
        }


        private void OnSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Selector == null)
                return;

            _isUpdatingIsChecked = true;
            try
            {
                if (SelectedItems.Count == 0)
                    IsChecked = false;
                else if (SelectedItems.Count == Selector.Items.Count)
                    IsChecked = true;
                else
                    IsChecked = null;
            }
            finally
            {
                _isUpdatingIsChecked = false;
            }
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Selector == null)
            {
                return;
            }

            IsEnabled = Selector.Items.Count > 0;
        }
    }
}
