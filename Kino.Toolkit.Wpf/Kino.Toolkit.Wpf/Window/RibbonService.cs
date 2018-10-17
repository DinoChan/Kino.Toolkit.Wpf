using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kino.Toolkit.Wpf
{
    public static class RibbonService
    {
        public static double RibbonContextualTabGroupHeight => SystemParameters.WindowNonClientFrameThickness.Top + 1;
    }
}
