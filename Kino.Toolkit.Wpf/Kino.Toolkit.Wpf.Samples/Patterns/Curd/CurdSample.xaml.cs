using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// CurdSample.xaml 的交互逻辑
    /// </summary>
    public partial class CurdSample
    {
        private AsyncRemoteCollectionView _asyncRemoteCollectionView;
        private TestCurdService _service;

        public CurdSample()
        {
            InitializeComponent();
            _service = new TestCurdService();
            _asyncRemoteCollectionView = new AsyncRemoteCollectionView(LoadAsync, OnAsyncLoadCompleted);
            var sortDescription = new SortDescription("Id", ListSortDirection.Descending);
            _asyncRemoteCollectionView.SortDescriptions.Add(sortDescription);
            DataContext = _asyncRemoteCollectionView;
            Loaded += PagingSample_Loaded;
        }

        private void PagingSample_Loaded(object sender, RoutedEventArgs e)
        {
            _asyncRemoteCollectionView.EntirelyRefresh();
        }

        private async Task<ILoadResult> LoadAsync()
        {
            var sortDescription = _asyncRemoteCollectionView.SortDescriptions.FirstOrDefault();
            var isDesc = sortDescription.Direction == ListSortDirection.Descending;
            Func<TestModel, object> sort = null;
            if (sortDescription.PropertyName == "Id")
                sort = (m) => m.Id;
            else
                sort = (m) => m.Name;

            var result = await _service.ReadModelsAsync(FilterTextBox.Text, _asyncRemoteCollectionView.PageSize, _asyncRemoteCollectionView.PageIndex, sort, isDesc);
            return result;
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            _asyncRemoteCollectionView.EntirelyRefresh();
        }

        private void OnAsyncLoadCompleted(ILoadResult loadResult)
        {

        }

        private async void OnDelete(object sender, RoutedEventArgs e)
        {
            _asyncRemoteCollectionView.IsRefreshing = true;
            try
            {
                var ids = DataElement.SelectedItems.Cast<TestModel>().Select(m => m.Id);
                await _service.Delete(ids);
                _asyncRemoteCollectionView.EntirelyRefresh();
            }
            catch (Exception)
            {
                _asyncRemoteCollectionView.IsRefreshing = false;
                throw;
            }

        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            var window = new TestModelWindow(_service);
            window.ShowDialog();
            if (window.DialogResult == true)
                _asyncRemoteCollectionView.Refresh();
        }
    }
}
