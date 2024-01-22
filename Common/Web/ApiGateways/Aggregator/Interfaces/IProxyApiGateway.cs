namespace DeviceServer.Api.Common.Web.ApiGateways.Aggregator.Interfaces;

public interface IProxyApiGateway
{
    Task<dynamic> SendCommand(string deviceAddress, string serverAddress, string port, string notificationRoute, DeviceCommandType commandType);
}

public enum DeviceCommandType 
{
    OnlineMode = 1
}
