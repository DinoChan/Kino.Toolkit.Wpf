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
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(KinoFormItem))]
    public class KinoForm : HeaderedItemsControl
    {
        /// <summary>
        /// 标识 CommandBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandBarProperty =
            DependencyProperty.Register(nameof(CommandBar), typeof(KinoFormCommandBar), typeof(KinoForm), new PropertyMetadata(default(KinoFormCommandBar), OnCommandBarChanged));

        /// <summary>
        /// 标识 LabelMemberPath 依赖属性。
        /// </summary>
        public static readonly DependencyProperty LabelMemberPathProperty =
            DependencyProperty.Register(nameof(LabelMemberPath), typeof(string), typeof(KinoForm), new PropertyMetadata(default(string), OnLabelMemberPathChanged));

        /// <summary>
        /// 标识 Description 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object), OnFormPropertyChanged));

        /// <summary>
        /// 标识 Label 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object), OnFormPropertyChanged));

        /// <summary>
        /// 标识 LabelTemplate 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty =
            DependencyProperty.RegisterAttached("LabelTemplate", typeof(DataTemplate), typeof(KinoForm), new PropertyMetadata(default(DataTemplate), OnFormPropertyChanged));

        /// <summary>
        /// 标识 IsItemItsOwnContainer 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsItemItsOwnContainerProperty =
            DependencyProperty.RegisterAttached("IsItemItsOwnContainer", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool), OnFormPropertyChanged));

        /// <summary>
        /// 标识 IsRequired 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool), OnFormPropertyChanged));

        /// <summary>
        /// 标识 ContainerStyle 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ContainerStyleProperty =
            DependencyProperty.RegisterAttached("ContainerStyle", typeof(Style), typeof(KinoForm), new PropertyMetadata(default(Style), OnFormPropertyChanged));

        private DataTemplate _labelMemberTemplate;

        public KinoForm()
        {
            DefaultStyleKey = typeof(KinoForm);
        }

        /// <summary>
        /// 获取或设置CommandBar的值
        /// </summary>
        public KinoFormCommandBar CommandBar
        {
            get => (KinoFormCommandBar)GetValue(CommandBarProperty);
            set => SetValue(CommandBarProperty, value);
        }

        /// <summary>
        /// 获取或设置LabelMemberPath的值
        /// </summary>
        public string LabelMemberPath
        {
            get => (string)GetValue(LabelMemberPathProperty);
            set => SetValue(LabelMemberPathProperty, value);
        }

        private DataTemplate LabelMemberTemplate
        {
            get
            {
                if (_labelMemberTemplate == null)
                {
                    _labelMemberTemplate = (DataTemplate)XamlReader.Parse(@"
                    <DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                    		<TextBlock Text=""{Binding " + LabelMemberPath + @"}"" VerticalAlignment=""Center""/>
                    </DataTemplate>");
                }

                return _labelMemberTemplate;
            }
        }

        /// <summary>
        /// 从指定元素获取 IsRequired 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetIsRequired(DependencyObject obj) => (bool)obj.GetValue(IsRequiredProperty);

        /// <summary>
        /// 将 IsRequired 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetIsRequired(DependencyObject obj, bool value) => obj.SetValue(IsRequiredProperty, value);

        /// <summary>
        /// 从指定元素获取 Label 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object GetLabel(DependencyObject obj) => obj.GetValue(LabelProperty);

        /// <summary>
        /// 将 Label 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetLabel(DependencyObject obj, object value) => obj.SetValue(LabelProperty, value);

        /// <summary>
        /// 从指定元素获取 LabelTemplate 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static DataTemplate GetLabelTemplate(DependencyObject obj) => (DataTemplate)obj.GetValue(LabelTemplateProperty);

        /// <summary>
        /// 将 LabelTemplate 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetLabelTemplate(DependencyObject obj, DataTemplate value) => obj.SetValue(LabelTemplateProperty, value);

        /// <summary>
        /// 从指定元素获取 Description 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object GetDescription(DependencyObject obj) => obj.GetValue(DescriptionProperty);

        /// <summary>
        /// 将 Description 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetDescription(DependencyObject obj, object value) => obj.SetValue(DescriptionProperty, value);

        /// <summary>
        /// 从指定元素获取 IsItemItsOwnContainer 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetIsItemItsOwnContainer(DependencyObject obj) => (bool)obj.GetValue(IsItemItsOwnContainerProperty);

        /// <summary>
        /// 将 IsItemItsOwnContainer 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetIsItemItsOwnContainer(DependencyObject obj, bool value) => obj.SetValue(IsItemItsOwnContainerProperty, value);

        /// <summary>
        /// 从指定元素获取 ContainerStyle 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static Style GetContainerStyle(DependencyObject obj) => (Style)obj.GetValue(ContainerStyleProperty);

        /// <summary>
        /// 将 ContainerStyle 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetContainerStyle(DependencyObject obj, Style value) => obj.SetValue(ContainerStyleProperty, value);

        /// <summary>
        /// CommandBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">CommandBar 属性的旧值。</param>
        /// <param name="newValue">CommandBar 属性的新值。</param>
        protected virtual void OnCommandBarChanged(KinoFormCommandBar oldValue, KinoFormCommandBar newValue)
        {
        }

        /// <summary>
        /// LabelMemberPath 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">LabelMemberPath 属性的旧值。</param>
        /// <param name="newValue">LabelMemberPath 属性的新值。</param>
        protected virtual void OnLabelMemberPathChanged(string oldValue, string newValue)
        {
            // refresh the label member template.
            _labelMemberTemplate = null;
            var newTemplate = LabelMemberPath;

            int count = Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is KinoFormItem formItem)
                    PrepareFormItem(formItem, Items[i]);
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool isItemItsOwnContainer = false;
            if (item is DependencyObject element)
                isItemItsOwnContainer = GetIsItemItsOwnContainer(element);

            return item is KinoFormItem || isItemItsOwnContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new KinoFormItem();
            return item;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is KinoFormItem formItem && item is KinoFormItem == false)
            {
                if (item is FrameworkElement content)
                    PrepareFormFrameworkElement(formItem, content);
                else
                    PrepareFormItem(formItem, item);
            }
        }

        private static void OnCommandBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (KinoFormCommandBar)args.OldValue;
            var newValue = (KinoFormCommandBar)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoForm;
            target?.OnCommandBarChanged(oldValue, newValue);
        }

        private static void OnLabelMemberPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as KinoForm;
            target?.OnLabelMemberPathChanged(oldValue, newValue);
        }

        private static void OnFormPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue)
                return;

            if (obj is FrameworkElement content && content.Parent is KinoForm form)
            {
                if (form.ItemContainerGenerator.ContainerFromItem(content) is KinoFormItem formItem)
                    form.PrepareFormFrameworkElement(formItem, content);
            }
        }

        private void PrepareFormFrameworkElement(KinoFormItem formItem, FrameworkElement content)
        {
            formItem.Label = GetLabel(content);
            formItem.Description = GetDescription(content);
            formItem.IsRequired = GetIsRequired(content);
            Style style = GetContainerStyle(content);
            if (style != null)
                formItem.Style = style;
            else if (ItemContainerStyle != null)
                formItem.Style = ItemContainerStyle;
            else
                formItem.ClearValue(FrameworkElement.StyleProperty);

            DataTemplate labelTemplate = GetLabelTemplate(content);
            if (labelTemplate != null)
                formItem.LabelTemplate = labelTemplate;
        }

        private void PrepareFormItem(KinoFormItem formItem, object item)
        {
            if (formItem == item)
                return;

            if (item is FrameworkElement)
                return;

            formItem.LabelTemplate = LabelMemberTemplate;
            formItem.Label = item;
        }
    }
}
