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
    /// FormSample.xaml 的交互逻辑
    /// </summary>
    public partial class FormSample 
    {
        public FormSample()
        {
            InitializeComponent();
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cancel");
        }

        private void OnShowNewDialog(object sender, RoutedEventArgs e)
        {
            var window = new FormSampleWindow();
            window.Show();
        }

        
    }

    public class FormTestModel
    {

        private string _name;

        /// <summary>
        /// 获取或设置 Name 的值
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                if (string.IsNullOrEmpty(value))
                    throw new Exception("can not be empty");

                _name = value;
            }
        }

        
    }
}
