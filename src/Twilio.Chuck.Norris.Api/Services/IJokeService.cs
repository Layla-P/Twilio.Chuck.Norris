using System.Threading.Tasks;
using Twilio.Chuck.Norris.Api.Models;

namespace Twilio.Chuck.Norris.Api.Services
{
    public interface IJokeService
    {
        Task<ChuckNorrisResponse> GetJokeAsync(string message);
    }
}
