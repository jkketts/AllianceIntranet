using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;
using System;

namespace AllianceIntranet.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public void SendEmail(string email, string subject, string message)
        {
            var exchange = new ExchangeService();
            exchange.Credentials = new WebCredentials("justin.ketterman", "Far1on1~Far1on1~", "prua");
            exchange.Url = new Uri("https://email.bhhsall.com/EWS/Exchange.asmx");

            EmailMessage msg = new EmailMessage(exchange);
            msg.Subject = subject;
            msg.Body = message;
            msg.ToRecipients.Add(email);

            try
            {
                msg.SendAndSaveCopy();
                _logger.LogInformation("Message sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
