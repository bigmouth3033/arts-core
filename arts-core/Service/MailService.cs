using arts_core.RequestModels;
using arts_core.Setting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace arts_core.Service
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;
        private readonly IConfiguration _configuration;

        public MailService(IOptions<MailSetting> mailSetting, IConfiguration configuration)
        {
            //IOption: doc json tu appsetting.json, parse vao model
            _mailSetting = mailSetting.Value;
            _configuration = configuration;
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
            using var smtp = new SmtpClient(); 
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Email, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public void SendMail(MailRequestNhan request)
        {
            var Host = _configuration.GetSection("MailSettings:Host").Value;
            var Port = _configuration.GetSection("MailSettings:Port").Value;
            var MailFrom = _configuration.GetSection("MailSettings:Mail").Value;
            var Password = _configuration.GetSection("MailSettings:Password").Value;
            var DisplayName = _configuration.GetSection("MailSettings:DisplayName").Value;


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(MailFrom));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(Host, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(MailFrom, Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        } 
    }


    public class MailRequestNhan
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public MailRequestNhan(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
