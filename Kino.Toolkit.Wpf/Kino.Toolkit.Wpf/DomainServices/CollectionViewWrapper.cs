/*
    Copyright (c) 2013, The Outercurve Foundation.
    This software is released under the Apache License 2.0 (the "License");
    you may not use the software except in compliance with the License.
    http://www.openriaservices.net/
*/
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1202
#pragma warning disable SA1214
#pragma warning disable SA1311
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
                return _collectionView;
            }

            set
            {
                if (_collectionView != value)
                {
                    if (_collectionView != null)
                    {
                        _collectionView.CollectionChanged -= OnCollectionViewCollectionChanged;
                        _collectionView.CurrentChanged -= OnCollectionViewCurrentChanged;
                        _collectionView.CurrentChanging -= OnCollectionViewCurrentChanging;

                        RemovePropertyChangedIfNeeded(_collectionView);
                    }

                    _collectionView = value;

                    if (_collectionView != null)
                    {
                        _collectionView.CollectionChanged += OnCollectionViewCollectionChanged;
                        _collectionView.CurrentChanged += OnCollectionViewCurrentChanged;
                        _collectionView.CurrentChanging += OnCollectionViewCurrentChanging;

                        AddPropertyChangedIfNeeded(_collectionView);
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
                if (CollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IcvNotSupported);
                }

                return CollectionView;
            }
        }

        /// <summary>
        /// Get or sets the <see cref="IEditableCollectionView"/> implementation to delegate to
        /// </summary>
        protected IEditableCollectionView EditableCollectionView
        {
            get
            {
                return _editableCollectionView;
            }

            set
            {
                if (_editableCollectionView != value)
                {
                    if (_editableCollectionView != null)
                    {
                        RemovePropertyChangedIfNeeded(_editableCollectionView);
                    }

                    _editableCollectionView = value;

                    if (_editableCollectionView != null)
                    {
                        AddPropertyChangedIfNeeded(_editableCollectionView);
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
                if (EditableCollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IecvNotSupported);
                }

                return EditableCollectionView;
            }
        }

        /// <summary>
        /// Get or sets the <see cref="IPagedCollectionView"/> implementation to delegate to
        /// </summary>
        protected IPagedCollectionView PagedCollectionView
        {
            get
            {
                return _pagedCollectionView;
            }

            set
            {
                if (_pagedCollectionView != value)
                {
                    if (_pagedCollectionView != null)
                    {
                        _pagedCollectionView.PageChanged -= OnPagedCollectionViewPageChanged;
                        _pagedCollectionView.PageChanging -= OnPagedCollectionViewPageChanging;

                        RemovePropertyChangedIfNeeded(_pagedCollectionView);
                    }

                    _pagedCollectionView = value;

                    if (_pagedCollectionView != null)
                    {
                        _pagedCollectionView.PageChanged += OnPagedCollectionViewPageChanged;
                        _pagedCollectionView.PageChanging += OnPagedCollectionViewPageChanging;

                        AddPropertyChangedIfNeeded(_pagedCollectionView);
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
                if (PagedCollectionView == null)
                {
                    throw new NotSupportedException(DomainServicesResources.IpcvNotSupported);
                }

                return PagedCollectionView;
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
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Raises collection changed events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles <see cref="ICollectionView.CurrentChanged"/> events raised by the <see cref="CollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="CollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            OnCurrentChanged(e);
        }

        /// <summary>
        /// Raises current changed events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCurrentChanged(EventArgs e)
        {
            CurrentChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles <see cref="ICollectionView.CurrentChanging"/> events raised by the <see cref="CollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="CollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnCollectionViewCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            OnCurrentChanging(e);
        }

        /// <summary>
        /// Raises current changing events
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            CurrentChanging?.Invoke(this, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether this view supports filtering by way of
        /// the <see cref="Filter"/> property.
        /// </summary>
        public virtual bool CanFilter => CollectionViewChecked.CanFilter;

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping by way of
        /// the <see cref="GroupDescriptions"/> property.
        /// </summary>
        public virtual bool CanGroup => CollectionViewChecked.CanGroup;

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting by way of
        /// the <see cref="SortDescriptions"/> property.
        /// </summary>
        public virtual bool CanSort => CollectionViewChecked.CanSort;

        /// <summary>
        /// Gets or sets the cultural information for any operations of the view that
        /// may differ by culture, such as sorting.
        /// </summary>
        public virtual CultureInfo Culture
        {
            get { return CollectionViewChecked.Culture; }
            set { CollectionViewChecked.Culture = value; }
        }

        /// <summary>
        /// Gets the current item in the view.
        /// </summary>
        public virtual object CurrentItem => CollectionViewChecked.CurrentItem;

        /// <summary>
        ///  Gets the ordinal position of the <see cref="CurrentItem"/> in the view.
        /// </summary>
        public virtual int CurrentPosition => CollectionViewChecked.CurrentPosition;

        /// <summary>
        /// Gets or sets a callback that is used to determine whether an item is appropriate
        /// for inclusion in the view.
        /// </summary>
        public virtual Predicate<object> Filter
        {
            get { return CollectionViewChecked.Filter; }
            set { CollectionViewChecked.Filter = value; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GroupDescription"/> objects that
        /// describe how the items in the collection are grouped in the view.
        /// </summary>
        public virtual ObservableCollection<GroupDescription> GroupDescriptions => CollectionViewChecked.GroupDescriptions;

        /// <summary>
        /// Gets the top-level groups.
        /// </summary>
        public virtual ReadOnlyObservableCollection<object> Groups => CollectionViewChecked.Groups;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CurrentItem"/>
        /// of the view is beyond the end of the collection.
        /// </summary>
        public virtual bool IsCurrentAfterLast => CollectionViewChecked.IsCurrentAfterLast;

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CurrentItem"/>
        /// of the view is beyond the start of the collection.
        /// </summary>
        public virtual bool IsCurrentBeforeFirst => CollectionViewChecked.IsCurrentBeforeFirst;

        /// <summary>
        /// Gets a value that indicates whether the view is empty.
        /// </summary>
        public virtual bool IsEmpty => CollectionViewChecked.IsEmpty;

        /// <summary>
        /// Gets a collection of <see cref="SortDescription"/> instances that
        /// describe how the items in the collection are sorted in the view.
        /// </summary>
        public virtual SortDescriptionCollection SortDescriptions => CollectionViewChecked.SortDescriptions;

        /// <summary>
        /// Gets the underlying collection.
        /// </summary>
        public virtual IEnumerable SourceCollection => CollectionViewChecked.SourceCollection;

        /// <summary>
        /// Gets a value that indicates whether a <see cref="DeferRefresh"/> method is pending
        /// </summary>
        protected bool IsRefreshDeferred => _deferCount > 0;

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
            return CollectionViewChecked.Contains(item);
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
            IDisposable disposable = CollectionViewChecked.DeferRefresh();
            _deferCount++;
            return new DeferHelper(OnDeferHelperDisposed, disposable);
        }

        /// <summary>
        /// Handles disposal of a <see cref="DeferHelper"/> by conditionally calling <see cref="Refresh"/>
        /// </summary>
        private void OnDeferHelperDisposed()
        {
            _deferCount--;
            if (_deferCount == 0)
            {
                Refresh();
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
            return CollectionViewChecked.GetEnumerator();
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
            return CollectionViewChecked.MoveCurrentTo(item);
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
            return CollectionViewChecked.MoveCurrentToFirst();
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
            return CollectionViewChecked.MoveCurrentToLast();
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
            return CollectionViewChecked.MoveCurrentToNext();
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
            return CollectionViewChecked.MoveCurrentToPosition(position);
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
            return CollectionViewChecked.MoveCurrentToPrevious();
        }

        /// <summary>
        /// Recreates the view.
        /// </summary>
        public virtual void Refresh()
        {
            CollectionViewChecked.Refresh();
        }

        #endregion

        #endregion

        #region IEditableCollectionView Members

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether a new item can be added to the collection.
        /// </summary>
        public virtual bool CanAddNew => EditableCollectionViewChecked.CanAddNew;

        /// <summary>
        /// Gets a value that indicates whether the collection view can discard pending
        /// changes and restore the original values of an edited object.
        /// </summary>
        public virtual bool CanCancelEdit => EditableCollectionViewChecked.CanCancelEdit;

        /// <summary>
        /// Gets a value that indicates whether an item can be removed from the collection.
        /// </summary>
        public virtual bool CanRemove => EditableCollectionViewChecked.CanRemove;

        /// <summary>
        /// Gets the item that is being added during the current add transaction.
        /// </summary>
        public virtual object CurrentAddItem => EditableCollectionViewChecked.CurrentAddItem;

        /// <summary>
        /// Gets the item in the collection that is being edited.
        /// </summary>
        public virtual object CurrentEditItem => EditableCollectionViewChecked.CurrentEditItem;

        /// <summary>
        /// Gets a value that indicates whether an add transaction is in progress.
        /// </summary>
        public virtual bool IsAddingNew => EditableCollectionViewChecked.IsAddingNew;

        /// <summary>
        /// Gets a value that indicates whether an edit transaction is in progress.
        /// </summary>
        public virtual bool IsEditingItem => EditableCollectionViewChecked.IsEditingItem;

        /// <summary>
        /// Gets or sets the position of the new item placeholder in the collection view.
        /// </summary>
        public virtual NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get { return EditableCollectionViewChecked.NewItemPlaceholderPosition; }
            set { EditableCollectionViewChecked.NewItemPlaceholderPosition = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new item to the underlying collection.
        /// </summary>
        /// <returns>The new item that is added to the collection.</returns>
        public virtual object AddNew()
        {
            return EditableCollectionViewChecked.AddNew();
        }

        /// <summary>
        /// Ends the edit transaction and, if possible, restores the original value of
        /// the item.
        /// </summary>
        public virtual void CancelEdit()
        {
            EditableCollectionViewChecked.CancelEdit();
        }

        /// <summary>
        /// Ends the add transaction and discards the pending new item.
        /// </summary>
        public virtual void CancelNew()
        {
            EditableCollectionViewChecked.CancelNew();
        }

        /// <summary>
        /// Ends the edit transaction and saves the pending changes.
        /// </summary>
        public virtual void CommitEdit()
        {
            EditableCollectionViewChecked.CommitEdit();
        }

        /// <summary>
        /// Ends the add transaction and saves the pending new item.
        /// </summary>
        public virtual void CommitNew()
        {
            EditableCollectionViewChecked.CommitNew();
        }

        /// <summary>
        /// Begins an edit transaction on the specified item.
        /// </summary>
        /// <param name="item">The item to edit.</param>
        public virtual void EditItem(object item)
        {
            EditableCollectionViewChecked.EditItem(item);
        }

        /// <summary>
        /// Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public virtual void Remove(object item)
        {
            EditableCollectionViewChecked.Remove(item);
        }

        /// <summary>
        /// Removes the item at the specified position from the collection.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        public virtual void RemoveAt(int index)
        {
            EditableCollectionViewChecked.RemoveAt(index);
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
            OnPageChanged(e);
        }

        /// <summary>
        /// Raises a <see cref="PageChanged"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPageChanged(EventArgs e)
        {
            PageChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles <see cref="IPagedCollectionView.PageChanging"/> events raised by the <see cref="PagedCollectionView"/>
        /// </summary>
        /// <param name="sender">The <see cref="PagedCollectionView"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnPagedCollectionViewPageChanging(object sender, PageChangingEventArgs e)
        {
            OnPageChanging(e);
        }

        /// <summary>
        /// Raises a <see cref="PageChanging"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPageChanging(PageChangingEventArgs e)
        {
            PageChanging?.Invoke(this, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether the <see cref="PageIndex"/>
        /// value can change.
        /// </summary>
        public virtual bool CanChangePage => PagedCollectionViewChecked.CanChangePage;

        /// <summary>
        /// Gets a value that indicates whether the page index is changing.
        /// </summary>
        public virtual bool IsPageChanging => PagedCollectionViewChecked.IsPageChanging;

        /// <summary>
        /// Gets the number of known items in the view before paging is applied.
        /// </summary>
        public virtual int ItemCount => PagedCollectionViewChecked.ItemCount;

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        public virtual int PageIndex => PagedCollectionViewChecked.PageIndex;

        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// </summary>
        public virtual int PageSize
        {
            get { return PagedCollectionViewChecked.PageSize; }
            set { PagedCollectionViewChecked.PageSize = value; }
        }

        /// <summary>
        /// Gets the total number of items in the view before paging is applied.
        /// </summary>
        public virtual int TotalItemCount => PagedCollectionViewChecked.TotalItemCount;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the first page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToFirstPage()
        {
            return PagedCollectionViewChecked.MoveToFirstPage();
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToLastPage()
        {
            return PagedCollectionViewChecked.MoveToLastPage();
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToNextPage()
        {
            return PagedCollectionViewChecked.MoveToNextPage();
        }

        /// <summary>
        /// Moves to the page at the specified index.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToPage(int pageIndex)
        {
            return PagedCollectionViewChecked.MoveToPage(pageIndex);
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public virtual bool MoveToPreviousPage()
        {
            return PagedCollectionViewChecked.MoveToPreviousPage();
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
                ((view == CollectionView) ? 1 : 0) +
                ((view == EditableCollectionView) ? 1 : 0) +
                ((view == PagedCollectionView) ? 1 : 0);
        }

        /// <summary>
        /// Adds a property changed event handler the first time the view is referenced
        /// </summary>
        /// <param name="view">The view to add a handler to</param>
        private void AddPropertyChangedIfNeeded(object view)
        {
            if (view is INotifyPropertyChanged notifyingView && (this.GetViewMatches(view) == 1) /* evaluated after adding */)
            {
                notifyingView.PropertyChanged += OnViewPropertyChanged;
            }
        }

        /// <summary>
        /// Removes a property changed event handler the last time a view reference is reset
        /// </summary>
        /// <param name="view">The view to remove a handler from</param>
        private void RemovePropertyChangedIfNeeded(object view)
        {
            if (view is INotifyPropertyChanged notifyingView && (this.GetViewMatches(view) == 1) /* evaluated before removing */)
            {
                notifyingView.PropertyChanged -= OnViewPropertyChanged;
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
            OnPropertyChanged(e);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
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
                _action = action;
                _disposable = disposable;
            }

            public void Dispose()
            {
                _disposable.Dispose();
                _action();
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
#pragma warning restore SA1201
#pragma warning restore SA1202
#pragma warning restore SA1214
#pragma warning restore SA1311