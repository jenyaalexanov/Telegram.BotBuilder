using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace JA.Telegram.WebApi.Handlers
{
    public class TextEchoerHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                update.Message.Chat, $"You said:\n {update.Message.Text}"
            );
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
}
