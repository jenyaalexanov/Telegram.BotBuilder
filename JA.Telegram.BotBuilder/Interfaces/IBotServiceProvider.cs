using System;

namespace JA.Telegram.BotBuilder.Interfaces
{
    public interface IBotServiceProvider : IServiceProvider, IDisposable
    {
        IBotServiceProvider CreateScope();
    }
}
