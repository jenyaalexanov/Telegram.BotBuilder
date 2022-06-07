using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace JA.Telegram.BotBuilder.Models
{
    /// <summary>
    /// Base handler implementation for a command such as "/start"
    /// </summary>
    public abstract class CommandBase : IUpdateHandler
    {
        public abstract Task HandleAsync(
            ITelegramBotClient botClient,
            Update update,
            string[] args,
            CancellationToken cancellationToken);

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            return HandleAsync(botClient, update, ParseCommandArgs(update.Message), cancellationToken);
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }

        private static string[] ParseCommandArgs(Message? message)
        {
            var messageEntityArray = message != null ? 
                message.Entities : 
                throw new ArgumentNullException(nameof(message));

            int num;
            if (messageEntityArray == null)
            {
                num = 1;
            }
            else
            {
                var type = messageEntityArray.FirstOrDefault()?.Type;
                num = !(type.GetValueOrDefault() == MessageEntityType.BotCommand & type.HasValue) ? 1 : 0;
            }
            if (num != 0)
                throw new ArgumentException("Message is not a command.", nameof(message));
            var stringList = new List<string>();
            if (message.Entities == null)
                return stringList.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
                    ;
            var input = message.Text?[message.Entities[0].Length..]?.TrimStart(Array.Empty<char>());
            if (input == null)
                return stringList.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            stringList.Add(input);
            string[] collection = Regex.Split(input, "\\s+");

            if (collection.Length > 1)
                stringList.AddRange(collection);

            return stringList.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}
