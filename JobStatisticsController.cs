/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.JobStatistics;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;
using System;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobStatisticsController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IJobStatisticsService service = null;

        public JobStatisticsController(IJobStatisticsService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("jobStatisticsAgents")]
        public ActionResult<JobStatisticsAgents> EnumerateJobStatisticsAgents()
        {
            return service.EnumerateJobStatisticsAgents();
        }

        [HttpGet("jobStatisticsAgents/{agentId}")]
        public ActionResult<JobStatisticsAgent> GetJobStatisticsAgent(string agentId)
        {
            return service.GetJobStatisticsAgent(agentId);
        }

        [HttpGet("jobStatisticsAgents/{agentId}/jobs")]
        public ActionResult<Jobs> EnumerateJobs(string agentId)
        {
            return service.EnumerateJobs(agentId);
        }

        [HttpPatch("jobStatisticsAgents/{agentId}/jobs")]
        public ActionResult<Jobs> ModifyJobs(string agentId, Jobs_Modify jobs_Modify)
        {
            return service.ModifyJobs(agentId, jobs_Modify);
        }

        [HttpGet("jobStatisticsAgents/{agentId}/jobs/{sequenceNumber}")]
        public ActionResult<Job> GetJob(string agentId, ulong sequenceNumber)
        {
            return service.GetJob(agentId, sequenceNumber);
        }
    }
}
