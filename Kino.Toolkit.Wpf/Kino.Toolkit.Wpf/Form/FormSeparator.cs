using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class FormSeparator : Separator
    {
        public FormSeparator()
        {
            DefaultStyleKey = typeof(FormSeparator);
            Form.SetIsItemItsOwnContainer(this, true);
        }
    }
}
