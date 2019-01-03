using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoFunctionBar : HeaderedItemsControl
    {
        public KinoFunctionBar()
        {
        }

        public ObservableCollection<object> Options { get; } = new ObservableCollection<object>();
    }
}
