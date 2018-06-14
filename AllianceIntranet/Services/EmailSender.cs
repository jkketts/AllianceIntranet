using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Logging;
using RazorLight;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AllianceIntranet.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IRazorLightEngine _engine;

        public EmailSender(
            ILogger<EmailSender> logger,
            IRazorLightEngine engine)
        {
            _logger = logger;
            _engine = engine;
        }

        public void SendEmail(string email, string subject, string message)
        {
            var exchange = new ExchangeService();
            var userName = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
            var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            var domain = Environment.GetEnvironmentVariable("EMAIL_DOMAIN");
            exchange.Credentials = new WebCredentials(userName, password, domain);
            exchange.Url = new Uri("https://email.bhhsall.com/EWS/Exchange.asmx");

            EmailMessage msg = new EmailMessage(exchange)
            {
                Subject = subject,
                Body = message
            };
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

        public async void SendEmailAsync(string email, string subject, string emailTemplateFileName, object model)
        {
            string resultFromFile;
            try
            {
                resultFromFile = await _engine.CompileRenderAsync(emailTemplateFileName, model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"The email template does not exist, {ex}");
                return;
            }
            
            var exchange = new ExchangeService();
            var userName = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
            var password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            var domain = Environment.GetEnvironmentVariable("EMAIL_DOMAIN");
            exchange.Credentials = new WebCredentials(userName, password, domain);
            exchange.Url = new Uri("https://email.bhhsall.com/EWS/Exchange.asmx");

            EmailMessage msg = new EmailMessage(exchange)
            {
                Subject = subject,
                Body = resultFromFile
            };
            msg.ToRecipients.Add(email);

            try
            {
                await msg.SendAndSaveCopy();
                _logger.LogInformation("Message sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        
    }
}
