using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    public class RemoteCollectionViewLoader : CollectionViewLoader
    {
        private readonly Func<ILoadOperation> _load;

        private readonly Action<ILoadOperation> _onLoadCompleted;

        private bool _isBusy;

        public event EventHandler LoadStarted;

        private ILoadOperation _currentOperation;

        private object _currentUserState;

        public RemoteCollectionViewLoader(Func<ILoadOperation> load, Action<ILoadOperation> onLoadCompleted)
        {
            _load = load ?? throw new ArgumentNullException("load");
            _onLoadCompleted = onLoadCompleted;
        }

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

        /// <summary>
        /// Gets or sets the current operation
        /// </summary>
        /// <remarks>
        /// Setting the current operation will cancel all pending operations and subscribe to the
        /// completion of the new operation.
        /// </remarks>
        internal ILoadOperation CurrentOperation
        {
            get
            {
                return _currentOperation;
            }

            set
            {
                if (_currentOperation != value)
                {
                    if (_currentOperation != null)
                    {
                        if (_currentOperation.CanCancel)
                        {
                            _currentOperation.Cancel();
                        }
                    }

                    _currentOperation = value;

                    if (_currentOperation != null)
                    {
                        _currentOperation.Completed += OnLoadCompleted;
                    }

                    if (_currentOperation == null)
                    {
                        IsBusy = false;
                    }
                }
            }
        }

        public RemoteCollectionView RemoteCollectionView { get; internal set; }

        private void OnLoadCompleted(object sender, EventArgs e)
        {
            IsBusy = false;
            var op = (ILoadOperation)sender;

            _onLoadCompleted?.Invoke(op);

            if (op == CurrentOperation)
            {
                OnLoadCompleted(new AsyncCompletedEventArgs(op.Error, op.IsCanceled, _currentUserState));
                _currentUserState = null;
                CurrentOperation = null;
            }
            else
            {
                OnLoadCompleted(new AsyncCompletedEventArgs(op.Error, op.IsCanceled, null));
            }
        }

        public override void Load(object userState)
        {
            _currentUserState = userState;

            if (IsBusy)
            {
                return;
            }

            if (RemoteCollectionView.PageIndex < 0)
            {
                return;
            }

            IsBusy = true;
            LoadStarted?.Invoke(this, EventArgs.Empty);

            try
            {
                CurrentOperation = _load();
            }
            catch (Exception)
            {
                IsBusy = false;
                throw;
            }
        }
    }
}