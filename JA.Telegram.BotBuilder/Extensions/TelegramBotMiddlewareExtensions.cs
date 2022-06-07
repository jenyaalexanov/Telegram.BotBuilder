﻿using System;
using System.Threading.Tasks;
using JA.Telegram.BotBuilder.Interfaces;
using JA.Telegram.BotBuilder.Middlewares;
using JA.Telegram.BotBuilder.Models;
using Microsoft.AspNetCore.Builder;

namespace JA.Telegram.BotBuilder.Extensions
{
    public static class TelegramBotMiddlewareExtensions
    {
        /// <summary>
        /// Add Telegram bot webhook handling functionality to the pipeline
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="botBuilder"></param>
        /// <param name="webHookUri"></param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static async Task<IApplicationBuilder> UseTelegramBotWebhook<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder,
            Uri webHookUri)
            where TBot : BotBase
        {
            var updateDelegate = botBuilder.Build();
            //IOptions<BotOptions<TBot>> requiredService = app.ApplicationServices.GetRequiredService<IOptions<BotOptions<TBot>>>();
            app.Map(webHookUri.AbsolutePath, builder => builder.UseMiddleware<TelegramBotMiddleware<TBot>>(updateDelegate));

            await app.EnsureWebhookSet<TBot>(webHookUri);

            return app;
        }
    }
}
