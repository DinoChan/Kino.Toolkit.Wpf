using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// for custom window
    /// </summary>
    public partial class WindowService
    {
        /// <summary>
        /// 标识 IsKeepInWorkArea 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsKeepInWorkAreaProperty =
            DependencyProperty.RegisterAttached("IsKeepInWorkArea", typeof(bool), typeof(WindowService), new PropertyMetadata(default(bool), OnIsKeepInWorkAreaChanged));

        /// <summary>
        /// 标识 IsActiveCommands 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsActiveCommandsProperty =
            DependencyProperty.RegisterAttached("IsActiveCommands", typeof(bool), typeof(WindowService), new PropertyMetadata(default(bool), OnIsActiveCommandsChanged));

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

        /// <summary>
        /// 从指定元素获取 IsActiveCommands 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static bool GetIsActiveCommands(Window obj) => (bool)obj.GetValue(IsActiveCommandsProperty);

        /// <summary>
        /// 将 IsActiveCommands 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetIsActiveCommands(Window obj, bool value) => obj.SetValue(IsActiveCommandsProperty, value);

        /// <summary>
        /// 从指定元素获取 IsKeepInWorkArea 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static bool GetIsKeepInWorkArea(Window obj) => (bool)obj.GetValue(IsKeepInWorkAreaProperty);

        /// <summary>
        /// 将 IsKeepInWorkArea 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetIsKeepInWorkArea(Window obj, bool value) => obj.SetValue(IsKeepInWorkAreaProperty, value);

        private static void OnIsActiveCommandsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (bool)args.NewValue;
            if (obj is Window window && newValue)
            {
                var service = new WindowCommandHelper(window);
                service.ActiveCommands();
            }
        }

        private static void OnIsKeepInWorkAreaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (bool)args.NewValue;
            if (obj is Window window && newValue)
            {
                window.SizeChanged += (s, e) =>
                {
                    Point point = window.PointToScreen(new Point(0, 0));

                    var allScreens = System.Windows.Forms.Screen.AllScreens.ToList();
                    System.Windows.Forms.Screen locationScreen = allScreens.SingleOrDefault(c => window.Left >= c.WorkingArea.Left && window.Left < c.WorkingArea.Right);
                    if (locationScreen != null)
                    {
                        var bottom = point.Y + window.ActualHeight - locationScreen.WorkingArea.Height;
                        if (bottom > 0)
                        {
                            window.Top -= bottom;
                        }
                    }
                };
                var service = new WindowCommandHelper(window);
                service.ActiveCommands();
            }
        }
    }
}
