using System.Linq;
using JA.Telegram.BotBuilder.Interfaces;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Types.Enums;

namespace JA.Telegram.BotBuilder.Extensions
{
    public static class When
    {
        public static bool Webhook(IUpdateContext context)
            => context.Items.ContainsKey(nameof(HttpContext));

        public static bool NewMessage(IUpdateContext context) =>
            context.Update.Message != null;

        public static bool NewTextMessage(IUpdateContext context) =>
            context.Update.Message?.Text != null;

        public static bool NewCommand(IUpdateContext context) =>
            context.Update.Message?.Entities?.FirstOrDefault()?.Type == MessageEntityType.BotCommand;

        public static bool MembersChanged(IUpdateContext context) =>
            context.Update.Message?.NewChatMembers != null ||
            context.Update.Message?.LeftChatMember != null ||
            context.Update.ChannelPost?.NewChatMembers != null ||
            context.Update.ChannelPost?.LeftChatMember != null
        ;

        public static bool NewMemberMessage(IUpdateContext context) =>
            context.Update.Message?.NewChatMembers != null
        ;

        public static bool NewMemberChannelPost(IUpdateContext context) =>
            context.Update.ChannelPost?.NewChatMembers != null
        ;

        public static bool LeftMemberMessage(IUpdateContext context) =>
            context.Update.Message?.LeftChatMember != null
        ;

        public static bool LeftMemberChannelPost(IUpdateContext context) =>
            context.Update.ChannelPost?.LeftChatMember != null
        ;

        public static bool LocationMessage(IUpdateContext context) =>
            context.Update.Message?.Location != null;

        public static bool StickerMessage(IUpdateContext context) =>
            context.Update.Message?.Sticker != null;

        public static bool CallbackQuery(IUpdateContext context) =>
            context.Update.CallbackQuery != null;
    }
}
