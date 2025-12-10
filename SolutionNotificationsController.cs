/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Types.SolutionManager;
using HP.Extensibility.Types.Target;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SolutionNotificationsController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ISolutionNotificationsService solutionNotificationsService = null;

        public SolutionNotificationsController(ISolutionNotificationsService solutionNotificationService)
        {
            this.solutionNotificationsService = solutionNotificationService;
        }


        [HttpPost]
        public IActionResult SolutionNotification(HttpPayloadWrapper payload)
        {
            // @StartCodeExample:PostSolutionNotification
            foreach (JObject payloadItemJson in payload.Payloads)
            {
                SolutionNotification solutionNotification = payloadItemJson.ToObject<SolutionNotification>();
                solutionNotificationsService.HandleNotification(solutionNotification);
            }
            // @EndCodeExample

            return Ok();
        }
    }
}
