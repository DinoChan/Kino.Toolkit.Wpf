using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace Kino.Toolkit.Wpf
{
    public class ExtendedWindow : Window
    {
        /// <summary>
        /// 标识 FunctionBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FunctionBarProperty =
            DependencyProperty.Register(nameof(FunctionBar), typeof(WindowFunctionBar), typeof(ExtendedWindow), new PropertyMetadata(default(WindowFunctionBar), OnFunctionBarChanged));

        private static readonly DependencyPropertyKey IsNonClientActivePropertyKey =
         DependencyProperty.RegisterReadOnly("IsNonClientActive", typeof(bool), typeof(ExtendedWindow), new FrameworkPropertyMetadata(false));

#pragma warning disable SA1202 // Elements must be ordered by access
        public static readonly DependencyProperty IsNonClientActiveProperty = IsNonClientActivePropertyKey.DependencyProperty;
#pragma warning restore SA1202 // Elements must be ordered by access

        private readonly IntPtr _trueValue = new IntPtr(1);

        public ExtendedWindow()
        {
            DefaultStyleKey = typeof(ExtendedWindow);
        }

        /// <summary>
        /// 获取或设置FunctionBar的值
        /// </summary>
        public WindowFunctionBar FunctionBar
        {
            get => (WindowFunctionBar)GetValue(FunctionBarProperty);
            set => SetValue(FunctionBarProperty, value);
        }

        public bool IsNonClientActive
        {
            get
            {
                return (bool)GetValue(IsNonClientActiveProperty);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
            {
                InvalidateMeasure();
            }

            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WndProc));
        }

        /// <summary>
        /// FunctionBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">FunctionBar 属性的旧值。</param>
        /// <param name="newValue">FunctionBar 属性的新值。</param>
        protected virtual void OnFunctionBarChanged(WindowFunctionBar oldValue, WindowFunctionBar newValue)
        {
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetValue(IsNonClientActivePropertyKey, true);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            SetValue(IsNonClientActivePropertyKey, false);
        }

        private static void OnFunctionBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (WindowFunctionBar)args.OldValue;
            var newValue = (WindowFunctionBar)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as ExtendedWindow;
            target?.OnFunctionBarChanged(oldValue, newValue);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowNotifications.WM_NCACTIVATE)
                SetValue(IsNonClientActivePropertyKey, wParam == _trueValue);

            return IntPtr.Zero;
        }
    }
}
