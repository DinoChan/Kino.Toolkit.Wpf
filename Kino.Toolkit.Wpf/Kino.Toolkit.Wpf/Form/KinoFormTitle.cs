using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf
{
    public class KinoFormTitle : ContentControl
    {
        /// <summary>
        /// 标识 Description 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(object), typeof(KinoFormTitle), new PropertyMetadata(default, OnDescriptionChanged));

        public KinoFormTitle()
        {
            DefaultStyleKey = typeof(KinoFormTitle);
            KinoForm.SetIsItemItsOwnContainer(this, true);
        }

        /// <summary>
        /// 获取或设置Description的值
        /// </summary>
        public object Description
        {
            get => GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// Description 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">Description 属性的旧值。</param>
        /// <param name="newValue">Description 属性的新值。</param>
        protected virtual void OnDescriptionChanged(object oldValue, object newValue)
        {
        }

        private static void OnDescriptionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = args.OldValue;
            var newValue = args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }

            var target = obj as KinoFormTitle;
            target?.OnDescriptionChanged(oldValue, newValue);
        }
    }
}
