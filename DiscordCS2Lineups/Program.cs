using DiscordCS2Lineups.config;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordCS2Lineups
{
    internal class Program
    {
        public static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents  = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands>();
    
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }

    public class Commands : BaseCommandModule
    {
        private JSONReader jsonReader;

        public Commands()
        {
            jsonReader = new JSONReader();
        }

        private async Task MapCommands(CommandContext context, string channelID, string nade = null, string position = null)
        {
            if (ulong.TryParse(channelID, out ulong id))
            {
                var channel = await Program.Client.GetChannelAsync(id);

                if (!string.IsNullOrEmpty(nade) && !string.IsNullOrEmpty(position))
                {
                    var messages = await channel.GetMessagesAsync(100);
                    foreach (var message in messages)
                    {
                        string content = message.Content;
                        string urlPatterns = @"https?://[^\s]+";
                        string tags = Regex.Replace(content, urlPatterns, string.Empty);
                        tags = tags.ToLower();
                        if (tags.Contains(nade) && tags.Contains(position))
                        {
                            string[] date = message.Timestamp.ToString().Split(' ');
                            await context.Channel.SendMessageAsync($"{date[0]} - {message.Content}");
                        }
                    }
                }
            }
            else
            {
                await context.Channel.SendMessageAsync($"Felaktigt ChannelID kopplat till {context.Command.Name}.");
            }
        }

        private async Task GetChannelID(CommandContext context, string map, string nade, string position)
        {
            string channelID;
            await jsonReader.ReadJSON();
            switch(map)
            {
                case "ancient":
                    channelID = jsonReader.ancient;
                    break;
                case "anubis":
                    channelID = jsonReader.anubis;
                    break;
                case "dust2":
                    channelID = jsonReader.dust2;
                    break;
                case "inferno":
                    channelID = jsonReader.inferno;
                    break;
                case "mirage":
                    channelID = jsonReader.mirage;
                    break;
                case "nuke":
                    channelID = jsonReader.nuke;
                    break;
                case "vertigo":
                    channelID = jsonReader.vertigo;
                    break;
                default:
                    await context.Channel.SendMessageAsync($"Kartan {map} är ej registrerad");
                    return;
            };
            await MapCommands(context, channelID, nade, position);
        }

        [Command("help")]
        public async Task Help(CommandContext context) => await context.Channel.SendMessageAsync("skriv in '-' följt av vilken karta, typ av granat och var på kartan. Ex '-dust2 smoke mid'");

        [Command("ancient")]
        public async Task Ancient(CommandContext context, string nade, string position) => await GetChannelID(context, context.Command.Name, nade, position);

        [Command("anubis")]
        public async Task Anubis(CommandContext context, string nade, string position) => await GetChannelID(context, "anubis", nade, position);

        [Command("dust2")]
        public async Task Dust2(CommandContext context, string nade, string position) => await GetChannelID(context, "dust2", nade, position);

        [Command("inferno")]
        public async Task Inferno(CommandContext context, string nade, string position) => await GetChannelID(context, "inferno", nade, position);

        [Command("mirage")]
        public async Task Mirage(CommandContext context, string nade, string position) => await GetChannelID(context, "mirage", nade, position);

        [Command("nuke")]
        public async Task Nuke(CommandContext context, string nade, string position) => await GetChannelID(context, "nuke", nade, position);

        [Command("vertigo")]
        public async Task Vertigo(CommandContext context, string nade, string position) => await GetChannelID(context, "vertigo", nade, position);
    }      
}