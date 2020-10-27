using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Url
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { get; set; }

        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; set; }
    }
}