using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Root
    {
        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }

        [JsonPropertyName("includes")]
        public Includes Includes { get; set; }
    }
}