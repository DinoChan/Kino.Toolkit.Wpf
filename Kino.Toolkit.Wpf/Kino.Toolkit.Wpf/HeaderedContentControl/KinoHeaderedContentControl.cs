using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kino.Toolkit.Wpf
{
    [TemplatePart(Name = HeaderElementName, Type = typeof(FrameworkElement))]
    [TemplateVisualState(Name = NormalName, GroupName = CommonStatesName)]
    [TemplateVisualState(Name = DisabledName, GroupName = CommonStatesName)]
    public class KinoHeaderedContentControl : HeaderedContentControl
    {
        private const string HeaderElementName = "HeaderElement";

        private const string CommonStatesName = "CommonStates";
        private const string NormalName = "Normal";
        private const string DisabledName = "Disabled";

        private FrameworkElement _headerElement;

        static KinoHeaderedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KinoHeaderedContentControl), new FrameworkPropertyMetadata(typeof(KinoHeaderedContentControl)));
        }

        public KinoHeaderedContentControl()
        {
            IsEnabledChanged += OnIsEnabledChanged;
        }

        /// <summary>
        /// 获取或设置 Property 的值
        /// </summary>
        public FrameworkElement HeaderElement
        {
            get
            {
                return _headerElement;
            }

            set
            {
                if (_headerElement == value)
                    return;

                if (_headerElement != null)
                    _headerElement.MouseLeftButtonDown -= OnHeaderMouseLeftButtonDown;

                _headerElement = value;
                if (_headerElement != null)
                    _headerElement.MouseLeftButtonDown += OnHeaderMouseLeftButtonDown;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HeaderElement = GetTemplateChild(HeaderElementName) as FrameworkElement;
            UpdateVisualState(false);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsEnabled ? NormalName : DisabledName, useTransitions);
        }

        private void OnHeaderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Content is Control control)
                control.Focus();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateVisualState(true);
        }
    }
}
