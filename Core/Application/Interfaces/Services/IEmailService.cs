using HealthChatBox.Core.Domain.Entities;
using HealthChatBox.Models;

namespace HealthChatBox.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailClient(string msg, string title, string email);
        Task<BaseResponse> SendNotificationToUserAsync(Profile profile);
        Task<bool> SendEmailAsync(MailRecieverDto model, MailRequests request);
    }
}

