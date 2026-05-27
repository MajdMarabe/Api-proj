using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("majood.ma2002@gmail.com", "tnom uzcj nlug rqjp")
            };

            return client.SendMailAsync(
                new MailMessage(from: "majood.ma2002@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml= true}
                );
        }
    }
}
