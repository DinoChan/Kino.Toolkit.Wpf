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
    /// Abstract base class that is responsible for loading data for the source collection
    /// of the collection view.
    /// </summary>
    public abstract class CollectionViewLoader
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionViewLoader"/> class.
        /// </summary>
        protected CollectionViewLoader()
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the value of <see cref="CanLoad"/> changes
        /// </summary>
        public event EventHandler CanLoadChanged;

        /// <summary>
        /// Event raised when an asynchronous <see cref="Load"/> operation completes
        /// </summary>
        public event AsyncCompletedEventHandler LoadCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Load"/> can be successfully invoked
        /// </summary>
        public abstract bool CanLoad { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Asynchronously loads data into the source collection of the collection view
        /// </summary>
        /// <remarks>
        /// The <see cref="LoadCompleted"/> event will be raised upon successful completion as well as
        /// when cancellation and exceptions occur.
        /// </remarks>
        /// <param name="userState">The user state will be returned in the <see cref="LoadCompleted"/> event
        /// args. This parameter is optional.
        /// </param>
        /// <exception cref="InvalidOperationException"> is thrown when <see cref="CanLoad"/> is false</exception>
        public abstract void Load(object userState);

        /// <summary>
        /// Raises a <see cref="CanLoadChanged"/> event
        /// </summary>
        protected virtual void OnCanLoadChanged()
        {
            CanLoadChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a <see cref="LoadCompleted"/> event
        /// </summary>
        /// <param name="e">The event to raise</param>
        protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
        {
            LoadCompleted?.Invoke(this, e);
        }

        #endregion
    }
}
