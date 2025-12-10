/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Security;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ISecurityService service = null;

        public SecurityController(ISecurityService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("securityAgents")]
        public ActionResult<SecurityAgents> EnumerateSecurityAgents()
        {
            return service.EnumerateSecurityAgents();
        }

        [HttpGet("securityAgents/{agentId}")]
        public ActionResult<SecurityAgent> GetSecurityAgent(string agentId)
        {
            return service.GetSecurityAgent(agentId);
        }

        [HttpPost("securityAgents/{agentId}/resolveSecurityExpression")]
        public ActionResult<SecurityAgent_ResolveSecurityExpression> ResolveSecurityExpression(string agentId, ResolveSecurityExpressionRequest request)
        {
            return service.ResolveSecurityExpression(agentId, request);
        }
    }
}
