/* 
    Copyright (c) 2013, The Outercurve Foundation. 
    This software is released under the Apache License 2.0 (the "License");
    you may not use the software except in compliance with the License. 
    http://www.openriaservices.net/
*/

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
        /// Initializes a new instance of the <see cref="PagedCollectionViewBase"/>
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
                return this._canChangePage;
            }

            protected set
            {
                if (this._canChangePage != value)
                {
                    this._canChangePage = value;
                    this.RaisePropertyChanged("CanChangePage");
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="PageCount"/> is valid
        /// </summary>
        protected bool IsPageCountKnown
        {
            get { return (this.TotalItemCount != -1) && (this.PageSize > 0); }
        }

        /// <summary>
        /// Gets a value that indicates whether the page index is changing.
        /// </summary>
        public bool IsPageChanging
        {
            get
            {
                return this._isPageChanging;
            }

            private set
            {
                if (this._isPageChanging != value)
                {
                    this._isPageChanging = value;
                    this.RaisePropertyChanged("IsPageChanging");
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
                return this._itemCount;
            }

            protected set
            {
                if (this._itemCount != value)
                {
                    this._itemCount = value;
                    this.RaisePropertyChanged("ItemCount");
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
                if (!this.IsPageCountKnown)
                {
                    return -1;
                }

                return (this.TotalItemCount + this.PageSize - 1) / this.PageSize;
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
                if (this._pageIndex != value)
                {
                    this._pageIndex = value;
                    this.RaisePropertyChanged("PageIndex");
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
                return this._pageSize;
            }
            set
            {
                if (this._pageSize != value)
                {
                    this._pageSize = value;
                    this.RaisePropertyChanged("PageSize");
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
                return this._totalItemCount;
            }

            protected set
            {
                if (this._totalItemCount != value)
                {
                    this._totalItemCount = value;
                    this.RaisePropertyChanged("TotalItemCount");
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
            return this.MoveToPage(0);
        }

        /// <summary>
        /// Sets the last page as the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToLastPage()
        {
            if (!this.IsPageCountKnown)
            {
                return false;
            }

            return this.MoveToPage(this.PageCount - 1);
        }

        /// <summary>
        /// Moves to the page after the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToNextPage()
        {
            return this.MoveToPage(this.PageIndex + 1);
        }

        /// <summary>
        /// Moves to the page before the current page.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToPreviousPage()
        {
            return this.MoveToPage(this.PageIndex - 1);
        }

        /// <summary>
        /// Moves to the page at the specified index.
        /// </summary>
        /// <param name="pageIndex">The index of the page to move to.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool MoveToPage(int pageIndex)
        {
            if ((this.PageIndex == pageIndex) ||
                (pageIndex < -1) ||
                (this.IsPageCountKnown && (pageIndex >= this.PageCount)) ||
                !this.CanChangePage)
            {
                return false;
            }

            return this.BeginMoveToPage(pageIndex);
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
            this.OnPageChanging(e); // re-entrant
            if (e.Cancel)
            {
                return false;
            }

            this.IsPageChanging = true; // re-entrant

            this.BeginMoveToPageCore(pageIndex);

            return true;
        }

        /// <summary>
        /// Ends a page move by calling <see cref="EndMoveToPageCore"/> before updating state
        /// and raising events.
        /// </summary>
        /// <param name="pageIndex">The index of the page that was moved to</param>
        protected void EndMoveToPage(int pageIndex)
        {
            if (this.IsPageChanging)
            {
                this.EndMoveToPageCore(pageIndex);

                this.PageIndex = pageIndex; // re-entrant
                this.IsPageChanging = false; // re-entrant
                this.OnPageChanged(EventArgs.Empty);
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
            EventHandler<EventArgs> handler = this.PageChanged;
            if (handler != null)
            {
                handler(this, e);
            }
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

            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
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
    }
}
