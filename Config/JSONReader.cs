using System.Text;
using Newtonsoft.Json;

namespace Meira.Config
{
    internal class JSONReader
    {
        public string Token { get; set; }
        public string Prefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json", new UTF8Encoding(false)))
            {
                string json = await sr.ReadToEndAsync();
                ConfigJSON obj = JsonConvert.DeserializeObject<ConfigJSON>(json);

                this.Token = obj.Token;
                this.Prefix = obj.Prefix;
            }
        }
    }
}