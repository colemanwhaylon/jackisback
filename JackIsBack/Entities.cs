using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Entities
    {
        [JsonPropertyName("urls")]
        public List<Url> Urls { get; set; }

        [JsonPropertyName("annotations")]
        public List<Annotation> Annotations { get; set; }
    }
}