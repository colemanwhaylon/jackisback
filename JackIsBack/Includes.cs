using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Includes
    {
        [JsonPropertyName("tweets")]
        public List<Tweet> Tweets { get; set; }
    }
}