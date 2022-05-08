using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailSender.App.EmailService
{
    public class MailMessage
    {
        public MailMessage(string to,string subject,string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
