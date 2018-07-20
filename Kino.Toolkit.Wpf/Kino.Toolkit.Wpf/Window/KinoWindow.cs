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
                DragMove();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }
    }
}
