using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    /// <summary>
    /// 单选菜单项
    /// </summary>
    public class KinoRadioButtonMenuItem : MenuItem
    {
        /// <summary>
        /// 标识 GroupName 依赖属性。
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(KinoRadioButtonMenuItem), new PropertyMetadata(default(string)));

        static KinoRadioButtonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KinoRadioButtonMenuItem), new FrameworkPropertyMetadata(typeof(KinoRadioButtonMenuItem)));
        }

        /// <summary>
        /// 获取或设置GroupName的值
        /// </summary>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <inheritdoc />
        protected override void OnClick()
        {
            base.OnClick();
            IsChecked = true;
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new KinoRadioButtonMenuItem();
        }

        /// <inheritdoc />
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            if (VisualTreeHelper.GetParent(this) is FrameworkElement parent)
            {
                foreach (var menuItem in this.GetVisualSiblings().OfType<KinoRadioButtonMenuItem>())
                {
                    if (menuItem.GroupName == GroupName && (menuItem.DataContext == parent.DataContext || menuItem.DataContext != DataContext))
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
        }
    }
}
