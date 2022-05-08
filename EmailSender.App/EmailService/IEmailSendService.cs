using System.Threading.Tasks;

namespace EmailSender.App.EmailService
{
    public interface IEmailSendService
    {
        void SendEmail(MailMessage message);
        Task SendEmailHtml(MailMessage message);
        
    }
}
