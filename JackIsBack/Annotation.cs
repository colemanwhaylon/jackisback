using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Annotation
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("normalized_text")]
        public string NormalizedText { get; set; }
    }
}