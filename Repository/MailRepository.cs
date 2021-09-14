using Contracts;
using Entities.Models.QueryParameters;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MailRepository : IMailRepository
    {
        EmailSettings _emailSettings = null;

        public MailRepository(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public bool SendEmail(EmailData emailData)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId));

                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = emailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);

                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }

        public async Task SendEmailAsync()
        {
            var message = new MimeMessage();

            //add the sender info that will appear in the email message
            var emailAddress = "amstrong@aun.bj";
            var password = "à retrouver";

            message.From.Add(new MailboxAddress("Tester",emailAddress));

            //add the receiver email address
            message.To.Add(MailboxAddress.Parse("stephane.adjakotan@gmail.com"));

            //add the message subject
            message.Subject = "le premier";

            message.Body = new TextPart("plain")
            {
                Text = @"Yes,
                Hello!.
                you are fantastique Stéphane"
            };

            SmtpClient client = new SmtpClient();

            await client.ConnectAsync("mail.aun.bj", 456, true);
            await client.AuthenticateAsync(emailAddress, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();



        }
    }
}

