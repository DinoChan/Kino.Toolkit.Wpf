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
    public class RadioButtonMenuItem : MenuItem
    {
        /// <summary>
        /// 标识 GroupName 依赖属性。
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(RadioButtonMenuItem), new PropertyMetadata(default(string)));

        static RadioButtonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonMenuItem), new FrameworkPropertyMetadata(typeof(RadioButtonMenuItem)));
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
            return new RadioButtonMenuItem();
        }

        /// <inheritdoc />
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            if (this.Parent is MenuItem parent)
            {
                foreach (var menuItem in parent.Items.OfType<RadioButtonMenuItem>())
                {
                    if (menuItem != this && menuItem.GroupName == GroupName && (menuItem.DataContext == parent.DataContext || menuItem.DataContext != DataContext))
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
        }
    }
}
