using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf.Samples
{
    public class SampleTreeViewItem : TreeViewItem
    {

        /// <summary>
        /// 获取或设置SampleType的值
        /// </summary>
        public Type SampleType
        {
            get => (Type)GetValue(SampleTypeProperty);
            set => SetValue(SampleTypeProperty, value);
        }

        /// <summary>
        /// 标识 SampleType 依赖属性。
        /// </summary>
        public static readonly DependencyProperty SampleTypeProperty =
            DependencyProperty.Register(nameof(SampleType), typeof(Type), typeof(SampleTreeViewItem), new PropertyMetadata(null));

    }
}
