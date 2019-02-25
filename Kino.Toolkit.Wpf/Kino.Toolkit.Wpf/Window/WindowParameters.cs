using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kino.Toolkit.Wpf
{
    public static class WindowParameters
    {
        private static Thickness? _paddedBorderThickness;

        private static double? _ribbonContextualTabGroupHeight;

        /// <summary>
        /// returns the border thickness padding around captioned windows,in pixels. Windows XP/2000:  This value is not supported.
        /// </summary>
        public static Thickness PaddedBorderThickness
        {
            [SecurityCritical]
            get
            {
                if (_paddedBorderThickness == null)
                {
                    var paddedBorder = NativeMethods.GetSystemMetrics(SM.CXPADDEDBORDER);
                    var dpi = GetDpi();
                    Size frameSize = new Size(paddedBorder, paddedBorder);
                    Size frameSizeInDips = DpiHelper.DeviceSizeToLogical(frameSize, dpi / 96.0, dpi / 96.0);
                    _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height, frameSizeInDips.Width, frameSizeInDips.Height);
                }

                return _paddedBorderThickness.Value;
            }
        }

        public static double RibbonContextualTabGroupHeight
        {

            get
            {
                if (_ribbonContextualTabGroupHeight == null)
                {
                    _ribbonContextualTabGroupHeight = SystemParameters.WindowNonClientFrameThickness.Top + (1d * GetDpi() / 96.0);
                }

                return _ribbonContextualTabGroupHeight.Value;
            }
        }


        /// <summary>
        /// Get Dpi
        /// </summary>
        /// <returns>Return 96,144/returns>
        public static double GetDpi()
        {
            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);

            var dpiX = (int)dpiXProperty.GetValue(null, null);
            return dpiX;
        }
    }
}
