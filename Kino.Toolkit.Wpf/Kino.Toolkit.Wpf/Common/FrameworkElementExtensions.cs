using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public static class FrameworkElementExtensions
    {
        public static bool ForceFocus(this FrameworkElement element)
        {
            if (element is Control control && control.IsTabStop && control.Focus())
            {
                return true;
            }

            foreach (Control item in element.GetLogicalChildren().OfType<Control>().Where(c => c.IsTabStop))
            {
                if (item.Focus())
                {
                    return true;
                }
            }

            foreach (Control item in element.GetVisualDescendants().OfType<Control>().Where(c => c.IsTabStop))
            {
                if (item.Focus())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
