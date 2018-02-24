using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Core;
using Twilio.Chuck.Norris.Api.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace Twilio.Chuck.Norris.Api.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : TwilioController
    {
        
        private readonly TwilioOptions _twilioOptions;

        public MessageController(IOptions<TwilioOptions> options)
        {
            _twilioOptions = options.Value;
        }
        
        [HttpPost]
        public async Task Post([FromBody] MessageModel messageModel)
        {
            var accountSid =  _twilioOptions.AccountSid;
            var authToken = _twilioOptions.AuthToken;

            TwilioClient.Init(accountSid, authToken);
            var phoneNumber = new PhoneNumber(messageModel.To);
            var message = messageModel.Body;


            try
            {
                await MessageResource.CreateAsync(
                    to: phoneNumber,
                    from: new PhoneNumber(_twilioOptions.PhoneNumber),
                    body: message);

            }
            catch (Exception exception)
            {
                Console.Write(exception);
            }
            

        }

        public class MessageModel
        {
            public string To { get; set; }
            public string Body { get; set; }
        }
    }
}
