using DeviceServer.Api.Common.Extensions;
using DeviceServer.Api.Common.Web.Rest.Enum;

namespace DeviceServer.Api.Common.Web.Rest;

public class HttpRequestDefinition
{
    public HttpRequestDataType DataType { get; private set; }
    public string BaseAddress { get; private set; }
    public IDictionary<string, IList<string>> Headers { get; }
    public string StringContent { get; private set; }
    public object FormContent { get; private set; }
    public string QueryString { get; private set; }
    public string Protocol { get; private set; }
    public TimeSpan Timeout { get; private set; }

    public HttpRequestDefinition()
    {
        Headers = new Dictionary<string, IList<string>>();
        Protocol = "http://";
        Timeout = TimeSpan.FromSeconds(20);
        QueryString = string.Empty;
        StringContent = string.Empty;
    }

    public HttpRequestDefinition Header(string name, string value)
    {
        if (!Headers.ContainsKey(name))
        {
            Headers.Add(name, new List<string>());
        }

        if (value.IsNotNullOrEmpty())
        {
            Headers[name].Add(value);
        }

        return this;
    }

    public HttpRequestDefinition Query(object parameters)
    {
        var qs = parameters.GetType().Name == "Dictionary`2" ? (Dictionary<string, object>)parameters : parameters.ToDictionary();

        QueryString = qs.ToQuerystring();

        return this;
    } 

    public HttpRequestDefinition Http()
    {
        Protocol = "http://";
        return this;
    }

    public HttpRequestDefinition Https()
    {
        Protocol = "https://";
        return this;
    }    

    public HttpRequestDefinition Host(string baseAddress)
    {
        BaseAddress = baseAddress;

        return SetProtocol(baseAddress);
    }

    private HttpRequestDefinition SetProtocol(string baseAddress)
    {
        var https = baseAddress.MatchRegex(@"\bhttps:\/\/");
        return https ? Https() : Http();
    }       
}
