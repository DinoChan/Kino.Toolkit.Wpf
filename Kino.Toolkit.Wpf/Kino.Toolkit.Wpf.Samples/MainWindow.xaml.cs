using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowChrome c = new WindowChrome();
            var d = SystemParameters.CaptionHeight;
            var e = SystemParameters.WindowResizeBorderThickness;
            var f = SystemParameters.WindowNonClientFrameThickness;
            var g = SystemParameters.FixedFrameHorizontalBorderHeight;
            GotFocus += MainWindow_GotFocus;

        }

        private async void MainWindow_GotFocus(object sender, RoutedEventArgs e)
        {

            while (true)
            {
              await  Task.Delay(1000);
                var c = FocusManager.GetFocusedElement(this);
                if (c != null)
                    Debug.WriteLine(c);
            }
        }

        private void OnTreeViewSelectionChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            this.UpdateSelectedView(e.NewValue as SampleTreeViewItem);
        }

        private void UpdateSelectedView(SampleTreeViewItem treeViewItem)
        {
            if (treeViewItem != null)
            {
                Type type = treeViewItem.SampleType;
                if (type != null)
                {
                    string name = type.FullName;

                    Assembly assembly = GetType().Assembly;
                    Type sampleType = assembly.GetType(name);

                    SampleContentControl.Content = (FrameworkElement)Activator.CreateInstance(sampleType);
                }
            }
        }
    }
}
