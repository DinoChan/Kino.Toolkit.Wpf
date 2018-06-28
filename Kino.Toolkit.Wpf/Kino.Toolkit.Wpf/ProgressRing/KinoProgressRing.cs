//Thanks: http://briandunnington.github.io/progressring-wp8.html

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
    public class KinoProgressRing : Control
    {
        bool hasAppliedTemplate = false;

        public KinoProgressRing()
        {
            this.DefaultStyleKey = typeof(KinoProgressRing);
            TemplateSettings = new TemplateSettingValues(60);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            hasAppliedTemplate = true;
            UpdateState(this.IsActive);
        }

        void UpdateState(bool isActive)
        {
            if (hasAppliedTemplate)
            {
                string state = isActive ? VisualStates.StateActive : VisualStates.StateInactive;
                System.Windows.VisualStateManager.GoToState(this, state, true);
            }
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            var width = 20d;
            var height = 20d;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                width = double.IsNaN(this.Width) == false ? this.Width : availableSize.Width;
                height = double.IsNaN(this.Height) == false ? this.Height : availableSize.Height;
            }
            TemplateSettings = new TemplateSettingValues(Math.Min(width, height));
            return base.MeasureOverride(availableSize);
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(KinoProgressRing), new PropertyMetadata(false, new PropertyChangedCallback(IsActiveChanged)));

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var pr = (KinoProgressRing)d;
            var isActive = (bool)args.NewValue;
            pr.UpdateState(isActive);
        }


        public TemplateSettingValues TemplateSettings
        {
            get { return (TemplateSettingValues)GetValue(TemplateSettingsProperty); }
            set { SetValue(TemplateSettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplateSettings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register("TemplateSettings", typeof(TemplateSettingValues), typeof(KinoProgressRing), new PropertyMetadata(null));


        public class TemplateSettingValues : System.Windows.DependencyObject
        {
            public TemplateSettingValues(double width)
            {
                MaxSideLength = 400;
                EllipseDiameter = width / 10 + 1;
                EllipseOffset = new System.Windows.Thickness(EllipseDiameter);
            }

            public double MaxSideLength
            {
                get { return (double)GetValue(MaxSideLengthProperty); }
                set { SetValue(MaxSideLengthProperty, value); }
            }

            // Using a DependencyProperty as the backing store for MaxSideLength.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty MaxSideLengthProperty =
                DependencyProperty.Register("MaxSideLength", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

            public double EllipseDiameter
            {
                get { return (double)GetValue(EllipseDiameterProperty); }
                set { SetValue(EllipseDiameterProperty, value); }
            }

            // Using a DependencyProperty as the backing store for EllipseDiameter.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty EllipseDiameterProperty =
                DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(TemplateSettingValues), new PropertyMetadata(0D));

            public Thickness EllipseOffset
            {
                get { return (Thickness)GetValue(EllipseOffsetProperty); }
                set { SetValue(EllipseOffsetProperty, value); }
            }

            // Using a DependencyProperty as the backing store for EllipseOffset.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty EllipseOffsetProperty =
                DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(TemplateSettingValues), new PropertyMetadata(new Thickness()));
        }
    }

}
