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
    /// DataGridSample.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridSample 
    {
        public DataGridSample()
        {
            InitializeComponent();

            SelectionComboBox.Items.Add(DataGridSelectionMode.Extended);
            SelectionComboBox.Items.Add(DataGridSelectionMode.Single);
            SelectionComboBox.SelectedIndex = 0;

            var list = new List<TestModel>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new TestModel { Name = "this is " + i });
            }
            DataContext = list;
        }
    }
}
