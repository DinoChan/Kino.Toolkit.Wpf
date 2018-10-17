/*
    Copyright (c) 2013, The Outercurve Foundation.
    This software is released under the Apache License 2.0 (the "License");
    you may not use the software except in compliance with the License.
    http://www.openriaservices.net/
*/

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
    /// <typeparam name="TItem">The item type of this view</typeparam>
    public class DomainCollectionView<TItem> : DomainCollectionView, IEnumerable<TItem>
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
        //public DomainCollectionView(Func<LoadOperation> load, IEnumerable<TItem> source)
        //    : base(load, source)
        //{
        //}

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
            return CollectionView.Cast<TItem>().GetEnumerator();
        }

        #endregion
    }
}
