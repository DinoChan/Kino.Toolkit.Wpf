using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class SquareBox : ContentControl
    {
        /// <summary>
        /// 标识 TemplateSettings 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register(nameof(TemplateSettings), typeof(SquareBoxTemplateSettings), typeof(SquareBox), new PropertyMetadata(null));

        public SquareBox()
        {
            DefaultStyleKey = typeof(SquareBox);
            TemplateSettings = new SquareBoxTemplateSettings(0);
        }

        /// <summary>
        /// 获取或设置TemplateSettings的值
        /// </summary>
        public SquareBoxTemplateSettings TemplateSettings
        {
            get => (SquareBoxTemplateSettings)GetValue(TemplateSettingsProperty);
            set => SetValue(TemplateSettingsProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var width = 20d;
            var height = 20d;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                width = double.IsNaN(Width) == false ? Width : constraint.Width;
                height = double.IsNaN(Height) == false ? Height : constraint.Height;
            }

            TemplateSettings = new SquareBoxTemplateSettings(Math.Min(width, height));
            return base.MeasureOverride(constraint);
        }
         }
}
