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
    public static class FrameworkElementExtensions
    {
        public static bool ForceFocus(this FrameworkElement element)
        {
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
            return element.MoveFocus(request);
        }
    }
}
