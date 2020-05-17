using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Telegram.NotificationBot
{
    public class MessageSender : ITelegramBot, IDisposable
    {
        private readonly TelegramBotClient telegramBotClient;

        public MessageSender(IConfiguration configuration)
        {
            var token = configuration["TelegramBotSettings:telegramBotToken"];
            var host = configuration["TelegramBotSettings:proxyHost"];
            var port = int.Parse(configuration["TelegramBotSettings:proxyPort"]);
            telegramBotClient = new TelegramBotClient(token, new HttpToSocks5Proxy(host, port));
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