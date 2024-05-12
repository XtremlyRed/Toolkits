using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Toolkits.Maui;

/// <summary>
/// a class of <see cref="CharExtension" />
/// </summary>
public class CharExtension : DataExtension<char>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CharExtension"/> class.
    /// </summary>
    public CharExtension() { }

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
    public DecimalExtension() { }

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
    public BooleanExtension() { }

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
    public StringExtension() { }

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
    public SByteExtension() { }

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
    public ByteExtension() { }

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
    public DoubleExtension() { }

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
    public SingleExtension() { }

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
    public UInt64Extension() { }

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
    public Int64Extension() { }

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
    public UInt16Extension() { }

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
    public Int16Extension() { }

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
    public UInt32Extension() { }

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
    public Int32Extension() { }

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
public abstract class DataExtension<T> : IMarkupExtension<T>, IMarkupExtension
{
    /// <summary>
    /// initializes a new instance of the <see cref="DataExtension{T}"/> class.
    /// </summary>
    public DataExtension() { }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// initializes a new instance of the <see cref="DataExtension{T}"/> class.
    /// </summary>
    /// <param name="value"></param>
    protected DataExtension(T? value)
    {
        Value = value;
    }

    /// <summary>
    /// Provides the value.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns></returns>
    public T ProvideValue(IServiceProvider serviceProvider) => Value!;

    /// <summary>
    /// Provides the value.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns></returns>
    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => Value!;
}
