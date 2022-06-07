using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.BotBuilder.Interfaces;
using Telegram.BotBuilder.Managers;
using Telegram.BotBuilder.Models;
using Telegram.BotBuilder.Providers;

namespace Telegram.BotBuilder.Extensions
{
    public static class AppStartupExtensions
    {
        public static Task UseTelegramBotLongPolling<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default
        )
            where TBot : BotBase
        {
            //var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            if (startAfter == default)
            {
                startAfter = TimeSpan.FromSeconds(2);
            }

            var updateManager = new UpdatePollingManager<TBot>(botBuilder, new BotServiceProvider(app));

            Task.Delay(startAfter, cancellationToken)
                .ConfigureAwait(false);

            updateManager.RunAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public static async Task EnsureWebhookSet<TBot>(this IApplicationBuilder app, Uri uri)
            where TBot : IBot
        {
            using var scope = app.ApplicationServices.CreateScope();
            //var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var bot = scope.ServiceProvider.GetRequiredService<TBot>();
            //var options = scope.ServiceProvider.GetRequiredService<IOptions<CustomBotOptions<TBot>>>();

            //logger.LogDebug("Setting webhook for bot \"{0}\" to URL \"{1}\"", typeof(TBot).Name, url);

            await bot.Client.SetWebhookAsync(uri.AbsoluteUri);

            await Task.Delay(2000);
        }
    }
}
