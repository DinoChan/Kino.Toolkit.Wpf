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
    /// FormSampleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FormSampleWindow : Window
    {
        private readonly Style _horizontalFormItemStyle;
        private readonly Style _verticalFormItemStyle;

        public FormSampleWindow()
        {
            InitializeComponent();
            _horizontalFormItemStyle = Resources["HorizontalFormItemStyle"] as Style;
            _verticalFormItemStyle = Resources["VerticalFormItemStyle"] as Style;
            IsLargeSize = true;
            Root.SizeChanged += OnRootSizeChanged;
        }

        private void OnRootSizeChanged(object sender, SizeChangedEventArgs e)
        {
            IsLargeSize = e.NewSize.Width > 350;
            Root.ItemWidth = e.NewSize.Width > 700 ? Math.Max(350, e.NewSize.Width / 2) : double.NaN;
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private bool _isLargeSize;

        /// <summary>
        /// 获取或设置 IsLargeSize 的值
        /// </summary>
        public bool IsLargeSize
        {
            get { return _isLargeSize; ; }
            set
            {
                if (_isLargeSize == value)
                    return;

                _isLargeSize = value;
                Root.Resources.Clear();
                if (_isLargeSize)
                    Root.Resources.Add(typeof(KinoFormItem), _horizontalFormItemStyle);
                else
                    Root.Resources.Add(typeof(KinoFormItem), _verticalFormItemStyle);
            }
        }
    }
}
