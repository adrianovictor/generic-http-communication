using System.Text;
using DeviceServer.Api.Common.Extensions;
using DeviceServer.Api.Common.Web.Rest.Enum;
using DeviceServer.Api.Common.Web.Rest.Interfaces;

namespace DeviceServer.Api.Common.Web.Rest;

public abstract class HttpClientBase : IHttpClient
{
    public abstract Task<HttpResponse> GetAsync(string uri, Action<HttpRequestDefinition> buildeRequest);
    public abstract Task<HttpResponse> PostAsync(string uri, Action<HttpRequestDefinition> buildeRequest);
    public abstract Task<HttpResponse> DoRequestAsync(HttpMethod method, string uri, Action<HttpRequestDefinition> buildeRequest);

    protected virtual HttpRequestMessage CreateRequest(HttpMethod httpMethod, string path, Action<HttpRequestDefinition> buildeRequestContext)
    {
        var requestDefinition = new HttpRequestDefinition();
        buildeRequestContext(requestDefinition);

        var uri = new[]
        {
            requestDefinition.BaseAddress,
            path,
            requestDefinition.QueryString
        }.Join("");

        var request = new HttpRequestMessage
        {
            Method = httpMethod,
            RequestUri = new Uri(uri)
        };

        BuildRequestBody(request, requestDefinition);

        return request;
    }

    private void BuildRequestBody(HttpRequestMessage request, HttpRequestDefinition requestContext)
    {
        if (requestContext.DataType == HttpRequestDataType.FormData)
        {
            var values = requestContext.FormContent.ToStringDictionary();
            request.Content = new FormUrlEncodedContent(values);
        }
        else if (requestContext.DataType == HttpRequestDataType.JSON)
        {
            request.Content = new StringContent(requestContext.StringContent, Encoding.UTF8, "application/json");
        }
    }
    
    protected virtual string FixPath(string path)
    {
        if (path.IsNullOrEmpty())
            return path;

        if (!path.StartsWith(@"/"))
            path = @"/" + path;

        return path;
    }    
}
