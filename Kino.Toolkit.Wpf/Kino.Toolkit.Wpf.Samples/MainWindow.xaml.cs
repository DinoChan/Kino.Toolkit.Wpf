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
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            HomeItem.IsSelected = true;
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
                    var page = (FrameworkElement)Activator.CreateInstance(sampleType);
                    SampleContentControl.Content = page;
                    if (page is HomePage homePage)
                    {
                        homePage.NavigateTo += OnNavigateTo;
                    }
                }
            }
        }

        private void OnNavigateTo(object sender, Type type)
        {
            foreach (var item in TreeView.Items.OfType<SampleTreeViewItem>())
            {
                SelectItem(item, type);
            }
        }

        private void SelectItem(SampleTreeViewItem item, Type type)
        {
            if (item.SampleType == type)
            {
                item.IsSelected = true;
                return;
            }

            foreach (var subItem in item.Items.OfType<SampleTreeViewItem>())
            {
                if (subItem.SampleType == type)
                {
                    subItem.IsSelected = true;
                    return;
                }

                SelectItem(subItem, type);
            }
        }
    }
}
