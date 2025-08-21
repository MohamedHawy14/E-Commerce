using E_Commerce.Core.DTO.Email;
using MimeKit;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var email = new MimeMessage
            {
                Subject = message.Subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content }
            };
            email.From.Add(new MailboxAddress("Bi3ly", _emailConfig.From));
            email.To.AddRange(message.To);

            using var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);

                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(email);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }

    }
}
