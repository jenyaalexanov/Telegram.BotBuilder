# Telegram.BotBuilder

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Telegram.BotBuilder?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/dt/Telegram.BotBuilder?style=for-the-badge)
[![GitHub license](https://img.shields.io/github/license/jenyaalexanov/Telegram.BotBuilder?style=for-the-badge)](https://github.com/jenyaalexanov/Telegram.BotBuilder/blob/master/LICENSE)

<img src="./icons/BotBuilder.svg" alt="Telegram BotBuilder Logo" width=200 height=200 />

- Telegram.BotBuilder is a library that allows you to structure your commands, handlers and logic of your program.
- Based on [`Telegram.Bot.Framework`](https://github.com/TelegramBots/Telegram.Bot.Framework) but modified for the new version of [`Telegram.Bot`](https://github.com/TelegramBots/Telegram.Bot) [`18.0.0-alpha.3`](https://www.nuget.org/packages/Telegram.Bot/18.0.0-alpha.3) 
- Based on dotnet standard 2.1

  >And of course I made this library for my own use because this approach is extremely convenient and useful when developing complex Telegram bots.
  >I am always open to your changes, commits and wishes :)

How do I get started?
--------------
First you should inherit your **class** from the base abstract **BotBase** class.

    public class TestBot1 : BotBase
    {
        public TestBot1(string username, ITelegramBotClient client) : base(username, client)
        {
        }

        public TestBot1(string username, string token) : base(username, token)
        {
        }
    }
After that you should register it as a **Transient**. You can register **any number** of your telegram bots.

    // Add\create your telegram bot as transient
	builder.Services.AddTransient(_ => new TestBot1("nameOfYourBot1", "tokenOfBot1"));
	builder.Services.AddTransient(_ => new TestBot2("nameOfYourBot2", "tokenOfBot2"));
For **commands**, you should inherit from **CommandBase** abstract class.

    public class MessageCommand : CommandBase
    {
        public override async Task HandleAsync(
            ITelegramBotClient botClient, 
            Update update, 
            string[] args, 
            CancellationToken cancellationToken
            )
        {
            // do smth what you want

            await botClient.SendTextMessageAsync(
                update.Message.Chat.Id, 
                "Some text from message command",
                cancellationToken: cancellationToken);

            // do smth what you want
        }
    }
For **handlers**, you should inherit from **IUpdateHandler**

    public class TodayHandler : IUpdateHandler
    {
        public static bool CanHandle(IUpdateContext context)
        {
            return
                context.Update.Type == UpdateType.CallbackQuery
                &&
                context.Update.IsCallbackCommand(DataConstants.Today);
        }

        public async Task HandleUpdateAsync(
            ITelegramBotClient botClient, 
            Update update, 
            CancellationToken cancellationToken
            )
        {
            await botClient.EditMessageTextAsync(
                update.CallbackQuery.Message.Chat.Id,
                update.CallbackQuery.Message.MessageId,
                $"Today's date: {DateTime.Today:d}",
                cancellationToken: cancellationToken
            );
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw exception;
        }
    }
Your **handlers** or **commands** should be registered as scoped.

    // add scoped your commands/handlers
	builder.Services.AddScoped<MessageCommand>();
	builder.Services.AddScoped<OtherCommand>();
	builder.Services.AddScoped<PingCommand>();
	builder.Services.AddScoped<StartCommand>();
	builder.Services.AddScoped<TodayHandler>();
	builder.Services.AddScoped<TomorrowHandler>();
	builder.Services.AddScoped<LocationHandler>();
	builder.Services.AddScoped<TextEchoerHandler>();
	builder.Services.AddScoped<StickerHandler>();
Next, you are ready for **building** with the help of **this library** of your logic

    // configure your bot builder
	var botBuilder = new BotBuilder()
	    .UseCommand<MessageCommand>(DataConstants.MessageCommand)
	    .UseCommand<OtherCommand>(DataConstants.OtherCommand)
	    .UseWhen<TodayHandler>(TodayHandler.CanHandle)
	    .UseWhen<TomorrowHandler>(TomorrowHandler.CanHandle);
For example for **.UseCommand<>** we pass a specific command(**MessageCommand**) that will be initialized after the user sends a certain message. (**/message**).

    .UseCommand<MessageCommand>("message")
For **.UseWhen<>** we pass a specific handler(**TodayHandler**) that will be initialized if we have true value in **TodayHandler.CanHandle** method.

    .UseWhen<TodayHandler>(TodayHandler.CanHandle)

Here's **CanHandle** example:

    public static bool CanHandle(IUpdateContext context)
    {
          return
              context.Update.Type == UpdateType.CallbackQuery
              &&
              context.Update.IsCallbackCommand(DataConstants.Today);
    }

You can even use **.MapWhen<>**. Here's some examples of builder:

    var botBuilderWithMapWhen = new BotBuilder()
    .UseWhen<LocationHandler>(When.LocationMessage)
    .MapWhen(When.NewMessage, msgBranch => msgBranch
        .MapWhen(When.NewTextMessage, txtBranch => txtBranch
                .MapWhen(When.NewCommand, cmdBranch => cmdBranch
                    .UseCommand<PingCommand>(DataConstants.PingCommand)
                    .UseCommand<StartCommand>(DataConstants.StartCommand)
                )
                .Use<TextEchoerHandler>()
        )
        .MapWhen<StickerHandler>(When.StickerMessage)
    );
After that you can **bind** using **LongPolling**:

    // bind as LongPolling
	await app.UseTelegramBotLongPolling<TestBot1>(botBuilder);
	await app.UseTelegramBotLongPolling<TestBot2>(botBuilder);
Or using **Webhooks**

    // or as Webhook
	await app.UseTelegramBotWebhook<TestBot1>(botBuilder, new Uri("https://example.com/test1/bot"));
	await app.UseTelegramBotWebhook<TestBot2>(botBuilderWithMapWhen, new Uri("https://example.com/test2/bot"));

You can find **all these examples** and **work with them** in the [webapi project](https://github.com/jenyaalexanov/Telegram.BotBuilder/tree/master/JA.Telegram.WebApi)
