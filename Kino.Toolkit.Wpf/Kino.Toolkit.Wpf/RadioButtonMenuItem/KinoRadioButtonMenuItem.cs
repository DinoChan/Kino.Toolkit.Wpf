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
        static KinoRadioButtonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KinoRadioButtonMenuItem), new FrameworkPropertyMetadata(typeof(KinoRadioButtonMenuItem)));
        }

        /// <inheritdoc />
        public KinoRadioButtonMenuItem()
        {
            Checked += OnChecked;
        }

        /// <inheritdoc />
        protected override void OnClick()
        {
            base.OnClick();
            this.IsChecked = true;
        }


        /// <summary>
        /// 获取或设置GroupName的值
        /// </summary>  
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <summary>
        /// 标识 GroupName 依赖属性。
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(KinoRadioButtonMenuItem), new PropertyMetadata(default(string)));


        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (IsChecked == false)
                return;

            var parent = VisualTreeHelper.GetParent(this) as FrameworkElement;
            if (parent != null)
            {
                var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childrenCount; i++)
                {
                    var menuItem = VisualTreeHelper.GetChild(parent, i) as KinoRadioButtonMenuItem;
                    if (menuItem != null && menuItem.GroupName == GroupName && menuItem != this && (menuItem.DataContext == parent.DataContext || menuItem.DataContext != DataContext))
                        menuItem.IsChecked = false;
                }
            }
        }


        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new KinoRadioButtonMenuItem();
        }
    }
}
