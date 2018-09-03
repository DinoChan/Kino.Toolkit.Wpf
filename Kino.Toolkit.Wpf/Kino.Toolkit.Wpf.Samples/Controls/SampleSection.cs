using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Kino.Toolkit.Wpf.Samples
{
    public class SampleSection : HeaderedContentControl
    {
        public SampleSection()
        {
            DefaultStyleKey = typeof(SampleSection);
        }


        /// <summary>
        /// 获取或设置ControlPanel的值
        /// </summary>  
        public Object ControlPanel
        {
            get => (Object)GetValue(ControlPanelProperty);
            set => SetValue(ControlPanelProperty, value);
        }

        /// <summary>
        /// 标识 ControlPanel 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ControlPanelProperty =
            DependencyProperty.Register(nameof(ControlPanel), typeof(Object), typeof(SampleSection), new PropertyMetadata(default(Object), OnControlPanelChanged));

        private static void OnControlPanelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (Object)args.OldValue;
            var newValue = (Object)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SampleSection;
            target?.OnControlPanelChanged(oldValue, newValue);
        }

        /// <summary>
        /// ControlPanel 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">ControlPanel 属性的旧值。</param>
        /// <param name="newValue">ControlPanel 属性的新值。</param>
        protected virtual void OnControlPanelChanged(Object oldValue, Object newValue)
        {
        }



        /// <summary>
        /// 获取或设置Description的值
        /// </summary>  
        public object Description
        {
            get => (object)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// 标识 Description 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(SampleSection), new PropertyMetadata(null));


        public ObservableCollection<object> SourceCodes { get; } = new ObservableCollection<object>();

    }
}
