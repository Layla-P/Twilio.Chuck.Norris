using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twilio.Chuck.Norris.Api.Enums;
using Twilio.Chuck.Norris.Api.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace Twilio.Chuck.Norris.Api.Services
{
    public class Messager : IMessager
    {
        private readonly TwilioOptions _twilioOptions;
        private readonly IJokeService _jokeService;

        public Messager(IOptions<TwilioOptions> options, IJokeService jokeService)
        {
            _twilioOptions = options.Value;
            _jokeService = jokeService ?? throw new ArgumentNullException(nameof(jokeService));
        }


        public async Task<bool> SendSms(MessageViewModel vm)
        {
            var accountSid = _twilioOptions.AccountSid;
            var authToken = _twilioOptions.AuthToken;

            TwilioClient.Init(accountSid, authToken);
            var phoneNumber = new PhoneNumber(vm.To);
            var message = "Want some Chuck Norris?  Please respond with one of the following categories: Random, " + string.Join(", ", Enum.GetNames(typeof(JokeCategory)));


            try
            {
                await MessageResource.CreateAsync(
                    to: phoneNumber,
                    from: new PhoneNumber(_twilioOptions.PhoneNumber),
                    body: message);

                return true;

            }
            catch (Exception exception)
            {
                LogHelper.Error($"MessageController:Post - CreateAsync Exceptioned with {exception.Message}");
            }

            return false;
        }
        public async Task<MessagingResponse> SendSmsResponse(string message)
        {
            var joke = await GetJoke(message).ConfigureAwait(false);
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message(joke);

           
            return await Task.FromResult(messagingResponse);
        }

        private async Task<string> GetJoke(string message)
        {
            var joke = await
                _jokeService.GetJokeAsync(message)
                .ConfigureAwait(false);

            return joke.Value;
        }

        
    }
}
