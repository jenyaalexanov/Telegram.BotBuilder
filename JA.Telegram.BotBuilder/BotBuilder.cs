using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JA.Telegram.BotBuilder.Delegates;
using JA.Telegram.BotBuilder.Interfaces;
using Telegram.Bot.Polling;

namespace JA.Telegram.BotBuilder
{
    public class BotBuilder : IBotBuilder
    {
        private readonly ICollection<Func<UpdateDelegate, UpdateDelegate>> _components;

        internal UpdateDelegate? UpdateDelegate { get; private set; }

        public BotBuilder() => _components = new List<Func<UpdateDelegate, UpdateDelegate>>();

        public IBotBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        public IBotBuilder Use<THandler>() where THandler : IUpdateHandler
        {
            _components.Add(next => (context, cancellationToken) =>
            {
                if (context.Services.GetService(typeof(THandler)) is IUpdateHandler updateHandler)
                    return updateHandler.HandleUpdateAsync(context.Bot.Client, context.Update, cancellationToken);
                throw new NullReferenceException("Unable to resolve handler of type " + typeof(THandler).FullName);
            });
            return this;
        }

        public IBotBuilder Use(UpdateDelegate component)
        {
            _components.Add(next => component);
            return this;
        }

        public IBotBuilder Use<THandler>(THandler handler) where THandler : IUpdateHandler
        {
            _components.Add(next => (context, cancellationToken) => handler.HandleUpdateAsync(context.Bot.Client, context.Update, cancellationToken));
            return this;
        }

        public IBotBuilder UseWhenMiddleware<THandler>(THandler handler) where THandler : IUpdateWhenMiddlewareHandler
        {
            _components.Add(next => (context, cancellationToken) => handler.HandleAsync(context, next));
            return this;
        }

        public UpdateDelegate Build()
        {
            var updateDelegate = (UpdateDelegate)((context, cancellationToken) =>
            {
                Console.WriteLine($"No handler for update {context.Update.Id} of type {context.Update.Type}.");
                return Task.FromResult(1);
            });

            updateDelegate = _components.Reverse().Aggregate(updateDelegate, (current, func) => func(current));

            return UpdateDelegate = updateDelegate;
        }
    }
}
