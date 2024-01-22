using System.Net;

namespace DeviceServer.Api.Common.Web.Rest;

public class HttpResponse
{
    public HttpMethod HttpMethod { get; }
    public HttpStatusCode StatusCode { get; }
    public string Payload { get; private set; }
    public IDictionary<string, string> Headers { get; }
    public long ContentLength { get; set; }
    public string ReasonPhrase { get; set; }
    public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    public HttpResponse(HttpMethod httpMethod, HttpStatusCode statusCode, string payload, IDictionary<string, string> headers = null, long contentLength = 0)
    {
        HttpMethod = httpMethod;
        StatusCode = statusCode;
        Payload = payload;
        Headers = headers;
        ContentLength = contentLength;
    }

    public override string ToString()
    {
        return $"/{HttpMethod} {StatusCode} with payload: {Payload}";
    }    
}
