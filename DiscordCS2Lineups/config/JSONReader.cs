using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace DiscordCS2Lineups.config
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public string ancient { get; set; }
        public string anubis { get; set; }
        public string dust2 { get; set; }
        public string inferno { get; set; }
        public string mirage { get; set; }
        public string nuke { get; set; }
        public string vertigo { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

                this.token = data.token;
                this.prefix = data.prefix;
                this.ancient = data.ancient;
                this.anubis = data.anubis;
                this.dust2 = data.dust2;
                this.inferno = data.inferno;
                this.mirage = data.mirage;
                this.nuke = data.nuke;
                this.vertigo = data.vertigo;
            }
        }
    }

    internal sealed class JSONStructure
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public string ancient{ get; set; }
        public string anubis { get; set; }
        public string dust2 { get; set; }
        public string inferno { get; set; }
        public string mirage { get; set; }
        public string nuke { get; set; }
        public string vertigo { get; set; }

    }
}
