using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Kino.Toolkit.Wpf
{
    public partial class Form : HeaderedItemsControl
    {
        /// <summary>
        /// 标识 FunctionBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty FunctionBarProperty =
            DependencyProperty.Register(nameof(FunctionBar), typeof(FormFunctionBar), typeof(Form), new PropertyMetadata(default(FormFunctionBar), OnFunctionBarChanged));

        /// <summary>
        /// 标识 LabelMemberPath 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LabelMemberPathProperty =
            DependencyProperty.Register(nameof(LabelMemberPath), typeof(string), typeof(Form), new PropertyMetadata(default(string), OnLabelMemberPathChanged));

        /// <summary>
        /// 标识 Description 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description", typeof(object), typeof(Form), new PropertyMetadata(default, OnFormPropertyChanged));

        /// <summary>
        /// 标识 Label 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(object), typeof(Form), new PropertyMetadata(default, OnFormPropertyChanged));

        /// <summary>
        /// 标识 LabelTemplate 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.RegisterAttached("LabelTemplate", typeof(DataTemplate), typeof(Form), new PropertyMetadata(default(DataTemplate), OnFormPropertyChanged));

        /// <summary>
        /// 标识 IsItemItsOwnContainer 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsItemItsOwnContainerProperty =
            DependencyProperty.RegisterAttached("IsItemItsOwnContainer", typeof(bool), typeof(Form), new PropertyMetadata(default(bool), OnFormPropertyChanged));

        /// <summary>
        /// 标识 IsRequired 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(Form), new PropertyMetadata(default(bool), OnFormPropertyChanged));

        /// <summary>
        /// 标识 ContainerStyle 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ContainerStyleProperty =
            DependencyProperty.RegisterAttached("ContainerStyle", typeof(Style), typeof(Form), new PropertyMetadata(default(Style), OnFormPropertyChanged));

        /// <summary>
        /// 获取或设置FunctionBar的值
        /// </summary>
        public FormFunctionBar FunctionBar
        {
            get => (FormFunctionBar)GetValue(FunctionBarProperty);
            set => SetValue(FunctionBarProperty, value);
        }

        /// <summary>
        /// 获取或设置LabelMemberPath的值
        /// </summary>
        public string LabelMemberPath
        {
            get => (string)GetValue(LabelMemberPathProperty);
            set => SetValue(LabelMemberPathProperty, value);
        }

        /// <summary>
        /// 从指定元素获取 IsRequired 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static bool GetIsRequired(FrameworkElement obj) => (bool)obj.GetValue(IsRequiredProperty);

        /// <summary>
        /// 将 IsRequired 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetIsRequired(FrameworkElement obj, bool value) => obj.SetValue(IsRequiredProperty, value);

        /// <summary>
        /// 从指定元素获取 Label 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static object GetLabel(FrameworkElement obj) => obj.GetValue(LabelProperty);

        /// <summary>
        /// 将 Label 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetLabel(FrameworkElement element, object value) => element.SetValue(LabelProperty, value);

        /// <summary>
        /// 从指定元素获取 LabelTemplate 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static DataTemplate GetLabelTemplate(FrameworkElement element) => (DataTemplate)element.GetValue(LabelTemplateProperty);

        /// <summary>
        /// 将 LabelTemplate 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetLabelTemplate(FrameworkElement obj, DataTemplate value) => obj.SetValue(LabelTemplateProperty, value);

        /// <summary>
        /// 从指定元素获取 Description 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static object GetDescription(FrameworkElement obj) => obj.GetValue(DescriptionProperty);

        /// <summary>
        /// 将 Description 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetDescription(FrameworkElement obj, object value) => obj.SetValue(DescriptionProperty, value);

        /// <summary>
        /// 从指定元素获取 IsItemItsOwnContainer 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static bool GetIsItemItsOwnContainer(FrameworkElement obj) => (bool)obj.GetValue(IsItemItsOwnContainerProperty);

        /// <summary>
        /// 将 IsItemItsOwnContainer 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetIsItemItsOwnContainer(FrameworkElement obj, bool value) => obj.SetValue(IsItemItsOwnContainerProperty, value);

        /// <summary>
        /// 从指定元素获取 ContainerStyle 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static Style GetContainerStyle(FrameworkElement obj) => (Style)obj.GetValue(ContainerStyleProperty);

        /// <summary>
        /// 将 ContainerStyle 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetContainerStyle(FrameworkElement obj, Style value) => obj.SetValue(ContainerStyleProperty, value);
    }
}
