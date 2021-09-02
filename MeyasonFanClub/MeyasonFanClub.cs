using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MeyasonFanClub.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

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

            if (CommandContext == "ハマチ" || CommandContext == "はまち") {
                await message.Channel.SendMessageAsync("中トロ");
            }

            if (CommandContext == "甘えびロール" || CommandContext == "甘海老ロール" || CommandContext == "あまえびろーる") {
                await message.Channel.SendMessageAsync("サーモン");
            }

            if (CommandContext == "ハンバーグ" || CommandContext == "はんばーぐ") {
                await message.Channel.SendMessageAsync("いなり");
            }

            if (CommandContext == "サラダ" || CommandContext == "さらだ") {
                await message.Channel.SendMessageAsync("サーモン炙り");
            }

            if (CommandContext == "パフェ" || CommandContext == "ぱふぇ") {
                await message.Channel.SendMessageAsync("いわし");
            }

            if (CommandContext == "meyason eroge") {
                await message.Channel.SendMessageAsync("めやそんおすすめのエロゲはR6Sです！！");
            }

            if (CommandContext == "meyason stats") {
                new statsCommand(message,_token).Execute();
            }
        }

        private Task Log(LogMessage message) {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

    }
}
