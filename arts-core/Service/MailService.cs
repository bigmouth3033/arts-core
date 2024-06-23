using arts_core.Models;
using arts_core.Setting;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
namespace arts_core.Service
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;

        public MailService(IOptions<MailSetting> mailSetting)
        {
            //IOption: doc json tu appsetting.json, parse vao model
            _mailSetting = mailSetting.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient(); //using MailKit.Net.Smtp;
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Email, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
