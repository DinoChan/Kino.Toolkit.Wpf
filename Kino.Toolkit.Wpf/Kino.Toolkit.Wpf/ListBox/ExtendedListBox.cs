using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Kino.Toolkit.Wpf.Primitives;

namespace Kino.Toolkit.Wpf
{
    public class ExtendedListBox : ListBox, IMultiSelector
    {
        // Using a DependencyProperty as the backing store for IsMultiSelectCheckBoxEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultiSelectCheckBoxEnabledProperty =
            DependencyProperty.Register(nameof(IsMultiSelectCheckBoxEnabled), typeof(bool), typeof(ExtendedListBox), new PropertyMetadata(true));

        public bool IsMultiSelectCheckBoxEnabled
        {
            get { return (bool)GetValue(IsMultiSelectCheckBoxEnabledProperty); }
            set { SetValue(IsMultiSelectCheckBoxEnabledProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new KinoListBoxItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (element is KinoListBoxItem listBoxItem)
                listBoxItem.Owner = this;
        }
    }
}
