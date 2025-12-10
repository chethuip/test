/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Copy;
using HP.Extensibility.Types.OptionProfile;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;
using System.Collections.Generic;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CopyController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ICopyService service = null;

        public CopyController(ICopyService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("defaultOptions")]
        public ActionResult<DefaultOptions> GetDefaultOptions()
        {
            return service.GetDefaultOptions();
        }

        [HttpGet("profile")]
        public ActionResult<Profile> GetProfile()
        {
            return service.GetProfile();
        }

        [HttpGet("copyAgents")]
        public ActionResult<CopyAgents> EnumerateCopyAgents()
        {
            return service.EnumerateCopyAgents();
        }

        [HttpGet("copyAgents/{agentId}")]
        public ActionResult<CopyAgent> GetCopyAgent(string agentId)
        {
            return service.GetCopyAgent(agentId);
        }

        [HttpGet("copyAgents/{agentId}/copyJobs")]
        public ActionResult<CopyJobs> EnumerateCopyJobs(string agentId)
        {
            return service.EnumerateCopyJobs(agentId);
        }

        [HttpPost("copyAgents/{agentId}/copyJobs")]
        public ActionResult<CopyJob> CreateCopyJob(string agentId, CopyJob_Create copyJob_Create)
        {

            return service.CreateCopyJob(agentId, copyJob_Create);
        }

        [HttpGet("copyAgents/{agentId}/copyJobs/{copyJobId}")]
        public ActionResult<CopyJob> GetCopyJob(string agentId, string copyJobId)
        {
            return service.GetCopyJob(agentId, copyJobId);
        }

        [HttpPost("copyAgents/{agentId}/copyJobs/{copyJobId}/cancel")]
        public ActionResult<CopyJob_Cancel> CancelCopyJob(string agentId, string copyJobId)
        {
            return service.CancelCopyJob(agentId, copyJobId);
        }

        [HttpPost("copyTicketHelper")]
        public ActionResult<List<OptionRuleNotification>> VerifyCopyTicket(CopyOptions copyOptions)
        {
            return service.VerifyCopyTicket(copyOptions);
        }
    }
}
