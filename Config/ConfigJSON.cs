using Newtonsoft.Json;

namespace Meira.Config
{
    internal struct ConfigJSON
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}