// Thanks: http://briandunnington.github.io/progressring-wp8.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    [TemplateVisualState(GroupName = VisualStates.GroupActive, Name = VisualStates.StateActive)]
    [TemplateVisualState(GroupName = VisualStates.GroupActive, Name = VisualStates.StateInactive)]
    public partial class ProgressRing : Control
    {
        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        // Using a DependencyProperty as the backing store for TemplateSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register("TemplateSettings", typeof(TemplateSettingValues), typeof(ProgressRing), new PropertyMetadata(null));

        private bool hasAppliedTemplate = false;

        public ProgressRing()
        {
            DefaultStyleKey = typeof(ProgressRing);
            TemplateSettings = new TemplateSettingValues(60);
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public TemplateSettingValues TemplateSettings
        {
            get { return (TemplateSettingValues)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            hasAppliedTemplate = true;
            UpdateState(IsActive);
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            var width = 20d;
            var height = 20d;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                width = double.IsNaN(Width) == false ? Width : availableSize.Width;
                height = double.IsNaN(Height) == false ? Height : availableSize.Height;
            }

            TemplateSettings = new TemplateSettingValues(Math.Min(width, height));
            return base.MeasureOverride(availableSize);
        }

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var pr = (ProgressRing)d;
            var isActive = (bool)args.NewValue;
            pr.UpdateState(isActive);
        }

        private void UpdateState(bool isActive)
        {
            if (hasAppliedTemplate)
            {
                string state = isActive ? VisualStates.StateActive : VisualStates.StateInactive;
                VisualStateManager.GoToState(this, state, true);
            }
        }
    }
}
