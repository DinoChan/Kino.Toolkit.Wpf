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
    public partial class FormItemSample
    {
        private readonly Style _formItemStyle;
        private readonly Style _editableFormItemStyle;

        public FormItemSample()
        {
            this.InitializeComponent();
            this._formItemStyle = this.Resources["FormItemStyle"] as Style;
            this._editableFormItemStyle = this.Resources["EditableFormItemStyle"] as Style;
            this.Root.Resources.Add(typeof(KinoFormItem), this._formItemStyle);
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK");
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cancel");
        }

        private void OnShowResponsiveDialog(object sender, RoutedEventArgs e)
        {
            var window = new FormSampleWindow();
            window.Show();
        }

        private async void OnCanEditChecked(object sender, RoutedEventArgs e)
        {
            this.Root.Resources.Clear();
            this.Root.Resources.Add(typeof(KinoFormItem), this._editableFormItemStyle);
            await this.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(100);
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                Root.MoveFocus(request);
            });
        }

        private void OnCanEditUnchecked(object sender, RoutedEventArgs e)
        {
            this.Root.Resources.Clear();
            this.Root.Resources.Add(typeof(KinoFormItem), this._formItemStyle);
        }
    }
}
