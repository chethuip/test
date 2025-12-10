/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Supplies;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ISuppliesService service = null;

        public SuppliesController(ISuppliesService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("suppliesAgents")]
        public ActionResult<SuppliesAgents> EnumerateSuppliesAgents()
        {
            return service.EnumerateSuppliesAgents();
        }

        [HttpGet("suppliesAgents/{agentId}")]
        public ActionResult<SuppliesAgent> GetSuppliesAgent(string agentId)
        {
            return service.GetSuppliesAgent(agentId);
        }

        [HttpGet("suppliesAgents/{agentId}/suppliesConfiguration")]
        public ActionResult<SuppliesConfiguration> GetSuppliesConfiguration(string agentId)
        {
            return service.GetSuppliesConfiguration(agentId);
        }

        [HttpGet("suppliesAgents/{agentId}/suppliesInfo")]
        public ActionResult<SuppliesInfo> GetSuppliesInfo(string agentId)
        {
            return service.GetSuppliesInfo(agentId);
        }

        [HttpGet("suppliesAgents/{agentId}/suppliesUsage")]
        public ActionResult<SuppliesUsage> GetSuppliesUsage(string agentId)
        {
            return service.GetSuppliesUsage(agentId);
        }
    }
}
