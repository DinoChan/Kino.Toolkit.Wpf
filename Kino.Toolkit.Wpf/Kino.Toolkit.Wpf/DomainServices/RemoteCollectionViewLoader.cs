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

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="M:Microsoft.Windows.Data.DomainServices.DomainCollectionViewLoader.Load(System.Object)" /> can be successfully invoked
        /// </summary>
        public override bool CanLoad
        {
            get
            {
                return !this.IsBusy;
            }
        }


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
                return this._isBusy;
            }
            set
            {
                if (this._isBusy != value)
                {
                    this._isBusy = value;
                    this.OnCanLoadChanged();
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
                return this._currentOperation;
            }

            set
            {
                if (this._currentOperation != value)
                {
                    if (this._currentOperation != null)
                    {
                        if (this._currentOperation.CanCancel)
                        {
                            this._currentOperation.Cancel();
                        }
                    }

                    this._currentOperation = value;

                    if (this._currentOperation != null)
                    {
                        this._currentOperation.Completed += OnLoadCompleted;
                    }
                    if (_currentOperation == null)
                        IsBusy = false;
                }
            }
        }

        public RemoteCollectionView RemoteCollectionView { get; internal set; }

        private void OnLoadCompleted(object sender, EventArgs e)
        {
            IsBusy = false;
            var op = (ILoadOperation)sender;

            if (this._onLoadCompleted != null)
            {
                this._onLoadCompleted(op);
            }

            if (op == this.CurrentOperation)
            {
                this.OnLoadCompleted(new AsyncCompletedEventArgs(op.Error, op.IsCanceled, this._currentUserState));
                this._currentUserState = null;
                this.CurrentOperation = null;
            }
            else
            {
                this.OnLoadCompleted(new AsyncCompletedEventArgs(op.Error, op.IsCanceled, null));
            }
        }

        public RemoteCollectionViewLoader(Func<ILoadOperation> load, Action<ILoadOperation> onLoadCompleted)
        {
            if (load == null)
            {
                throw new ArgumentNullException("load");
            }
            this._load = load;
            this._onLoadCompleted = onLoadCompleted;
        }

        public override void Load(object userState)
        {
            if (!this.CanLoad)
            {
                throw new InvalidOperationException(DomainServicesResources.CannotLoad);
            }

            if (RemoteCollectionView.PageIndex < 0)
                return;

            this._currentUserState = userState;
            IsBusy = true;
            if (LoadStarted != null)
                LoadStarted(this, EventArgs.Empty);

            try
            {
                this.CurrentOperation = this._load();
            }
            catch (Exception)
            {
                IsBusy = false;
                throw;
            }
        }
    }
}