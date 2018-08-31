using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// SimpleShell.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleShell 
    {
        public SimpleShell()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UsernameItem.Header = User.Username;
        }

        public UserInfo User { get; internal set; }

        private void OnSignOut(object sender, RoutedEventArgs e)
        {
            Close();
            var dialog = new UserinfoDialog();
            dialog.Owner = Owner;
            dialog.ShowDialog();
        }
    }
}
