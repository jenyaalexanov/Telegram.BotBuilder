using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace JA.Telegram.BotBuilder.Interfaces
{
    public interface IUpdatePollingManager<TBot> where TBot : IBot
    {
        Task RunAsync(GetUpdatesRequest? requestParams = null, CancellationToken cancellationToken = default);
    }
}
