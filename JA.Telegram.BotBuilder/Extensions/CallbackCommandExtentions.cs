using System;
using Telegram.Bot.Types;

namespace Telegram.BotBuilder.Extensions
{
    public static class CallbackCommandExtensions
    {
        public static bool IsCallbackCommand(this Update update, string command) =>
            update.CallbackQuery?.Data?.StartsWith(
                command,
                StringComparison.Ordinal) ?? false;

        public static string TrimCallbackCommand(this Update update, string pattern) =>
            update.CallbackQuery?.Data?.Replace(pattern, string.Empty) ?? "";
    }
}
