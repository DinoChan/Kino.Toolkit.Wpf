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
            var target = obj as Window;
            if (target == null || newValue == false)
                return;

            var service = new WindowCommandHelper(target);
            service.ActiveCommands();
        }

        //public static double WindowCaptionHeightWithBorder
        //{
        //    get
        //    {
        //        return SystemParameters.WindowCaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight;
        //    }
        //}




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
                Point point = _window.PointToScreen(new Point(0, 0));
                if (_window.WindowState == WindowState.Maximized)
                {
                    var length = WindowService.PaddedBorder;
                    var dpi = VisualTreeHelper.GetDpi(_window);
                    var lengthWithScale = length / dpi.DpiScaleX;
                    point.Y += SystemParameters.WindowCaptionHeight + lengthWithScale + SystemParameters.WindowResizeBorderThickness.Top;
                    point.X += lengthWithScale + SystemParameters.WindowResizeBorderThickness.Left;
                }
                else
                {
                    point.X += _window.BorderThickness.Left;
                    point.Y += SystemParameters.WindowCaptionHeight + _window.BorderThickness.Top;
                }
                CompositionTarget compositionTarget = PresentationSource.FromVisual(_window).CompositionTarget;
                SystemCommands.ShowSystemMenu(_window, compositionTarget.TransformFromDevice.Transform(point));
                e.Handled = true;
            }
            #endregion
        }
    }
}
