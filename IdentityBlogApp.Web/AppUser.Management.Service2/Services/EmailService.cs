using AppUser.Management.Service2.Configurations;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUser.Management.Service2.Models;
using MailKit.Net.Smtp;

namespace AppUser.Management.Service2.Services
{
    public class EMailService : IEmailService
    {
        //Dependency Injection
        private readonly EmailConfig _emailConfig;
        public EMailService(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(MailMessage mailMessage)
        {
            var emailMessage = CreateEmailMessage(mailMessage);
            Send(emailMessage);
        }

        private void Send(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(MailMessage mailMessage)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Ertan Osmanoğlu", _emailConfig.From));
            emailMessage.To.AddRange(mailMessage.To);
            emailMessage.Subject = mailMessage.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = mailMessage.Body };
            return emailMessage;

        }
    }
}
