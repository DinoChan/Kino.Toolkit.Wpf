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
    public partial class PagingSample 
    {
        private RemoteCollectionView _remoteCollectionView;
        private AsyncRemoteCollectionView _asyncRemoteCollectionView;

        public PagingSample()
        {
            InitializeComponent();
            _remoteCollectionView = new RemoteCollectionView(Load, OnLoadCompleted);
            FirstDemoRoot.DataContext = _remoteCollectionView;

            _asyncRemoteCollectionView = new AsyncRemoteCollectionView(LoadAsync, OnAsyncLoadCompleted);
            AsyncDemoRoot.DataContext = _asyncRemoteCollectionView;
            Loaded += PagingSample_Loaded;
        }

        private void PagingSample_Loaded(object sender, RoutedEventArgs e)
        {
            _remoteCollectionView.EntirelyRefresh();
            _asyncRemoteCollectionView.EntirelyRefresh();
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


        private async Task<ILoadResult> LoadAsync()
        {
            var service = new TestRemoteService();
            var result = await service.LoadDataAsync(_asyncRemoteCollectionView.PageIndex, _asyncRemoteCollectionView.PageSize);
            return result;
        }

        private void OnAsyncLoadCompleted(ILoadResult loadResult)
        {
            //MessageBox.Show("LoadCompleted");
        }
    }
}
