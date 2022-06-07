using System;
using JA.Telegram.BotBuilder.Delegates;
using JA.Telegram.BotBuilder.Interfaces;
using JA.Telegram.BotBuilder.Middlewares;
using JA.Telegram.BotBuilder.Models;
using Telegram.Bot.Polling;

namespace JA.Telegram.BotBuilder.Extensions
{
    public static class BotBuilderExtensions
    {
        public static IBotBuilder UseWhen(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure)
        {
            builder.Use(next => (context, cancellationToken) =>
            {
                if (!predicate(context))
                    return next(context, cancellationToken);

                var botBuilder = new BotBuilder();
                configure(botBuilder);
                botBuilder.Use(next);

                return botBuilder.Build()(context, cancellationToken);
            });
            return builder;
        }

        public static IBotBuilder UseWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate)
            where THandler : IUpdateHandler
        {
            builder.UseWhen(predicate, botBuilder => botBuilder.Use<THandler>());
            return builder;
        }

        public static IBotBuilder MapWhen(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate,
            Action<IBotBuilder> configure
        )
        {
            var mapBuilder = new BotBuilder();
            configure(mapBuilder);
            var mapDelegate = mapBuilder.Build();

            builder.UseWhenMiddleware(new MapWhenMiddleware(predicate, mapDelegate));

            return builder;
        }

        public static IBotBuilder MapWhen<THandler>(
            this IBotBuilder builder,
            Predicate<IUpdateContext> predicate
        )
            where THandler : IUpdateHandler
        {
            var branchDelegate = new BotBuilder().Use<THandler>().Build();
            builder.UseWhenMiddleware(new MapWhenMiddleware(predicate, branchDelegate));
            return builder;
        }

        public static IBotBuilder UseCommand<TCommand>(
            this IBotBuilder builder,
            string command)
            where TCommand : CommandBase
        {
            return builder.UseWhen(ctx => 
                    ctx.Bot.CanHandleCommand(command, ctx.Update.Message), 
                botBuilder => botBuilder.Use<TCommand>()
                );
        }
    }
}
