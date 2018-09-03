/*************************************************************************************

   Toolkit for WPF

   Copyright (C) 2007-2016 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at https://xceed.com/xceed-toolkit-plus-for-wpf/

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Resources;
using Xceed.Wpf.Toolkit.LiveExplorer.Core;

namespace Xceed.Wpf.Toolkit.LiveExplorer.Core
{
    public abstract class CodeBox : Xceed.Wpf.Toolkit.RichTextBox
    {
        protected CodeBox()
        {
            this.IsReadOnly = true;
            this.FontFamily = new FontFamily("Consolas");
            this.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            this.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            SizeChanged += OnControlSizeChanged;
        }

        private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Document.PageWidth = Math.Max(800, e.NewSize.Width - 10);
        }

        public string CodeSource
        {
            set
            {
                if (value == null)
                    this.Text = null;

                this.Text = this.GetDataFromResource(value);
            }
        }

        private string GetDataFromResource(string uriString)
        {
            Uri uri = new Uri(uriString, UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);

            StreamReader reader = new StreamReader(info.Stream);
            string data = reader.ReadToEnd();
            reader.Close();

            return data;
        }

    }

    public class XamlBox : CodeBox
    {
        public XamlBox() { this.TextFormatter = new Core.XamlFormatter(); }
    }

    public class CSharpBox : CodeBox
    {
        public CSharpBox() { this.TextFormatter = new Core.CSharpFormatter(); }
    }
}
