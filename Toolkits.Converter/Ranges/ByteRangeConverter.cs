using System;
using System.Collections.Generic;
using System.Text;

namespace Toolkits.Converter;

/// <summary>
///  a <see langword="class"/> of <see cref="ByteRangeConverter"/>
/// </summary>
public class ByteRangeConverter : RangeConverter<byte> { }

/// <summary>
///  a <see langword="class"/> of <see cref="SByteRangeConverter"/>
/// </summary>
public class SByteRangeConverter : RangeConverter<sbyte> { }

/// <summary>
///  a <see langword="class"/> of <see cref="ShortRangeConverter"/>
/// </summary>
public class ShortRangeConverter : RangeConverter<short> { }

/// <summary>
///  a <see langword="class"/> of <see cref="UShortRangeConverter"/>
/// </summary>
public class UShortRangeConverter : RangeConverter<ushort> { }

/// <summary>
///  a <see langword="class"/> of <see cref="IntRangeConverter"/>
/// </summary>
public class IntRangeConverter : RangeConverter<int> { }

/// <summary>
///  a <see langword="class"/> of <see cref="UIntRangeConverter"/>
/// </summary>
public class UIntRangeConverter : RangeConverter<uint> { }

/// <summary>
///  a <see langword="class"/> of <see cref="IntRangeConverter"/>
/// </summary>
public class LongRangeConverter : RangeConverter<long> { }

/// <summary>
///  a <see langword="class"/> of <see cref="UIntRangeConverter"/>
/// </summary>
public class ULongRangeConverter : RangeConverter<ulong> { }

/// <summary>
///  a <see langword="class"/> of <see cref="SingleRangeConverter"/>
/// </summary>
public class SingleRangeConverter : RangeConverter<float> { }

/// <summary>
///  a <see langword="class"/> of <see cref="DoubleRangeConverter"/>
/// </summary>
public class DoubleRangeConverter : RangeConverter<double> { }

/// <summary>
///  a <see langword="class"/> of <see cref="DecimalRangeConverter"/>
/// </summary>
public class DecimalRangeConverter : RangeConverter<decimal> { }
