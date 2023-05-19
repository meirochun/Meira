using System.Text;
using Newtonsoft.Json;

namespace Meira.Config
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string prefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader(".\\Config\\config.json", new UTF8Encoding(false)))
            {
                string json = await sr.ReadToEndAsync();
                ConfigJSON obj = JsonConvert.DeserializeObject<ConfigJSON>(json);

                this.token = obj.Token;
                this.prefix = obj.Prefix;
            }
        }
    }
}