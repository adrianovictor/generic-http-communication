using DeviceServer.Api.Common.Utils;
using DeviceServer.Api.Common.Web.ApiGateways.Services.interfaces;
using DeviceServer.Api.Common.Web.Rest.Interfaces;

namespace DeviceServer.Api.Common.Web.ApiGateways.Services;

public class IntelbrasDeviceService : BaseGateway, IIntelbrasDeviceService
{
    protected override string LogContext => "intelbras-gateway";
    private readonly IHttpClient _httpClient;

    public IntelbrasDeviceService(IHttpClient httpClient): base(httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetConfigurationManager(string devideAddress, bool enable, string serverAddress, string communicationPort, string notificationRoute)
    {
        return await Executor.TryAsync(async () => {
            var data = await HttpClient.GetAsync(UrlsConfig.IntelbrasOperations.GetConfigManager(),
                with => with.Host(devideAddress).Query(new { action = "setConfig" }));

            return Deserialize(data.Payload, string.Empty);
        }, ex =>
        {
            return string.Empty;
        });
    }
}
