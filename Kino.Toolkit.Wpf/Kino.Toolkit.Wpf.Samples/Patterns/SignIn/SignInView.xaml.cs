using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Prism.Mvvm;

namespace Kino.Toolkit.Wpf.Samples
{
    /// <summary>
    /// SignInView.xaml 的交互逻辑
    /// </summary>
    public partial class SignInView : UserControl
    {
        private UserInfo _user;

        public SignInView()
        {
            InitializeComponent();
            _user = new UserInfo();
            DataContext = _user;
        }

        public event EventHandler SignUp;

        public event EventHandler<UserInfo> Finished;

        private void OnSignUp(object sender, RoutedEventArgs e)
        {
            SignUp?.Invoke(this, EventArgs.Empty);
        }

        private async void OnSignIn(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            var result = false;
            try
            {
                SignInButton.State = ProgressState.Busy;
                await Task.Delay(2000);
                if (_user.Username != "admin" || _user.Password != "admin")
                {
                    _user.ErrorsContainer.SetErrors(nameof(UserInfo.Password), new List<string> { "请输入admin/admin" });
                    SignInButton.State = ProgressState.Faulted;
                }
                else
                {
                    result = true;
                    SignInButton.State = ProgressState.Completed;
                }
            }
            finally
            {
                IsEnabled = true;
            }

            await Task.Delay(2000);
            SignInButton.State = ProgressState.None;
            if (result)
                Finished?.Invoke(this, _user);
        }
    }

    public class UserInfo : ModelBase
    {
        private string _username;

        /// <summary>
        /// 获取或设置 Usernme 的值
        /// </summary>
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username == value)
                    return;

                if (string.IsNullOrWhiteSpace(value))
                    ErrorsContainer.SetErrors(nameof(Username), new List<string> { "请输入用户名" });
                else
                    ErrorsContainer.SetErrors(nameof(Username), null);

                _username = value;
                RaisePropertyChanged();
            }
        }


        private string _passwrd;

        /// <summary>
        /// 获取或设置 Password 的值
        /// </summary>
        public string Password
        {
            get { return _passwrd; }
            set
            {
                if (_passwrd == value)
                    return;

                _passwrd = value;

                if (string.IsNullOrWhiteSpace(value))
                    ErrorsContainer.SetErrors(nameof(Password), new List<string> { "请输入密码" });
                else
                    ErrorsContainer.SetErrors(nameof(Password), null);

                RaisePropertyChanged();
            }
        }
    }
}
