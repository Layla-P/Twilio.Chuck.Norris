using System.Collections.Generic;
using Newtonsoft.Json;

namespace Twilio.Chuck.Norris.Api.Models
{
    public class ChuckNorrisResponse
    {
        public List<string> Category { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
    }
}
