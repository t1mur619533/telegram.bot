using System.Threading.Tasks;

namespace Telegram.NotificationBot
{
    public interface IMessageSender
	{
        Task Send(long chatId, string message);
    }
}
