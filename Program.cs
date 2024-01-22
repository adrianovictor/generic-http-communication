using DeviceServer.Api.Common.Web.ApiGateways.Aggregator.Interfaces;
using DeviceServer.Api.Common.Web.ApiGateways.Aggregator;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DeviceServer.Api.Common.Web.ApiGateways.Services.interfaces;
using DeviceServer.Api.Common.Web.ApiGateways.Services;
using DeviceServer.Api.Common.Web.Rest.Interfaces;
using DeviceServer.Api.Common.Web.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.TryAddScoped<IHttpClient, StandardHttpClient>();

builder.Services.TryAddScoped<IIntelbrasDeviceService, IntelbrasDeviceService>();
builder.Services.TryAddScoped<IProxyApiGateway, ProxyApiGateway>();

var app = builder.Build();

app.MapGet("/", (IProxyApiGateway proxyApiGateway) =>
{
    var result = proxyApiGateway.SendCommand("192.168.0.2", "192.168.0.1", "3001", "/notifications", DeviceCommandType.OnlineMode);

    return Results.Ok(new { result = "Ok" });
});

app.MapPost("/notifications", () => "Ok");

app.Run();
