using JA.Telegram.BotBuilder;
using JA.Telegram.BotBuilder.Extensions;
using JA.Telegram.WebApi.Bots;
using JA.Telegram.WebApi.Commands;
using JA.Telegram.WebApi.Constants;
using JA.Telegram.WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add\create your telegram bot as transient
builder.Services.AddTransient(_ => new TestBot1("nameOfYourBot1", "tokenOfBot1"));
builder.Services.AddTransient(_ => new TestBot2("nameOfYourBot2", "tokenOfBot2"));

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

// configure your bot builder
var botBuilder = new BotBuilder()
    .UseCommand<MessageCommand>(DataConstants.MessageCommand)
    .UseCommand<OtherCommand>(DataConstants.OtherCommand)
    .UseWhen<TodayHandler>(TodayHandler.CanHandle)
    .UseWhen<TomorrowHandler>(TomorrowHandler.CanHandle);

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

// bind as LongPolling
//await app.UseTelegramBotLongPolling<TestBot1>(botBuilder);
//await app.UseTelegramBotLongPolling<TestBot2>(botBuilder);

// or as Webhook
await app.UseTelegramBotWebhook<TestBot1>(botBuilder, new Uri("https://example.com/test1/bot"));
await app.UseTelegramBotWebhook<TestBot2>(botBuilderWithMapWhen, new Uri("https://example.com/test2/bot"));

app.Run();
