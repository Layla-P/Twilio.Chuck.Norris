using System.Threading.Tasks;
using Twilio.Chuck.Norris.Api.Models;
using Twilio.TwiML;

namespace Twilio.Chuck.Norris.Api.Services
{
    public interface IMessager
    {
        Task<bool> SendSms(MessageViewModel message);
        Task<MessagingResponse> SendSmsResponse(string message);
    }
}
