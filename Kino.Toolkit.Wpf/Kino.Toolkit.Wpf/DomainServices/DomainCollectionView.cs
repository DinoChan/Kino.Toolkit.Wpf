// https://github.com/OpenRIAServices/OpenRiaServices/blob/master/OpenRiaServices.Data.DomainServices/Framework/DomainCollectionView.cs
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
#pragma warning disable SA1642 // Constructor summary documentation must begin with standard text
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// Collection view implementation that allows the view to be updated asynchronously
    /// via a <see cref="DomainServices.CollectionViewLoader"/>. In conjunction with the
    /// <see cref="DomainCollectionViewLoader"/>, this allows collection view properties
    /// like sorting, grouping, and paging to be applied on the server.
    /// </summary>
    public class DomainCollectionView :
        CollectionViewWrapper,
        ICollectionView,
        IEditableCollectionView,
        IPagedCollectionView,
        INotifyPropertyChanged
    {
        #region Member fields

        private readonly CollectionViewLoader _collectionViewLoader;

        private readonly ObservableCollection<GroupDescription> _groupDescriptions = new ObservableCollection<GroupDescription>();
        private readonly SortDescriptionCollection _sortDescriptions = new SortDescriptionCollection();

        private bool _canChangePage;
        private bool _canGroup;
        private bool _canSort;

        private int _pageIndex;

        private object _currentLoadToken;

        private bool _ignoreRefresh;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCollectionView"/> that
        /// uses the specified callback for loading data.
        /// </summary>
        /// <param name="load">The callback to use for loading data</param>
        /// <param name="source">The source collection for this view. All updates to the
        /// source will be reflected in the view.
        /// </param>
        // public DomainCollectionView(Func<LoadOperation> load, IEnumerable source)
        //    : this(new DomainCollectionViewLoader(load), source)
        // {
        // }
#pragma warning disable SA1404 // Code analysis suppression must have justification
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCollectionView"/>
        /// </summary>
        /// <param name="collectionViewLoader">The <see cref="CollectionViewLoader"/> to use for loading data</param>
        /// <param name="source">The source collection for this view. All updates to the
        /// source will be reflected in the view.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
#pragma warning restore SA1404 // Code analysis suppression must have justification
        public DomainCollectionView(CollectionViewLoader collectionViewLoader, IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this._collectionViewLoader = collectionViewLoader ?? throw new ArgumentNullException("collectionViewLoader");
            this._collectionViewLoader.CanLoadChanged += this.OnCollectionViewLoaderCanLoadChanged;
            this._collectionViewLoader.LoadCompleted += this.OnLoadCompleted;

            this.CollectionView = CreateView(source);
            this.EditableCollectionView = this.CollectionView as IEditableCollectionView;
            if (this.EditableCollectionView == null)
            {
                throw new InvalidOperationException(DomainServicesResources.MustImplementIecv);
            }

            this.PagedCollectionView = new DomainPagedCollectionView(this.LoadPage);

            this.SetPageIndex(this.PagedCollectionView.PageIndex);

            this.CalculateAll();
        }

        /// <summary>
        /// Creates a view to wrap using the specified source
        /// </summary>
        /// <param name="source">The source to create a view for</param>
        /// <returns>A view over the specified source</returns>
        private static ICollectionView CreateView(IEnumerable source)
        {
            CollectionViewSource cvs = new CollectionViewSource
            {
                Source = source
            };
            return cvs.View;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether the <see cref="PageIndex"/>
        /// value can change.
        /// </summary>
        /// <remarks>
        /// Overridden to return <c>false</c> when <see cref="DomainServices.CollectionViewLoader.CanLoad"/> is <c>false</c>
        /// </remarks>
        public override bool CanChangePage
        {
            get { return this._canChangePage; }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping by way of
        /// the <see cref="GroupDescriptions"/> property.
        /// </summary>
        /// <remarks>
        /// Overridden to return <c>false</c> when <see cref="DomainServices.CollectionViewLoader.CanLoad"/> is <c>false</c>
        /// </remarks>
        public override bool CanGroup
        {
            get { return this._canGroup; }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting by way of
        /// the <see cref="SortDescriptions"/> property.
        /// </summary>
        /// <remarks>
        /// Overridden to return <c>false</c> when <see cref="DomainServices.CollectionViewLoader.CanLoad"/> is <c>false</c>
        /// </remarks>
        public override bool CanSort
        {
            get { return this._canSort; }
        }

        /// <summary>
        /// Gets the <see cref="DomainServices.CollectionViewLoader"/> this view uses to asynchronously load
        /// the source collection.
        /// </summary>
        public CollectionViewLoader CollectionViewLoader
        {
            get { return this._collectionViewLoader; }
        }

        /// <summary>
        /// Gets a collection of <see cref="GroupDescription"/> objects that
        /// describe how the items in the collection are grouped in the view.
        /// </summary>
        /// <remarks>
        /// Overridden to preserve view state while an asynchronous load is taking place
        /// </remarks>
        public override ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { return this._groupDescriptions; }
        }

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        /// <remarks>
        /// Overridden to preserve view state while an asynchronous load is taking place
        /// </remarks>
        public override int PageIndex
        {
            get { return this._pageIndex; }
        }

        /// <summary>
        /// Gets a collection of <see cref="SortDescription"/> instances that
        /// describe how the items in the collection are sorted in the view.
        /// </summary>
        /// <remarks>
        /// Overridden to preserve view state while an asynchronous load is taking place
        /// </remarks>
        public override SortDescriptionCollection SortDescriptions
        {
            get { return this._sortDescriptions; }
        }

        /// <summary>
        /// Returns the <see cref="CollectionViewWrapper.PagedCollectionView"/> as a
        /// <see cref="DomainPagedCollectionView"/>
        /// </summary>
        private DomainPagedCollectionView PagedCollectionViewPrivate
        {
            get { return this.PagedCollectionView as DomainPagedCollectionView; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <see cref="IPagedCollectionView.TotalItemCount"/>.
        /// </summary>
        /// <remarks>
        /// This value is used by the pager to display the page count. When this
        /// value is set to <c>-1</c>, no page count is displayed.
        /// </remarks>
        /// <param name="totalItemCount">The total item count</param>
        public void SetTotalItemCount(int totalItemCount)
        {
            this.UpdateItemCounts(totalItemCount);
        }

        /// <summary>
        /// Overridden to reload the view using the <see cref="CollectionViewLoader"/>
        /// </summary>
        public override void Refresh()
        {
            if (this._ignoreRefresh)
            {
                return;
            }

            this.Load();
        }

        /// <summary>
        /// Reloads the view using the <see cref="CollectionViewLoader"/>
        /// </summary>
        protected void Load()
        {
            if (this.IsRefreshDeferred)
            {
                return;
            }

            this._currentLoadToken = new object();
            this.CollectionViewLoader.Load(this._currentLoadToken);
        }

        /// <summary>
        /// Sets the <see cref="PageIndex"/> and reloads the view
        /// </summary>
        /// <param name="pageIndex">The page index to load</param>
        private void LoadPage(int pageIndex)
        {
            this.SetPageIndex(pageIndex);
            this.Load();
        }

        /// <summary>
        /// Handles the <see cref="DomainServices.CollectionViewLoader.LoadCompleted"/> event and updates
        /// the view
        /// </summary>
        /// <param name="sender">The <see cref="CollectionViewLoader"/></param>
        /// <param name="e">The event args</param>
        protected virtual void OnLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState == this._currentLoadToken)
            {
                if ((e.Error != null) || e.Cancelled)
                {
                    this.SyncToWrappedValues();
                }
                else
                {
                    this.SyncToCurrentValues();
                }

                this.PagedCollectionViewPrivate.CompleteMoveToPage(this.PageIndex);
                this._currentLoadToken = null;
            }
        }

        /// <summary>
        /// Applies the wrapped view state to the current view
        /// </summary>
        private void SyncToWrappedValues()
        {
            this._ignoreRefresh = true;

            using (this.DeferRefresh())
            {
                this.SyncToWrappedGroupDescriptions();
                this.SyncToWrappedSortDescriptions();
                this.SyncToWrappedPageIndex();
            }

            this._ignoreRefresh = false;
        }

        /// <summary>
        /// Applies the wrapped view descriptions to the <see cref="GroupDescriptions"/>
        /// </summary>
        private void SyncToWrappedGroupDescriptions()
        {
            CopyGroupDescriptions(base.GroupDescriptions, this.GroupDescriptions);
        }

        /// <summary>
        /// Applies the wrapped view descriptions to the <see cref="SortDescriptions"/>
        /// </summary>
        private void SyncToWrappedSortDescriptions()
        {
            CopySortDescriptions(base.SortDescriptions, this.SortDescriptions);
        }

        /// <summary>
        /// Applies the wrapped view index to the <see cref="PageIndex"/>
        /// </summary>
        protected void SyncToWrappedPageIndex()
        {
            // The single case where we do not sync to the wrapped value is after we've attempted
            // the first page move (base.PageIndex = -1). In that case, we'll keep the current index.
            if (base.PageIndex >= 0)
            {
                this.SetPageIndex(base.PageIndex);
            }
        }

        /// <summary>
        /// Applies the current view state to the wrapped view
        /// </summary>
        private void SyncToCurrentValues()
        {
            this._ignoreRefresh = true;

            using (this.DeferRefresh())
            {
                this.SyncToCurrentGroupDescriptions();
                this.SyncToCurrentSortDescriptions();
                this.SyncToCurrentItemCounts();
            }

            this._ignoreRefresh = false;
        }

        /// <summary>
        /// Applies the current <see cref="GroupDescriptions"/> to the wrapped view
        /// </summary>
        private void SyncToCurrentGroupDescriptions()
        {
            CopyGroupDescriptions(this.GroupDescriptions, base.GroupDescriptions);
        }

        /// <summary>
        /// Applies the current <see cref="SortDescriptions"/> to the wrapped view
        /// </summary>
        private void SyncToCurrentSortDescriptions()
        {
            CopySortDescriptions(this.SortDescriptions, base.SortDescriptions);
        }

        /// <summary>
        /// Updates the <see cref="IPagedCollectionView.ItemCount"/> property based on the
        /// values in the view
        /// </summary>
        protected void SyncToCurrentItemCounts()
        {
            this.UpdateItemCounts(this.TotalItemCount);
        }

        /// <summary>
        /// Copies the <paramref name="from"/> collection to the <paramref name="to"/> collection
        /// </summary>
        /// <param name="from">The group descriptions to copy from</param>
        /// <param name="to">The group descriptions to copy to</param>
        private static void CopyGroupDescriptions(ObservableCollection<GroupDescription> from, ObservableCollection<GroupDescription> to)
        {
            to.Clear();
            foreach (GroupDescription gd in from)
            {
                to.Add(gd);
            }
        }

        /// <summary>
        /// Copies the <paramref name="from"/> collection to the <paramref name="to"/> collection
        /// </summary>
        /// <param name="from">The sort descriptions to copy from</param>
        /// <param name="to">The sort descriptions to copy to</param>
        private static void CopySortDescriptions(SortDescriptionCollection from, SortDescriptionCollection to)
        {
            to.Clear();
            foreach (SortDescription sd in from)
            {
                to.Add(sd);
            }
        }

        /// <summary>
        /// Updates the <see cref="IPagedCollectionView.TotalItemCount"/> and
        /// <see cref="IPagedCollectionView.ItemCount"/> properties based on the
        /// specified <paramref name="totalItemCount"/> and values in the view
        /// </summary>
        /// <param name="totalItemCount">The total item count</param>
        private void UpdateItemCounts(int totalItemCount)
        {
            int itemCount = totalItemCount;
            if (itemCount == -1)
            {
                // We only want to update the ItemCount when we're on a page with items
                int count = this.SourceCollection.Cast<object>().Count();
                if (count > 0)
                {
                    itemCount = Math.Max(this.ItemCount, (this.PageIndex * this.PageSize) + count);
                }
            }

            this.UpdateItemCounts(totalItemCount, itemCount);
        }

        /// <summary>
        /// Updates the <see cref="IPagedCollectionView.TotalItemCount"/> and
        /// <see cref="IPagedCollectionView.ItemCount"/> properties based on the
        /// specified <paramref name="totalItemCount"/> and <paramref name="itemCount"/>
        /// </summary>
        /// <param name="totalItemCount">The total item count</param>
        /// <param name="itemCount">The item count</param>
        private void UpdateItemCounts(int totalItemCount, int itemCount)
        {
            this.PagedCollectionViewPrivate.SetItemCounts(totalItemCount, itemCount);
        }

        /// <summary>
        /// Handles the <see cref="DomainServices.CollectionViewLoader.CanLoadChanged"/> event
        /// </summary>
        /// <param name="sender">The <see cref="CollectionViewLoader"/></param>
        /// <param name="e">The event args</param>
        private void OnCollectionViewLoaderCanLoadChanged(object sender, EventArgs e)
        {
            this.CalculateAll();
        }

        /// <summary>
        /// Handles <see cref="INotifyPropertyChanged.PropertyChanged"/> events raised by the wrapped views
        /// </summary>
        /// <remarks>
        /// Overridden to re-calculate property values
        /// </remarks>
        /// <param name="sender">The wrapped views</param>
        /// <param name="e">The event args</param>
        protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CanChangePage":
                    this.CalculateCanChangePage();
                    break;
                case "CanGroup":
                    this.CalculateCanGroup();
                    break;
                case "CanSort":
                    this.CalculateCanSort();
                    break;
                case "PageIndex":
                    // do nothing
                    break;
                default:
                    base.OnViewPropertyChanged(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Calculates all the properties based on the underlying state
        /// </summary>
        private void CalculateAll()
        {
            this.CalculateCanChangePage();
            this.CalculateCanGroup();
            this.CalculateCanSort();
        }

        /// <summary>
        /// Calculates the value of <see cref="CanChangePage"/> using <see cref="DomainServices.CollectionViewLoader.CanLoad"/>
        /// </summary>
        private void CalculateCanChangePage()
        {
            bool canChangePage = base.CanChangePage && this.CollectionViewLoader.CanLoad;
            if (this._canChangePage != canChangePage)
            {
                this._canChangePage = canChangePage;
                this.RaisePropertyChanged("CanChangePage");
            }
        }

        /// <summary>
        /// Calculates the value of <see cref="CanGroup"/> using <see cref="DomainServices.CollectionViewLoader.CanLoad"/>
        /// </summary>
        private void CalculateCanGroup()
        {
            bool canGroup = base.CanGroup && this.CollectionViewLoader.CanLoad;
            if (this._canGroup != canGroup)
            {
                this._canGroup = canGroup;
                this.RaisePropertyChanged("CanGroup");
            }
        }

        /// <summary>
        /// Calculates the value of <see cref="CanSort"/> using <see cref="DomainServices.CollectionViewLoader.CanLoad"/>
        /// </summary>
        private void CalculateCanSort()
        {
            bool canSort = base.CanSort && this.CollectionViewLoader.CanLoad;
            if (this._canSort != canSort)
            {
                this._canSort = canSort;
                this.RaisePropertyChanged("CanSort");
            }
        }

        /// <summary>
        /// Sets <see cref="PageIndex"/> using the specified <paramref name="pageIndex"/>
        /// </summary>
        /// <param name="pageIndex">The page index</param>
        private void SetPageIndex(int pageIndex)
        {
            if (this._pageIndex != pageIndex)
            {
                this._pageIndex = pageIndex;
                this.RaisePropertyChanged("PageIndex");
            }
        }

        /// <summary>
        /// Raises <see cref="INotifyPropertyChanged.PropertyChanged"/> events for the specified property
        /// </summary>
        /// <param name="propertyName">The property to raise an event for</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Concrete implementation of the <see cref="IPagedCollectionView"/> interface that
        /// moves pages using a callback.
        /// </summary>
        private class DomainPagedCollectionView : PagedCollectionViewBase
        {
            private readonly Action<int> _moveToPageAction;

            public DomainPagedCollectionView(Action<int> moveToPageAction)
            {
                this._moveToPageAction = moveToPageAction;
            }

            protected override void BeginMoveToPageCore(int pageIndex)
            {
                this._moveToPageAction(pageIndex);
            }

            /// <summary>
            /// Completes a page move
            /// </summary>
            /// <param name="pageIndex">The index that was moved to</param>
            public void CompleteMoveToPage(int pageIndex)
            {
                this.EndMoveToPage(pageIndex);
            }

            /// <summary>
            /// Sets the values for <see cref="IPagedCollectionView.TotalItemCount"/> and
            /// <see cref="IPagedCollectionView.ItemCount"/>.
            /// </summary>
            /// <param name="totalItemCount">The total item count to set</param>
            /// <param name="itemCount">The item count to set</param>
            public void SetItemCounts(int totalItemCount, int itemCount)
            {
                this.TotalItemCount = totalItemCount;
                this.ItemCount = itemCount;
            }
        }

        #endregion
    }

#pragma warning disable SA1402 // File may only contain a single class
                              /// <summary>
                              /// Collection view implementation that allows the view to be updated asynchronously
                              /// via a <see cref="DomainServices.CollectionViewLoader"/>. In conjunction with the
                              /// <see cref="DomainCollectionViewLoader"/>, this allows collection view properties
                              /// like sorting, grouping, and paging to be applied on the server.
                              /// </summary>
                              /// <typeparam name="TItem">The item type of this view</typeparam>
    public class DomainCollectionView<TItem> : DomainCollectionView, IEnumerable<TItem>
#pragma warning restore SA1402 // File may only contain a single class
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCollectionView"/> that
        /// uses the specified callback for loading data.
        /// </summary>
        /// <param name="load">The callback to use for loading data</param>
        /// <param name="source">The source collection for this view. All updates to the
        /// source will be reflected in the view.
        /// </param>
        // public DomainCollectionView(Func<LoadOperation> load, IEnumerable<TItem> source)
        //    : base(load, source)
        // {
        // }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCollectionView"/>
        /// </summary>
        /// <param name="collectionViewLoader">The <see cref="CollectionViewLoader"/> to use for loading data</param>
        /// <param name="source">The source collection for this view. All updates to the
        /// source will be reflected in the view.
        /// </param>
        public DomainCollectionView(CollectionViewLoader collectionViewLoader, IEnumerable<TItem> source)
            : base(collectionViewLoader, source)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<TItem> GetEnumerator()
        {
            return this.CollectionView.Cast<TItem>().GetEnumerator();
        }

        #endregion
    }
}
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1202
#pragma warning disable SA1214
#pragma warning disable SA1311
#pragma warning restore SA1642 // Constructor summary documentation must begin with standard text