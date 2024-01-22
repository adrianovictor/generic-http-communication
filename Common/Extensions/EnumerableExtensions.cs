using DeviceServer.Api.Common.Extensions;

namespace DeviceServer.Api;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Compact<T>(this IEnumerable<T> items) => items.Where(_ => _.IsNotNull() && _.ToString().IsNotEmpty());
}
