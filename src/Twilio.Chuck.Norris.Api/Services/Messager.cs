using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twilio.Chuck.Norris.Api.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Twilio.Chuck.Norris.Api.Services
{
    public class Messager : IMessager
    {
        private readonly TwilioOptions _twilioOptions;

        public Messager(IOptions<TwilioOptions> options)
        {
            _twilioOptions = options.Value;
        }

        public async Task<bool> SendSms(MessageViewModel vm)
        {
            var accountSid = _twilioOptions.AccountSid;
            var authToken = _twilioOptions.AuthToken;

            TwilioClient.Init(accountSid, authToken);
            var phoneNumber = new PhoneNumber(vm.To);
            var message = vm.Body;


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
    }
}
