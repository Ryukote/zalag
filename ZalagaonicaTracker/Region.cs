using System.Text.Json.Serialization;

namespace ZalagaonicaTracker
{
    public class Region
    {
        [JsonPropertyName("place_name")]
        public required string PlaceName { get; set; }
        [JsonPropertyName("admin_name1")]
        public required string Zupanija { get; set; }
        [JsonPropertyName("admin_name3")]
        public required string Place { get; set; }
    }
}
