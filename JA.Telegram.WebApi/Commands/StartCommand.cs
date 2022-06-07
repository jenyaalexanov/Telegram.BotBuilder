using JA.Telegram.BotBuilder.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JA.Telegram.WebApi.Commands
{
    public class StartCommand : CommandBase
    {
        public override async Task HandleAsync(ITelegramBotClient botClient, Update update, string[] args, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat, "Hello, World!", cancellationToken: cancellationToken);
        }
    }
}
