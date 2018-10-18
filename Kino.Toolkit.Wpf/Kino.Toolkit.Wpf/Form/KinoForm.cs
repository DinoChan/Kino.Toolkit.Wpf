using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kino.Toolkit.Wpf
{
    public class KinoForm : ItemsControl
    {
        /// <summary>
        /// 标识 CommandBar 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CommandBarProperty =
            DependencyProperty.Register(nameof(CommandBar), typeof(KinoFormCommandBar), typeof(KinoForm), new PropertyMetadata(default(KinoFormCommandBar), OnCommandBarChanged));

        /// <summary>
        /// 标识 Description 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标识 Header 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标识 IsItemItsOwnContainer 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsItemItsOwnContainerProperty =
            DependencyProperty.RegisterAttached("IsItemItsOwnContainer", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 标识 IsRequired 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool)));

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
        /// 从指定元素获取 Header 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object GetHeader(DependencyObject obj) => (object)obj.GetValue(HeaderProperty);

        /// <summary>
        /// 将 Header 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetHeader(DependencyObject obj, object value) => obj.SetValue(HeaderProperty, value);

        /// <summary>
        /// 从指定元素获取 Description 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static object GetDescription(DependencyObject obj) => (object)obj.GetValue(DescriptionProperty);

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
        /// CommandBar 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">CommandBar 属性的旧值。</param>
        /// <param name="newValue">CommandBar 属性的新值。</param>
        protected virtual void OnCommandBarChanged(KinoFormCommandBar oldValue, KinoFormCommandBar newValue)
        {
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool isItemItsOwnContainer = false;
            if (item is DependencyObject element)
            {
                isItemItsOwnContainer = GetIsItemItsOwnContainer(element);
            }

            return item is KinoFormItem || item is KinoFormTitle || item is KinoFormSeparator || isItemItsOwnContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new KinoFormItem();
            return item;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is KinoFormItem kinoFormItem)
            {
                if (item is KinoFormItem == false && item is DependencyObject content)
                {
                    kinoFormItem.Header = GetHeader(content);
                    kinoFormItem.Description = GetDescription(content);
                    kinoFormItem.IsRequired = GetIsRequired(content);
                }
            }
        }

        private static void OnCommandBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (KinoFormCommandBar)args.OldValue;
            var newValue = (KinoFormCommandBar)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as KinoForm;
            target?.OnCommandBarChanged(oldValue, newValue);
        }
    }
}
