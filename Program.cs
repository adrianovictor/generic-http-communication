using DeviceServer.Api.Common.Web.ApiGateways.Aggregator.Interfaces;
using DeviceServer.Api.Common.Web.ApiGateways.Aggregator;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DeviceServer.Api.Common.Web.ApiGateways.Services.interfaces;
using DeviceServer.Api.Common.Web.ApiGateways.Services;
using DeviceServer.Api.Common.Web.Rest.Interfaces;
using DeviceServer.Api.Common.Web.Rest;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DeviceServer.Api.Common.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<StandardHttpClient>()
    .ConfigureHttpMessageHandlerBuilder(configureBuilder =>
    {
        new HttpClientHandler()
        {
            Credentials = new NetworkCredential(GlobalConstants.Credential.AdminUsername, GlobalConstants.Credential.AdminPassword),
            PreAuthenticate = true
        };
    });

builder.Services.TryAddScoped<IHttpClient, StandardHttpClient>();

builder.Services.TryAddScoped<IIntelbrasDeviceService, IntelbrasDeviceService>();
builder.Services.TryAddScoped<IProxyApiGateway, ProxyApiGateway>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async ([FromServices] IProxyApiGateway proxyApiGateway, string deviceAddress, string serverAddress, string communicationPort, string notificationRoute) =>
{
    //var result = proxyApiGateway.SendCommand("http://192.168.0.2", "http://192.168.0.1", "3001", "/notifications", DeviceCommandType.OnlineMode);
    var result = await proxyApiGateway.SendCommand(
        deviceAddress: deviceAddress,
        serverAddress: serverAddress,
        port: communicationPort,
        notificationRoute: notificationRoute,
        commandType: DeviceCommandType.OnlineMode
    );

    return Results.Ok(result);
});

app.MapPost("/notifications", () => "Ok");

app.Run();
