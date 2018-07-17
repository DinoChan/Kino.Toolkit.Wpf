using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public static class WindowService
    {
        private static double? _paddedBorder;

        /// <summary>
        ///  returns the amount of extra border padding around captioned windows 
        /// </summary>
        public static double PaddedBorder
        {
            get
            {
                if (_paddedBorder == null)
                {
                    _paddedBorder = NativeMethods.GetSystemMetrics(SM.CXPADDEDBORDER);
                }

                return _paddedBorder.Value;
            }
        }
    }
}
