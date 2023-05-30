using System.Text;
using Newtonsoft.Json;

namespace Meira.Config
{
    internal class JSONReader
    {
        public string Token { get; set; }

        public async Task<T> ReadJSON<T>(string file)
        {
            using (StreamReader sr = new(file, new UTF8Encoding(false)))
            {
                string json = await sr.ReadToEndAsync();
                T obj = JsonConvert.DeserializeObject<T>(json);

                return obj;
            }
        }
    }
}