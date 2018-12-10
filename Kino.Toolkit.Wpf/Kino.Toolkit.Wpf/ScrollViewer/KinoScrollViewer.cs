using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// 主要解决嵌套ScrollViewer滚动劫持的问题。如果此ScrollViewer的VerticalScrollBarVisibility == ScrollBarVisibility.Disabled，不处理鼠标滚动事件。
    /// </summary>
    public class KinoScrollViewer : ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled)
                return;

            base.OnMouseWheel(e);
        }
    }
}
