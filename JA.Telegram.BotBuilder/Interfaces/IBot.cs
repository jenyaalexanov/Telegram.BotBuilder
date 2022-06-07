using Telegram.Bot;

namespace JA.Telegram.BotBuilder.Interfaces
{
    public interface IBot
    {
        string Username { get; }

        ITelegramBotClient Client { get; }
    }
}
