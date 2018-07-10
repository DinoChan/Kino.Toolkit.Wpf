using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoSquareBox : ContentControl
    {
        public KinoSquareBox()
        {
            DefaultStyleKey = typeof(KinoSquareBox);
            TemplateSettings = new KinoSquareBoxTemplateSettings(0);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var width = 20d;
            var height = 20d;
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                width = double.IsNaN(this.Width) == false ? this.Width : constraint.Width;
                height = double.IsNaN(this.Height) == false ? this.Height : constraint.Height;
            }
            TemplateSettings = new KinoSquareBoxTemplateSettings(Math.Min(width, height));
            return base.MeasureOverride(constraint);
        }



        /// <summary>
        /// 获取或设置TemplateSettings的值
        /// </summary>  
        public KinoSquareBoxTemplateSettings TemplateSettings
        {
            get => (KinoSquareBoxTemplateSettings)GetValue(TemplateSettingsProperty);
            set => SetValue(TemplateSettingsProperty, value);
        }

        /// <summary>
        /// 标识 TemplateSettings 依赖属性。
        /// </summary>
        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register(nameof(TemplateSettings), typeof(KinoSquareBoxTemplateSettings), typeof(KinoSquareBox), new PropertyMetadata(null));


    }
}
