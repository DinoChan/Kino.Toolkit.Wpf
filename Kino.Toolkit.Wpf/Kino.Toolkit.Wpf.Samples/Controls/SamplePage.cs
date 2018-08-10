using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// 
    /// </summary>
    public class SamplePage : HeaderedContentControl
    {
        static SamplePage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamplePage), new FrameworkPropertyMetadata(typeof(SamplePage)));
        }


        /// <summary>
        /// 获取或设置Link的值
        /// </summary>  
        public Uri Link
        {
            get => (Uri)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        /// <summary>
        /// 标识 Link 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register(nameof(Link), typeof(Uri), typeof(SamplePage), new PropertyMetadata(default(Uri), OnLinkChanged));

        private static void OnLinkChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (Uri)args.OldValue;
            var newValue = (Uri)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SamplePage;
            target?.OnLinkChanged(oldValue, newValue);
        }

        /// <summary>
        /// Link 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Link 属性的旧值。</param>
        /// <param name="newValue">Link 属性的新值。</param>
        protected virtual void OnLinkChanged(Uri oldValue, Uri newValue)
        {
        }
    }
}
