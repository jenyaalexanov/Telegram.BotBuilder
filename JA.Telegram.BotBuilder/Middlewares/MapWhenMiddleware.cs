﻿using System;
using System.Threading.Tasks;
using Telegram.BotBuilder.Delegates;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Middlewares
{
    public class MapWhenMiddleware : IUpdateWhenMiddlewareHandler
    {
        private readonly Predicate<IUpdateContext> _predicate;
        private readonly UpdateDelegate _branch;

        public MapWhenMiddleware(Predicate<IUpdateContext> predicate, UpdateDelegate branch)
        {
            _predicate = predicate;
            _branch = branch;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next)
            => _predicate(context)
                ? _branch(context)
                : next(context);
    }
}
