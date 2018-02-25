using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Core;
using Twilio.Chuck.Norris.Api.Models;
using Twilio.Chuck.Norris.Api.Services;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace Twilio.Chuck.Norris.Api.Controllers
{
    [Route("api/[controller]")]
    public class MessageResponseController : TwilioController
    {
        
        private readonly IMessager _messager;

        public MessageResponseController(IMessager messager)
        {
            _messager = messager ?? throw new ArgumentException(nameof(messager));
        }

        [HttpPost]
        public async Task<TwiMLResult> Post(TwimlResponseMessage response)
        {
            var messagingResponse = await
                _messager.SendSmsResponse(response.Body)
                    .ConfigureAwait(false);

            return TwiML(messagingResponse);
        }
    }
}
