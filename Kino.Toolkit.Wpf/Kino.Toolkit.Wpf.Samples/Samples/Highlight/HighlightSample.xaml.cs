using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// HighlightSample.xaml 的交互逻辑
    /// </summary>
    public partial class HighlightSample
    {
        private CollectionViewSource _viewSource;

        public HighlightSample()
        {
            InitializeComponent();
            _viewSource = new CollectionViewSource { Source = Employee.AllExecutives };
            _viewSource.View.Culture = new System.Globalization.CultureInfo("zh-CN");
            _viewSource.View.Filter = (obj) => (obj as Employee).DisplayName.ToLower().Contains(FilterElement.Text);
            _viewSource.View.SortDescriptions.Add(new SortDescription(nameof(Employee.FirstName), ListSortDirection.Ascending));
            EmployeeElement.ItemsSource = _viewSource.View;
        }

        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewSource != null)
                _viewSource.View.Refresh();
        }
    }
}
