using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public class AsyncRemoteCollectionViewLoader : CollectionViewLoader
    {
        private readonly Func<Task<ILoadResult>> _load;

        private readonly Action<ILoadResult> _onLoadCompleted;

        private bool _isBusy;

        private object _currentUserState;

        public AsyncRemoteCollectionViewLoader(Func<Task<ILoadResult>> load, Action<ILoadResult> onLoadCompleted)
        {
            _load = load ?? throw new ArgumentNullException("load");
            _onLoadCompleted = onLoadCompleted;
        }

        public event EventHandler LoadStarted;

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="M:Microsoft.Windows.Data.DomainServices.DomainCollectionViewLoader.Load(System.Object)" /> can be successfully invoked
        /// </summary>
        public override bool CanLoad => !IsBusy;

        /// <summary>
        /// Gets or sets a value that indicates whether the loader is busy
        /// </summary>
        /// <remarks>
        /// Setting this value will also update <see cref="P:Microsoft.Windows.Data.DomainServices.DomainCollectionViewLoader.CanLoad" />.
        /// </remarks>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnCanLoadChanged();
                }
            }
        }

        public AsyncRemoteCollectionView AsyncRemoteCollectionView { get; internal set; }

        internal ILoadResult CurrentResult { get; private set; }

        public override async void Load(object userState)
        {
            _currentUserState = userState;

            if (IsBusy)
            {
                return;
            }

            if (AsyncRemoteCollectionView.PageIndex < 0)
            {
                return;
            }

            IsBusy = true;
            LoadStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                var result = await _load();
                CurrentResult = result;
                _onLoadCompleted?.Invoke(result);
                OnLoadCompleted(new AsyncCompletedEventArgs(result.Error, result.IsCanceled, _currentUserState));
            }
            catch (Exception ex)
            {
                OnLoadCompleted(new AsyncCompletedEventArgs(ex, false, null));
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
