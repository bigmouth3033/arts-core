using arts_core.Models;

namespace arts_core.Service
{
   public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
