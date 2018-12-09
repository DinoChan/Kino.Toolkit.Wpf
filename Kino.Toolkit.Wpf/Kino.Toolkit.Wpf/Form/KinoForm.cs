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
    public partial class KinoForm : HeaderedItemsControl
    {
        private DataTemplate _labelMemberTemplate;

        public KinoForm()
        {
            DefaultStyleKey = typeof(KinoForm);
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
                var formItem = ItemContainerGenerator.ContainerFromIndex(i) as KinoFormItem;
                if (formItem != null)
                    PrepareFormItem(formItem, Items[i]);
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool isItemItsOwnContainer = false;
            if (item is FrameworkElement element)
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
            var style = GetContainerStyle(content);
            if (style != null)
                formItem.Style = style;
            else if (ItemContainerStyle != null)
                formItem.Style = ItemContainerStyle;
            else
                formItem.ClearValue(FrameworkElement.StyleProperty);

            var labelTemplate = GetLabelTemplate(content);
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
