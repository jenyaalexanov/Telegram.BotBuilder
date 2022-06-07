using System;

namespace Telegram.BotBuilder.Interfaces
{
    public interface IBotServiceProvider : IServiceProvider, IDisposable
    {
        IBotServiceProvider CreateScope();
    }
}
