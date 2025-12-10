/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.DeviceUsage;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceUsageController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IDeviceUsageService service = null;

        public DeviceUsageController(IDeviceUsageService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("deviceUsageAgents")]
        public ActionResult<DeviceUsageAgents> EnumerateDeviceUsageAgents()
        {
            return service.EnumerateDeviceUsageAgents();
        }

        [HttpGet("deviceUsageAgents/{agentId}")]
        public ActionResult<DeviceUsageAgent> GetDeviceUsageAgent(string agentId)
        {
            return service.GetDeviceUsageAgent(agentId);
        }

        [HttpGet("deviceUsageAgents/{agentId}/lifetimeCounters")]
        public ActionResult<LifetimeCounters> GetLifetimeCounters(string agentId)
        {
            return service.GetLifetimeCounters(agentId);
        }
    }
}
