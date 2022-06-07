using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.BotBuilder.Contexts;
using Telegram.BotBuilder.Delegates;
using Telegram.BotBuilder.Interfaces;

namespace Telegram.BotBuilder.Managers
{
    public class UpdatePollingManager<TBot> : IUpdatePollingManager<TBot> where TBot : IBot
    {
        private readonly UpdateDelegate _updateDelegate;
        private readonly IBotServiceProvider _rootProvider;

        public UpdatePollingManager(IBotBuilder botBuilder, IBotServiceProvider rootProvider)
        {
            _updateDelegate = botBuilder.Build();
            _rootProvider = rootProvider;
        }

        public async Task RunAsync(
          GetUpdatesRequest? requestParams = null,
          CancellationToken cancellationToken = default)
        {
            var bot = (TBot)_rootProvider.GetService(typeof(TBot));

            var configuredTaskAwaitable = bot.Client.DeleteWebhookAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            await configuredTaskAwaitable;

            var getUpdatesRequest = requestParams ?? new GetUpdatesRequest()
            {
                Offset = 0,
                Timeout = 500,
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            requestParams = getUpdatesRequest;
            while (!cancellationToken.IsCancellationRequested)
            {
                var updates = await bot.Client.MakeRequestAsync(requestParams, cancellationToken)
                    .ConfigureAwait(false);
                var updateArray = updates;

                foreach (var u in updateArray)
                {
                    using var scopeProvider = _rootProvider.CreateScope();
                    configuredTaskAwaitable = _updateDelegate(new UpdateContext(bot, u, scopeProvider), cancellationToken)
                        .ConfigureAwait(false);
                    await configuredTaskAwaitable;
                }

                updateArray = null;

                if (updates.Length != 0)
                    requestParams.Offset = updates[^1].Id + 1;

                updates = null;
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
