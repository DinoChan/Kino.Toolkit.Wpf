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
    public class KinoFormItem : ContentControl
    {
        /// <summary>
        /// 标识 IsRequired 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(KinoFormItem), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 标识 Description 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(KinoFormItem), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标识 Label 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(object), typeof(KinoFormItem), new PropertyMetadata(default, OnLabelChanged));

        /// <summary>
        /// 标识 LabelTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.Register(nameof(LabelTemplate), typeof(DataTemplate), typeof(KinoFormItem), new PropertyMetadata(default(DataTemplate), OnLabelTemplateChanged));

        public KinoFormItem()
        {
            DefaultStyleKey = typeof(KinoFormItem);
        }

        /// <summary>
        /// 获取或设置Label的值
        /// </summary>
        public object Label
        {
            get => (object)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        /// <summary>
        /// 获取或设置LabelTemplate的值
        /// </summary>
        public DataTemplate LabelTemplate
        {
            get => (DataTemplate)GetValue(LabelTemplateProperty);
            set => SetValue(LabelTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置Description的值
        /// </summary>
        public object Description
        {
            get => (object)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// 获取或设置IsRequired的值
        /// </summary>
        public bool IsRequired
        {
            get => (bool)GetValue(IsRequiredProperty);
            set => SetValue(IsRequiredProperty, value);
        }

        /// <summary>
        /// Label 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Label 属性的旧值。</param>
        /// <param name="newValue">Label 属性的新值。</param>
        protected virtual void OnLabelChanged(object oldValue, object newValue)
        {
        }

        /// <summary>
        /// LabelTemplate 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">LabelTemplate 属性的旧值。</param>
        /// <param name="newValue">LabelTemplate 属性的新值。</param>
        protected virtual void OnLabelTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
        }

        private static void OnLabelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (object)args.OldValue;
            var newValue = (object)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoFormItem;
            target?.OnLabelChanged(oldValue, newValue);
        }

        private static void OnLabelTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (DataTemplate)args.OldValue;
            var newValue = (DataTemplate)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoFormItem;
            target?.OnLabelTemplateChanged(oldValue, newValue);
        }
    }
}
