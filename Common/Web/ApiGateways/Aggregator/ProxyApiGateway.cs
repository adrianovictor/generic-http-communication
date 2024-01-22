using DeviceServer.Api.Common.Extensions;
using DeviceServer.Api.Common.Web.ApiGateways.Aggregator.Interfaces;
using DeviceServer.Api.Common.Web.ApiGateways.Services.interfaces;

namespace DeviceServer.Api.Common.Web.ApiGateways.Aggregator;

public class ProxyApiGateway: IProxyApiGateway
{
    private readonly IIntelbrasDeviceService _intelbrasService;

    public ProxyApiGateway(IIntelbrasDeviceService intelbrasDeviceService)
    {
        _intelbrasService = intelbrasDeviceService;
    }

    public async Task<dynamic> SendCommand(string deviceAddress, string serverAddress, string port, string notificationRoute, DeviceCommandType commandType)
    {
        var handlers = new Dictionary<DeviceCommandType, Func<Task<dynamic>>>
        {
            { DeviceCommandType.OnlineMode, GetOnlineMode(deviceAddress, serverAddress, port, notificationRoute) }
        };

        var handler = handlers[commandType];

        if (handler is not null)
        {
            var result = await handlers[commandType].Invoke();
            return result;
        }

        return await Task.FromResult(new { });
    }

    private Func<Task<dynamic>> GetOnlineMode(string deviceAddress, string serverAddress, string port, string notificationRoute)
    {
        return async () => 
        {
            var result = await _intelbrasService.GetConfigurationManager(deviceAddress, true, serverAddress, port, notificationRoute);
            var data = result?.ToString();

            return data.IsNull() ? string.Empty : data;
        };
    }
}
