using EmailSender.App.EmailService;
using EmailSender.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSendService _emailSendService;
        public HomeController(IEmailSendService emailSendService)
        {
            
            _emailSendService = emailSendService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMail(string toMail, string subject, string content, bool htmluse)
        {
            if (htmluse)
                _emailSendService.SendEmailHtml(new MailMessage(toMail, subject, content)).Wait();
            else
                _emailSendService.SendEmail(new MailMessage(toMail, subject, content));
            TempData["Message"] = "E-mail has been sent";
            return RedirectToAction("Index");
        }


    }
}
