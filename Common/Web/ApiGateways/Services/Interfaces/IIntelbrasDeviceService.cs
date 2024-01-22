namespace DeviceServer.Api.Common.Web.ApiGateways.Services.interfaces;

public interface IIntelbrasDeviceService
{
    Task<string> GetConfigurationManager(string devideAddress, bool enable, string serverAddress, string communicationPort, string notificationRoute);
}
