using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace identity.Data.Repositorys
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = true)
        {

            using (var client = new SmtpClient())
            {

                var credentials = new NetworkCredential()
                {
                    UserName = "Mohammad.AmirBeygi98", // without @gmail.com
                    Password = "mo1998mo9406mo"
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress("Mohammad.AmirBeygi98@gmail.com"), // with @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };

                client.Send(emailMessage);
            }

            return Task.CompletedTask;


        }
    }
}
