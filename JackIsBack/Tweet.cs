using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JackIsBack
{
    public class Tweet
    {
        [JsonPropertyName("possibly_sensitive")]
        public bool PossiblySensitive { get; set; }

        [JsonPropertyName("referenced_tweets")]
        public List<ReferencedTweet> ReferencedTweets { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("entities")]
        public Entities Entities { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("public_metrics")]
        public PublicMetrics PublicMetrics { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}