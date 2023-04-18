using SocialReview.BLL.EmailSender.Interfaces;
using System.Net.Mail;
using System.Net;

namespace SocialReview.BLL.EmailSender.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _fromAddress;
        private readonly string _fromName;
        private readonly string _fromPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public SmtpEmailSender(string fromAddress, string fromName, string fromPassword, string smtpHost, int smtpPort)
        {
            _fromAddress = fromAddress;
            _fromName = fromName;
            _fromPassword = fromPassword;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(_fromAddress, _fromName);
            var toAddress = new MailAddress(recipientEmail);

            var smtp = new SmtpClient
            {
                Host = _smtpHost,
                Port = _smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}
