using JA.Telegram.WebApi.Constants;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.BotBuilder.Extensions;
using Telegram.BotBuilder.Interfaces;

namespace JA.Telegram.WebApi.Handlers
{
    public class TomorrowHandler : IUpdateHandler
    {
        public static bool CanHandle(IUpdateContext context)
        {
            return
                context.Update.Type == UpdateType.CallbackQuery
                &&
                context.Update.IsCallbackCommand(DataConstants.Tomorrow);
        }

        public async Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
        )
        {
            await botClient.SendTextMessageAsync(
                update.CallbackQuery.Message.Chat.Id,
                $"Tomorrow's date: {DateTime.Today.AddDays(1):d}",
                cancellationToken: cancellationToken
            );
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
}
