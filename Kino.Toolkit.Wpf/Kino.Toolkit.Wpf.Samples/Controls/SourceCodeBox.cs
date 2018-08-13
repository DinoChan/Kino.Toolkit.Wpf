using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.LiveExplorer.Core;

namespace Kino.Toolkit.Wpf.Samples
{
    [TemplatePart(Name = SourceCoeePlacementName, Type = typeof(ContentControl))]
    public class SourceCodeBox : ContentControl
    {
        private const string SourceCoeePlacementName = "SourceCodePlacement";
        private ContentControl _sourceCodePlacement;

        public SourceCodeBox()
        {
            DefaultStyleKey = typeof(SourceCodeBox);
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy));
        }

        /// <summary>
        /// 获取或设置SourceCodeType的值
        /// </summary>  
        public SourceCodeType SourceCodeType
        {
            get => (SourceCodeType)GetValue(SourceCodeTypeProperty);
            set => SetValue(SourceCodeTypeProperty, value);
        }

        /// <summary>
        /// 标识 SourceCodeType 依赖属性。
        /// </summary>
        public static readonly DependencyProperty SourceCodeTypeProperty =
            DependencyProperty.Register(nameof(SourceCodeType), typeof(SourceCodeType), typeof(SourceCodeBox), new PropertyMetadata(default(SourceCodeType), OnSourceCodeTypeChanged));

        private static void OnSourceCodeTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (SourceCodeType)args.OldValue;
            var newValue = (SourceCodeType)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SourceCodeBox;
            target?.OnSourceCodeTypeChanged(oldValue, newValue);
        }

        /// <summary>
        /// SourceCodeType 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">SourceCodeType 属性的旧值。</param>
        /// <param name="newValue">SourceCodeType 属性的新值。</param>
        protected virtual void OnSourceCodeTypeChanged(SourceCodeType oldValue, SourceCodeType newValue)
        {
            UpdateSourceCodePlacement();
        }



        /// <summary>
        /// 获取或设置CodeSource的值
        /// </summary>  
        public string CodeSource
        {
            get => (string)GetValue(CodeSourceProperty);
            set => SetValue(CodeSourceProperty, value);
        }

        /// <summary>
        /// 标识 CodeSource 依赖属性。
        /// </summary>
        public static readonly DependencyProperty CodeSourceProperty =
            DependencyProperty.Register(nameof(CodeSource), typeof(string), typeof(SourceCodeBox), new PropertyMetadata(default(string), OnCodeSourceChanged));

        private static void OnCodeSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            var target = obj as SourceCodeBox;
            target?.OnCodeSourceChanged(oldValue, newValue);
        }

        /// <summary>
        /// CodeSource 属性更改时调用此方法。
        /// </summary>
        /// <param name="oldValue">CodeSource 属性的旧值。</param>
        /// <param name="newValue">CodeSource 属性的新值。</param>
        protected virtual void OnCodeSourceChanged(string oldValue, string newValue)
        {
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _sourceCodePlacement = GetTemplateChild(SourceCoeePlacementName) as ContentControl;
            UpdateSourceCodePlacement();
        }

        private void Copy(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (_sourceCodePlacement != null && _sourceCodePlacement.Content is CodeBox codeBox)
            {
                Clipboard.SetText(codeBox.Text);
            }
        }

        private void UpdateSourceCodePlacement()
        {
            if (_sourceCodePlacement == null || string.IsNullOrWhiteSpace(CodeSource))
                return;

            CodeBox codeBox = null;
            switch (SourceCodeType)
            {
                case SourceCodeType.Xaml:
                    codeBox = new XamlBox();
                    break;
                case SourceCodeType.CSharp:
                    codeBox = new CSharpBox();
                    break;
                default:
                    break;
            }

            codeBox.CodeSource = CodeSource;
            _sourceCodePlacement.Content = codeBox;
        }
    }
}
