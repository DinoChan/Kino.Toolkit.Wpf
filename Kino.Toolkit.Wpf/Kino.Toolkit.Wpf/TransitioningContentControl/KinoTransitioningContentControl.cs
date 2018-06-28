using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// Represents a control with a single piece of content and when that content 
    /// changes performs a transition animation. 
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    /// <remarks>The API for this control will change considerably in the future.</remarks>
    [TemplateVisualState(GroupName = PresentationGroup, Name = NormalState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = DefaultTransitionState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = LeftTransitionState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = UpTransitionState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = RightTransitionState)]
    [TemplateVisualState(GroupName = PresentationGroup, Name = DownTransitionState)]
    [TemplatePart(Name = PreviousContentPresentationSitePartName, Type = typeof(ContentControl))]
    [TemplatePart(Name = CurrentContentPresentationSitePartName, Type = typeof(ContentControl))]
    public class KinoTransitioningContentControl : ContentControl
    {
        #region Visual state names
        /// <summary>
        /// The name of the group that holds the presentation states.
        /// </summary>
        private const string PresentationGroup = "PresentationStates";

        /// <summary>
        /// The name of the state that represents a normal situation where no
        /// transition is currently being used.
        /// </summary>
        private const string NormalState = "Normal";

        /// <summary>
        /// The name of the state that represents the default transition.
        /// </summary>
        public const string DefaultTransitionState = "DefaultTransition";

        public const string LeftTransitionState = "LeftTransition";

        public const string UpTransitionState = "UpTransition";

        public const string RightTransitionState = "RightTransition";

        public const string DownTransitionState = "DownTransition";

        #endregion Visual state names

        #region Template part names
        /// <summary>
        /// The name of the control that will display the previous content.
        /// </summary>
        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

        /// <summary>
        /// The name of the control that will display the current content.
        /// </summary>
        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        #endregion Template part names

        #region TemplateParts
        /// <summary>
        /// Gets or sets the current content presentation site.
        /// </summary>
        /// <value>The current content presentation site.</value>
        private ContentPresenter CurrentContentPresentationSite { get; set; }

        /// <summary>
        /// Gets or sets the previous content presentation site.
        /// </summary>
        /// <value>The previous content presentation site.</value>
        private ContentPresenter PreviousContentPresentationSite { get; set; }
        #endregion TemplateParts

        #region public bool IsTransitioning

        /// <summary>
        /// Indicates whether the control allows writing IsTransitioning.
        /// </summary>
        private bool _allowIsTransitioningWrite;

        /// <summary>
        /// Gets a value indicating whether this instance is currently performing
        /// a transition.
        /// </summary>
        public bool IsTransitioning
        {
            get { return (bool)GetValue(IsTransitioningProperty); }
            private set
            {
                _allowIsTransitioningWrite = true;
                SetValue(IsTransitioningProperty, value);
                _allowIsTransitioningWrite = false;
            }
        }

        /// <summary>
        /// Identifies the IsTransitioning dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTransitioningProperty =
            DependencyProperty.Register(
                "IsTransitioning",
                typeof(bool),
                typeof(KinoTransitioningContentControl),
                new PropertyMetadata(OnIsTransitioningPropertyChanged));

        /// <summary>
        /// IsTransitioningProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its IsTransitioning.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoTransitioningContentControl source = (KinoTransitioningContentControl)d;

            if (!source._allowIsTransitioningWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException(Properties.Resources.TransitiotioningContentControl_IsTransitioningReadOnly);
            }
        }
        #endregion public bool IsTransitioning

        /// <summary>
        /// The storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard _currentTransition;

        /// <summary>
        /// Gets or sets the storyboard that is used to transition old and new content.
        /// </summary>
        private Storyboard CurrentTransition
        {
            get { return _currentTransition; }
            set
            {
                // decouple event
                if (_currentTransition != null)
                {
                    _currentTransition.Completed -= OnTransitionCompleted;
                }

                _currentTransition = value;

                if (_currentTransition != null)
                {
                    _currentTransition.Completed += OnTransitionCompleted;
                }
            }
        }

        #region public string Transition
        /// <summary>
        /// Gets or sets the name of the transition to use. These correspond
        /// directly to the VisualStates inside the PresentationStates group.
        /// </summary>
        public string Transition
        {
            get { return GetValue(TransitionProperty) as string; }
            set { SetValue(TransitionProperty, value); }
        }

        /// <summary>
        /// Identifies the Transition dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionProperty =
            DependencyProperty.Register(
                "Transition",
                typeof(string),
                typeof(KinoTransitioningContentControl),
                new PropertyMetadata(DefaultTransitionState, OnTransitionPropertyChanged));

        /// <summary>
        /// TransitionProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its Transition.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KinoTransitioningContentControl source = (KinoTransitioningContentControl)d;
            string oldTransition = e.OldValue as string;
            string newTransition = e.NewValue as string;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            // find new transition
            Storyboard newStoryboard = source.GetStoryboard(newTransition);

            // unable to find the transition.
            if (newStoryboard == null)
            {
                // could be during initialization of xaml that presentationgroups was not yet defined
                if (VisualStates.TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    // will delay check
                    source.CurrentTransition = null;
                }
                else
                {
                    // revert to old value
                    source.SetValue(TransitionProperty, oldTransition);

                    throw new ArgumentException(
                        string.Format(CultureInfo.CurrentCulture, Properties.Resources.TransitioningContentControl_TransitionNotFound, newTransition));
                }
            }
            else
            {
                source.CurrentTransition = newStoryboard;
            }
        }


        /// <summary>
        /// 获取或设置TransitionType的值
        /// </summary>  
        public TransitionType TransitionType
        {
            get => (TransitionType)GetValue(TransitionTypeProperty);
            set => SetValue(TransitionTypeProperty, value);
        }

        /// <summary>
        /// 标识 TransitionType 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty =
            DependencyProperty.Register(nameof(TransitionType), typeof(TransitionType), typeof(KinoTransitioningContentControl), new PropertyMetadata(default(TransitionType), OnTransitionTypeChanged));

        private static void OnTransitionTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (TransitionType)args.OldValue;
            var newValue = (TransitionType)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoTransitioningContentControl;
            target?.OnTransitionTypeChanged(oldValue, newValue);
        }

        /// <summary>
        /// TransitionType 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">TransitionType 属性的旧值。</param>
        /// <param name="newValue">TransitionType 属性的新值。</param>
        protected virtual void OnTransitionTypeChanged(TransitionType oldValue, TransitionType newValue)
        {
            switch (newValue)
            {
                case TransitionType.Default:
                    Transition = DefaultTransitionState;
                    break;
                case TransitionType.Left:
                    Transition = LeftTransitionState;
                    break;
                case TransitionType.Up:
                    Transition = UpTransitionState;
                    break;
                case TransitionType.Right:
                    Transition = RightTransitionState;
                    break;
                case TransitionType.Down:
                    Transition = DownTransitionState;
                    break;
                default:
                    break;
            }
        }
        #endregion public string Transition

        #region public bool RestartTransitionOnContentChange
        /// <summary>
        /// Gets or sets a value indicating whether the current transition
        /// will be aborted when setting new content during a transition.
        /// </summary>
        public bool RestartTransitionOnContentChange
        {
            get { return (bool)GetValue(RestartTransitionOnContentChangeProperty); }
            set { SetValue(RestartTransitionOnContentChangeProperty, value); }
        }

        /// <summary>
        /// Identifies the RestartTransitionOnContentChange dependency property.
        /// </summary>
        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty =
            DependencyProperty.Register(
                "RestartTransitionOnContentChange",
                typeof(bool),
                typeof(KinoTransitioningContentControl),
                new PropertyMetadata(false, OnRestartTransitionOnContentChangePropertyChanged));

        /// <summary>
        /// RestartTransitionOnContentChangeProperty property changed handler.
        /// </summary>
        /// <param name="d">TransitioningContentControl that changed its RestartTransitionOnContentChange.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KinoTransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called when the RestartTransitionOnContentChangeProperty changes.
        /// </summary>
        /// <param name="oldValue">The old value of RestartTransitionOnContentChange.</param>
        /// <param name="newValue">The new value of RestartTransitionOnContentChange.</param>
        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {
        }
        #endregion public bool RestartTransitionOnContentChange

        #region Events
        /// <summary>
        /// Occurs when the current transition has completed.
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;
        #endregion Events

        /// <summary>
        /// Initializes a new instance of the <see cref="KinoTransitioningContentControl"/> class.
        /// </summary>
        public KinoTransitioningContentControl()
        {
            DefaultStyleKey = typeof(KinoTransitioningContentControl);
        }

        /// <summary>
        /// Builds the visual tree for the TransitioningContentControl control 
        /// when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }

            base.OnApplyTemplate();

            PreviousContentPresentationSite = GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            CurrentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;

            if (CurrentContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = Content;
            }

            // hookup currenttransition
            Storyboard transition = GetStoryboard(Transition);
            CurrentTransition = transition;
            if (transition == null)
            {
                string invalidTransition = Transition;
                // revert to default
                Transition = DefaultTransitionState;

                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Properties.Resources.TransitioningContentControl_TransitionNotFound, invalidTransition));
            }

            VisualStateManager.GoToState(this, NormalState, false);
        }

        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes.
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            StartTransition(oldContent, newContent);
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        private void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.Content = newContent;

                PreviousContentPresentationSite.Content = oldContent;

                // and start a new transition
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, Transition, true);
                }
            }
        }


        protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);

            StartTransition(oldContentTemplate, newContentTemplate);
        }

        private void StartTransition(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (CurrentContentPresentationSite != null && PreviousContentPresentationSite != null)
            {
                CurrentContentPresentationSite.ContentTemplate = newContentTemplate;

                PreviousContentPresentationSite.ContentTemplate = oldContentTemplate;

                // and start a new transition
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, Transition, true);
                }
            }
        }

        /// <summary>
        /// Handles the Completed event of the transition storyboard.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();

            RoutedEventHandler handler = TransitionCompleted;
            if (handler != null)
            {
                handler(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Aborts the transition and releases the previous content.
        /// </summary>
        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            VisualStateManager.GoToState(this, NormalState, false);
            IsTransitioning = false;
            if (PreviousContentPresentationSite != null)
            {
                PreviousContentPresentationSite.Content = null;
            }
        }

        /// <summary>
        /// Attempts to find a storyboard that matches the newTransition name.
        /// </summary>
        /// <param name="newTransition">The new transition.</param>
        /// <returns>A storyboard or null, if no storyboard was found.</returns>
        private Storyboard GetStoryboard(string newTransition)
        {
            VisualStateGroup presentationGroup = VisualStates.TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
            {
                newStoryboard = presentationGroup.States
                    .OfType<VisualState>()
                    .Where(state => state.Name == newTransition)
                    .Select(state => state.Storyboard)
                    .FirstOrDefault();
            }
            return newStoryboard;
        }
    }
}
