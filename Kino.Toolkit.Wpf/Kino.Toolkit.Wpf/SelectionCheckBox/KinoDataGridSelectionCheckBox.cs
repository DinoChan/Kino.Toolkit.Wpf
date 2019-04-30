using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoDataGridSelectionCheckBox : SelectionCheckBox
    {
        /// <summary>
        /// 标识 RelativeDataGrid 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RelativeDataGridProperty =
            DependencyProperty.Register("RelativeDataGrid", typeof(DataGrid), typeof(KinoDataGridSelectionCheckBox), new PropertyMetadata(null, OnRelativeDataGridChanged));

        /// <summary>
        /// 获取或设置RelativeDataGrid的值
        /// </summary>
        public DataGrid RelativeDataGrid
        {
            get { return (DataGrid)GetValue(RelativeDataGridProperty); }
            set { SetValue(RelativeDataGridProperty, value); }
        }

        protected override IList SelectedItems
        {
            get
            {
                if (RelativeDataGrid == null)
                {
                    return new List<object>();
                }
                else
                {
                    return RelativeDataGrid.SelectedItems;
                }
            }
        }

        protected virtual void OnRelativeDataGridChanged(DataGrid oldValue, DataGrid newValue)
        {
            Selector = newValue;
        }

        protected override void SelectAll()
        {
            if (RelativeDataGrid != null)
            {
                RelativeDataGrid.SelectAll();
            }
        }

        protected override void UnselectAll()
        {
            if (RelativeDataGrid != null)
            {
                RelativeDataGrid.UnselectAll();
            }
        }

        private static void OnRelativeDataGridChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            KinoDataGridSelectionCheckBox target = obj as KinoDataGridSelectionCheckBox;
            DataGrid oldValue = (DataGrid)args.OldValue;
            DataGrid newValue = (DataGrid)args.NewValue;
            if (oldValue != newValue)
            {
                target.OnRelativeDataGridChanged(oldValue, newValue);
            }
        }
    }
}