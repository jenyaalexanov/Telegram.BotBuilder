using Telegram.Bot;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Models
{
    public abstract class BotBase : IBot
    {
        public ITelegramBotClient Client { get; }

        public string Username { get; }

        protected BotBase(string username, ITelegramBotClient client)
        {
            this.Username = username;
            this.Client = client;
        }

        protected BotBase(string username, string token)
            : this(username, new TelegramBotClient(token))
        {
        }

        //protected BotBase(IBotOptions options)
        //    : this(options.Username, (ITelegramBotClient)new TelegramBotClient(options.ApiToken, (HttpClient)null))
        //{
        //}
    }
}
