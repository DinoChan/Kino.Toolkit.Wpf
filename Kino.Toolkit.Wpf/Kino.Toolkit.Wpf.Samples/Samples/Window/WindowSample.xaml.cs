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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// WindowSample.xaml 的交互逻辑
    /// </summary>
    public partial class WindowSample 
    {
        public WindowSample()
        {
            InitializeComponent();
            
        }

        private void OnTestWindowShow(object sender, RoutedEventArgs e)
        {
            var window= new TestWindow();
            SetupWindowStyle(window);
            window.Show();
        }

        private void OnTestWindowShowDalog(object sender, RoutedEventArgs e)
        {
            var window = new TestWindow();
            SetupWindowStyle(window);
            window.ShowDialog();
        }

        private void OnTestRibbonWindowShow(object sender, RoutedEventArgs e)
        {
            var window = new TestRibbonWindow();
            SetupWindowStyle(window);
            window.Show();
        }

        private void OnTestRibbonWindowShowDialog(object sender, RoutedEventArgs e)
        {
            var window = new TestRibbonWindow();
            SetupWindowStyle(window);
            window.ShowDialog();
        }

        private void OnSystemWindowShowDialog(object sender, RoutedEventArgs e)
        {
            var window = new TestSystemWindow();
            SetupWindowStyle(window);
            window.Show();
        }

        private void OnSystemWindowShow(object sender, RoutedEventArgs e)
        {
            var window = new TestSystemWindow();
            SetupWindowStyle(window);
            window.ShowDialog();
        }

        private void OnSystemRibbonWindowShow(object sender, RoutedEventArgs e)
        {
            var window = new TestSystemRibbonWindow();
            SetupWindowStyle(window);
            window.Show();
        }

        private void OnSystemRibbonWindowShowDialog(object sender, RoutedEventArgs e)
        {
            var window = new TestSystemRibbonWindow();
            SetupWindowStyle(window);
            window.ShowDialog();
        }

        private void SetupWindowStyle(Window window)
        {
            window.WindowState = (WindowState)WindowStateListBox.SelectedItem;
            window.WindowStartupLocation = (WindowStartupLocation)WindowStartupLocationListBox.SelectedItem;
            window.ResizeMode = (ResizeMode)ResizeModeListBox.SelectedItem;
        }

      
    }
}
