using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kino.Toolkit.Wpf.Samples
{
    public class SourceCodeItemsControl : ItemsControl
    {

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SourceCodeBox();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var sourceCodeBox = element as SourceCodeBox;
            var sourceCodeModel = item as SourceCodeModel;
            if (sourceCodeBox == null || sourceCodeModel == null)
                return;

            sourceCodeBox.Content = sourceCodeModel.Haader;
            sourceCodeBox.CodeSource = sourceCodeModel.CodeSource;
            sourceCodeBox.SourceCodeType = sourceCodeModel.SourceCodeType;
        }
    }
}
