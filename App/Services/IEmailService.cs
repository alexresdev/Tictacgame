using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailTo, string subject, string message);
    }
}