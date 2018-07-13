using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace Kino.Toolkit.Wpf
{
    [StyleTypedProperty(Property = nameof(RibbonStyle), StyleTargetType = typeof(Ribbon))]
    public class KinoRibbonWindow : RibbonWindow
    {
        public KinoRibbonWindow()
        {
            DefaultStyleKey = typeof(KinoRibbonWindow);
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }


        /// <summary>
        /// 获取或设置RibbonStyle的值
        /// </summary>  
        public Style RibbonStyle
        {
            get => (Style)GetValue(RibbonStyleProperty);
            set => SetValue(RibbonStyleProperty, value);
        }

        /// <summary>
        /// 标识 RibbonStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty RibbonStyleProperty =
            DependencyProperty.Register(nameof(RibbonStyle), typeof(Style), typeof(KinoRibbonWindow), new PropertyMetadata(default(Style), OnRibbonStyleChanged));

        private static void OnRibbonStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (Style)args.OldValue;
            var newValue = (Style)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoRibbonWindow;
            target?.OnRibbonStyleChanged(oldValue, newValue);
        }

        /// <summary>
        /// RibbonStyle 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">RibbonStyle 属性的旧值。</param>
        /// <param name="newValue">RibbonStyle 属性的新值。</param>
        protected virtual void OnRibbonStyleChanged(Style oldValue, Style newValue)
        {
            Resources.Remove(typeof(Ribbon));
            if (newValue != null)
                Resources.Add(typeof(Ribbon), newValue);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        #region Window Commands

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }

        #endregion
    }
}
