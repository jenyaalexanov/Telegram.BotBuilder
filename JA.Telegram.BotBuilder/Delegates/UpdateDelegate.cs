using System.Threading;
using System.Threading.Tasks;
using JA.Telegram.BotBuilder.Interfaces;
using Telegram.Bot.Types;

namespace JA.Telegram.BotBuilder.Delegates
{
    public delegate Task UpdateDelegate(
        IUpdateContext context,
        CancellationToken cancellationToken = default);
}
