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
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FormItem))]
    public partial class Form : HeaderedItemsControl
    {
        private DataTemplate _labelMemberTemplate;

        public Form()
        {
            DefaultStyleKey = typeof(Form);
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
        /// FunctionBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">FunctionBar 属性的旧值。</param>
        /// <param name="newValue">FunctionBar 属性的新值。</param>
        protected virtual void OnFunctionBarChanged(FormFunctionBar oldValue, FormFunctionBar newValue)
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
#pragma warning disable IDE0059 // 从不使用分配给符号的值
            var newTemplate = LabelMemberPath;
#pragma warning restore IDE0059 // 从不使用分配给符号的值

            int count = Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (ItemContainerGenerator.ContainerFromIndex(i) is FormItem formItem)
                    PrepareFormItem(formItem, Items[i]);
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool isItemItsOwnContainer = false;
            if (item is FrameworkElement element)
                isItemItsOwnContainer = GetIsItemItsOwnContainer(element);

            return item is FormItem || isItemItsOwnContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new FormItem();
            return item;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is FormItem formItem && item is FormItem == false)
            {
                if (item is FrameworkElement content)
                    PrepareFormFrameworkElement(formItem, content);
                else
                    PrepareFormItem(formItem, item);
            }
        }

        protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
        {
            return container is FormItem;
        }

        private static void OnFunctionBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (FormFunctionBar)args.OldValue;
            var newValue = (FormFunctionBar)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as Form;
            target?.OnFunctionBarChanged(oldValue, newValue);
        }

        private static void OnLabelMemberPathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as Form;
            target?.OnLabelMemberPathChanged(oldValue, newValue);
        }

        private static void OnFormPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue)
                return;

            if (obj is FrameworkElement content && content.Parent is Form form)
            {
                if (form.ItemContainerGenerator.ContainerFromItem(content) is FormItem formItem)
                    form.PrepareFormFrameworkElement(formItem, content);
            }
        }

        private void PrepareFormFrameworkElement(FormItem formItem, FrameworkElement content)
        {
            formItem.Label = GetLabel(content);
            formItem.Description = GetDescription(content);
            formItem.IsRequired = GetIsRequired(content);
            formItem.ClearValue(DataContextProperty);
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

            var binding = new Binding(nameof(Visibility))
            {
                Source = content,
                Mode = BindingMode.OneWay
            };
            formItem.SetBinding(VisibilityProperty, binding);
        }

        private void PrepareFormItem(FormItem formItem, object item)
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
