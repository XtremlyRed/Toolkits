using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace Toolkits.Animation;

internal interface IAnimationInfo
{
    void Invoke();

    EventMode EventMode { get; }
}
