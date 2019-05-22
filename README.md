# Kino.Toolkit.Wpf

![dotnet-version](https://img.shields.io/badge/.net-%3E%3D4.5-blue.svg?style=flat-square) [![nuget-version](https://img.shields.io/nuget/v/Kino.Toolkit.Wpf.svg?style=flat-square)](https://www.nuget.org/packages/Kino.Toolkit.Wpf/) [![MIT License](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](https://github.com/DinoChan/Kino.Toolkit.Wpf/blob/master/LICENSE)


![](https://raw.githubusercontent.com/DinoChan/Kino.Toolkit.Wpf/master/demo.png)

Kino.Toolkit.Wpf是一组简单实用的WPF控件与工具，用于介绍自定义控件的入门。相关博客地址如下：


[开始一个自定义控件库项目](https://www.cnblogs.com/dino623/p/CustomControLibrary.html)

介绍开始一个自定义控件库项目需要考虑的地方，包括版本号、目录结构等。

[从ContentControl开始入门自定义控件](https://www.cnblogs.com/dino623/p/How-To-Create-CustomControl.html)

ContentControl是WPF中最基础的一种控件，Window、Button、ScrollViewer、Label、ListBoxItem等都继承自ContentControl。而且ContentControl的结构十分简单，很适合用来入门自定义控件。

这篇文章通过自定义一个ContentControl来介绍自定义控件的一些基础概念，包括自定义控件的基本步骤及其组成。

[了解如何自定义ItemsControl](https://www.cnblogs.com/dino623/p/Custom-ItemsControl.html)

WPF提供了一大堆ItemsControl的派生类：HeaderedItemsControl、TreeView、Menu、StatusBar、ListBox、ListView、ComboBox；而且配合Style或DataTemplate足以完成大部分的定制化工作。可以说ItemsControl是XAML系统灵活性的最佳代表。这篇文章介绍简单的自定义ItemsControl知识，通过重写GetContainerForItemOverride和IsItemItsOwnContainerOverride、PrepareContainerForItemOverride函数并使用ItemContainerGenerator等自定义一个简单的IItemsControl控件。

[自定义控件的代码如何与ControlTemplate交互](https://www.cnblogs.com/dino623/p/interact_with_ControlTemplate.html)

介绍自定义控件的代码如何和ControlTemplate交互，涉及的知识包括RelativeSource、Trigger、TemplatePart和VisualState，以及它们之间的选择。
