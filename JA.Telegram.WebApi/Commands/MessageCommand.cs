using JA.Telegram.BotBuilder.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JA.Telegram.WebApi.Commands
{
    public class MessageCommand : CommandBase
    {
        public override async Task HandleAsync(
            ITelegramBotClient botClient, 
            Update update, 
            string[] args, 
            CancellationToken cancellationToken
            )
        {
            // do smth what you want

            await botClient.SendTextMessageAsync(
                update.Message.Chat.Id, 
                "Some text from message command",
                cancellationToken: cancellationToken);

            // do smth what you want
        }
    }
}
