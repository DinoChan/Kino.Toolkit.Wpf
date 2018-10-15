using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    public class KinoWindow : Window
    {
        public KinoWindow()
        {
            DefaultStyleKey = typeof(KinoWindow);

            var length = WindowService.PaddedBorder;
            var dpi = VisualTreeHelper.GetDpi(this);
            var lengthWithScale = length / dpi.DpiScaleX;
            ExtraBorderPadding = new Thickness(lengthWithScale);
        }

        public Thickness ExtraBorderPadding { get; }

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
        /// 获取或设置CommandBar的值
        /// </summary>
        public KinoWindowCommandBar CommandBar
        {
            get => (KinoWindowCommandBar)GetValue(CommandBarProperty);
            set => SetValue(CommandBarProperty, value);
        }

        /// <summary>
        /// 标识 CommandBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandBarProperty =
            DependencyProperty.Register(nameof(CommandBar), typeof(KinoWindowCommandBar), typeof(KinoWindow), new PropertyMetadata(default(KinoWindowCommandBar), OnCommandBarChanged));

        private static void OnCommandBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (KinoWindowCommandBar)args.OldValue;
            var newValue = (KinoWindowCommandBar)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as KinoWindow;
            target?.OnCommandBarChanged(oldValue, newValue);
        }

        /// <summary>
        /// CommandBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">CommandBar 属性的旧值。</param>
        /// <param name="newValue">CommandBar 属性的新值。</param>
        protected virtual void OnCommandBarChanged(KinoWindowCommandBar oldValue, KinoWindowCommandBar newValue)
        {
        }
    }
}
