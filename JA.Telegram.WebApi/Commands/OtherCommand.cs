using JA.Telegram.WebApi.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.BotBuilder.Models;

namespace JA.Telegram.WebApi.Commands
{
    public class OtherCommand : CommandBase
    {
        public override async Task HandleAsync(
            ITelegramBotClient botClient,
            Update update,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            // do smth what you want

            var keyboardRows = new List<IEnumerable<InlineKeyboardButton>>
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        "Get today (edit message)",
                        DataConstants.Today
                    ),
                    InlineKeyboardButton.WithCallbackData(
                        "Get tomorrow (send new message)",
                        DataConstants.Tomorrow
                    )
                }
            };

            await botClient.SendTextMessageAsync(
                update.Message.Chat.Id,
                "Some text from other command",
                cancellationToken: cancellationToken,
                replyMarkup: new InlineKeyboardMarkup(keyboardRows)
                );

            // do smth what you want
        }
    }
}
