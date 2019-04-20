using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Kino.Toolkit.Wpf.Primitives;

namespace Kino.Toolkit.Wpf
{
    public class KinoDataGrid : DataGrid, IMultiSelector
    {
        // Using a DependencyProperty as the backing store for IsMultiSelectCheckBoxEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultiSelectCheckBoxEnabledProperty =
            DependencyProperty.Register(nameof(IsMultiSelectCheckBoxEnabled), typeof(bool), typeof(KinoDataGrid), new PropertyMetadata(true));

        public KinoDataGrid()
        {
            DefaultStyleKey = typeof(KinoDataGrid);
        }

        public bool IsMultiSelectCheckBoxEnabled
        {
            get { return (bool)GetValue(IsMultiSelectCheckBoxEnabledProperty); }
            set { SetValue(IsMultiSelectCheckBoxEnabledProperty, value); }
        }
    }
}
