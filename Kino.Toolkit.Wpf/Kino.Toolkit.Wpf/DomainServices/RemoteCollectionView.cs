using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public class RemoteCollectionView : DomainCollectionView
    {
        public RemoteCollectionView(Func<ILoadOperation> load, Action<ILoadOperation> onLoadCompleted) : base(new RemoteCollectionViewLoader(load, onLoadCompleted), new List<Object>())
        {
            PageSize = 50;
            (CollectionViewLoader as RemoteCollectionViewLoader).LoadStarted += OnLoadStarted;
            (CollectionViewLoader as RemoteCollectionViewLoader).RemoteCollectionView = this;
            SetTotalItemCount(0);
        }


        public event EventHandler Refreshing;

        public event EventHandler Refreshed;


        private bool _isRefreshing;

        /// <summary>
        /// 获取或设置 IsRefreshing 的值
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                if (_isRefreshing == value)
                    return;

                _isRefreshing = value;
                RaisePropertyChanged("IsRefreshing");
            }
        }


        private RemoteCollectionViewLoader loader;

        private List<object> _result;

        private readonly Func<ILoadOperation> _load;

        private readonly Action<ILoadOperation> _onLoadCompleted;

        protected override void OnLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsRefreshing = false;
            if (Refreshed != null)
                Refreshed(this, EventArgs.Empty);

            var loader = CollectionViewLoader as RemoteCollectionViewLoader;
            var operation = loader.CurrentOperation as ILoadOperation;
            if (operation.Error != null || operation.IsCanceled)
                return;

            var result = operation.Result.Cast<object>();
            var source = CollectionView.SourceCollection as List<Object>;
            source.Clear();
            foreach (var item in result)
            {
                source.Add(item);
            }
            base.SetTotalItemCount(operation.TotalCount);
            base.OnLoadCompleted(sender, e);
        }

        private void OnLoadStarted(object sender, EventArgs e)
        {
            RaiseRefreshing();
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        public void EntirelyRefresh()
        {
            using (this.DeferRefresh())
            {
                // This will lead us to re-query for the total count
                this.SetTotalItemCount(-1);
                this.MoveToFirstPage();
            }
        }

        public override bool MoveToPreviousPage()
        {
            if (PageIndex <= 0)
                return false;

            if ((CollectionViewLoader as RemoteCollectionViewLoader).IsBusy)
                return false;

            return base.MoveToPreviousPage();
        }

        private void OnLoaderLoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            RaiseRefreshed();
        }

        private void RaiseRefreshing()
        {
            IsRefreshing = true;
            if (Refreshing != null)
                Refreshing(this, EventArgs.Empty);
        }

        private void RaiseRefreshed()
        {
            IsRefreshing = false;
            if (Refreshed != null)
                Refreshed(this, EventArgs.Empty);
        }


    }
}
