using System.Collections;
using System.Text.RegularExpressions;

namespace DeviceServer.Api.Common.Extensions;

public static class StringExtensions
{
    public static bool MatchRegex(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
    {
        return input.IsNotNullOrEmpty() && Regex.IsMatch(input, pattern, options);
    }
        
    public static string ToQuerystring(this IDictionary<string, object> parameters)
    {
        var query = new List<string>();

        foreach (var q in parameters)
        {
            if (q.Value == null) continue;

            if (q.Value.IsArray())
            {
                query.AddRange(
                    ((IEnumerable)q.Value).Cast<object>().Select(_ => $"{q.Key}[]={_}"));
            }
            else
            {
                query.Add($"{q.Key}={q.Value}");
            }
        }

        return $"?{query.Join("&")}";
    }

    public static string ToQuerystring(this object parameters)
    {
        var qs = parameters.ToDictionary();

        return qs.ToQuerystring();
    }

    public static string Join(this IEnumerable<string> values, string separator = ",")
    {
        return string.Join(separator, values.Compact());
    }   

    public static bool IsNotEmpty(this string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }     
}
