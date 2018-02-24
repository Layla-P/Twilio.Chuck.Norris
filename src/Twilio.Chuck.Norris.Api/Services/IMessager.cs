using System.Threading.Tasks;
using Twilio.Chuck.Norris.Api.Models;

namespace Twilio.Chuck.Norris.Api.Services
{
    public interface IMessager
    {
        Task<bool> SendSms(MessageViewModel meesage);
    }
}
