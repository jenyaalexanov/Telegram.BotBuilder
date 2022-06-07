using System.Threading;
using System.Threading.Tasks;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Delegates
{
    public delegate Task UpdateDelegate(
        IUpdateContext context,
        CancellationToken cancellationToken = default);
}
