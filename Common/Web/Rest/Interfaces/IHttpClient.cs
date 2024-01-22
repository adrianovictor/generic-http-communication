using DeviceServer.Api.Common.Web.Rest;

namespace DeviceServer.Api.Common.Web.Rest.Interfaces;

public interface IHttpClient
{
    Task<HttpResponse> GetAsync(string uri, Action<HttpRequestDefinition> buildRequest);
    Task<HttpResponse> PostAsync(string uri, Action<HttpRequestDefinition> buildRequest);
    Task<HttpResponse> DoRequestAsync(HttpMethod method, string uri, Action<HttpRequestDefinition> buildRequest);
}
