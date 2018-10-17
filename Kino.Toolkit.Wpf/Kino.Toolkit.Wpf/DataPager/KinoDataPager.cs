//-----------------------------------------------------------------------
// <copyright file="DataPager.cs" company="Microsoft">
//      (c) Copyright Microsoft Corporation.
//      This source is subject to the Microsoft Public License (Ms-PL).
//      Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//      All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1202
#pragma warning disable SA1214
#pragma warning disable SA1311
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// Handles paging for an <see cref="T:System.ComponentModel.IPagedCollectionView" />.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplatePart(Name = DATAPAGER_elementFirstPageButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DATAPAGER_elementPreviousPageButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DATAPAGER_elementCurrentPagePrefixTextBlockn, Type = typeof(TextBlock))]
    [TemplatePart(Name = DATAPAGER_elementCurrentPageSuffixTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = DATAPAGER_elementCurrentPageTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = DATAPAGER_elementNextPageButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DATAPAGER_elementLastPageButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DATAPAGER_elementNumericButtonPanel, Type = typeof(Panel))]
    [TemplateVisualState(Name = DATAPAGER_stateNormal, GroupName = DATAPAGER_groupCommon)]
    [TemplateVisualState(Name = DATAPAGER_stateDisabled, GroupName = DATAPAGER_groupCommon)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveEnabled, GroupName = DATAPAGER_groupMove)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveDisabled, GroupName = DATAPAGER_groupMove)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveFirstEnabled, GroupName = DATAPAGER_groupMoveFirst)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveFirstDisabled, GroupName = DATAPAGER_groupMoveFirst)]
    [TemplateVisualState(Name = DATAPAGER_stateMovePreviousEnabled, GroupName = DATAPAGER_groupMovePrevious)]
    [TemplateVisualState(Name = DATAPAGER_stateMovePreviousDisabled, GroupName = DATAPAGER_groupMovePrevious)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveNextEnabled, GroupName = DATAPAGER_groupMoveNext)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveNextDisabled, GroupName = DATAPAGER_groupMoveNext)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveLastEnabled, GroupName = DATAPAGER_groupMoveLast)]
    [TemplateVisualState(Name = DATAPAGER_stateMoveLastDisabled, GroupName = DATAPAGER_groupMoveLast)]
    [TemplateVisualState(Name = DATAPAGER_stateTotalPageCountKnown, GroupName = DATAPAGER_groupTotalPageCountKnown)]
    [TemplateVisualState(Name = DATAPAGER_stateTotalPageCountUnknown, GroupName = DATAPAGER_groupTotalPageCountKnown)]
    [TemplateVisualState(Name = DATAPAGER_stateFirstLastNumeric, GroupName = DATAPAGER_groupDisplayMode)]
    [TemplateVisualState(Name = DATAPAGER_stateFirstLastPreviousNext, GroupName = DATAPAGER_groupDisplayMode)]
    [TemplateVisualState(Name = DATAPAGER_stateFirstLastPreviousNextNumeric, GroupName = DATAPAGER_groupDisplayMode)]
    [TemplateVisualState(Name = DATAPAGER_stateNumeric, GroupName = DATAPAGER_groupDisplayMode)]
    [TemplateVisualState(Name = DATAPAGER_statePreviousNext, GroupName = DATAPAGER_groupDisplayMode)]
    [TemplateVisualState(Name = DATAPAGER_statePreviousNextNumeric, GroupName = DATAPAGER_groupDisplayMode)]
    [StyleTypedProperty(Property = DATAPAGER_styleNumericButton, StyleTargetType = typeof(ToggleButton))]
    public class KinoDataPager : Control
    {
        ////------------------------------------------------------
        ////
        ////  Static Fields and Constants
        ////
        ////------------------------------------------------------

        #region Constants

        // Automation Id constants
        private const string DATAPAGER_currentPageTextBoxAutomationId = "CurrentPage";
        private const string DATAPAGER_firstPageButtonAutomationId = "LargeDecrement";
        private const string DATAPAGER_lastPageButtonAutomationId = "LargeIncrement";
        private const string DATAPAGER_nextPageButtonAutomationId = "SmallIncrement";
        private const string DATAPAGER_numericalButtonAutomationId = "MoveToPage";
        private const string DATAPAGER_previousPageButtonAutomationId = "SmallDecrement";

        // Parts constants
        private const string DATAPAGER_elementCurrentPagePrefixTextBlockn = "CurrentPagePrefixTextBlock";
        private const string DATAPAGER_elementCurrentPageSuffixTextBlock = "CurrentPageSuffixTextBlock";
        private const string DATAPAGER_elementCurrentPageTextBox = "CurrentPageTextBox";
        private const string DATAPAGER_elementFirstPageButton = "FirstPageButton";
        private const string DATAPAGER_elementLastPageButton = "LastPageButton";
        private const string DATAPAGER_elementNextPageButton = "NextPageButton";
        private const string DATAPAGER_elementNumericButtonPanel = "NumericButtonPanel";
        private const string DATAPAGER_elementPreviousPageButton = "PreviousPageButton";

        // Styles constants
        private const string DATAPAGER_styleNumericButton = "NumericButtonStyle";

        // Common states constants
        private const string DATAPAGER_groupCommon = "CommonStates";
        private const string DATAPAGER_stateNormal = "Normal";
        private const string DATAPAGER_stateDisabled = "Disabled";

        // Move states constants
        private const string DATAPAGER_groupMove = "MoveStates";
        private const string DATAPAGER_stateMoveEnabled = "MoveEnabled";
        private const string DATAPAGER_stateMoveDisabled = "MoveDisabled";

        // MoveFirst states constants
        private const string DATAPAGER_groupMoveFirst = "MoveFirstStates";
        private const string DATAPAGER_stateMoveFirstEnabled = "MoveFirstEnabled";
        private const string DATAPAGER_stateMoveFirstDisabled = "MoveFirstDisabled";

        // MovePrevious states constants
        private const string DATAPAGER_groupMovePrevious = "MovePreviousStates";
        private const string DATAPAGER_stateMovePreviousEnabled = "MovePreviousEnabled";
        private const string DATAPAGER_stateMovePreviousDisabled = "MovePreviousDisabled";

        // MovePrevious states constants
        private const string DATAPAGER_groupMoveNext = "MoveNextStates";
        private const string DATAPAGER_stateMoveNextEnabled = "MoveNextEnabled";
        private const string DATAPAGER_stateMoveNextDisabled = "MoveNextDisabled";

        // MovePrevious states constants
        private const string DATAPAGER_groupMoveLast = "MoveLastStates";
        private const string DATAPAGER_stateMoveLastEnabled = "MoveLastEnabled";
        private const string DATAPAGER_stateMoveLastDisabled = "MoveLastDisabled";

        // TotalPageCountKnown states constants
        private const string DATAPAGER_groupTotalPageCountKnown = "TotalPageCountKnownStates";
        private const string DATAPAGER_stateTotalPageCountKnown = "TotalPageCountKnown";
        private const string DATAPAGER_stateTotalPageCountUnknown = "TotalPageCountUnknown";

        // DisplayModeStates states constants
        private const string DATAPAGER_groupDisplayMode = "DisplayModeStates";
        private const string DATAPAGER_stateFirstLastNumeric = "FirstLastNumeric";
        private const string DATAPAGER_stateFirstLastPreviousNext = "FirstLastPreviousNext";
        private const string DATAPAGER_stateFirstLastPreviousNextNumeric = "FirstLastPreviousNextNumeric";
        private const string DATAPAGER_stateNumeric = "Numeric";
        private const string DATAPAGER_statePreviousNext = "PreviousNext";
        private const string DATAPAGER_statePreviousNextNumeric = "PreviousNextNumeric";

        // Default property value constants
        private const PagerDisplayMode DATAPAGER_defaultDisplayMode = PagerDisplayMode.FirstLastPreviousNext;
        private const int DATAPAGER_defaultNumericButtonCount = 5;
        private const int DATAPAGER_defaultPageIndex = -1;

        #endregion Constants

        #region Static Fields

        /// <summary>
        /// Identifies the AutoEllipsis dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoEllipsisProperty =
            DependencyProperty.Register(
                "AutoEllipsis",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnAutoEllipsisPropertyChanged));

        /// <summary>
        /// Identifies the CanChangePage dependency property.
        /// </summary>
        public static readonly DependencyProperty CanChangePageProperty =
            DependencyProperty.Register(
                "CanChangePage",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the CanMoveToFirstPage dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToFirstPageProperty =
            DependencyProperty.Register(
                "CanMoveToFirstPage",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the CanMoveToLastPage dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToLastPageProperty =
            DependencyProperty.Register(
                "CanMoveToLastPage",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the CanMoveToNextPage dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToNextPageProperty =
            DependencyProperty.Register(
                "CanMoveToNextPage",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the CanMoveToPreviousPage dependency property.
        /// </summary>
        public static readonly DependencyProperty CanMoveToPreviousPageProperty =
            DependencyProperty.Register(
                "CanMoveToPreviousPage",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the DisplayMode dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(
                "DisplayMode",
                typeof(PagerDisplayMode),
                typeof(KinoDataPager),
                new PropertyMetadata(DATAPAGER_defaultDisplayMode, OnDisplayModePropertyChanged));

        /// <summary>
        /// Identifies the IsTotalItemCountFixed dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTotalItemCountFixedProperty =
            DependencyProperty.Register(
                "IsTotalItemCountFixed",
                typeof(bool),
                typeof(KinoDataPager),
                new PropertyMetadata(OnIsTotalItemCountFixedPropertyChanged));

        /// <summary>
        /// Identifies the ItemCount dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemCountProperty =
            DependencyProperty.Register(
                "ItemCount",
                typeof(int),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the NumericButtonCount dependency property.
        /// </summary>
        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register(
                "NumericButtonCount",
                typeof(int),
                typeof(KinoDataPager),
                new PropertyMetadata(DATAPAGER_defaultNumericButtonCount, OnNumericButtonCountPropertyChanged));

        /// <summary>
        /// Identifies the NumericButtonStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty NumericButtonStyleProperty =
            DependencyProperty.Register(
                "NumericButtonStyle",
                typeof(Style),
                typeof(KinoDataPager),
                new PropertyMetadata(OnNumericButtonStylePropertyChanged));

        /// <summary>
        /// Identifies the PageCount dependency property.
        /// </summary>
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register(
                "PageCount",
                typeof(int),
                typeof(KinoDataPager),
                new PropertyMetadata(OnReadOnlyPropertyChanged));

        /// <summary>
        /// Identifies the PageIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(
                "PageIndex",
                typeof(int),
                typeof(KinoDataPager),
                new PropertyMetadata(DATAPAGER_defaultPageIndex, OnPageIndexPropertyChanged));

        /// <summary>
        /// Identifies the PageSize dependency property.
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(
                "PageSize",
                typeof(int),
                typeof(KinoDataPager),
                new PropertyMetadata(OnPageSizePropertyChanged));

        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(IEnumerable),
                typeof(KinoDataPager),
                new PropertyMetadata(OnSourcePropertyChanged));

        #endregion Static Fields

        ////------------------------------------------------------
        ////
        ////  Private Fields
        ////
        ////------------------------------------------------------

        #region Private Fields

        /// <summary>
        /// Private accessor for the text block appearing before the current page text box.
        /// </summary>
        private TextBlock _currentPagePrefixTextBlock;

        /// <summary>
        /// Private accessor for the text block appearing after the current page text box.
        /// </summary>
        private TextBlock _currentPageSuffixTextBlock;

        /// <summary>
        /// Private accessor for the current page text box.
        /// </summary>
        private TextBox _currentPageTextBox;

        /// <summary>
        /// Private accessor for the first page ButtonBase.
        /// </summary>
        private ButtonBase _firstPageButtonBase;

        /// <summary>
        /// Page index corresponding to the ToggleButton that has keyboard focus
        /// -1 if no ToggleButton has focus.
        /// </summary>
        private int _focusedToggleButtonIndex = -1;

        /// <summary>
        /// Set to True when the ToggleButton_Checked notification needs to be ignored.
        /// </summary>
        private bool _ignoreToggleButtonCheckedNotification;

        /// <summary>
        /// Set to True when the ToggleButton_GotFocus and ToggleButton_LostFocus
        /// notifications need to be ignored.
        /// </summary>
        private bool _ignoreToggleButtonFocusNotification;

        /// <summary>
        /// Set to True when a ToggleButton_Unchecked notification needs to be ignored.
        /// </summary>
        private bool _ignoreToggleButtonUncheckedNotification;

        /// <summary>
        /// Private accessor for the last page ButtonBase.
        /// </summary>
        private ButtonBase _lastPageButtonBase;

        /// <summary>
        /// Set to True when a PageChanging notification is expected to be raised
        /// before the next PagedChanged notification.
        /// </summary>
        private bool _needPageChangingNotification = true;

        /// <summary>
        /// Private accessor for the next page ButtonBase.
        /// </summary>
        private ButtonBase _nextPageButtonBase;

        /// <summary>
        /// Private accessor for the panel hosting the buttons.
        /// </summary>
        private Panel _numericButtonPanel;

        /// <summary>
        /// Private accessor for the previous page ButtonBase.
        /// </summary>
        private ButtonBase _previousPageButtonBase;

        /// <summary>
        /// The new index of the current page, used to change the
        /// current page when a user enters something into the
        /// current page text box.
        /// </summary>
        private int _requestedPageIndex;

        /// <summary>
        /// Holds the weak event listener for the INotifyPropertyChanged.PropertyChanged event.
        /// </summary>
        private WeakEventListener<KinoDataPager, object, PropertyChangedEventArgs> _weakEventListenerPropertyChanged;

        /// <summary>
        /// Delegate for calling page move operations
        /// </summary>
        /// <returns>Boolean value for whether the operation succeeded</returns>
        private delegate bool PageMoveOperationDelegate();

        #endregion Private Fields

        ////------------------------------------------------------
        ////
        ////  Constructors
        ////
        ////------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataPager class.
        /// </summary>
        public KinoDataPager()
        {
            DefaultStyleKey = typeof(KinoDataPager);

            // Listening to the IsEnabled changes so the DataPager states can be updated accordingly.
            IsEnabledChanged += new DependencyPropertyChangedEventHandler(OnDataPagerIsEnabledChanged);
        }

        #endregion Constructors

        ////------------------------------------------------------
        ////
        ////  Events
        ////
        ////------------------------------------------------------

        #region Events

        /// <summary>
        /// EventHandler for when PageIndex is changing.
        /// </summary>
        public event EventHandler<CancelEventArgs> PageIndexChanging;

        /// <summary>
        /// EventHandler for when PageIndex has changed.
        /// </summary>
        public event EventHandler<EventArgs> PageIndexChanged;

        #endregion Events

        ////------------------------------------------------------
        ////
        ////  Public Properties
        ////
        ////------------------------------------------------------

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether or not to use an ellipsis as the last button.
        /// </summary>
        public bool AutoEllipsis
        {
            get
            {
                return (bool)GetValue(AutoEllipsisProperty);
            }

            set
            {
                SetValue(AutoEllipsisProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the user is allowed to move to another page
        /// </summary>
        public bool CanChangePage
        {
            get
            {
                return (bool)GetValue(CanChangePageProperty);
            }

            private set
            {
                this.SetValueNoCallback(CanChangePageProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the <see cref="T:System.Windows.Controls.DataPager" /> will
        /// allow the user to attempt to move to the first page if <see cref="P:System.Windows.Controls.DataPager.CanChangePage" /> is true.
        /// </summary>
        public bool CanMoveToFirstPage
        {
            get
            {
                return (bool)GetValue(CanMoveToFirstPageProperty);
            }

            private set
            {
                this.SetValueNoCallback(CanMoveToFirstPageProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the <see cref="T:System.Windows.Controls.DataPager" />
        /// will allow the user to attempt to move to the last page if <see cref="P:System.Windows.Controls.DataPager.CanChangePage" /> is true.
        /// </summary>
        public bool CanMoveToLastPage
        {
            get
            {
                return (bool)GetValue(CanMoveToLastPageProperty);
            }

            private set
            {
                this.SetValueNoCallback(CanMoveToLastPageProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the <see cref="T:System.Windows.Controls.DataPager" />
        /// will allow the user to attempt to move to the next page if<see cref="P:System.Windows.Controls.DataPager.CanChangePage" /> is true.
        /// </summary>
        public bool CanMoveToNextPage
        {
            get
            {
                return (bool)GetValue(CanMoveToNextPageProperty);
            }

            private set
            {
                this.SetValueNoCallback(CanMoveToNextPageProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that indicates whether or not the <see cref="T:System.Windows.Controls.DataPager" />
        /// will allow the user to attempt to move to the previous page if <see cref="P:System.Windows.Controls.DataPager.CanChangePage" /> is true.
        /// </summary>
        public bool CanMoveToPreviousPage
        {
            get
            {
                return (bool)GetValue(CanMoveToPreviousPageProperty);
            }

            private set
            {
                this.SetValueNoCallback(CanMoveToPreviousPageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates how the
        /// <see cref="T:System.Windows.Controls.DataPager" /> user interface is displayed
        /// </summary>
        public PagerDisplayMode DisplayMode
        {
            get
            {
                return (PagerDisplayMode)GetValue(DisplayModeProperty);
            }

            set
            {
                SetValue(DisplayModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether or not the total number of items in the collection is fixed.
        /// </summary>
        public bool IsTotalItemCountFixed
        {
            get
            {
                return (bool)GetValue(IsTotalItemCountFixedProperty);
            }

            set
            {
                SetValue(IsTotalItemCountFixedProperty, value);
            }
        }

        /// <summary>
        /// Gets the current number of known items in the <see cref="T:System.ComponentModel.IPagedCollectionView" /> .
        /// </summary>
        public int ItemCount
        {
            get
            {
                return (int)GetValue(ItemCountProperty);
            }

            private set
            {
                this.SetValueNoCallback(ItemCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.IPagedCollectionView" /> .
        /// </summary>
        public IEnumerable Source
        {
            get
            {
                return GetValue(SourceProperty) as IEnumerable;
            }

            set
            {
                SetValue(SourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the number of page buttons shown
        /// on the <see cref="T:System.Windows.Controls.DataPager" /> user interface.
        /// </summary>
        public int NumericButtonCount
        {
            get
            {
                return (int)GetValue(NumericButtonCountProperty);
            }

            set
            {
                SetValue(NumericButtonCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the style that will be used for the numeric buttons.
        /// </summary>
        public Style NumericButtonStyle
        {
            get
            {
                return (Style)GetValue(NumericButtonStyleProperty);
            }

            set
            {
                SetValue(NumericButtonStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets the current number of known pages in the <see cref="T:System.ComponentModel.IPagedCollectionView" /> .
        /// </summary>
        public int PageCount
        {
            get
            {
                return (int)GetValue(PageCountProperty);
            }

            private set
            {
                this.SetValueNoCallback(PageCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="P:System.ComponentModel.IPagedCollectionView.PageIndex" />
        /// in the <see cref="T:System.ComponentModel.IPagedCollectionView" /> .
        /// </summary>
        [DefaultValueAttribute(-1)]
        public int PageIndex
        {
            get
            {
                return (int)GetValue(PageIndexProperty);
            }

            set
            {
                SetValue(PageIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="P:System.ComponentModel.IPagedCollectionView.PageSize" /> in the <see cref="T:System.ComponentModel.IPagedCollectionView" /> .
        /// </summary>
        public int PageSize
        {
            get
            {
                return (int)GetValue(PageSizeProperty);
            }

            set
            {
                SetValue(PageSizeProperty, value);
            }
        }

        #endregion Public Properties

        ////------------------------------------------------------
        ////
        ////  Internal Properties
        ////
        ////------------------------------------------------------

        #region Internal Properties

        /// <summary>
        /// Gets the TextBox holding the current PageIndex value, if any.
        /// </summary>
        internal TextBox CurrentPageTextBox => _currentPageTextBox;

        /// <summary>
        /// Gets the Source as an IPagedCollectionView
        /// </summary>
        internal IPagedCollectionView PagedSource => Source as IPagedCollectionView;

        #endregion Internal Properties

        ////------------------------------------------------------
        ////
        ////  Public Methods
        ////
        ////------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Applies the control's template, retrieves the elements
        /// within it, and sets up events.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // unsubscribe event handlers for previous template parts
            if (_firstPageButtonBase != null)
            {
                _firstPageButtonBase.Click -= new RoutedEventHandler(OnFirstPageButtonBaseClick);
            }

            if (_previousPageButtonBase != null)
            {
                _previousPageButtonBase.Click -= new RoutedEventHandler(OnPreviousPageButtonBaseClick);
            }

            if (_nextPageButtonBase != null)
            {
                _nextPageButtonBase.Click -= new RoutedEventHandler(OnNextPageButtonBaseClick);
            }

            if (_lastPageButtonBase != null)
            {
                _lastPageButtonBase.Click -= new RoutedEventHandler(OnLastPageButtonBaseClick);
            }

            if (_currentPageTextBox != null)
            {
                _currentPageTextBox.KeyDown -= new System.Windows.Input.KeyEventHandler(OnCurrentPageTextBoxKeyDown);
                _currentPageTextBox.LostFocus -= new RoutedEventHandler(OnCurrentPageTextBoxLostFocus);
            }

            // get new template parts
            _firstPageButtonBase = GetTemplateChild(DATAPAGER_elementFirstPageButton) as ButtonBase;
            _previousPageButtonBase = GetTemplateChild(DATAPAGER_elementPreviousPageButton) as ButtonBase;
            _nextPageButtonBase = GetTemplateChild(DATAPAGER_elementNextPageButton) as ButtonBase;
            _lastPageButtonBase = GetTemplateChild(DATAPAGER_elementLastPageButton) as ButtonBase;

            if (_firstPageButtonBase != null)
            {
                _firstPageButtonBase.Click += new RoutedEventHandler(OnFirstPageButtonBaseClick);
                AutomationProperties.SetAutomationId(_firstPageButtonBase, DATAPAGER_firstPageButtonAutomationId);
            }

            if (_previousPageButtonBase != null)
            {
                _previousPageButtonBase.Click += new RoutedEventHandler(OnPreviousPageButtonBaseClick);
                AutomationProperties.SetAutomationId(_previousPageButtonBase, DATAPAGER_previousPageButtonAutomationId);
            }

            if (_nextPageButtonBase != null)
            {
                _nextPageButtonBase.Click += new RoutedEventHandler(OnNextPageButtonBaseClick);
                AutomationProperties.SetAutomationId(_nextPageButtonBase, DATAPAGER_nextPageButtonAutomationId);
            }

            if (_lastPageButtonBase != null)
            {
                _lastPageButtonBase.Click += new RoutedEventHandler(OnLastPageButtonBaseClick);
                AutomationProperties.SetAutomationId(_lastPageButtonBase, DATAPAGER_lastPageButtonAutomationId);
            }

            // remove previous panel + buttons.
            if (_numericButtonPanel != null)
            {
                _numericButtonPanel.Children.Clear();
            }

            _numericButtonPanel = GetTemplateChild(DATAPAGER_elementNumericButtonPanel) as Panel;

            // add new buttons to panel
            if (_numericButtonPanel != null)
            {
                if (_numericButtonPanel.Children.Count > 0)
                {
                    throw new InvalidOperationException(PagerResources.InvalidButtonPanelContent);
                }

                UpdateButtonCount();
            }

            _currentPageTextBox = GetTemplateChild(DATAPAGER_elementCurrentPageTextBox) as TextBox;
            _currentPagePrefixTextBlock = GetTemplateChild(DATAPAGER_elementCurrentPagePrefixTextBlockn) as TextBlock;
            _currentPageSuffixTextBlock = GetTemplateChild(DATAPAGER_elementCurrentPageSuffixTextBlock) as TextBlock;

            if (_currentPageTextBox != null)
            {
                _currentPageTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(OnCurrentPageTextBoxKeyDown);
                _currentPageTextBox.LostFocus += new RoutedEventHandler(OnCurrentPageTextBoxLostFocus);
                AutomationProperties.SetAutomationId(_currentPageTextBox, DATAPAGER_currentPageTextBoxAutomationId);
            }

            UpdateControl();
        }

        #endregion Public Methods

        ////------------------------------------------------------
        ////
        ////  Protected Methods
        ////
        ////------------------------------------------------------

        #region Protected Methods

        #endregion Protected Methods

        ////------------------------------------------------------
        ////
        ////  Private Static Methods
        ////
        ////------------------------------------------------------
        #region Private Static Methods

        /// <summary>
        /// AutoEllipsis property changed handler.
        /// </summary>
        /// <param name="d">NumericButton that changed its AutoEllipsis.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnAutoEllipsisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;
            pager.UpdateButtonDisplay();
        }

        /// <summary>
        /// DisplayMode property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its DisplayMode.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;
            if (!pager.AreHandlersSuspended())
            {
                if (!Enum.IsDefined(typeof(PagerDisplayMode), e.NewValue))
                {
                    pager.SetValueNoCallback(e.Property, e.OldValue);
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            PagerResources.InvalidEnumArgumentException_InvalidEnumArgument,
                            "value",
                            e.NewValue.ToString(),
                            typeof(PagerDisplayMode).Name));
                }

                pager.UpdateControl();
            }
        }

        /// <summary>
        /// IsTotalItemCountFixed property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed IsTotalItemCountFixed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnIsTotalItemCountFixedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;
            pager.UpdateControl();
        }

        /// <summary>
        /// NumericButtonCount property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its NumericButtonCount.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnNumericButtonCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;

            if (!pager.AreHandlersSuspended())
            {
                if ((int)e.NewValue < 0)
                {
                    pager.SetValueNoCallback(e.Property, e.OldValue);
                    throw new ArgumentOutOfRangeException(
                        "value",
                        string.Format(
                            CultureInfo.InvariantCulture,
                            PagerResources.ValueMustBeGreaterThanOrEqualTo,
                            "NumericButtonCount",
                            0));
                }

                pager.UpdateButtonCount();
            }
        }

        /// <summary>
        /// NumericButtonStyle property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its NumericButtonStyle.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnNumericButtonStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;

            if (pager._numericButtonPanel != null)
            {
                // update button styles
                foreach (UIElement uiElement in pager._numericButtonPanel.Children)
                {
                    if (uiElement is ToggleButton button)
                    {
                        button.Style = pager.NumericButtonStyle;
                    }
                }
            }
        }

        /// <summary>
        /// PageIndex property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its PageIndex.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnPageIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;

            if (!pager.AreHandlersSuspended())
            {
                int newPageIndex = (int)e.NewValue;

                if ((pager.Source == null || pager.PageSize == 0) && newPageIndex != -1)
                {
                    pager.SetValueNoCallback(e.Property, e.OldValue);
                    throw new ArgumentOutOfRangeException(
                        "value",
                        PagerResources.PageIndexMustBeNegativeOne);
                }

                if (pager.Source != null && pager.PageSize != 0 && newPageIndex < 0)
                {
                    pager.SetValueNoCallback(e.Property, e.OldValue);
                    throw new ArgumentOutOfRangeException(
                        "value",
                        string.Format(
                            CultureInfo.InvariantCulture,
                            PagerResources.ValueMustBeGreaterThanOrEqualTo,
                            "PageIndex",
                            0));
                }

                if (pager.PagedSource != null)
                {
                    if (newPageIndex != pager.PagedSource.PageIndex)
                    {
                        pager.PageMoveHandler((int)e.OldValue, newPageIndex, null);
                    }
                }
                else if (pager.Source != null)
                {
                    if (pager.PageSize != 0 && newPageIndex != 0)
                    {
                        // When the Source is an IEnumerable the PageIndex must be 0
                        pager.SetValueNoCallback(e.Property, e.OldValue);
                    }
                    else
                    {
                        // PageIndex changes are not cancellable when the Source is an IEnumerable.
                        pager.RaisePageIndexChangeEvents(true /*raisePageChanged*/);
                    }
                }
                else if (newPageIndex == -1)
                {
                    // Source is reset and PageIndex goes from >= 0 to -1
                    pager.RaisePageIndexChangeEvents(true /*raisePageChanged*/);
                }
                else
                {
                    // keep value set to -1 if there is no source
                    pager.SetValueNoCallback(e.Property, -1);
                }
            }
        }

        /// <summary>
        /// PageSize property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its PageSize.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnPageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;

            if (!pager.AreHandlersSuspended())
            {
                int newPageSize = (int)e.NewValue;

                if (newPageSize < 0)
                {
                    pager.SetValueNoCallback(e.Property, e.OldValue);
                    throw new ArgumentOutOfRangeException(
                        "value",
                        string.Format(
                            CultureInfo.InvariantCulture,
                            PagerResources.ValueMustBeGreaterThanOrEqualTo,
                            "PageSize",
                            0));
                }

                if (pager.PagedSource != null)
                {
                    try
                    {
                        pager.PagedSource.PageSize = newPageSize;
                    }
                    catch
                    {
                        pager.SetValueNoCallback(e.Property, e.OldValue);
                        throw;
                    }
                }
                else if (pager.Source != null)
                {
                    pager.PageIndex = pager.PageSize == 0 ? -1 : 0;
                }

                pager.UpdateControl();
            }
        }

        /// <summary>
        /// Called when a Read-Only dependency property is changed
        /// </summary>
        /// <param name="d">DataPager that changed its read-only property.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KinoDataPager pager && !pager.AreHandlersSuspended())
            {
                pager.SetValueNoCallback(e.Property, e.OldValue);
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        PagerResources.UnderlyingPropertyIsReadOnly,
                        e.Property.ToString()));
            }
        }

        /// <summary>
        /// SourceProperty property changed handler.
        /// </summary>
        /// <param name="d">DataPager that changed its Source.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoDataPager pager = d as KinoDataPager;

            if (e.OldValue is INotifyPropertyChanged oldNotifyPropertyChanged && pager._weakEventListenerPropertyChanged != null)
            {
                pager._weakEventListenerPropertyChanged.Detach();
                pager._weakEventListenerPropertyChanged = null;
            }

            if (e.NewValue is IPagedCollectionView newPagedCollectionView)
            {
                if (e.NewValue is INotifyPropertyChanged newNotifyPropertyChanged)
                {
                    pager._weakEventListenerPropertyChanged = new WeakEventListener<KinoDataPager, object, PropertyChangedEventArgs>(pager)
                    {
                        OnEventAction = (instance, source, eventArgs) => instance.OnSourcePropertyChanged(source, eventArgs),
                        OnDetachAction = (weakEventListener) => newNotifyPropertyChanged.PropertyChanged -= weakEventListener.OnEvent
                    };
                    newNotifyPropertyChanged.PropertyChanged += pager._weakEventListenerPropertyChanged.OnEvent;
                }

                if (pager.PageSize != 0)
                {
                    newPagedCollectionView.PageSize = pager.PageSize;
                }
                else
                {
                    pager.PageSize = newPagedCollectionView.PageSize;
                }

                if (pager.PageIndex != newPagedCollectionView.PageIndex)
                {
                    if (newPagedCollectionView.PageIndex == -1 && newPagedCollectionView.IsPageChanging)
                    {
                        // Avoid ArgumentOutOfRangeException in situation where the
                        // IPagedCollectionView's PageIndex is still set to -1 while
                        // a page move is in progress
                        pager.SetValueNoCallback(KinoDataPager.PageIndexProperty, -1);
                    }
                    else
                    {
                        pager.PageIndex = newPagedCollectionView.PageIndex;
                    }

                    // Raise PageIndex change notifications for a non-cancellable change
                    pager.RaisePageIndexChangeEvents(true /*raisePageChanged*/);
                }

                pager.ItemCount = newPagedCollectionView.ItemCount;
                pager.UpdatePageCount();
                if (newPagedCollectionView.IsPageChanging)
                {
                    // Raise non-cancellable PageIndex changing notification since the source is already
                    // in the middle of a page change
                    pager.RaisePageIndexChangeEvents(false /*raisePageChanged*/);
                }
            }
            else
            {
                if (e.NewValue is IEnumerable enumerable)
                {
                    IEnumerable<object> genericEnumerable = enumerable.Cast<object>();
                    pager.ItemCount = genericEnumerable.Count();
                    pager.PageCount = 1;
                    pager.PageIndex = pager.PageSize == 0 ? -1 : 0;
                }
                else
                {
                    pager.ItemCount = 0;
                    pager.PageCount = 0;
                    pager.PageIndex = -1;
                }
            }

            pager.UpdateControl();
        }

        #endregion Private Static Methods

        ////------------------------------------------------------
        ////
        ////  Private Methods
        ////
        ////------------------------------------------------------

        #region Private Methods

        /// <summary>
        /// Gets the starting index that our buttons should be labeled with.
        /// </summary>
        /// <returns>Starting index for our buttons</returns>
        private int GetButtonStartIndex()
        {
            // Because we have a starting PageIndex, we want to try and center the current pages
            // around this value. But if we are at the end of the collection, we display the last
            // available buttons.
            return Math.Min(
                Math.Max((PageIndex + 1) - (NumericButtonCount / 2), 1), /* center buttons around pageIndex */
                Math.Max(PageCount - NumericButtonCount + 1, 1));        /* lastPage - number of buttons */
        }

        /// <summary>
        /// Attempts to move the current page index to the value
        /// in the current page textbox.
        /// </summary>
        private void MoveCurrentPageToTextboxValue()
        {
            if (_currentPageTextBox.Text != (PageIndex + 1).ToString(CultureInfo.CurrentCulture))
            {
                if (PagedSource != null && TryParseTextBoxPage())
                {
                    MoveToRequestedPage();
                }

                _currentPageTextBox.Text = (PageIndex + 1).ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Given the new value of _requestedPageIndex, this method will attempt a page move
        /// and set the _currentPageIndex variable accordingly.
        /// </summary>
        private void MoveToRequestedPage()
        {
            if (_requestedPageIndex >= 0 && _requestedPageIndex < PageCount)
            {
                // Requested page is within the known range
                PageIndex = _requestedPageIndex;
            }
            else if (_requestedPageIndex >= PageCount)
            {
                if (IsTotalItemCountFixed && PagedSource.TotalItemCount != -1)
                {
                    PageIndex = PageCount - 1;
                }
                else
                {
                    PageIndex = _requestedPageIndex;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event on the current page text box.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnCurrentPageTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                MoveCurrentPageToTextboxValue();
            }
        }

        /// <summary>
        /// Handles the loss of focus for the current page text box.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnCurrentPageTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            MoveCurrentPageToTextboxValue();
        }

        /// <summary>
        /// Handles the notifications for the DataPager.IsEnabled changes
        /// </summary>
        /// <param name="sender">DataPager that changed its IsEnabled property</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for this event.</param>
        private void OnDataPagerIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateCommonState();
        }

        /// <summary>
        /// Handles the click of the first page ButtonBase.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnFirstPageButtonBaseClick(object sender, RoutedEventArgs e)
        {
            if (PagedSource != null)
            {
                int oldPageIndex = PagedSource.PageIndex;
                if (oldPageIndex != 0)
                {
                    PageMoveHandler(oldPageIndex, -1, PagedSource.MoveToFirstPage);
                }
            }
        }

        /// <summary>
        /// Handles the click of the last page ButtonBase.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnLastPageButtonBaseClick(object sender, RoutedEventArgs e)
        {
            if (PagedSource != null)
            {
                int oldPageIndex = PagedSource.PageIndex;
                if (oldPageIndex != PageCount)
                {
                    PageMoveHandler(oldPageIndex, -1, PagedSource.MoveToLastPage);
                }
            }
        }

        /// <summary>
        /// Handles the click of the next page ButtonBase.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnNextPageButtonBaseClick(object sender, RoutedEventArgs e)
        {
            if (PagedSource != null)
            {
                int oldPageIndex = PagedSource.PageIndex;
                if (oldPageIndex != PageIndex + 1)
                {
                    PageMoveHandler(oldPageIndex, -1, PagedSource.MoveToNextPage);
                }
            }
        }

        /// <summary>
        /// Handles the click of the previous page ButtonBase.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnPreviousPageButtonBaseClick(object sender, RoutedEventArgs e)
        {
            if (PagedSource != null)
            {
                int oldPageIndex = PagedSource.PageIndex;
                if (oldPageIndex != PageIndex - 1)
                {
                    PageMoveHandler(oldPageIndex, -1, PagedSource.MoveToPreviousPage);
                }
            }
        }

        /// <summary>
        /// This helper method will take care of calling the specified page move
        /// operation on the source collection, or MoveToPage if left null, while
        /// also firing the PageIndexChanging and PageIndexChanged events.
        /// </summary>
        /// <param name="oldPageIndex">The oldPageIndex value before we change pages</param>
        /// <param name="newPageIndex">The page index to use with MoveToPage. This argument is ignored otherwise</param>
        /// <param name="pageMoveOperation">The delegate to call, or null when MoveToPage must be called</param>
        private void PageMoveHandler(int oldPageIndex, int newPageIndex, PageMoveOperationDelegate pageMoveOperation)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs(false);
            RaisePageIndexChanging(cancelArgs);

            // When the IPagedCollectionView implementation updates its PageIndex property,
            // the DataPager gets a notification and raises the PageIndexChanged event.
            if (cancelArgs.Cancel)
            {
                // Revert back to old value, since operation was canceled
                this.SetValueNoCallback(KinoDataPager.PageIndexProperty, oldPageIndex);
            }
            else
            {
                bool pageMoveOperationResult;
                if (pageMoveOperation == null)
                {
                    Debug.Assert(PagedSource != null, "Unexpected this.PagedSource == null");
                    pageMoveOperationResult = PagedSource.MoveToPage(newPageIndex);
                }
                else
                {
                    pageMoveOperationResult = pageMoveOperation();
                }

                if (!pageMoveOperationResult)
                {
                    // Revert back to old value, since operation failed
                    this.SetValueNoCallback(KinoDataPager.PageIndexProperty, oldPageIndex);

                    // The PageIndexChanged needs to be raised even though no move occurred,
                    // because of the PageIndexChanging notification above.
                    RaisePageIndexChanged();
                }
            }
        }

        /// <summary>
        /// Handles a property change within the Source.
        /// </summary>
        /// <param name="sender">The object firing this event.</param>
        /// <param name="e">The event args for this event.</param>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Assert(PagedSource != null, "Unexpected null this.PagedSource");

            switch (e.PropertyName)
            {
                case "Count":
                case "ItemCount":
                    ItemCount = PagedSource.ItemCount;
                    UpdatePageCount();
                    UpdateControl();
                    break;

                case "PageIndex":
                    int oldPageIndex = PageIndex;

                    PageIndex = PagedSource.PageIndex;
                    RaisePageIndexChanged();

                    //DataPagerAutomationPeer peer = DataPagerAutomationPeer.FromElement(this) as DataPagerAutomationPeer;
                    //if (peer != null && oldPageIndex != this.PageIndex)
                    //{
                    //    peer.RefreshPageIndex(oldPageIndex);
                    //}
                    break;

                case "PageSize":
                    PageSize = PagedSource.PageSize;
                    UpdatePageCount();
                    UpdateControl();
                    break;

                case "CanChangePage":
                case "Filter":
                case "TotalItemCount":
                case "SortDescriptions":
                    UpdateControl();
                    break;
            }
        }

        /// <summary>
        /// Raises a non-cancellable PageIndexChanging and optional PageIndexChanged events.
        /// </summary>
        /// <param name="raisePageChanged">True when the PageChanged event needs to be raised</param>
        private void RaisePageIndexChangeEvents(bool raisePageChanged)
        {
            RaisePageIndexChanging(new CancelEventArgs(false));
            if (raisePageChanged)
            {
                RaisePageIndexChanged();
            }
        }

        /// <summary>
        /// Raises the PageIndexChanged event.
        /// </summary>
        private void RaisePageIndexChanged()
        {
            UpdateControl();

            if (_needPageChangingNotification)
            {
                RaisePageIndexChangeEvents(false /*raisePageChanged*/);
            }

            _needPageChangingNotification = true;

            PageIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the PageIndexChanging event.
        /// </summary>
        /// <param name="e">The event args to use for the event.</param>
        private void RaisePageIndexChanging(CancelEventArgs e)
        {
            if (_needPageChangingNotification)
            {
                PageIndexChanging?.Invoke(this, e);

                // A PageIndexChanging notification is still required when
                // the change was cancelled.
                if (!e.Cancel)
                {
                    _needPageChangingNotification = false;
                }
            }
        }

        /// <summary>
        /// Update DataPager UI for paging enabled.
        /// </summary>
        /// <param name="needPage">Boolean that specifies if a page is needed</param>
        private void SetCannotChangePage(bool needPage)
        {
            if (_currentPageTextBox != null && !needPage)
            {
                _currentPageTextBox.Text = string.Empty;
            }

            VisualStateManager.GoToState(this, DATAPAGER_stateMoveDisabled, true);
            VisualStateManager.GoToState(this, DATAPAGER_stateMoveFirstDisabled, true);
            VisualStateManager.GoToState(this, DATAPAGER_stateMovePreviousDisabled, true);
            VisualStateManager.GoToState(this, DATAPAGER_stateMoveNextDisabled, true);
            VisualStateManager.GoToState(this, DATAPAGER_stateMoveLastDisabled, true);
        }

        /// <summary>
        /// Update DataPager UI for paging disabled.
        /// </summary>
        private void SetCanChangePage()
        {
            VisualStateManager.GoToState(this, DATAPAGER_stateMoveEnabled, true);

            if (_currentPageTextBox != null)
            {
                _currentPageTextBox.Text = (PageIndex + 1).ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Notification raised when a numeric toggle button gets checked
        /// </summary>
        /// <param name="sender">The numeric toggle button</param>
        /// <param name="e">Routed event for the notification</param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_ignoreToggleButtonCheckedNotification)
            {
                return;
            }

            // Ignore notifications when the source is an IEnumerable
            if (PagedSource != null)
            {
                ToggleButton button = sender as ToggleButton;
                int uiIndex = _numericButtonPanel.Children.IndexOf(button);
                int pageIndex = GetButtonStartIndex() + uiIndex - 1;

                PageMoveHandler(PageIndex, pageIndex, null);

                if (PagedSource.PageIndex != pageIndex)
                {
                    try
                    {
                        _ignoreToggleButtonUncheckedNotification = true;

                        // The toggle button that was checked must remain unchecked
                        // while the page move occurs, or because the page move initiation failed.
                        button.IsChecked = false;
                    }
                    finally
                    {
                        _ignoreToggleButtonUncheckedNotification = false;
                    }
                }
            }
        }

        /// <summary>
        /// Notification raised when a numeric toggle button gets focus
        /// </summary>
        /// <param name="sender">The numeric toggle button</param>
        /// <param name="e">Routed event for the notification</param>
        private void ToggleButton_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!_ignoreToggleButtonFocusNotification)
            {
                ToggleButton button = sender as ToggleButton;
                int uiIndex = _numericButtonPanel.Children.IndexOf(button);

                // Remember which toggle button got focus so the same page index can
                // regain focus when the numeric buttons are shifted.
                _focusedToggleButtonIndex = GetButtonStartIndex() + uiIndex - 1;
            }
        }

        /// <summary>
        /// Notification raised when a numeric toggle button loses focus
        /// </summary>
        /// <param name="sender">The numeric toggle button</param>
        /// <param name="e">Routed event for the notification</param>
        private void ToggleButton_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!_ignoreToggleButtonFocusNotification)
            {
                // -1 is an indication that no toggle button has focus
                _focusedToggleButtonIndex = -1;
            }
        }

        /// <summary>
        /// Notification raised when a numeric toggle button gets unchecked
        /// </summary>
        /// <param name="sender">The numeric toggle button</param>
        /// <param name="e">Routed event for the notification</param>
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_ignoreToggleButtonUncheckedNotification)
            {
                try
                {
                    _ignoreToggleButtonCheckedNotification = true;

                    // Attempts to uncheck a numeric toggle button, other than willingly
                    // by internal logic, must fail.
                    ToggleButton button = sender as ToggleButton;
                    button.IsChecked = true;
                }
                finally
                {
                    _ignoreToggleButtonCheckedNotification = false;
                }
            }
        }

        /// <summary>
        /// Attempts to put the integer value of the string in _currentPageTextBox into _requestedPageIndex.
        /// </summary>
        /// <returns>Whether or not the parsing of the string succeeded.</returns>
        private bool TryParseTextBoxPage()
        {
            bool successfullyParsed = int.TryParse(_currentPageTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out _requestedPageIndex);

            if (successfullyParsed)
            {
                // Subtract one to make it zero-based.
                _requestedPageIndex--;
            }

            return successfullyParsed;
        }

        /// <summary>
        /// Updates the visual display of the number of buttons that we display.
        /// </summary>
        private void UpdateButtonCount()
        {
            // what we should use as the button count
            int buttonCount = Math.Min(NumericButtonCount, PageCount);

            if (_numericButtonPanel != null)
            {
                // add new
                while (_numericButtonPanel.Children.Count < buttonCount)
                {
                    ToggleButton button = new ToggleButton
                    {
                        Style = NumericButtonStyle
                    };
                    button.Checked += new RoutedEventHandler(ToggleButton_Checked);
                    button.Unchecked += new RoutedEventHandler(ToggleButton_Unchecked);
                    button.GotFocus += new RoutedEventHandler(ToggleButton_GotFocus);
                    button.LostFocus += new RoutedEventHandler(ToggleButton_LostFocus);
                    _numericButtonPanel.Children.Add(button);
                }

                // remove excess
                while (_numericButtonPanel.Children.Count > buttonCount)
                {
                    if (_numericButtonPanel.Children[0] is ToggleButton button)
                    {
                        button.Checked -= new RoutedEventHandler(ToggleButton_Checked);
                        button.Unchecked -= new RoutedEventHandler(ToggleButton_Unchecked);
                        button.GotFocus -= new RoutedEventHandler(ToggleButton_GotFocus);
                        button.LostFocus -= new RoutedEventHandler(ToggleButton_LostFocus);
                        _numericButtonPanel.Children.Remove(button);
                    }
                }

                UpdateButtonDisplay();
            }
        }

        /// <summary>
        /// Updates the visual content and style of the buttons that we display.
        /// </summary>
        private void UpdateButtonDisplay()
        {
            if (_numericButtonPanel != null)
            {
                // what we should use as the start index
                int startIndex = GetButtonStartIndex();

                // what we should use as the button count
                int buttonCount = Math.Min(NumericButtonCount, PageCount);

                // by default no focus restoration needs to occur
                bool isToggleButtonFocused = false;

                int index = startIndex;
                foreach (UIElement ui in _numericButtonPanel.Children)
                {
                    if (ui is ToggleButton button)
                    {
                        if (PagedSource == null)
                        {
                            Debug.Assert(index == 1, "Unexpected index value for IEnumerable Source");

                            // The single toggle button needs to be checked.
                            button.IsChecked = true;
                        }
                        else if (PagedSource != null && PagedSource.PageIndex == index - 1)
                        {
                            try
                            {
                                _ignoreToggleButtonCheckedNotification = true;

                                // The toggle button corresponding to the Source's current page
                                // needs to be checked.
                                button.IsChecked = true;
                            }
                            finally
                            {
                                _ignoreToggleButtonCheckedNotification = false;
                            }
                        }
                        else
                        {
                            if ((bool)button.IsChecked)
                            {
                                try
                                {
                                    _ignoreToggleButtonUncheckedNotification = true;

                                    // All other toggle buttons needs to be unchecked.
                                    button.IsChecked = false;
                                }
                                finally
                                {
                                    _ignoreToggleButtonUncheckedNotification = false;
                                }
                            }
                        }

                        if (AutoEllipsis && index == startIndex + buttonCount - 1 &&
                            (index != PageCount))
                        {
                            button.Content = PagerResources.AutoEllipsisString;
                        }
                        else
                        {
                            button.Content = index;
                        }

                        if (_focusedToggleButtonIndex != -1 &&
                            _focusedToggleButtonIndex == index - 1)
                        {
                            try
                            {
                                _ignoreToggleButtonFocusNotification = true;

                                // When the numeric toggle buttons are shifted because the
                                // checked one is centered, the previously focused button
                                // needs to be shifted as well.
                                button.Focus();
                            }
                            finally
                            {
                                _ignoreToggleButtonFocusNotification = false;
                            }

                            isToggleButtonFocused = true;
                        }

                        AutomationProperties.SetAutomationId(button, DATAPAGER_numericalButtonAutomationId + index.ToString(CultureInfo.CurrentCulture));

                        index++;
                    }
                }

                if (_focusedToggleButtonIndex != -1 && !isToggleButtonFocused)
                {
                    // The page index of the previously focused toggle button is now out of range.
                    // Focus the toggle button representing the current page instead.
                    foreach (UIElement ui in _numericButtonPanel.Children)
                    {
                        if (ui is ToggleButton button && (bool)button.IsChecked)
                        {
                            button.Focus();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the state related to the IsEnabled property
        /// </summary>
        private void UpdateCommonState()
        {
            VisualStateManager.GoToState(this, IsEnabled ? DATAPAGER_stateNormal : DATAPAGER_stateDisabled, true);
        }

        /// <summary>
        /// Updates the current page, the total pages, and the
        /// state of the control.
        /// </summary>
        private void UpdateControl()
        {
            UpdatePageModeDisplay();
            UpdateButtonCount();

            bool needPage = Source != null &&
                ((PagedSource == null && PageSize > 0) ||
                 (PagedSource != null && PagedSource.PageSize > 0));

            CanMoveToFirstPage = needPage && PageIndex > 0;

            CanMoveToPreviousPage = CanMoveToFirstPage;

            CanMoveToNextPage = needPage && PagedSource != null &&
                (!IsTotalItemCountFixed || PagedSource.TotalItemCount == -1 || PageIndex < PageCount - 1);

            CanMoveToLastPage = needPage && PagedSource != null &&
                PagedSource.TotalItemCount != -1 && PageIndex < PageCount - 1;

            CanChangePage = needPage && (PagedSource == null || PagedSource.CanChangePage);

            UpdateCurrentPagePrefixAndSuffix(needPage);

            if (!needPage || !CanChangePage)
            {
                SetCannotChangePage(needPage);
            }
            else
            {
                SetCanChangePage();
                UpdateCanPageFirstAndPrevious();
                UpdateCanPageNextAndLast();
            }

            //DataPagerAutomationPeer peer = DataPagerAutomationPeer.FromElement(this) as DataPagerAutomationPeer;
            //if (peer != null)
            //{
            //    peer.RefreshProperties();
            //}
        }

        /// <summary>
        /// Updates the states of whether the pager can page to the first
        /// and to the previous page.
        /// </summary>
        private void UpdateCanPageFirstAndPrevious()
        {
            VisualStateManager.GoToState(this, CanMoveToFirstPage ? DATAPAGER_stateMoveFirstEnabled : DATAPAGER_stateMoveFirstDisabled, true);
            VisualStateManager.GoToState(this, CanMoveToPreviousPage ? DATAPAGER_stateMovePreviousEnabled : DATAPAGER_stateMovePreviousDisabled, true);
        }

        /// <summary>
        /// Updates the states of whether the pager can page to the next
        /// and to the last page.
        /// </summary>
        private void UpdateCanPageNextAndLast()
        {
            VisualStateManager.GoToState(this, CanMoveToNextPage ? DATAPAGER_stateMoveNextEnabled : DATAPAGER_stateMoveNextDisabled, true);
            VisualStateManager.GoToState(this, CanMoveToLastPage ? DATAPAGER_stateMoveLastEnabled : DATAPAGER_stateMoveLastDisabled, true);
        }

        /// <summary>
        /// Goes into the TotalPageCountKnown or TotalPageCountUnknown state according to Source.TotalItemCount
        /// and updates the captions of the text blocks surrounding the current page text box.
        /// </summary>
        /// <param name="needPage">True when a Source is set and PageSize > 0</param>
        private void UpdateCurrentPagePrefixAndSuffix(bool needPage)
        {
            bool needTotalPageCountUnknownState = !needPage || (PagedSource != null && PagedSource.TotalItemCount == -1);
            string textBlockCaption;

            if (_currentPagePrefixTextBlock != null)
            {
                if (_currentPagePrefixTextBlock != null)
                {
                    if (needTotalPageCountUnknownState)
                    {
                        textBlockCaption = PagerResources.CurrentPagePrefix_TotalPageCountUnknown;
                    }
                    else
                    {
                        textBlockCaption = string.Format(
                            CultureInfo.InvariantCulture,
                            PagerResources.CurrentPagePrefix_TotalPageCountKnown,
                            PageCount.ToString(CultureInfo.CurrentCulture));
                    }

                    //this._currentPagePrefixTextBlock.Text = textBlockCaption;
                    if (string.IsNullOrEmpty(textBlockCaption))
                    {
                        _currentPagePrefixTextBlock.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        _currentPagePrefixTextBlock.Visibility = Visibility.Visible;
                    }
                }
            }

            if (_currentPageSuffixTextBlock != null)
            {
                if (needTotalPageCountUnknownState)
                {
                    textBlockCaption = PagerResources.CurrentPageSuffix_TotalPageCountUnknown;
                }
                else
                {
                    textBlockCaption = string.Format(
                        CultureInfo.InvariantCulture,
                        PagerResources.CurrentPageSuffix_TotalPageCountKnown,
                        PageCount.ToString(CultureInfo.CurrentCulture));
                }

                //this._currentPageSuffixTextBlock.Text = textBlockCaption;
                if (string.IsNullOrEmpty(textBlockCaption))
                {
                    _currentPageSuffixTextBlock.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _currentPageSuffixTextBlock.Visibility = Visibility.Visible;
                }
            }

            VisualStateManager.GoToState(this, needTotalPageCountUnknownState ? DATAPAGER_stateTotalPageCountUnknown : DATAPAGER_stateTotalPageCountKnown, true);
        }

        /// <summary>
        /// Updates the visual display to show the current page mode
        /// we have selected.
        /// </summary>
        private void UpdatePageModeDisplay()
        {
            VisualStateManager.GoToState(this, Enum.GetName(typeof(PagerDisplayMode), DisplayMode), true);
        }

        /// <summary>
        /// Updates the page count based on the number of items and the page size.
        /// </summary>
        private void UpdatePageCount()
        {
            if (PagedSource.PageSize > 0)
            {
                PageCount = Math.Max(1, (int)Math.Ceiling((double)PagedSource.ItemCount / PagedSource.PageSize));
            }
            else
            {
                PageCount = 1;
            }
        }

        #endregion Private Methods
    }
}
#pragma warning restore SA1201
#pragma warning restore SA1202
#pragma warning restore SA1214
#pragma warning restore SA1311