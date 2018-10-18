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
#pragma warning disable SA1124 // Do not use regions
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// Abstract implementation of the <see cref="IPagedCollectionView"/> interface.
    /// </summary>
    /// <remarks>
    /// Derived classes will typically only have to implement <see cref="BeginMoveToPageCore"/>
    /// and set properties like <see cref="TotalItemCount"/>.
    /// </remarks>
    public abstract class PagedCollectionViewBase : IPagedCollectionView, INotifyPropertyChanged
    {
        #region Member Fields

        private bool _canChangePage = true;
        private int _itemCount;
        private bool _isPageChanging;
        private int _pageIndex = -1; // when _pageSize = 0
        private int _pageSize;
        private int _totalItemCount = -1; // unknown

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedCollectionViewBase"/> class.
        /// </summary>
        protected PagedCollectionViewBase()
        {
        }

        #endregion

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
        /// Raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the <see cref="PageIndex"/>
        /// value can change.
        /// </summary>
        public bool CanChangePage
        {
            get
            {
                return _canChangePage;
            }

            protected set
            {
                if (_canChangePage != value)
                {
                    _canChangePage = value;
                    RaisePropertyChanged("CanChangePage");
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="PageCount"/> is valid
        /// </summary>
        protected bool IsPageCountKnown => (TotalItemCount != -1) && (PageSize > 0);

        /// <summary>
        /// Gets a value that indicates whether the page index is changing.
        /// </summary>
        public bool IsPageChanging
        {
            get
            {
                return _isPageChanging;
            }

            private set
            {
                if (_isPageChanging != value)
                {
                    _isPageChanging = value;
                    RaisePropertyChanged("IsPageChanging");
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of known items in the view before paging is applied.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return _itemCount;
            }

            protected set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    RaisePropertyChanged("ItemCount");
                }
            }
        }

        /// <summary>
        /// Gets the number of pages in the view
        /// </summary>
        protected int PageCount
        {
            get
            {
                if (!IsPageCountKnown)
                {
                    return -1;
                }

                return (TotalItemCount + PageSize - 1) / PageSize;
            }
        }

        /// <summary>
        /// Gets the zero-based index of the current page.
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            private set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                    RaisePropertyChanged("PageIndex");
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of items to display on a page.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    RaisePropertyChanged("PageSize");
                }
            }
        }

        /// <summary>
        /// Gets or sets the total number of items in the view before paging is applied.
        /// </summary>
        public int TotalItemCount
        {
            get
            {
                return _totalItemCount;
            }

            protected set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    RaisePropertyChanged("TotalItemCount");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the first page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToFirstPage()
        {
            return MoveToPage(0);
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToLastPage()
        {
            if (!IsPageCountKnown)
            {
                return false;
            }

            return MoveToPage(PageCount - 1);
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToNextPage()
        {
            return MoveToPage(PageIndex + 1);
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToPreviousPage()
        {
            return MoveToPage(PageIndex - 1);
        }

        /// <summary>
        /// Moves to the page at the specified index.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToPage(int pageIndex)
        {
            if ((PageIndex == pageIndex) ||
                (pageIndex < -1) ||
                (IsPageCountKnown && (pageIndex >= PageCount)) ||
                !CanChangePage)
            {
                return false;
            }

            return BeginMoveToPage(pageIndex);
        }

        /// <summary>
        /// Begins a page move by raising events and updating state before calling
        /// <see cref="BeginMoveToPageCore"/>.
        /// </summary>
        /// <remarks>
        /// Every successful call to <see cref="BeginMoveToPage"/> should be matched by a call
        /// to <see cref="EndMoveToPage"/> when the page move is complete.
        /// </remarks>
        /// <param name="pageIndex">The index of the page to move to</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        protected bool BeginMoveToPage(int pageIndex)
        {
            PageChangingEventArgs e = new PageChangingEventArgs(pageIndex);
            OnPageChanging(e); // re-entrant
            if (e.Cancel)
            {
                return false;
            }

            IsPageChanging = true; // re-entrant

            BeginMoveToPageCore(pageIndex);

            return true;
        }

        /// <summary>
        /// Ends a page move by calling <see cref="EndMoveToPageCore"/> before updating state
        /// and raising events.
        /// </summary>
        /// <param name="pageIndex">The index of the page that was moved to</param>
        protected void EndMoveToPage(int pageIndex)
        {
            if (IsPageChanging)
            {
                EndMoveToPageCore(pageIndex);

                PageIndex = pageIndex; // re-entrant
                IsPageChanging = false; // re-entrant
                OnPageChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Invoked from <see cref="BeginMoveToPage"/> to move to the specified page.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to</param>
        protected abstract void BeginMoveToPageCore(int pageIndex);

        /// <summary>
        /// Invoked from <see cref="EndMoveToPage"/> after the specified page has been moved to.
        /// </summary>
        /// <param name="pageIndex">The index of the page that was moved to</param>
        protected virtual void EndMoveToPageCore(int pageIndex)
        {
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
        /// Raises a <see cref="PageChanging"/> event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnPageChanging(PageChangingEventArgs e)
        {
            PageChanging?.Invoke(this, e);
        }

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> event for the specified property
        /// </summary>
        /// <param name="propertyName">The property to raise the event for</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
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
    }
}
#pragma warning restore SA1201
#pragma warning restore SA1202
#pragma warning restore SA1214
#pragma warning restore SA1311
#pragma warning restore SA1124 // Do not use regions