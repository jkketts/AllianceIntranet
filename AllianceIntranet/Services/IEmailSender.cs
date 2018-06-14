using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllianceIntranet.Services
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string message);
        void SendEmailAsync(string email, string subject, string emailTemplateFileName, object model);
    }
}
