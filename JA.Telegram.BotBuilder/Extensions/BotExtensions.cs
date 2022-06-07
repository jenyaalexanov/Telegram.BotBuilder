using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Extensions
{
    public static class BotExtensions
    {
        public static bool CanHandleCommand(this IBot bot, string commandName, Message? message)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("Invalid command name", nameof(commandName));
            if (commandName.StartsWith("/"))
                throw new ArgumentException("Command name must not start with '/'.", nameof(commandName));
            if (message == null || message is {Text: null})
                return false;

            var entities = message.Entities;
            int num;
            if (entities == null)
            {
                num = 0;
            }
            else
            {
                var type = entities.FirstOrDefault()?.Type;
                num = type.GetValueOrDefault() == MessageEntityType.BotCommand & type.HasValue ? 1 : 0;
            }
            if (num == 0)
                return false;

            return message.EntityValues != null && Regex.IsMatch(message.EntityValues.First(), "^/" + commandName + "(?:@" + bot.Username + ")?$", RegexOptions.IgnoreCase);
        }
    }
}
