using Newtonsoft.Json;

namespace Schlauchboot.Ripper.Crunchyroll.Manager.Models
{
    /// <summary>
    /// Used to Parse Strings in an easier to use object.
    /// </summary>
    public class Stream
    {
        [JsonProperty(Required = Required.AllowNull)]
        public string format { get; set; }
        [JsonProperty(Required = Required.AllowNull)]
        public string audio_lang { get; set; }
        [JsonProperty(Required = Required.AllowNull)]
        public string hardsub_lang { get; set; }
        [JsonProperty(Required = Required.AllowNull)]
        public string url { get; set; }
        [JsonProperty(Required = Required.AllowNull)]
        public string resolution { get; set; }
    }
}
