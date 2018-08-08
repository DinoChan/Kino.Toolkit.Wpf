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
    /// DataPagerSample.xaml 的交互逻辑
    /// </summary>
    public partial class DataPagerSample : UserControl
    {
        public DataPagerSample()
        {
            InitializeComponent();
            var list = new List<TestModel>();
            for (int i = 0; i < 99; i++)
            {
                list.Add(new TestModel { Name = "this is " + i });
            }
            var pagedCollectionView = new PagedCollectionView(list);
            pagedCollectionView.PageSize = 10;
            DataContext = pagedCollectionView;
        }

       
    }
}
