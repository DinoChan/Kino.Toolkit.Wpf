using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public class KinoWindow : Window
    {
        /// <summary>
        /// 标识 FunctionBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FunctionBarProperty =
            DependencyProperty.Register(nameof(FunctionBar), typeof(KinoWindowFunctionBar), typeof(KinoWindow), new PropertyMetadata(default(KinoWindowFunctionBar), OnFunctionBarChanged));

        public KinoWindow()
        {
            DefaultStyleKey = typeof(KinoWindow);

            var length = WindowService.PaddedBorder;
            DpiScale dpi = VisualTreeHelper.GetDpi(this);
            var lengthWithScale = length / dpi.DpiScaleX;
            ExtraBorderPadding = new Thickness(lengthWithScale);
        }

        public Thickness ExtraBorderPadding { get; }

        /// <summary>
        /// 获取或设置FunctionBar的值
        /// </summary>
        public KinoWindowFunctionBar FunctionBar
        {
            get => (KinoWindowFunctionBar)GetValue(FunctionBarProperty);
            set => SetValue(FunctionBarProperty, value);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
            {
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// FunctionBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">FunctionBar 属性的旧值。</param>
        /// <param name="newValue">FunctionBar 属性的新值。</param>
        protected virtual void OnFunctionBarChanged(KinoWindowFunctionBar oldValue, KinoWindowFunctionBar newValue)
        {
        }

        private static void OnFunctionBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (KinoWindowFunctionBar)args.OldValue;
            var newValue = (KinoWindowFunctionBar)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as KinoWindow;
            target?.OnFunctionBarChanged(oldValue, newValue);
        }
    }
}
