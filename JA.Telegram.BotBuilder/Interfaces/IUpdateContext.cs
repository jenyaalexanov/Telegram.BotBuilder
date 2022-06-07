using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Telegram.BotBuilder.Interfaces
{
    public interface IUpdateContext
    {
        IBot Bot { get; }

        Update Update { get; }

        IServiceProvider Services { get; }

        IDictionary<string, object> Items { get; }
    }
}
