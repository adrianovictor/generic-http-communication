using System.Text.Json.Serialization;

namespace DeviceServer.Api.Model.Request
{
    public class QueryEnableOnelineMode
    {
        [JsonPropertyName("deviceAddress")]
        public string DeviceAddress { get; set; }

        [JsonPropertyName("devicePort")]
        public string DevicePort { get; set; }

        [JsonPropertyName("serverAddress")]
        public string ServerAddress { get; set; }

        [JsonPropertyName("notificationRoute")]
        public string NotificationRoute { get; set; }
    }
}
