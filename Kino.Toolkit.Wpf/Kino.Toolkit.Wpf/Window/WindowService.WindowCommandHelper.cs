using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public partial class WindowService
    {
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
                DpiScale dpi = VisualTreeHelper.GetDpi(_window);
                if (_window.WindowState == WindowState.Maximized)
                {
                    // 因为不想在最大化时改变标题高度，所以这里再加上 SystemParameters.FixedFrameHorizontalBorderHeight 才是正确的高度
                    point.Y += (SystemParameters.WindowNonClientFrameThickness.Top * dpi.DpiScaleX) + WindowParameters.PaddedBorderThickness.Top + (SystemParameters.FixedFrameHorizontalBorderHeight * dpi.DpiScaleX);
                    point.X += (SystemParameters.WindowNonClientFrameThickness.Left * dpi.DpiScaleX) + WindowParameters.PaddedBorderThickness.Top;
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
        }
    }
}
