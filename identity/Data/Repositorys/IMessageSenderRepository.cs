using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Data.Repositorys
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = true);
    }
}
