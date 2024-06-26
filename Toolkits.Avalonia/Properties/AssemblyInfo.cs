﻿global using System;
global using System.Linq;
global using BF = System.Reflection.BindingFlags;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Avalonia.Metadata;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Toolkits.Avalonia")]
[assembly: XmlnsDefinition("https://github.com/xtremlyred/toolkits", "Toolkits.Avalonia")]
[assembly: XmlnsPrefix("https://github.com/xtremlyred/toolkits", "toolkits")]

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}
