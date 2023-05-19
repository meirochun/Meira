using Newtonsoft.Json;

namespace Meira.Config
{
    internal struct ConfigJSON
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("valorantApiCall")]
        public string ValorantApiCall { get; private set; }
    }
}
