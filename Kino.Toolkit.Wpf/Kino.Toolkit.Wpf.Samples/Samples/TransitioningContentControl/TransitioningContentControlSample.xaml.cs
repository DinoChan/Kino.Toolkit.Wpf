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
    /// TransitioningContentControlSample.xaml 的交互逻辑
    /// </summary>
    public partial class TransitioningContentControlSample
    {

        private int _index = 0;

        public TransitioningContentControlSample()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            CustomContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            while (true)
            {
                await Task.Delay(2000);
                ContentControl.Content = Airport.SampleAirports[_index];
                _index++;
                if (_index >= Airport.SampleAirports.Count)
                    _index = 0;
            }
        }

        private void OnChangByDefault(object sender, RoutedEventArgs e)
        {
            DefaultContent.Transition = TransitioningContentControl.DefaultTransitionState;
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangeByLeft(object sender, RoutedEventArgs e)
        {
            DefaultContent.Transition = TransitioningContentControl.LeftTransitionState;
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangeByUp(object sender, RoutedEventArgs e)
        {
            DefaultContent.Transition = TransitioningContentControl.UpTransitionState;
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangeByRight(object sender, RoutedEventArgs e)
        {
            DefaultContent.Transition = TransitioningContentControl.RightTransitionState;
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangeByDown(object sender, RoutedEventArgs e)
        {
            DefaultContent.Transition = TransitioningContentControl.DownTransitionState;
            DefaultContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangByCustom(object sender, RoutedEventArgs e)
        {
            CustomContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void OnChangByCustom2(object sender, RoutedEventArgs e)
        {
            CustomContent.Transition = "CustomTransition";
            CustomContent.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
