using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.IO;
using System.Threading.Tasks;

namespace EmailSender.App.EmailService
{
    public class EmailSendService : IEmailSendService
    {
        private EMailConfiguration _emailConfig;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmailSendService(EMailConfiguration emailConfig, IWebHostEnvironment hostEnvironment)
        {
            _emailConfig = emailConfig;
            _hostEnvironment = hostEnvironment;

        }

        public void SendEmail(MailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailHtml(MailMessage message)
        {
            var contentHtml = await GetMailHtmlTemplate("mailcontent");
            // Prepare Content from Html Template
            message.Content = contentHtml.Replace("#StrContent#", message.Content);

            var emailMessage = CreateEmailMessageHtml(message);

            Send(emailMessage);
        }



        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = delegate { return true; };
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.UseSsl);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(emailMessage);
                }
                catch
                {

                    throw;
                }
                finally
                {

                    client.Disconnect(true);
                    client.Dispose();
                }


            }
        }

        private MimeMessage CreateEmailMessage(MailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private MimeMessage CreateEmailMessageHtml(MailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            emailMessage.To.Add(MailboxAddress.Parse(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }



        // Load Html Template For Mail
        public async Task<string> GetMailHtmlTemplate(string TemplateName)
        {



            string path = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/mailtemplate", TemplateName + ".html");
            string content = "";
            try
            {
                using var sr = new StreamReader(path);
                content = await sr.ReadToEndAsync();
            }
            catch
            {

            }

            return content;





        }


    }
}
