using System;
using JA.Telegram.BotBuilder.Delegates;
using Telegram.Bot.Polling;

namespace JA.Telegram.BotBuilder.Interfaces
{
    public interface IBotBuilder
    {
        IBotBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware);

        IBotBuilder Use<THandler>() where THandler : IUpdateHandler;

        IBotBuilder Use<THandler>(THandler handler) where THandler : IUpdateHandler;

        IBotBuilder UseWhenMiddleware<THandler>(THandler handler) where THandler : IUpdateWhenMiddlewareHandler;

        UpdateDelegate Build();
    }
}
