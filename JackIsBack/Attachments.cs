using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Attachments
    {
        [JsonPropertyName("media_keys")]
        public List<string> MediaKeys { get; set; }
    }
}