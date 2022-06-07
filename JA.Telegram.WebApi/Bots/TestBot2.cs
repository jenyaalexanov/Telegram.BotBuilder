using Telegram.Bot;
using Telegram.BotBuilder.Models;

namespace JA.Telegram.WebApi.Bots
{
    public class TestBot2 : BotBase
    {
        public TestBot2(string username, ITelegramBotClient client) : base(username, client)
        {
        }

        public TestBot2(string username, string token) : base(username, token)
        {
        }
    }
}
