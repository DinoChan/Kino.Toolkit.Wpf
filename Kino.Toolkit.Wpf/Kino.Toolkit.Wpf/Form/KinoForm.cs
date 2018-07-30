using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoForm : ItemsControl
    {
        #region Dependency Properties
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
        /// 标识 IsRequired 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool)));


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
        /// 标识 Header 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object)));

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
        /// 标识 Description 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description", typeof(object), typeof(KinoForm), new PropertyMetadata(default(object)));



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
        /// 标识 IsItemItsOwnContainer 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty IsItemItsOwnContainerProperty =
            DependencyProperty.RegisterAttached("IsItemItsOwnContainer", typeof(bool), typeof(KinoForm), new PropertyMetadata(default(bool)));

        #endregion

        public KinoForm()
        {
            DefaultStyleKey = typeof(KinoForm);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            bool isItemItsOwnContainer = false;
            if (item is DependencyObject element)
                isItemItsOwnContainer = GetIsItemItsOwnContainer(element);

            return item is KinoFormItem || item is KinoFormTitle || item is KinoFormSeparator || isItemItsOwnContainer;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new KinoFormItem();
            return item;
        }
    }
}
