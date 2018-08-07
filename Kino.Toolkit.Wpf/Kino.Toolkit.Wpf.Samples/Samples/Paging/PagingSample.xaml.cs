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
    /// PagingSample.xaml 的交互逻辑
    /// </summary>
    public partial class PagingSample : UserControl
    {
        private RemoteCollectionView _remoteCollectionView;

        public PagingSample()
        {
            InitializeComponent();
            _remoteCollectionView = new RemoteCollectionView(Load, OnLoadCompleted);
            FirstDemoRoot.DataContext = _remoteCollectionView;
            Loaded += PagingSample_Loaded;
        }

        private void PagingSample_Loaded(object sender, RoutedEventArgs e)
        {
            _remoteCollectionView.EntirelyRefresh();
        }

        private ILoadOperation Load()
        {
            var result = new TestRemoteService();
            result.LoadData(_remoteCollectionView.PageIndex, _remoteCollectionView.PageSize);
            return result;
        }

        private void OnLoadCompleted(ILoadOperation loadOperation)
        {
            //MessageBox.Show("LoadCompleted");
        }
    }
}
