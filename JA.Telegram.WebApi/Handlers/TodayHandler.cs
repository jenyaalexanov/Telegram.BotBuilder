using JA.Telegram.BotBuilder.Extensions;
using JA.Telegram.BotBuilder.Interfaces;
using JA.Telegram.WebApi.Constants;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace JA.Telegram.WebApi.Handlers
{
    public class TodayHandler : IUpdateHandler
    {
        public static bool CanHandle(IUpdateContext context)
        {
            return
                context.Update.Type == UpdateType.CallbackQuery
                &&
                context.Update.IsCallbackCommand(DataConstants.Today);
        }

        public async Task HandleUpdateAsync(
            ITelegramBotClient botClient, 
            Update update, 
            CancellationToken cancellationToken
            )
        {
            await botClient.EditMessageTextAsync(
                update.CallbackQuery.Message.Chat.Id,
                update.CallbackQuery.Message.MessageId,
                $"Today's date: {DateTime.Today:d}",
                cancellationToken: cancellationToken
            );
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
}
