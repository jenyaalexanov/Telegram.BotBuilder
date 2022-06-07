using System.Threading.Tasks;
using JA.Telegram.BotBuilder.Delegates;

namespace JA.Telegram.BotBuilder.Interfaces
{
    public interface IUpdateWhenMiddlewareHandler
    {
        Task HandleAsync(IUpdateContext context, UpdateDelegate next);
    }
}
