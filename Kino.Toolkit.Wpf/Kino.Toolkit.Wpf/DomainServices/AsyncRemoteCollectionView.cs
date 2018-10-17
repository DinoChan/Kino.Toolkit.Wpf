using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public class AsyncRemoteCollectionView : DomainCollectionView
    {
        private bool _isRefreshing;

        public AsyncRemoteCollectionView(Func<Task<ILoadResult>> load, Action<ILoadResult> onLoadCompleted)
            : base(new AsyncRemoteCollectionViewLoader(load, onLoadCompleted), new List<object>())
        {
            PageSize = 50;
            (CollectionViewLoader as AsyncRemoteCollectionViewLoader).LoadStarted += OnLoadStarted;
            (CollectionViewLoader as AsyncRemoteCollectionViewLoader).AsyncRemoteCollectionView = this;
            SetTotalItemCount(0);
        }

        public event EventHandler Refreshing;

        public event EventHandler Refreshed;

        /// <summary>
        /// 获取或设置 IsRefreshing 的值
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }

            set
            {
                if (_isRefreshing == value)
                {
                    return;
                }

                _isRefreshing = value;
                RaisePropertyChanged("IsRefreshing");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        public void EntirelyRefresh()
        {
            using (DeferRefresh())
            {
                // This will lead us to re-query for the total count
                SetTotalItemCount(-1);
                MoveToFirstPage();
            }
        }

        public override bool MoveToPreviousPage()
        {
            if (PageIndex <= 0)
            {
                return false;
            }

            if ((CollectionViewLoader as AsyncRemoteCollectionViewLoader).IsBusy)
            {
                return false;
            }

            return base.MoveToPreviousPage();
        }

        protected override void OnLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsRefreshing = false;
            Refreshed?.Invoke(this, EventArgs.Empty);

            var loader = CollectionViewLoader as AsyncRemoteCollectionViewLoader;
            var operation = loader.CurrentResult;
            if (operation.Error != null || operation.IsCanceled)
            {
                return;
            }

            var result = operation.Result.Cast<object>();
            var source = CollectionView.SourceCollection as List<object>;
            source.Clear();
            foreach (var item in result)
            {
                source.Add(item);
            }

            SetTotalItemCount(operation.TotalCount);
            base.OnLoadCompleted(sender, e);
        }

        private void RaiseRefreshing()
        {
            IsRefreshing = true;
            Refreshing?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseRefreshed()
        {
            IsRefreshing = false;
            Refreshed?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoadStarted(object sender, EventArgs e)
        {
            RaiseRefreshing();
        }
    }
}
