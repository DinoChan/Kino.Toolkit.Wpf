using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// Abstract wrapper implementation for collection view interfaces.
    /// </summary>
    /// <remarks>
    /// This class unofficially implements the <see cref="ICollectionView"/>,
    /// <see cref="IEditableCollectionView"/>, <see cref="IPagedCollectionView"/>, and
    /// <see cref="INotifyPropertyChanged"/> interfaces by wrapping an existing collection
    /// view. A derived implementation will need to set a view property and implement the
    /// interface that it corresponds to. For instance, a collection view implementation
    /// should set <see cref="CollectionView"/> and implement <see cref="ICollectionView"/>.
    /// </remarks>
    public abstract class CollectionViewWrapper
    {
        #region Member fields

        private ICollectionView _collectionView;
        private IEditableCollectionView _editableCollectionView;
        private IPagedCollectionView _pagedCollectionView;
        private int _deferCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewWrapper"/>
        /// </summary>
        protected CollectionViewWrapper()
        {
        }

        #endregion

        #region Views

        /// <summary>
        /// Gets or sets the <see cref="ICollectionView"/> implementation to delegate to.
        /// </summary>
        protected ICollectionView CollectionView
        {
            get
            {
                return this._collectionView;
            }

            set
            {
                if (this._collectionView != value)
                {
                    if (this._collectionView != null)
                    {
                        this._collectionView.CollectionChanged -= this.OnCollectionViewCollectionChanged;
                        this._collectionView.CurrentChanged -= this.OnCollectionViewCurrentChanged;
                        this._collectionView.CurrentChanging -= this.OnCollectionViewCurrentChanging;

                        this.RemovePropertyChangedIfNeeded(this._collectionView);
                    }

                    this._collectionView = value;

                    if (this._collectionView != null)
                    {
                        this._collectionView.CollectionChanged += this.OnCollectionViewCollectionChanged;
                        this._collectionView.CurrentChanged += this.OnCollectionViewCurrentChanged;
                        this._collectionView.CurrentChanging += this.OnCollectionViewCurrentChanging;

                        this.AddPropertyChangedIfNeeded(this._collectionView);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="CollectionView"/> property
        /// </summary>
        /// <exception cref="NotSupportedException"> is thrown if the <see cref="CollectionView"/>
        /// property is <c>null</c>
        /// </exception>
        private ICollectionView CollectionViewChecked
        {
            get
            {
                if (this.CollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IcvNotSupported);
                }
                return this.CollectionView;
            }
        }

        /// <summary>
        /// Get or sets the <see cref="IEditableCollectionView"/> implementation to delegate to
        /// </summary>
        protected IEditableCollectionView EditableCollectionView
        {
            get
            {
                return this._editableCollectionView;
            }

            set
            {
                if (this._editableCollectionView != value)
                {
                    if (this._editableCollectionView != null)
                    {
                        this.RemovePropertyChangedIfNeeded(this._editableCollectionView);
                    }

                    this._editableCollectionView = value;

                    if (this._editableCollectionView != null)
                    {
                        this.AddPropertyChangedIfNeeded(this._editableCollectionView);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="EditableCollectionView"/> property
        /// </summary>
        /// <exception cref="NotSupportedException"> is thrown if the <see cref="EditableCollectionView"/>
        /// property is <c>null</c>
        /// </exception>
        private IEditableCollectionView EditableCollectionViewChecked
        {
            get
            {
                if (this.EditableCollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IecvNotSupported);
                }
                return this.EditableCollectionView;
            }
        }

        /// <summary>
        /// Get or sets the <see cref="IPagedCollectionView"/> implementation to delegate to
        /// </summary>
        protected IPagedCollectionView PagedCollectionView
        {
            get
            {
                return this._pagedCollectionView;
            }

            set
            {
                if (this._pagedCollectionView != value)
                {
                    if (this._pagedCollectionView != null)
                    {
                        this._pagedCollectionView.PageChanged -= this.OnPagedCollectionViewPageChanged;
                        this._pagedCollectionView.PageChanging -= this.OnPagedCollectionViewPageChanging;

                        this.RemovePropertyChangedIfNeeded(this._pagedCollectionView);
                    }

                    this._pagedCollectionView = value;

                    if (this._pagedCollectionView != null)
                    {
                        this._pagedCollectionView.PageChanged += this.OnPagedCollectionViewPageChanged;
                        this._pagedCollectionView.PageChanging += this.OnPagedCollectionViewPageChanging;

                        this.AddPropertyChangedIfNeeded(this._pagedCollectionView);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="PagedCollectionView"/> property
        /// </summary>
        /// <exception cref="NotSupportedException"> is thrown if the <see cref="PagedCollectionView"/>
        /// property is <c>null</c>
        /// </exception>
        private IPagedCollectionView PagedCollectionViewChecked
        {
            get
            {
                if (this.PagedCollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IpcvNotSupported);
                }
                return this.PagedCollectionView;
            }
        }

        #endregion

        #region ICollectionView Members

        #region Events

        /// <summary>
        /// Occurs when the items list of the collection has changed or the collection is reset
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs after the current item has been changed
        /// </summary>
        public event EventHandler CurrentChanged;

        /// <summary>
        /// Occurs before the current item changes
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;

        /// <summary>
        /// Handles <see cref="INotifyCollectionChanged.CollectionChanged"/> events raised by the <see cref="CollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="CollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Raises collection changed events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles <see cref="ICollectionView.CurrentChanged"/> events raised by the <see cref="CollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="CollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            this.OnCurrentChanged(e);
        }

        /// <summary>
        /// Raises current changed events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCurrentChanged(EventArgs e)
        {
            EventHandler handler = this.CurrentChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles <see cref="ICollectionView.CurrentChanging"/> events raised by the <see cref="CollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="CollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionViewCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            this.OnCurrentChanging(e);
        }

        /// <summary>
        /// Raises current changing events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            CurrentChangingEventHandler handler = this.CurrentChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether this view supports filtering by way of
        /// the <see cref="Filter"/> property.
        /// </summary>
        public virtual bool CanFilter
        {
            get { return this.CollectionViewChecked.CanFilter; }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping by way of
        /// the <see cref="GroupDescriptions"/> property.
        /// </summary>
        public virtual bool CanGroup
        {
            get { return this.CollectionViewChecked.CanGroup; }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting by way of
        /// the <see cref="SortDescriptions"/> property.
        /// </summary>
        public virtual bool CanSort
        {
            get { return this.CollectionViewChecked.CanSort; }
        }

        /// <summary>
        /// Gets or sets the cultural information for any operations of the view that
        /// may differ by culture, such as sorting.
        /// </summary>
        public virtual CultureInfo Culture
        {
            get { return this.CollectionViewChecked.Culture; }
            set { this.CollectionViewChecked.Culture = value; }
        }

        /// <summary>
        /// Gets the current item in the view.
        /// </summary>
        public virtual object CurrentItem
        {
            get { return this.CollectionViewChecked.CurrentItem; }
        }

        /// <summary>
        ///  Gets the ordinal position of the <see cref="CurrentItem"/> in the view.
        /// </summary>
        public virtual int CurrentPosition
        {
            get { return this.CollectionViewChecked.CurrentPosition; }
        }

        /// <summary>
        /// Gets or sets a callback that is used to determine whether an item is appropriate
        /// for inclusion in the view.
        /// </summary>
        public virtual Predicate<object> Filter
        {
            get { return this.CollectionViewChecked.Filter; }
            set { this.CollectionViewChecked.Filter = value; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GroupDescription"/> objects that
        /// describe how the items in the collection are grouped in the view.
        /// </summary>
        public virtual ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return this.CollectionViewChecked.GroupDescriptions; }
        }

        /// <summary>
        /// Gets the top-level groups.
        /// </summary>
        public virtual ReadOnlyObservableCollection<object> Groups
        {
            get { return this.CollectionViewChecked.Groups; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CurrentItem"/>
        /// of the view is beyond the end of the collection.
        /// </summary>
        public virtual bool IsCurrentAfterLast
        {
            get { return this.CollectionViewChecked.IsCurrentAfterLast; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CurrentItem"/>
        /// of the view is beyond the start of the collection.
        /// </summary>
        public virtual bool IsCurrentBeforeFirst
        {
            get { return this.CollectionViewChecked.IsCurrentBeforeFirst; }
        }

        /// <summary>
        /// Gets a value that indicates whether the view is empty.
        /// </summary>
        public virtual bool IsEmpty
        {
            get { return this.CollectionViewChecked.IsEmpty; }
        }

        /// <summary>
        /// Gets a collection of <see cref="SortDescription"/> instances that
        /// describe how the items in the collection are sorted in the view.
        /// </summary>
        public virtual SortDescriptionCollection SortDescriptions
        {
            get { return this.CollectionViewChecked.SortDescriptions; }
        }

        /// <summary>
        /// Gets the underlying collection.
        /// </summary>
        public virtual IEnumerable SourceCollection
        {
            get { return this.CollectionViewChecked.SourceCollection; }
        }

        /// <summary>
        /// Gets a value that indicates whether a <see cref="DeferRefresh"/> method is pending
        /// </summary>
        protected bool IsRefreshDeferred
        {
            get { return this._deferCount > 0; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indicates whether the specified item belongs to this collection view.
        /// </summary>
        /// <param name="item">The object to check</param>
        /// <returns>
        /// <c>true</c> if the item belongs to this collection view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Contains(object item)
        {
            return this.CollectionViewChecked.Contains(item);
        }

        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay
        /// automatic refresh.
        /// </summary>
        /// <returns>
        /// The typical usage is to create a using scope with an implementation of this
        /// method and then include multiple view-changing calls within the scope. The
        /// implementation should delay automatic refresh until after the using scope
        /// exits.
        /// </returns>
        public virtual IDisposable DeferRefresh()
        {
            IDisposable disposable = this.CollectionViewChecked.DeferRefresh();
            this._deferCount++;
            return new DeferHelper(this.OnDeferHelperDisposed, disposable);
        }

        /// <summary>
        /// Handles disposal of a <see cref="DeferHelper"/> by conditionally calling <see cref="Refresh"/>
        /// </summary>
        private void OnDeferHelperDisposed()
        {
            this._deferCount--;
            if (this._deferCount == 0)
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through
        /// the collection.
        /// </returns>
        public virtual IEnumerator GetEnumerator()
        {
            return this.CollectionViewChecked.GetEnumerator();
        }

        /// <summary>
        /// Sets the specified item in the view as the <see cref="CurrentItem"/>.
        /// </summary>
        /// <param name="item">The item to set as the current item.</param>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentTo(object item)
        {
            return this.CollectionViewChecked.MoveCurrentTo(item);
        }

        /// <summary>
        /// Sets the first item in the view as the <see cref="CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentToFirst()
        {
            return this.CollectionViewChecked.MoveCurrentToFirst();
        }

        /// <summary>
        /// Sets the last item in the view as the <see cref="CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentToLast()
        {
            return this.CollectionViewChecked.MoveCurrentToLast();
        }

        /// <summary>
        /// Sets the item after the <see cref="CurrentItem"/>
        /// in the view as the <see cref="CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentToNext()
        {
            return this.CollectionViewChecked.MoveCurrentToNext();
        }

        /// <summary>
        /// Sets the item at the specified index to be the <see cref="CurrentItem"/>
        /// in the view.
        /// </summary>
        /// <param name="position">The index to set the <see cref="CurrentItem"/> to.</param>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentToPosition(int position)
        {
            return this.CollectionViewChecked.MoveCurrentToPosition(position);
        }

        /// <summary>
        /// Sets the item before the <see cref="CurrentItem"/>
        /// in the view to the <see cref="CurrentItem"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the resulting <see cref="CurrentItem"/> is
        /// an item in the view; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool MoveCurrentToPrevious()
        {
            return this.CollectionViewChecked.MoveCurrentToPrevious();
        }

        /// <summary>
        /// Recreates the view.
        /// </summary>
        public virtual void Refresh()
        {
            this.CollectionViewChecked.Refresh();
        }

        #endregion

        #endregion

        #region IEditableCollectionView Members

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether a new item can be added to the collection.
        /// </summary>
        public virtual bool CanAddNew
        {
            get { return this.EditableCollectionViewChecked.CanAddNew; }
        }

        /// <summary>
        /// Gets a value that indicates whether the collection view can discard pending
        /// changes and restore the original values of an edited object.
        /// </summary>
        public virtual bool CanCancelEdit
        {
            get { return this.EditableCollectionViewChecked.CanCancelEdit; }
        }

        /// <summary>
        /// Gets a value that indicates whether an item can be removed from the collection.
        /// </summary>
        public virtual bool CanRemove
        {
            get { return this.EditableCollectionViewChecked.CanRemove; }
        }

        /// <summary>
        /// Gets the item that is being added during the current add transaction.
        /// </summary>
        public virtual object CurrentAddItem
        {
            get { return this.EditableCollectionViewChecked.CurrentAddItem; }
        }

        /// <summary>
        /// Gets the item in the collection that is being edited.
        /// </summary>
        public virtual object CurrentEditItem
        {
            get { return this.EditableCollectionViewChecked.CurrentEditItem; }
        }

        /// <summary>
        /// Gets a value that indicates whether an add transaction is in progress.
        /// </summary>
        public virtual bool IsAddingNew
        {
            get { return this.EditableCollectionViewChecked.IsAddingNew; }
        }

        /// <summary>
        /// Gets a value that indicates whether an edit transaction is in progress.
        /// </summary>
        public virtual bool IsEditingItem
        {
            get { return this.EditableCollectionViewChecked.IsEditingItem; }
        }

        /// <summary>
        /// Gets or sets the position of the new item placeholder in the collection view.
        /// </summary>
        public virtual NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get { return this.EditableCollectionViewChecked.NewItemPlaceholderPosition; }
            set { this.EditableCollectionViewChecked.NewItemPlaceholderPosition = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new item to the underlying collection.
        /// </summary>
        /// <returns>The new item that is added to the collection.</returns>
        public virtual object AddNew()
        {
            return this.EditableCollectionViewChecked.AddNew();
        }

        /// <summary>
        /// Ends the edit transaction and, if possible, restores the original value of
        /// the item.
        /// </summary>
        public virtual void CancelEdit()
        {
            this.EditableCollectionViewChecked.CancelEdit();
        }

        /// <summary>
        /// Ends the add transaction and discards the pending new item.
        /// </summary>
        public virtual void CancelNew()
        {
            this.EditableCollectionViewChecked.CancelNew();
        }

        /// <summary>
        /// Ends the edit transaction and saves the pending changes.
        /// </summary>
        public virtual void CommitEdit()
        {
            this.EditableCollectionViewChecked.CommitEdit();
        }

        /// <summary>
        /// Ends the add transaction and saves the pending new item.
        /// </summary>
        public virtual void CommitNew()
        {
            this.EditableCollectionViewChecked.CommitNew();
        }

        /// <summary>
        /// Begins an edit transaction on the specified item.
        /// </summary>
        /// <param name="item">The item to edit.</param>
        public virtual void EditItem(object item)
        {
            this.EditableCollectionViewChecked.EditItem(item);
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public virtual void Remove(object item)
        {
            this.EditableCollectionViewChecked.Remove(item);
        }

        /// <summary>
        /// Removes the item at the specified position from the collection.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public virtual void RemoveAt(int index)
        {
            this.EditableCollectionViewChecked.RemoveAt(index);
        }

        #endregion

        #endregion

        #region IPagedCollectionView Members

        #region Events

        /// <summary>
        /// Raised after the <see cref="PageIndex"/> has changed.
        /// </summary>
        public event EventHandler<EventArgs> PageChanged;

        /// <summary>
        /// Raised before the <see cref="PageIndex"/> has changed.
        /// </summary>
        public event EventHandler<PageChangingEventArgs> PageChanging;

        /// <summary>
        /// Handles <see cref="IPagedCollectionView.PageChanged"/> events raised by the <see cref="PagedCollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="PagedCollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnPagedCollectionViewPageChanged(object sender, EventArgs e)
        {
            this.OnPageChanged(e);
        }

        /// <summary>
        /// Raises a <see cref="PageChanged"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPageChanged(EventArgs e)
        {
            EventHandler<EventArgs> handler = this.PageChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles <see cref="IPagedCollectionView.PageChanging"/> events raised by the <see cref="PagedCollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="PagedCollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnPagedCollectionViewPageChanging(object sender, PageChangingEventArgs e)
        {
            this.OnPageChanging(e);
        }

        /// <summary>
        /// Raises a <see cref="PageChanging"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPageChanging(PageChangingEventArgs e)
        {
            EventHandler<PageChangingEventArgs> handler = this.PageChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether the <see cref="PageIndex"/>
        /// value can change.
        /// </summary>
        public virtual bool CanChangePage
        {
            get { return this.PagedCollectionViewChecked.CanChangePage; }
        }

        /// <summary>
        /// Gets a value that indicates whether the page index is changing.
        /// </summary>
        public virtual bool IsPageChanging
        {
            get { return this.PagedCollectionViewChecked.IsPageChanging; }
        }

        /// <summary>
        /// Gets the number of known items in the view before paging is applied.
        /// </summary>
        public virtual int ItemCount
        {
            get { return this.PagedCollectionViewChecked.ItemCount; }
        }

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        public virtual int PageIndex
        {
            get { return this.PagedCollectionViewChecked.PageIndex; }
        }

        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// </summary>
        public virtual int PageSize
        {
            get { return this.PagedCollectionViewChecked.PageSize; }
            set { this.PagedCollectionViewChecked.PageSize = value; }
        }

        /// <summary>
        /// Gets the total number of items in the view before paging is applied.
        /// </summary>
        public virtual int TotalItemCount
        {
            get { return this.PagedCollectionViewChecked.TotalItemCount; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the first page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToFirstPage()
        {
            return this.PagedCollectionViewChecked.MoveToFirstPage();
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToLastPage()
        {
            return this.PagedCollectionViewChecked.MoveToLastPage();
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToNextPage()
        {
            return this.PagedCollectionViewChecked.MoveToNextPage();
        }

        /// <summary>
        /// Moves to the page at the specified index.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToPage(int pageIndex)
        {
            return this.PagedCollectionViewChecked.MoveToPage(pageIndex);
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToPreviousPage()
        {
            return this.PagedCollectionViewChecked.MoveToPreviousPage();
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns the number of times the wrapper references the specified view
        /// </summary>
        /// <param name="view">The view to match</param>
        /// <returns>The number of times the wrapper references the view</returns>
        private int GetViewMatches(object view)
        {
            return
                ((view == this.CollectionView) ? 1 : 0) +
                ((view == this.EditableCollectionView) ? 1 : 0) +
                ((view == this.PagedCollectionView) ? 1 : 0);
        }

        /// <summary>
        /// Adds a property changed event handler the first time the view is referenced
        /// </summary>
        /// <param name="view">The view to add a handler to</param>
        private void AddPropertyChangedIfNeeded(object view)
        {
            INotifyPropertyChanged notifyingView = view as INotifyPropertyChanged;
            if ((notifyingView != null) && (this.GetViewMatches(view) == 1) /* evaluated after adding */)
            {
                notifyingView.PropertyChanged += this.OnViewPropertyChanged;
            }
        }

        /// <summary>
        /// Removes a property changed event handler the last time a view reference is reset
        /// </summary>
        /// <param name="view">The view to remove a handler from</param>
        private void RemovePropertyChangedIfNeeded(object view)
        {
            INotifyPropertyChanged notifyingView = view as INotifyPropertyChanged;
            if ((notifyingView != null) && (this.GetViewMatches(view) == 1) /* evaluated before removing */)
            {
                notifyingView.PropertyChanged -= this.OnViewPropertyChanged;
            }
        }

        /// <summary>
        /// Handles <see cref="INotifyPropertyChanged.PropertyChanged"/> events raised by any of the
        /// wrapped collection views.
        /// </summary>
        /// <param name="sender">A wrapped collection view</param>
        /// <param name="e">The event args</param>
        protected virtual void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// <see cref="IDisposable"/> that allows the wrapper to track <see cref="DeferRefresh"/> invocations
        /// </summary>
        private class DeferHelper : IDisposable
        {
            private readonly Action _action;
            private readonly IDisposable _disposable;

            public DeferHelper(Action action, IDisposable disposable)
            {
                this._action = action;
                this._disposable = disposable;
            }

            public void Dispose()
            {
                this._disposable.Dispose();
                this._action();
            }
        }

#if DEBUG
        // Exists to make sure CollectionViewWrapper correctly implements the following interfaces
        private class CompileHelper :
            CollectionViewWrapper,
            ICollectionView,
            IEditableCollectionView,
            IPagedCollectionView,
            INotifyPropertyChanged
        {
        }
#endif

        #endregion
    }
}
