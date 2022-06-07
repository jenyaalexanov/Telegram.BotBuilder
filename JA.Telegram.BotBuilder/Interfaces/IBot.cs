using Telegram.Bot;

namespace Telegram.BotBuilder.Interfaces
{
    public interface IBot
    {
        string Username { get; }

        ITelegramBotClient Client { get; }
    }
}
