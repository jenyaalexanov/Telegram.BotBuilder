using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace JA.Telegram.WebApi.Handlers
{
    public class LocationHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                update.Message.Chat,
                "Thank you for sharing your location :)", 
                cancellationToken: cancellationToken);
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
}
