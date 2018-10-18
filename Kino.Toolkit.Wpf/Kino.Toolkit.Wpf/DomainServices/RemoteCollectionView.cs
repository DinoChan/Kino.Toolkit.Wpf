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
        private bool _isRefreshing;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = ".")]
        public RemoteCollectionView(Func<ILoadOperation> load, Action<ILoadOperation> onLoadCompleted)
            : base(new RemoteCollectionViewLoader(load, onLoadCompleted), new List<object>())
        {
            PageSize = 50;
            (CollectionViewLoader as RemoteCollectionViewLoader).LoadStarted += OnLoadStarted;
            (CollectionViewLoader as RemoteCollectionViewLoader).RemoteCollectionView = this;
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

        protected override void OnLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsRefreshing = false;
            Refreshed?.Invoke(this, EventArgs.Empty);

            var loader = CollectionViewLoader as RemoteCollectionViewLoader;
            var operation = loader.CurrentOperation as ILoadOperation;
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

            if ((CollectionViewLoader as RemoteCollectionViewLoader).IsBusy)
            {
                return false;
            }

            return base.MoveToPreviousPage();
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
    }
}
