/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Authentication;
using HP.Extensibility.Types.Authentication;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService service = null;

        public AuthenticationController(IAuthenticationService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpPost("authenticationAgents/{authenticationAgentId}/login")]
        public ActionResult<AuthenticationAgent_Login> AthenticationAgentLogin(string authenticationAgentId, PrePromptResult prePrompt)
        {
            return service.AuthenticationAgentLogin(authenticationAgentId, prePrompt);
        }

        [HttpGet("authenticationAgents/{authenticationAgentId}")]
        public ActionResult<AuthenticationAgent> GetAuthenticationAgent(string authenticationAgentId)
        {
            return service.GetAuthenticationAgent(authenticationAgentId);
        }

        [HttpGet("authenticationAgents")]
        public ActionResult<AuthenticationAgents> EnumerateAuthenticationAgents()
        {
            return service.EnumerateAuthenticationAgents();
        }

        [HttpPost("authenticationAccessPoints/{authenticationAccessPointid}/initiateLogin")]
        public ActionResult<AuthenticationAccessPoint_InitiateLogin> AuthenticationAccessPointInitiateLogin(string authenticationAccessPointId)
        {
            return service.AuthenticationAccessPointInitiateLogin(authenticationAccessPointId);
        }

        [HttpGet("authenticationAccessPoints/{authenticationAccessPointid}")]
        public ActionResult<AuthenticationAccessPoint> GetAuthenticationAccessPoint(string authenticationAccessPointId)
        {
            return service.GetAuthenticationAccessPoint(authenticationAccessPointId);
        }

        [HttpGet("authenticationAccessPoints")]
        public ActionResult<AuthenticationAccessPoints> EnumerateAuthenticationAccessPoints()
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateAuthenticationAccessPoints(query);
        }

        [HttpPost("session/forceLogout")]
        public ActionResult<Session_ForceLogout> SessionForceLogout()
        {
            return service.SessionForceLogout();
        }
    }
}
