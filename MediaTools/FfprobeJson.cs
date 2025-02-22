using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediaTools
{
    public class FfProbeJson
    {
        [JsonRequired, JsonPropertyName("streams")]
        public List<StreamInfo>? Streams { get; set; }

        [JsonRequired, JsonPropertyName("format")]
        public FormatInfo? Format { get; set; }
    }

    public class StreamInfo
    {
        [JsonPropertyName("index")] public int Index { get; set; }

        [JsonPropertyName("codec_name")] public string? CodecName { get; set; }

        [JsonPropertyName("codec_long_name")] public string? CodecLongName { get; set; }

        [JsonPropertyName("profile")] public string? Profile { get; set; }

        [JsonPropertyName("codec_type")] public string? CodecType { get; set; }

        [JsonPropertyName("codec_tag_string")] public string? CodecTagString { get; set; }

        [JsonPropertyName("codec_tag")] public string? CodecTag { get; set; }

        [JsonPropertyName("width")] public int? Width { get; set; }

        [JsonPropertyName("height")] public int? Height { get; set; }

        [JsonPropertyName("coded_width")] public int? CodedWidth { get; set; }

        [JsonPropertyName("coded_height")] public int? CodedHeight { get; set; }

        [JsonPropertyName("closed_captions")] public int? ClosedCaptions { get; set; }

        [JsonPropertyName("film_grain")] public int? FilmGrain { get; set; }

        [JsonPropertyName("has_b_frames")] public int? HasBFrames { get; set; }

        [JsonPropertyName("sample_aspect_ratio")]
        public string? SampleAspectRatio { get; set; }

        [JsonPropertyName("display_aspect_ratio")]
        public string? DisplayAspectRatio { get; set; }

        [JsonPropertyName("pix_fmt")] public string? PixelFormat { get; set; }

        [JsonPropertyName("level")] public int? Level { get; set; }

        [JsonPropertyName("color_range")] public string? ColorRange { get; set; }

        [JsonPropertyName("color_space")] public string? ColorSpace { get; set; }

        [JsonPropertyName("color_transfer")] public string? ColorTransfer { get; set; }

        [JsonPropertyName("color_primaries")] public string? ColorPrimaries { get; set; }

        [JsonPropertyName("refs")] public int? Refs { get; set; }

        [JsonPropertyName("r_frame_rate")] public string? RFrameRate { get; set; }

        [JsonPropertyName("avg_frame_rate")] public string? AvgFrameRate { get; set; }

        [JsonPropertyName("time_base")] public string? TimeBase { get; set; }

        [JsonPropertyName("start_pts")] public long? StartPts { get; set; }

        [JsonPropertyName("start_time")] public string? StartTime { get; set; }

        [JsonPropertyName("disposition"), JsonConverter(typeof(LowercaseKeyDictionaryConverter<int>))]
        public Dictionary<string, int>? Disposition { get; set; }

        [JsonPropertyName("tags"), JsonConverter(typeof(LowercaseKeyDictionaryConverter<string>))]
        public Dictionary<string, string>? Tags { get; set; }
    }

    public class FormatInfo
    {
        [JsonPropertyName("filename")] public string? Filename { get; set; }

        [JsonPropertyName("nb_streams")] public int NbStreams { get; set; }

        [JsonPropertyName("nb_programs")] public int NbPrograms { get; set; }

        [JsonPropertyName("nb_stream_groups")] public int NbStreamGroups { get; set; }

        [JsonPropertyName("format_name")] public string? FormatName { get; set; }

        [JsonPropertyName("format_long_name")] public string? FormatLongName { get; set; }

        [JsonPropertyName("start_time")] public string? StartTime { get; set; }

        [JsonPropertyName("duration"), JsonConverter(typeof(StringToDoubleConverter))]
        public double Duration { get; set; }

        [JsonPropertyName("size")] public string? Size { get; set; }

        [JsonPropertyName("bit_rate")] public string? BitRate { get; set; }

        [JsonPropertyName("probe_score")] public int ProbeScore { get; set; }

        [JsonPropertyName("tags"), JsonConverter(typeof(LowercaseKeyDictionaryConverter<string>))]
        public Dictionary<string, string>? Tags { get; set; }
    }

    internal class StringToDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.String when double.TryParse(reader.GetString(), out var result) => result,
                _ => throw new JsonException($"Cannot convert {reader.GetString()} to double.")
            };
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    internal class LowercaseKeyDictionaryConverter<TValue> : JsonConverter<Dictionary<string, TValue>>
    {
        public override Dictionary<string, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, TValue>>(ref reader, options);
            var lowerDict = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);

            if (dict == null)
            {
                return lowerDict;
            }

            foreach (var kvp in dict)
            {
                lowerDict[kvp.Key.ToLower()] = kvp.Value;
            }

            return lowerDict;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, TValue> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}