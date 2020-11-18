using System;
using System.Linq;

public static class EnumArray <TEnum>
    where TEnum : struct, Enum
{
    private static readonly TEnum[] _values;
    public static TEnum[] Values => _values;

    static EnumArray() =>
        _values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
}