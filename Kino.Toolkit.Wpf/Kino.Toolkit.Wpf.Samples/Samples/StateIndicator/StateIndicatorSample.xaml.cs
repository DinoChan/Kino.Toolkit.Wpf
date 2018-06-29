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
    /// StateIndicatorSample.xaml 的交互逻辑
    /// </summary>
    public partial class StateIndicatorSample : UserControl
    {
        public StateIndicatorSample()
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
    }
}
