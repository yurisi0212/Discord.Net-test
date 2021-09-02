using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MeyasonFanClub.Command {
    class statsCommand : Command{

        private SocketUserMessage _message;

        private TokenManager _token;
        public statsCommand(SocketUserMessage message,TokenManager tokenManager) {
            _message = message;
            _token = tokenManager;
        }

        public async void Execute() {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.R6SToken);
            var generic = await httpClient.GetAsync(@"https://api2.r6stats.com/public-api/stats/Apeiria.Network/pc/generic");
            var season = await httpClient.GetAsync(@"https://api2.r6stats.com/public-api/stats/Apeiria.Network/pc/seasonal");

            var genericResult = await generic.Content.ReadAsStringAsync();
            var seasonResult = await season.Content.ReadAsStringAsync();

            var genericJson = JObject.Parse(genericResult);
            var seasonJson = JObject.Parse(seasonResult);

            if (genericJson["error"] != null) {
                await _message.Channel.SendMessageAsync("むり");
                return;
            }

            var level = genericJson["progression"]["level"];
            var kill = genericJson["stats"]["general"]["kills"];
            var death = genericJson["stats"]["general"]["deaths"];
            var assist = genericJson["stats"]["general"]["assists"];
            var kd = genericJson["stats"]["general"]["kd"];
            var win = genericJson["stats"]["general"]["wins"];
            var lose = genericJson["stats"]["general"]["losses"];
            var wl = genericJson["stats"]["general"]["wl"];
            var image = genericJson["avatar_url_146"];
            var rank = seasonJson["seasons"]["crimson_heist"]["regions"]["ncsa"][0]["max_rank_text"];


            var embed = new EmbedBuilder();
            embed.WithTitle("Apeiria.Network");
            embed.WithDescription("おいらめやそん！ 好きな食べ物はシャチの漬物ときゅうりの刺し身！");
            embed.WithColor(Color.Purple);
            embed.WithAuthor("", _message.Author.GetAvatarUrl());
            embed.WithThumbnailUrl(image.ToString());
            embed.AddField("PlayerLevel", level, true);
            embed.AddField("最高ランク", rank, true);
            embed.AddField("\u200b", "\u200b", true);
            embed.AddField("K/D", kd, true);
            embed.AddField("W/L", wl, true);
            embed.AddField("\u200b", "\u200b", true);
            embed.AddField("キル", kill, true);
            embed.AddField("デス", death, true);
            embed.AddField("アシスト", assist, true);
            embed.AddField("win", win, true);
            embed.AddField("lose", lose, true);
            embed.AddField("\u200b", "\u200b", true);
            embed.WithTimestamp(DateTime.Now);
            await _message.Channel.SendMessageAsync(embed: embed.Build());
        }

    }
}
