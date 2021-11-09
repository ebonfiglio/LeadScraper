using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LeadScraper.Domain.Models
{
    public partial class SearchResult
    {
        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("queryContext")]
        public QueryContext QueryContext { get; set; }

        [JsonProperty("webPages")]
        public WebPages WebPages { get; set; }

        [JsonProperty("relatedSearches")]
        public RelatedSearches RelatedSearches { get; set; }

        [JsonProperty("rankingResponse")]
        public RankingResponse RankingResponse { get; set; }
    }

    public partial class QueryContext
    {
        [JsonProperty("originalQuery")]
        public string OriginalQuery { get; set; }
    }

    public partial class RankingResponse
    {
        [JsonProperty("mainline")]
        public Mainline Mainline { get; set; }

        [JsonProperty("sidebar")]
        public Sidebar Sidebar { get; set; }
    }

    public partial class Mainline
    {
        [JsonProperty("items")]
        public HashSet<MainlineItem> Items { get; set; }
    }

    public partial class MainlineItem
    {
        [JsonProperty("answerType")]
        public AnswerType AnswerType { get; set; }

        [JsonProperty("resultIndex")]
        public long ResultIndex { get; set; }

        [JsonProperty("value")]
        public ItemValue Value { get; set; }
    }

    public partial class ItemValue
    {
        [JsonProperty("id")]
        public Uri Id { get; set; }
    }

    public partial class Sidebar
    {
        [JsonProperty("items")]
        public HashSet<SidebarItem> Items { get; set; }
    }

    public partial class SidebarItem
    {
        [JsonProperty("answerType")]
        public string AnswerType { get; set; }

        [JsonProperty("value")]
        public ItemValue Value { get; set; }
    }

    public partial class RelatedSearches
    {
        [JsonProperty("id")]
        public Uri Id { get; set; }

        [JsonProperty("value")]
        public HashSet<RelatedSearchesValue> Value { get; set; }
    }

    public partial class RelatedSearchesValue
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("webSearchUrl")]
        public Uri WebSearchUrl { get; set; }
    }

    public partial class WebPages
    {
        [JsonProperty("webSearchUrl")]
        public Uri WebSearchUrl { get; set; }

        [JsonProperty("totalEstimatedMatches")]
        public long TotalEstimatedMatches { get; set; }

        [JsonProperty("value")]
        public HashSet<WebPagesValue> Value { get; set; }
    }

    public partial class WebPagesValue
    {
        [JsonProperty("id")]
        public Uri Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("isFamilyFriendly")]
        public bool IsFamilyFriendly { get; set; }

        [JsonProperty("displayUrl")]
        public string DisplayUrl { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("dateLastCrawled")]
        public DateTimeOffset DateLastCrawled { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("isNavigational")]
        public bool IsNavigational { get; set; }

        [JsonProperty("about", NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<About> About { get; set; }

        [JsonProperty("deepLinks", NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<DeepLink> DeepLinks { get; set; }
    }

    public partial class About
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public enum AnswerType { WebPages, Images, Videos, News, RelatedSearches };


    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, LeadScraper.Domain.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, LeadScraper.Domain.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AnswerTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AnswerTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AnswerType) || t == typeof(AnswerType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "WebPages")
            {
                return AnswerType.WebPages;
            }
            throw new Exception("Cannot unmarshal type AnswerType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AnswerType)untypedValue;
            if (value == AnswerType.WebPages)
            {
                serializer.Serialize(writer, "WebPages");
                return;
            }
            throw new Exception("Cannot marshal type AnswerType");
        }

        public static readonly AnswerTypeConverter Singleton = new AnswerTypeConverter();
    }

    
    public partial class DeepLink
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}
