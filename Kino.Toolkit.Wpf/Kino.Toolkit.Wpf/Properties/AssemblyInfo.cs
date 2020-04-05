using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;



// 将 ComVisible 设置为 false 将使此程序集中的类型
// 对 COM 组件不可见。  如果需要从 COM 访问此程序集中的类型，
// 请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 若要开始生成可本地化的应用程序，请设置
// .csproj 文件中的 <UICulture>CultureYouAreCodingWith</UICulture>
// 例如，如果您在源文件中使用的是美国英语，
// 使用的是美国英语，请将 <UICulture> 设置为 en-US。  然后取消
// 对以下 NeutralResourceLanguage 特性的注释。  更新
// 以下行中的“en-US”以匹配项目文件中的 UICulture 设置。

// [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly)
]


[assembly: XmlnsPrefix("https://github.com/DinoChan/Kino.Toolkit.Wpf", "kino")]
[assembly: XmlnsDefinition("https://github.com/DinoChan/Kino.Toolkit.Wpf", "Kino.Toolkit.Wpf")]
[assembly: XmlnsDefinition("https://github.com/DinoChan/Kino.Toolkit.Wpf", "Kino.Toolkit.Wpf.Primitives")]
