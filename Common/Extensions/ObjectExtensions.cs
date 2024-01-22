using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DeviceServer.Api.Common.Extensions;

public static class ObjectExtensions
{
    public static IDictionary<string, object> ToDictionary(this object obj)
    {
        if (obj.IsNull()) return new Dictionary<string, object>();

        return TypeDescriptor.GetProperties(obj)
            .OfType<PropertyDescriptor>()
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(obj)
            );
    }

    public static IDictionary<string, string> ToStringDictionary(this object obj)
    {
        if (obj.IsNull()) return new Dictionary<string, string>();

        return TypeDescriptor.GetProperties(obj)
            .OfType<PropertyDescriptor>()
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(obj).ToString()
            );
    }    

    public static bool IsNull(this object obj) => obj == null;

    public static bool IsNotNull(this object obj) => obj != null;

    public static bool IsNotNullOrEmpty(this object obj) =>
        obj.IsNotNull() && obj.ToString().IsNotEmpty();

    public static bool IsNullOrEmpty(this object obj) =>
        obj.IsNull() || obj.ToString().IsEmpty();

    public static bool IsArray(this object obj)
    {
        var type = obj.GetType();
        var typeDefinitions = new[] { typeof(IEnumerable<>), typeof(IList<>), typeof(List<>), typeof(Enumerable), typeof(Collection<>) };

        return type.IsArray || 
                type.IsAssignableFrom(typeof (IEnumerable)) ||
                (type.IsGenericType && typeDefinitions.Any(t => t == type.GetGenericTypeDefinition()));
    }    
}
