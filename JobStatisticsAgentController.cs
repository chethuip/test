/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.JobStatistics;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class JobStatisticsAgentController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IJobStatisticsAgentService jobStatisticsAgentService = null;

        public JobStatisticsAgentController(IJobStatisticsAgentService jobStatisticsAgentService)
        {
            this.jobStatisticsAgentService = jobStatisticsAgentService;
        }

        [Consumes("application/json")]
        [HttpPost("jobStatistics")]
        public ActionResult<StatisticsCallbackResponse> JobStatisticsNotification([FromBody] StatisticsCallbackPayload notification)
        {
            return jobStatisticsAgentService.HandleNotification(notification);
        }
    }
}
