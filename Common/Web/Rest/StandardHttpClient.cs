using DeviceServer.Api.Common.Extensions;
using System.Net;

namespace DeviceServer.Api.Common.Web.Rest;

public class StandardHttpClient : HttpClientBase
{
    private readonly IHttpClientFactory _factory;

    public StandardHttpClient(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public override async Task<HttpResponse> DoRequestAsync(HttpMethod method, string uri, Action<HttpRequestDefinition> buildeRequest)
    {
        uri = FixPath(uri);
        var requestMessage = CreateRequest(method, uri, buildeRequest);

        var requestDefinition = new HttpRequestDefinition();
        buildeRequest(requestDefinition);

        var client = _factory.CreateClient();
        client.Timeout = requestDefinition.Timeout;
        var cts = new CancellationTokenSource();

        try
        {
            var response = await client.SendAsync(requestMessage, cts.Token);
            var responseAsText = await HttpResponseLogging.ToStringAsync(response);

            var payload = await response.Content.ReadAsStringAsync();
            var headers = response.Headers.ToDictionary(K => K.Key, K => K.Value.Join());
            var contentLength = response.Content.Headers.ContentLength;

            return new HttpResponse(method, response.StatusCode, payload, headers, contentLength ?? 0);
        }
        catch (Exception ex)
        {
            return new HttpResponse(method, (HttpStatusCode)504, "Gateway Timeour", new Dictionary<string, string>());
        }
    }

    public override async Task<HttpResponse> GetAsync(string uri, Action<HttpRequestDefinition> buildeRequest)
    {
        return await DoRequestAsync(HttpMethod.Get, uri, buildeRequest);
    }

    public override async Task<HttpResponse> PostAsync(string uri, Action<HttpRequestDefinition> buildeRequest)
    {
        return await DoRequestAsync(HttpMethod.Post, uri, buildeRequest);
    }

    private class HttpResponseLogging
    {
        private readonly HttpResponseMessage _response;

        private HttpResponseLogging(HttpResponseMessage response)
        {
            _response = response;
        }

        public static Task<string> ToStringAsync(HttpResponseMessage response)
            => new HttpResponseLogging(response).ReadAsStringAsync();

        private async Task<string> ReadAsStringAsync()
        {
            var body = await _response.Content.ReadAsStringAsync();
            var headers = _response.Headers.ToDictionary(k => k.Key, v => v.Value.Join())
                .Aggregate("", (c, next) => c + $"{next.Key}={next.Value}");

            return $"StatusCode: {_response.StatusCode}, ReasonPhrase: {_response.ReasonPhrase}, Version: {_response.Version}\r\nContent: {body}\r\nHeaders: {headers}";
        }
    }
}
