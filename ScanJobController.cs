/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.ScanJob;
using HP.Extensibility.Types.OptionProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OXPd2ExamplesHost.Services;
using OXPd2ExamplesHost.Utilities;
using Microsoft.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanJobController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IScanJobService service = null;

        public ScanJobController(IScanJobService service)
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

        [HttpGet("scanJobAgents")]
        public ActionResult<ScanJobAgents> EnumerateScanJobAgents()
        {
            return service.EnumerateScanJobAgents();
        }

        [HttpGet("scanJobAgents/{agentId}")]
        public ActionResult<ScanJobAgent> GetScanJobAgent(string agentId)
        {
            return service.GetScanJobAgent(agentId);
        }

        [HttpGet("scanJobAgents/{agentId}/scanJobs")]
        public ActionResult<ScanJobs> EnumerateScanJobs(string agentId)
        {
            return service.EnumerateScanJobs(agentId);
        }

        [HttpPost("scanJobAgents/{agentId}/scanJobs")]
        public ActionResult<ScanJob> CreateScanJob(string agentId, ScanJob_Create scanJob_Create)
        {

            return service.CreateScanJob(agentId, scanJob_Create);
        }

        [HttpGet("scanJobAgents/{agentId}/scanJobs/{scanJobId}")]
        public ActionResult<ScanJob> GetScanJob(string agentId, string scanJobId)
        {
            return service.GetScanJob(agentId, scanJobId);
        }

        [HttpPost("scanJobAgents/{agentId}/scanJobs/{scanJobId}/cancel")]
        public ActionResult<ScanJob_Cancel> CancelScanJob(string agentId, string scanJobId)
        {
            return service.CancelScanJob(agentId, scanJobId);
        }

        [HttpGet("profile")]
        public ActionResult<Profile> GetProfile()
        {
            return service.GetProfile();
        }

        [HttpPost("scanTicketHelper")]
        public ActionResult<List<OptionRuleNotification>> VerifyScanTicket(ScanOptions scanOptions)
        {
            return service.VerifyScanTicket(scanOptions);
        }
    }
}
