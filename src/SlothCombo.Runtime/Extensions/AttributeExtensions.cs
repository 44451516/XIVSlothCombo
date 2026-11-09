using System;
using System.Collections.Generic;
using System.Linq;

namespace SlothCombo.Extensions;

/// <summary>
/// Helpers for the attributes of the presets.
/// </summary>
internal static class AttributeExtensions
{
    /// <summary>Gets the attribute of the given type on the given value.</summary>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <param name="value">The value to read the attribute from.</param>
    /// <returns>The attribute, or <c>null</c> when the value carries none.</returns>
    public static T? GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name is null)
        {
            return null;
        }

        var field = type.GetField(name);
        return field?.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
    }
}
