using System;
using System.Collections.Generic;
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
    /// ListBoxSample.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxSample
    {
        public ListBoxSample()
        {
            InitializeComponent();

            SelectionComboBox.Items.Add(SelectionMode.Extended);
            SelectionComboBox.Items.Add(SelectionMode.Multiple);
            SelectionComboBox.Items.Add(SelectionMode.Single);
            SelectionComboBox.SelectedIndex = 0;
            var list = new List<TestModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestModel { Name = "this is " + i });
            }
            DataContext = list;
        }

        private void OnChangeToMultipleSelection(object sender, RoutedEventArgs e)
        {
            SquareSelectionCheckBox.Visibility = Visibility.Visible;
            SquareListBox.SelectionMode = SelectionMode.Multiple;
        }

        private void OnChangeToSingleSelection(object sender, RoutedEventArgs e)
        {
            SquareSelectionCheckBox.Visibility = Visibility.Collapsed;
            SquareListBox.SelectionMode = SelectionMode.Single;
        }
    }
}
