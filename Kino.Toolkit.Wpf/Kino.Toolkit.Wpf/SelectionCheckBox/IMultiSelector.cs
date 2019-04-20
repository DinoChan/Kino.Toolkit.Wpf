using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf.Primitives
{
    public interface IMultiSelector
    {
        ItemCollection Items { get; }

        IList SelectedItems { get; }

        event SelectionChangedEventHandler SelectionChanged;

        void SelectAll();
        
        void UnselectAll();
    }
}
