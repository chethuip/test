/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Copy;
using HP.Extensibility.Types.Target;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CopyAgentController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ICopyAgentService copyAgentService = null;

        public CopyAgentController(ICopyAgentService copyAgentService)
        {
            this.copyAgentService = copyAgentService;
        }

        [Consumes("application/json")]
        [HttpPost("copyJobNotification")]
        public IActionResult CopyJobNotification([FromBody] HttpPayloadWrapper payload)
        {
            foreach (JObject payloadItemJson in payload.Payloads)
            {
                CopyNotification notification = payloadItemJson.ToObject<CopyNotification>();
                copyAgentService.HandleNotification(notification);
            }
            return Ok();
        }
    }
}
