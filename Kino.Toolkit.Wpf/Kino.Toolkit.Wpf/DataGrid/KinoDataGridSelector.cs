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
    public class KinoDataGridSelector : CheckBox
    {
        private bool _isUpdatingIsChecked;

        public KinoDataGridSelector()
        {
        }




        /// <summary>
        /// 获取或设置RelativeDataGrid的值
        /// </summary>  
        public DataGrid RelativeDataGrid
        {
            get { return (DataGrid)GetValue(RelativeDataGridProperty); }
            set { SetValue(RelativeDataGridProperty, value); }
        }

        /// <summary>
        /// 标识 RelativeDataGrid 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeDataGridProperty =
            DependencyProperty.Register("RelativeDataGrid", typeof(DataGrid), typeof(KinoDataGridSelector), new PropertyMetadata(null, OnRelativeDataGridChanged));

        private static void OnRelativeDataGridChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as KinoDataGridSelector;
            var oldValue = (DataGrid)args.OldValue;
            var newValue = (DataGrid)args.NewValue;
            if (oldValue != newValue)
                target.OnRelativeDataGridChanged(oldValue, newValue);
        }

        protected virtual void OnRelativeDataGridChanged(DataGrid oldValue, DataGrid newValue)
        {
            if (oldValue != null)
            {
                oldValue.SelectionChanged -= OnRelativeDataGridSelectionChanged;
                (oldValue.Items as INotifyCollectionChanged).CollectionChanged -= OnRelativeDataGridItemsChanged;
            }

            if (newValue != null)
            {
                newValue.SelectionChanged += OnRelativeDataGridSelectionChanged;
                (newValue.Items as INotifyCollectionChanged).CollectionChanged += OnRelativeDataGridItemsChanged;
            }

            IsEnabled = RelativeDataGrid != null && RelativeDataGrid.Items.Count > 0;
        }

        private void OnRelativeDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RelativeDataGrid == null)
                return;

            _isUpdatingIsChecked = true;
            try
            {
                if (RelativeDataGrid.SelectedItems.Count == 0)
                    IsChecked = false;
                else if (RelativeDataGrid.SelectedItems.Count == RelativeDataGrid.Items.Count)
                    IsChecked = true;
                else
                    IsChecked = null;
            }
            finally
            {
                _isUpdatingIsChecked = false;
            }
        }

        private void OnRelativeDataGridItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (RelativeDataGrid == null)
                return;

            IsEnabled = RelativeDataGrid.Items.Count > 0;
        }


        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (_isUpdatingIsChecked || RelativeDataGrid == null)
                return;

            RelativeDataGrid.SelectAll();
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            if (_isUpdatingIsChecked || RelativeDataGrid == null)
                return;

            RelativeDataGrid.UnselectAll();
        }


    }
}
