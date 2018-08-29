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
    /// ButtonSample.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonSample
    {
        public ButtonSample()
        {
            InitializeComponent();
            StatesListBox.Items.Add(ProgressState.None);
            StatesListBox.Items.Add(ProgressState.Idle);
            StatesListBox.Items.Add(ProgressState.Busy);
            StatesListBox.Items.Add(ProgressState.Completed);
            StatesListBox.Items.Add(ProgressState.Faulted);
            StatesListBox.Items.Add(ProgressState.Other);
            StatesListBox.SelectedIndex = 0;
            
        }

        private void OnContentOnlyCheck(object sender, RoutedEventArgs e)
        {
            BookmarkButton.Content = "bookmark";
            BookmarkButton.Icon = null;
        }

        private void OnIcnOnlyCheck(object sender, RoutedEventArgs e)
        {
            BookmarkButton.Content = null;
            if (BookmarkButton.Icon == null)
                BookmarkButton.Icon = CreateIcon();
        }

        private void OnBothCheck(object sender, RoutedEventArgs e)
        {
            BookmarkButton.Content = "bookmark";
            if (BookmarkButton.Icon == null)
                BookmarkButton.Icon = CreateIcon();
        }

        private object CreateIcon()
        {
            var textBlock = new TextBlock();
            textBlock.Text = "\xf02e";
            textBlock.Style = this.FindResource("FontAwesome") as Style;
            return textBlock;
        }

        private async void OnComment(object sender, RoutedEventArgs e)
        {
            var button = sender as KinoButton;
            button.IsEnabled = false;
            button.State = ProgressState.Busy;

            await Task.Delay(TimeSpan.FromSeconds(3));
            button.State = ProgressState.Completed;
            await Task.Delay(TimeSpan.FromSeconds(1));
            button.State = ProgressState.None;
            button.IsEnabled = true;
        }
    }
}
