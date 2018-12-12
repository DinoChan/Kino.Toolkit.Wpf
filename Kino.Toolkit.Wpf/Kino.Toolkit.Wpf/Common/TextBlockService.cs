using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Kino.Toolkit.Wpf
{
    //TODO 要做TextChanged处理，或者最好做成控件
    public class TextBlockService
    {
        /// <summary>
        /// 标识 HighlightText 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty HighlightTextProperty =
            DependencyProperty.RegisterAttached("HighlightText", typeof(string), typeof(TextBlockService), new PropertyMetadata(default(string), OnHighlightTextChanged));

        private static readonly Brush _noHighlightBrush = new SolidColorBrush(Color.FromArgb(168, 0, 0, 0));

        /// <summary>
        /// 从指定元素获取 HighlightText 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static string GetHighlightText(TextBlock obj) => (string)obj.GetValue(HighlightTextProperty);

        /// <summary>
        /// 将 HighlightText 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetHighlightText(TextBlock obj, string value) => obj.SetValue(HighlightTextProperty, value);

        private static void OnHighlightTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;
            if (oldValue == newValue)
                return;

            if (obj is TextBlock target)
            {
                MarkHighlight(target, newValue);
            }
        }

        private static void MarkHighlight(TextBlock target, string highlightText)
        {
            target.Inlines.Clear();
            var text = target.Text;
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (string.IsNullOrWhiteSpace(highlightText))
            {
                target.Inlines.Add(new Run { Text = text });
                return;
            }

            while (text.Length > 0)
            {
                var runText = string.Empty;
                var index = text.IndexOf(highlightText, StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                {
                    runText = text.Substring(0, index);
                    target.Inlines.Add(new Run { Text = runText, Foreground = _noHighlightBrush });
                }
                else if (index == 0)
                {
                    runText = text.Substring(0, highlightText.Length);
                    target.Inlines.Add(new Run { Text = runText });
                }
                else if (index == -1)
                {
                    runText = text;
                    target.Inlines.Add(new Run { Text = runText, Foreground = _noHighlightBrush });
                }

                text = text.Substring(runText.Length);
            }
        }
    }
}
