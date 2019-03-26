using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf.Primitives
{
    public class InnerContentControl : ContentControl
    {
        public event EventHandler Measuring;

        protected override Size MeasureOverride(Size constraint)
        {
            if (IsMeasureValid == false)
                Measuring?.Invoke(this, EventArgs.Empty);

            return base.MeasureOverride(constraint);
        }
    }
}
