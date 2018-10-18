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
    /// UserinfoDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserinfoDialog
    {
        public UserinfoDialog()
        {
            InitializeComponent();
        }



        private void OnSignUp(object sender, EventArgs e)
        {
            var signUpView = new SignUpView();
            signUpView.Finished += OnFinished;
            signUpView.GoBack += OnGoBack;
            ContentElement.Content = signUpView;
        }

        private void OnGoBack(object sender, EventArgs e)
        {
            var signInView = new SignInView();
            signInView.Finished += OnFinished;
            signInView.SignUp += OnSignUp;
            ContentElement.Content = signInView;
        }


        private void OnFinished(object sender, UserInfo e)
        {
            Close();
            var shell = new SimpleShell
            {
                User = e,
                Owner = Owner
            };
            shell.ShowDialog();
        }
    }
}
