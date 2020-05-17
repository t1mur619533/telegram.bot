using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.NotificationBot;

namespace Notifications.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMessageSender messageSender;

        public NotificationsController(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }

        [HttpPost("{chatId}")]
        public async Task Post(long chatId, [FromQuery]string markdownMessage)
        {
            await messageSender.Send(chatId, markdownMessage);
        }
    }
}