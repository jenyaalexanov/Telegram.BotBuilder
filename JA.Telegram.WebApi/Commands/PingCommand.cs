using JA.Telegram.BotBuilder.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace JA.Telegram.WebApi.Commands
{
    public class PingCommand : CommandBase
    {
        public override async Task HandleAsync(ITelegramBotClient botClient, Update update, string[] args, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                update.Message.Chat,
                "*PONG*",
                ParseMode.Markdown,
                replyToMessageId: update.Message.MessageId,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
                ),
                cancellationToken: cancellationToken
            );
        }
    }
}
