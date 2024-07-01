using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace arts_core.Service
{
    public interface IMailService
    {
        void SendMail(MailRequest request);
    }
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendMail(MailRequest request)
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
    public class MailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public MailRequest(string to,string subject,string body)
        {
            To = to; 
            Subject = subject; 
            Body = body;
        }
    }
}
