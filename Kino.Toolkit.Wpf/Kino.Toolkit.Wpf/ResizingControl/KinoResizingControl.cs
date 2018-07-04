using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Kino.Toolkit.Wpf
{
    [TemplatePart(Name = ContentPresenterName, Type = typeof(ContentPresenter))]
    public class KinoResizingControl : ContentControl
    {
        private const string ContentPresenterName = "ContentPresenter";

        public KinoResizingControl()
        {
            DefaultStyleKey = typeof(KinoResizingControl);
            SizeChanged += OnControlSizeChanged;
            _resizingStoryboard = new Storyboard();
            _resizingStoryboard.FillBehavior = FillBehavior.HoldEnd;
            _resizingStoryboard.Completed += OnResizingCompleted;
        }

        private void OnResizingCompleted(object sender, EventArgs e)
        {
            _isResizing = false;
        }

        private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _hasFirstSizeChanged = true;
        }

        protected ScrollViewer ScrollViewer { get; set; }

        private ContentPresenter _contentPreseter;
        protected ContentPresenter ContentPresenter
        {
            get
            {
                return _contentPreseter;
            }
            set
            {
                if (_contentPreseter != null)
                    _contentPreseter.SizeChanged -= OnContentSizeChanged;

                _contentPreseter = value;

                if (_contentPreseter != null)
                    _contentPreseter.SizeChanged += OnContentSizeChanged;
            }
        }

        private Storyboard _resizingStoryboard;
        private DoubleAnimation _defaultHeightAnimation;
        private DoubleAnimation _defaultWidthAnimation;
        private bool _isResizing;

        private bool _hasFirstSizeChanged;

        /// <summary>
        /// 获取或设置Animation的值
        /// </summary>  
        public DoubleAnimation Animation
        {
            get => (DoubleAnimation)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// 标识 Animation 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AnimationProperty =
            DependencyProperty.Register(nameof(Animation), typeof(DoubleAnimation), typeof(KinoResizingControl), new PropertyMetadata(default(DoubleAnimation), OnAnimationChanged));

        private static void OnAnimationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (DoubleAnimation)args.OldValue;
            var newValue = (DoubleAnimation)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoResizingControl;
            target?.OnAnimationChanged(oldValue, newValue);
        }

        /// <summary>
        /// Animation 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Animation 属性的旧值。</param>
        /// <param name="newValue">Animation 属性的新值。</param>
        protected virtual void OnAnimationChanged(DoubleAnimation oldValue, DoubleAnimation newValue)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentPresenter = GetTemplateChild(ContentPresenterName) as ContentPresenter;

        }

        private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(_hasFirstSizeChanged);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ChangeSize(_hasFirstSizeChanged);
        }

        private void ChangeSize(bool useAnimation)
        {
            if (ContentPresenter == null)
                return;

            if (useAnimation == false)
            {
                Height = ContentPresenter.ActualHeight;
                Width = ContentPresenter.ActualWidth;
            }
            else
            {
                if (_isResizing)
                    ResizingStoryboard.Stop();

                _isResizing = true;
                ResizingStoryboard.Begin();
            }
        }

        private Storyboard ResizingStoryboard
        {
            get
            {
                DoubleAnimation heightAnimation;
                DoubleAnimation widthAnimation;
                if (Animation != null)
                {
                    heightAnimation = Animation.Clone();
                    Storyboard.SetTarget(heightAnimation, this);
                    Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));

                    widthAnimation = Animation.Clone();
                    Storyboard.SetTarget(widthAnimation, this);
                    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));
                }
                else
                {
                    heightAnimation = _defaultHeightAnimation;
                    widthAnimation = _defaultWidthAnimation;
                }
                //heightAnimation.From = this.ActualHeight;
                //widthAnimation.From = this.ActualWidth;
                heightAnimation.To = ContentPresenter.ActualHeight;
                widthAnimation.To = ContentPresenter.ActualWidth;

                _resizingStoryboard.Children.Clear();
                _resizingStoryboard.Children.Add(heightAnimation);
                _resizingStoryboard.Children.Add(widthAnimation);
                return _resizingStoryboard;
            }
        }
    }
}
