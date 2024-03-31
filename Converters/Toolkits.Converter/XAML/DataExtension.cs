using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Toolkits;

#if ___AVALONIA___


using global::Avalonia.Markup.Xaml;

#endif

#if ___WPF___ || ___AVALONIA___


/// <summary>
/// a class of <see cref="CharExtension" />
/// </summary>
public class CharExtension : DataExtension<char>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public CharExtension(char value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="DecimalExtension" />
/// </summary>
public class DecimalExtension : DataExtension<decimal>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public DecimalExtension(decimal value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="BooleanExtension" />
/// </summary>
public class BooleanExtension : DataExtension<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanExtension"/> class.
    /// </summary>
    /// <param name="value">if set to <c>true</c> [value].</param>
    public BooleanExtension(bool value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="StringExtension" />
/// </summary>
public class StringExtension : DataExtension<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public StringExtension(string? value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="SByteExtension" />
/// </summary>
public class SByteExtension : DataExtension<sbyte>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SByteExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public SByteExtension(sbyte value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="ByteExtension" />
/// </summary>
public class ByteExtension : DataExtension<byte>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByteExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ByteExtension(byte value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="DoubleExtension" />
/// </summary>
public class DoubleExtension : DataExtension<double>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoubleExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public DoubleExtension(double value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="SingleExtension" />
/// </summary>
public class SingleExtension : DataExtension<float>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleExtension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public SingleExtension(float value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="UInt64Extension" />
/// </summary>
public class UInt64Extension : DataExtension<ulong>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UInt64Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public UInt64Extension(ulong value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="Int64Extension" />
/// </summary>
public class Int64Extension : DataExtension<long>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Int64Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public Int64Extension(long value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="UInt16Extension" />
/// </summary>
public class UInt16Extension : DataExtension<ushort>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UInt16Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public UInt16Extension(ushort value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="Int16Extension" />
/// </summary>
public class Int16Extension : DataExtension<short>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Int16Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public Int16Extension(short value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="UInt32Extension" />
/// </summary>
public class UInt32Extension : DataExtension<uint>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UInt32Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public UInt32Extension(uint value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="Int32Extension" />
/// </summary>
public class Int32Extension : DataExtension<int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Int32Extension"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public Int32Extension(int value)
        : base(value) { }
}

/// <summary>
/// a class of <see cref="DataExtension{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
public abstract class DataExtension<T> : MarkupExtension
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    [ConstructorArgument(nameof(Value))]
    public T? Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    protected DataExtension(T? value)
    {
        Value = value;
    }

    /// <summary>
    /// </summary>
    public override object ProvideValue(IServiceProvider serviceProvider) => Value!;
}


#endif
