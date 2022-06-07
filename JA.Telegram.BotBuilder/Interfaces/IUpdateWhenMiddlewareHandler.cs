using System.Threading.Tasks;
using Telegram.BotBuilder.Delegates;

namespace Telegram.BotBuilder.Interfaces
{
    public interface IUpdateWhenMiddlewareHandler
    {
        Task HandleAsync(IUpdateContext context, UpdateDelegate next);
    }
}
