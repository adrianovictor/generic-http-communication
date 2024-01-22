namespace DeviceServer.Api.Common.Web.Rest;

public class HttpClientFactory : Interfaces.IHttpClientFactory
{
    private readonly string _correlationToken;

    public HttpClientFactory()
    {
        _correlationToken = Guid.NewGuid().ToString();
    }

    public HttpClient Create()
    {
        var correlationToken = _correlationToken;

        var client = new HttpClient();

        return client;
    }
}
