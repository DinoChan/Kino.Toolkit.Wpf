using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Kino.Toolkit.Wpf
{
    /// <summary>Represents a button control that displays a hyperlink.</summary>
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
    public class KinoHyperlinkButton : ButtonBase
    {
        /// <summary>Identifies the <see cref="P:System.Windows.Controls.HyperlinkButton.NavigateUri" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.HyperlinkButton.NavigateUri" /> dependency property.</returns>
        public readonly static DependencyProperty NavigateUriProperty;

        /// <summary>Identifies the <see cref="P:System.Windows.Controls.HyperlinkButton.TargetName" /> dependency property.</summary>
        /// <returns>The identifier for the <see cref="P:System.Windows.Controls.HyperlinkButton.TargetName" /> dependency property.</returns>
        public readonly static DependencyProperty TargetNameProperty;

        /// <summary>Gets or sets the URI to navigate to when the <see cref="T:System.Windows.Controls.HyperlinkButton" /> is clicked. </summary>
        /// <returns>The URI to navigate to when the <see cref="T:System.Windows.Controls.HyperlinkButton" /> is clicked.</returns>
        [TypeConverter(typeof(UriTypeConverter))]
        public Uri NavigateUri
        {
            get
            {
                return base.GetValue(KinoHyperlinkButton.NavigateUriProperty) as Uri;
            }
            set
            {
                base.SetValue(KinoHyperlinkButton.NavigateUriProperty, value);
            }
        }

        /// <summary>Gets or sets the name of the target window or frame that the Web page should open in, or the name of the object within the Silverlight application to navigate to.</summary>
        /// <returns>Gets or sets the name of the target window or frame that the Web page should open in, or the name of the object within the Silverlight application to navigate to.</returns>
        public string TargetName
        {
            get
            {
                return base.GetValue(KinoHyperlinkButton.TargetNameProperty) as string;
            }
            set
            {
                base.SetValue(KinoHyperlinkButton.TargetNameProperty, value);
            }
        }

        static KinoHyperlinkButton()
        {
            NavigateUriProperty = DependencyProperty.Register("NavigateUri", typeof(Uri), typeof(KinoHyperlinkButton), null);
            TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(KinoHyperlinkButton), null);
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HyperlinkButton" /> class.</summary>
        public KinoHyperlinkButton()
        {
            base.DefaultStyleKey = typeof(KinoHyperlinkButton);
            if (IsPermissionGranted() == false)
            {
                this.IsEnabled = false;
            }
        }


        /// <summary>Provides handling for the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="P:System.Windows.Controls.HyperlinkButton.NavigateUri" /> property is not or cannot be converted to an absolute URI.</exception>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.HyperlinkButton.TargetName" /> property is set to a value that contains embedded whitespace.</exception>
        protected override void OnClick()
        {
            base.OnClick();
            if (NavigateUri != null && NavigateUri.IsAbsoluteUri)
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(NavigateUri.AbsoluteUri));
                }
                catch (Win32Exception)
                {
                }
            }
        }

        private static bool IsPermissionGranted()
        {
            try
            {
                var requestedPermission = new System.Security.Permissions.UIPermission(System.Security.Permissions.UIPermissionWindow.AllWindows);
                requestedPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
