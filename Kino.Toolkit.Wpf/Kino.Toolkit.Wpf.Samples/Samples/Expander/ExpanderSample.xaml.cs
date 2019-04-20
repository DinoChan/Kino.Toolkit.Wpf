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
    /// ExpanderSample.xaml 的交互逻辑
    /// </summary>
    public partial class ExpanderSample
    {
        public ExpanderSample()
        {
            InitializeComponent(); Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var expanders = new List<KinoExpander>();
            Expander firstExpander = null;
            for (int i = 0; i < 10; i++)
            {
                var expander = new KinoExpander() { Header = "This is AccordionItem " + i };
                if (i == 0)
                    firstExpander = expander;

                Grid.SetRow(expander, i);
                var panel = new StackPanel();
                panel.Children.Add(new CheckBox { Content = "Calendar" });
                panel.Children.Add(new CheckBox { Content = "中国节假日" });
                panel.Children.Add(new CheckBox { Content = "Birthdays" });
                expander.Content = panel;
                MenuRoot.Children.Add(expander);
                MenuRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                int index = i;
                expander.Expanded += (s, args) =>
                {

                    var lastExpander = expanders.Where(p => p.IsExpanded && p != s).FirstOrDefault();
                    if (lastExpander != null)
                        lastExpander.IsExpanded = false;

                    MenuRoot.RowDefinitions[index].Height = new GridLength(1, GridUnitType.Star);
                };

                expander.Collapsed += (s, args) =>
                  {
                      if (expanders.Any(p => p.IsExpanded) == false)
                      {
                          expander.IsExpanded = true;
                          return;
                      }

                      MenuRoot.RowDefinitions[index].Height = new GridLength(1, GridUnitType.Auto);
                  };
                expanders.Add(expander);
            }


            firstExpander.IsExpanded = true;
        }
    }
}
