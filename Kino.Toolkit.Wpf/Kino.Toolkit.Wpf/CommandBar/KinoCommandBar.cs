using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Kino.Toolkit.Wpf
{
    public class KinoCommandBar : ItemsControl
    {
        public KinoCommandBar()
        {
        }

        public ObservableCollection<object> Options { get; } = new ObservableCollection<object>();
    }
}
