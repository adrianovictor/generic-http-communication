using DeviceServer.Api.Common.Web.Rest.Interfaces;
using Newtonsoft.Json;

namespace DeviceServer.Api.Common.Web.ApiGateways.Services;

public abstract class BaseGateway
{
    protected IHttpClient HttpClient { get; }
    protected abstract string LogContext { get; }

    public BaseGateway(IHttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected virtual T Deserialize<T>(string data, T? defaultValue = default)
    {
#pragma warning disable CS8603 // Possible null reference return.
        if (string.IsNullOrWhiteSpace(data))
        {
            return defaultValue != null ? defaultValue : default;
        }
        return JsonConvert.DeserializeObject<T>(data);
#pragma warning restore CS8603 // Possible null reference return.
    }
}
