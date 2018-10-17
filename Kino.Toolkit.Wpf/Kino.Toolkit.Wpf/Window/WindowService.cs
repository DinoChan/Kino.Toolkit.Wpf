using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// for custom window
    /// </summary>
    public class WindowService
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
        /// 标识 IsActiveCommands 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsActiveCommandsProperty =
            DependencyProperty.RegisterAttached("IsActiveCommands", typeof(bool), typeof(WindowService), new PropertyMetadata(default(bool), OnIsActiveCommandsChanged));

        private static void OnIsActiveCommandsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (bool)args.NewValue;
            if (obj is Window window && newValue)
            {
                var service = new WindowCommandHelper(window);
                service.ActiveCommands();
            }
        }

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

        /// <summary>
        /// 标识 IsKeepInWorkArea 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsKeepInWorkAreaProperty =
            DependencyProperty.RegisterAttached("IsKeepInWorkArea", typeof(bool), typeof(WindowService), new PropertyMetadata(default(bool), OnIsKeepInWorkAreaChanged));

        private static void OnIsKeepInWorkAreaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var newValue = (bool)args.NewValue;
            if (obj is Window window && newValue)
            {
                window.SizeChanged += (s, e) =>
                {
                    var point = window.PointToScreen(new Point(0, 0));

                    var allScreens = System.Windows.Forms.Screen.AllScreens.ToList();
                    var locationScreen = allScreens.SingleOrDefault(c => window.Left >= c.WorkingArea.Left && window.Left < c.WorkingArea.Right);
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

        private class WindowCommandHelper
        {
            private Window _window;

            public WindowCommandHelper(Window window)
            {
                _window = window;
            }

            public void ActiveCommands()
            {
                _window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
                _window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
                _window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
                _window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
                _window.CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
            }

            #region Window Commands

            private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = _window.ResizeMode == ResizeMode.CanResize || _window.ResizeMode == ResizeMode.CanResizeWithGrip;
            }

            private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = _window.ResizeMode != ResizeMode.NoResize;
            }

            private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
            {
                _window.Close();
            }

            private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
            {
                SystemCommands.MaximizeWindow(_window);
                e.Handled = true;
            }

            private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
            {
                SystemCommands.MinimizeWindow(_window);
                e.Handled = true;
            }

            private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
            {
                SystemCommands.RestoreWindow(_window);
                e.Handled = true;
            }

            private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
            {
                var point = _window.PointToScreen(new Point(0, 0));
                var dpi = VisualTreeHelper.GetDpi(_window);
                if (_window.WindowState == WindowState.Maximized)
                {
                    // 因为不想在最大化时改变标题高度，所以这里再加上 SystemParameters.FixedFrameHorizontalBorderHeight 才是正确的高度
                    point.Y += (SystemParameters.WindowNonClientFrameThickness.Top * dpi.DpiScaleX) + PaddedBorder + (SystemParameters.FixedFrameHorizontalBorderHeight * dpi.DpiScaleX);
                    point.X += (SystemParameters.WindowNonClientFrameThickness.Left * dpi.DpiScaleX) + PaddedBorder;
                }
                else
                {
                    point.X += _window.BorderThickness.Left;
                    point.Y += SystemParameters.WindowNonClientFrameThickness.Top * dpi.DpiScaleX;
                }

                CompositionTarget compositionTarget = PresentationSource.FromVisual(_window).CompositionTarget;
                SystemCommands.ShowSystemMenu(_window, compositionTarget.TransformFromDevice.Transform(point));
                e.Handled = true;
            }
            #endregion
        }
    }
}
