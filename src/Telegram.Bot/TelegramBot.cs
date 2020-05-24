using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.NotificationBot.Options;

namespace Telegram.NotificationBot
{
    public class MessageSender : ITelegramBot, IDisposable
    {
        private readonly TelegramBotClient telegramBotClient;

        public MessageSender(IOptions<TelegramBot> options)
        {
            telegramBotClient = string.IsNullOrWhiteSpace(options.Value.ProxyHost)
                ? new TelegramBotClient(options.Value.TelegramBotToken)
                : new TelegramBotClient(options.Value.TelegramBotToken,
                    new HttpToSocks5Proxy(options.Value.ProxyHost, options.Value.ProxyPort));
            telegramBotClient.OnMessage += BotOnMessageReceived;
            telegramBotClient.StartReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            await Send(e.Message.Chat.Id, $"Your chat id *{e.Message.Chat.Id}*");
        }

        public async Task Send(long chatId, string message)
        {
            await telegramBotClient.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
        }

        public void Dispose()
        {
            telegramBotClient.OnMessage -= BotOnMessageReceived;
            telegramBotClient.StopReceiving();
        }
    }
}