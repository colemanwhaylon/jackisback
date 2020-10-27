using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class ContextAnnotation
    {
        [JsonPropertyName("domain")]
        public Domain Domain { get; set; }

        [JsonPropertyName("entity")]
        public Entity Entity { get; set; }
    }
}