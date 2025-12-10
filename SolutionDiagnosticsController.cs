/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.SolutionDiagnostics;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;
using System;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionDiagnosticsController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ISolutionDiagnosticsService service = null;

        public SolutionDiagnosticsController(ISolutionDiagnosticsService solutionDiagnosticsService)
        {
            this.service = solutionDiagnosticsService;
        }

        [HttpGet("capabilities")]
        [Consumes("application/json")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("solutionDiagnosticsAgents")]
        public ActionResult<SolutionDiagnosticsAgents> EnumerateSolutionDiagnosticsAgents()
        {
            return service.EnumerateSolutionDiagnosticsAgents();
        }

        [HttpGet("solutionDiagnosticsAgents/{agentId}")]
        public ActionResult<SolutionDiagnosticsAgent> GetSolutionDiagnosticsAgent(string agentId)
        {
            return service.GetSolutionDiagnosticsAgent(agentId);
        }

        [HttpGet("solutionDiagnosticsAgents/{agentId}/log")]
        public ActionResult<Tuple<Log, byte[]>> GetAgentLog(string agentId)
        {
            return service.GetAgentLog(agentId);
        }
    }
}
