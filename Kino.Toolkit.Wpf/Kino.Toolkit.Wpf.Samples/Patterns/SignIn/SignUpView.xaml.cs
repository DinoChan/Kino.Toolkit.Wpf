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
    /// SignUpView.xaml 的交互逻辑
    /// </summary>
    public partial class SignUpView : UserControl
    {
        private readonly UserInfo _user;
        public SignUpView()
        {
            InitializeComponent();
            _user = new UserInfo();
            DataContext = _user;
        }

        public event EventHandler GoBack;

        public event EventHandler<UserInfo> Finished;

        private async void OnSignUp(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            try
            {
                SignUpButton.State = ProgressState.Busy;
                await Task.Delay(2000);
                Finished?.Invoke(this, _user);
            }
            finally
            {
                IsEnabled = true;
            }

            await Task.Delay(2000);
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            GoBack?.Invoke(this, EventArgs.Empty);
        }
    }
}
