using System;
using System.IO;
using System.Threading.Tasks;
using JA.Telegram.BotBuilder.Contexts;
using JA.Telegram.BotBuilder.Delegates;
using JA.Telegram.BotBuilder.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace JA.Telegram.BotBuilder.Middlewares
{
    internal class TelegramBotMiddleware<TBot> where TBot : BotBase
    {
        private readonly RequestDelegate _next;
        private readonly UpdateDelegate _updateDelegate;
        //private readonly ILogger<TelegramBotMiddleware<TBot>> _logger;

        /// <summary>Initializes an instance of middleware</summary>
        /// <param name="next">Instance of request delegate</param>
        /// <param name="updateDelegate"></param>
        public TelegramBotMiddleware(
          RequestDelegate next,
          UpdateDelegate updateDelegate)
        {
            _next = next;
            _updateDelegate = updateDelegate;
            //this._logger = logger;
        }

        /// <summary>Gets invoked to handle the incoming request</summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Post)
            {
                await _next(context).ConfigureAwait(false);
            }
            else
            {
                string str;
                using (var reader = new StreamReader(context.Request.Body))
                    str = await reader.ReadToEndAsync().ConfigureAwait(false);
                //this._logger.LogDebug("Update payload:\n{0}", (object)str);
                Update? update = null;
                try
                {
                    update = JsonConvert.DeserializeObject<Update>(str);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"TelegramBotMiddleware error. JsonException: {ex.Message}");
                    //this._logger.LogError("Unable to deserialize update payload. " + ((Exception)ex).Message);
                }
                if (update == null)
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    using (var scope = context.RequestServices.CreateScope())
                    {
                        var updateContext = new UpdateContext(scope.ServiceProvider.GetRequiredService<TBot>(), update, scope.ServiceProvider);
                        updateContext.Items.Add("HttpContext", context);
                        try
                        {
                            await _updateDelegate(updateContext).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"TelegramBotMiddleware error: {ex.Message}");
                            //this._logger.LogError(string.Format("Error occured while handling update `{0}`. {1}", (object)update.Id, (object)ex.Message));
                            context.Response.StatusCode = 500;
                        }
                    }
                    if (context.Response.HasStarted)
                        return;
                    context.Response.StatusCode = 201;
                }
            }
        }
    }
}
