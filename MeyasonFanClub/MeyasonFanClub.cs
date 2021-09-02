using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace MeyasonFanClub {
    class MeyasonFanClub {

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private TokenManager _token;

        static void Main(string[] args) {
            new MeyasonFanClub().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync() {
            _client = new DiscordSocketClient(new DiscordSocketConfig {
                LogLevel = LogSeverity.Info
            });
            _client.Log += Log;
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
            _client.MessageReceived += CommandRecieved;
            _token = new TokenManager();
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, _token.DiscordToken);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task CommandRecieved(SocketMessage messageParam) {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            if (message.Author.IsBot) return;

            var context = new CommandContext(_client, message);
            var CommandContext = message.Content;

            if (CommandContext == "meyason") {
                await message.Channel.SendMessageAsync("俺の女はスピニーだ");
            }

            if (CommandContext == "meyason stats") {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.R6SToken);
                var genericResult = httpClient.GetAsync("https://api2.r6stats.com/public-api/stats/Apeiria.Network/pc/generic").Result.Content.ReadAsStringAsync().Result;
                var seasonResult = httpClient.GetAsync("https://api2.r6stats.com/public-api/stats/Apeiria.Network/pc/seasonal").Result.Content.ReadAsStringAsync().Result;

                var genericJson = JObject.Parse(genericResult);
                var seasonJson = JObject.Parse(seasonResult);

                var level = genericJson["progression"]["level"];
                var kd = genericJson["stats"]["queue"]["casual"]["kd"];
                var rank = seasonJson["seasons"]["crimson_heist"]["regions"]["ncsa"][0]["max_rank_text"];
                await message.Channel.SendMessageAsync("おいらめやそん Lv"+level+"！\nシージのキルレは"+kd+"\n最高ランクは"+rank+ "だぞ！");
            }
        }

        private Task Log(LogMessage message) {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

    }
}
