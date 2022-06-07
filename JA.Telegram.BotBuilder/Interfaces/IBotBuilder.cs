using System;
using Telegram.Bot.Polling;
using Telegram.BotBuilder.Delegates;

namespace Telegram.BotBuilder.Interfaces
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
