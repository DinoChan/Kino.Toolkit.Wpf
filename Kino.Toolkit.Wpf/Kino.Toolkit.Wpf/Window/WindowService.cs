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
        public static Thickness WindowOuterBorderThickness
        {
            get
            {
                var resizeBorderThicknes = SystemParameters.WindowResizeBorderThickness;
                var nonClientFrameThickness = SystemParameters.WindowNonClientFrameThickness;
                var left = resizeBorderThicknes.Left + nonClientFrameThickness.Left;
                var top = resizeBorderThicknes.Top + nonClientFrameThickness.Top - SystemParameters.WindowCaptionHeight;
                var right = resizeBorderThicknes.Right + nonClientFrameThickness.Right;
                var bottom = resizeBorderThicknes.Bottom + nonClientFrameThickness.Bottom;

                if (Application.Current != null && Application.Current.MainWindow != null)
                {
                    var dpi = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
                    if (dpi.DpiScaleX > 1 )
                    {
                        var scale = 1 / dpi.DpiScaleX;
                        left += scale;
                        top += scale;
                        right += scale;
                        bottom += scale;
                    }
                }

                return new Thickness(left, top, right, bottom);
            }
        }
    }
}
