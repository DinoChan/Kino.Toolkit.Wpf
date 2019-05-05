using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            var version = Assembly.GetAssembly(typeof(ExtendedButton)).GetName().Version;
            VersionElement.Text = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            foreach (var button in ControlsPanel.Children.OfType<HyperlinkButton>())
            {
                button.Click += (s, e) =>
                {
                    NavigateTo?.Invoke(this, button.Tag as Type);
                };
            }
        }

        public event EventHandler<Type> NavigateTo;
    }
}
