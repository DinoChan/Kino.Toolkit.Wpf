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
    /// TestModelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestModelWindow
    {
        private readonly TestCurdService _curdService;
        private readonly bool _isNewObject;

        public TestModelWindow(TestCurdService curdService, TestModel testModel = null)
        {
            InitializeComponent();
            _isNewObject = testModel == null;
            if (testModel == null)
                TestModel = new TestModel();
            else
                TestModel = new TestModel { Age = testModel.Age, Name = testModel.Name, Id = testModel.Id };

            DataContext = TestModel;
            _curdService = curdService;
        }


        public TestModel TestModel { get; private set; }

        private async void OnSave(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;
            try
            {
                if (_isNewObject)
                    TestModel = await _curdService.Create(TestModel);
                else
                    await _curdService.Update(TestModel);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                BusyIndicator.IsBusy = false;
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
