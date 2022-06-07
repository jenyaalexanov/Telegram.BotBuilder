using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace JA.Telegram.WebApi.Handlers
{
    public class StickerHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var msg = update.Message;
            var incomingSticker = msg.Sticker;

            var evilMindsSet = await botClient.GetStickerSetAsync("EvilMinds", cancellationToken);

            var similarEvilMindSticker = evilMindsSet.Stickers
                .FirstOrDefault(sticker => incomingSticker?.Emoji?.Contains(sticker.Emoji!) ?? false);

            var replySticker = similarEvilMindSticker ?? evilMindsSet.Stickers.First();

            await botClient.SendStickerAsync(
                msg.Chat,
                replySticker.FileId,
                replyToMessageId: msg.MessageId, 
                cancellationToken: cancellationToken
                );
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
