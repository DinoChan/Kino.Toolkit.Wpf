using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class FunctionBar : HeaderedItemsControl
    {
        public FunctionBar()
        {
        }

        public ObservableCollection<object> Options { get; } = new ObservableCollection<object>();
    }
}
