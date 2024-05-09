global using System;
global using System.Linq;
global using BF = System.Reflection.BindingFlags;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using XmlnsDefinitionAttribute = Microsoft.Maui.Controls.XmlnsDefinitionAttribute;
using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Toolkits.Maui")]
[assembly: XmlnsDefinition("https://github.com/xtremlyred/toolkits", "Toolkits.Maui")]
[assembly: XmlnsPrefix("https://github.com/xtremlyred/toolkits", "toolkits")]

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}
