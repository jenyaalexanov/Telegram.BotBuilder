using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Contexts
{
    public class UpdateContext : IUpdateContext
    {
        public IBot Bot { get; }

        public Update Update { get; }

        public IServiceProvider Services { get; }

        public IDictionary<string, object> Items { get; }

        public UpdateContext(IBot bot, Update update, IServiceProvider services)
        {
            Bot = bot;
            Update = update;
            Services = services;
            Items = new ConcurrentDictionary<string, object>();
        }
    }
}
