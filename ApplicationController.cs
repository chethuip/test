/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Application;
using HP.Extensibility.Types.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OXPd2ExamplesHost.Services;
using OXPd2ExamplesHost.Utilities;
using System.IO;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IApplicationService service = null;

        public ApplicationController(IApplicationService applicationService)
        {
            this.service = applicationService;
        }

        [HttpGet("capabilities")]
        [Consumes("application/json")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("applicationRuntime")]
        [Consumes("application/json")]
        public ActionResult<ApplicationRuntime> GetApplicationRuntime()
        {
            return service.GetApplicationRuntime();
        }

        [HttpPost("applicationRuntime/reset")]
        [Consumes("application/json")]
        public ActionResult<ApplicationRuntime_Reset> ResetApplicationRuntime(ResetRequest resetRequest)
        {
            return service.ResetApplicationRuntime(resetRequest);
        }

        [HttpGet("applicationRuntime/currentContext")]
        [Consumes("application/json")]
        public ActionResult<CurrentContext> GetCurrentContext()
        {
            return service.GetCurrentContext();
        }

        [HttpPost("applicationRuntime/currentContext/resetInactivityTimer")]
        [Consumes("application/json")]
        public ActionResult<CurrentContext_ResetInactivityTimer> ResetInactivityTimerCurrentContext(ResetInactivityTimerRequest resetRequest)
        {
            return service.ResetInactivityTimerCurrentContext(resetRequest);
        }

        [HttpPost("applicationRuntime/currentContext/exit")]
        [Consumes("application/json")]
        public ActionResult<CurrentContext_Exit> ExitCurrentContext(ExitRequest exitRequest)
        {
            return service.ExitCurrentContext(exitRequest);
        }

        [HttpGet("applicationRuntime/currentContext/runtimeChrome")]
        [Consumes("application/json")]
        public ActionResult<RuntimeChrome> GetRuntimeChrome()
        {
            return service.GetRuntimeChrome();
        }

        [HttpPut("applicationRuntime/currentContext/runtimeChrome")]
        [Consumes("application/json")]
        public ActionResult<RuntimeChrome> ReplaceRuntimeChrome(RuntimeChrome_Replace runtimeChromeReplace)
        {
            return service.ReplaceRuntimeChrome(runtimeChromeReplace);
        }

        [HttpPatch("applicationRuntime/currentContext/runtimeChrome")]
        [Consumes("application/json")]
        public ActionResult<RuntimeChrome> ModifyRuntimeChrome(RuntimeChrome_Modify runtimeChromeModify)
        {
            return service.ModifyRuntimeChrome(runtimeChromeModify);
        }

        [HttpGet("applicationRuntime/currentContext/startIntent")]
        [Consumes("application/json")]
        public ActionResult<StartIntent> GetStartIntent()
        {
            return service.GetStartIntent();
        }

        [HttpGet("applicationAgents")]
        [Consumes("application/json")]
        public ActionResult<ApplicationAgents> EnumerateApplicationAgents()
        {
            return service.EnumerateApplicationAgents();
        }

        [HttpGet("applicationAgents/{applicationId}")]
        [Consumes("application/json")]
        public ActionResult<ApplicationAgent> GetApplicationAgent(string applicationId)
        {
            return service.GetApplicationAgent(applicationId);
        }

        [HttpPost("applicationAgents/{applicationId}/refresh")]
        [Consumes("application/json")]
        public ActionResult<ApplicationAgent_Refresh> RefreshApplicationAgent(RefreshRequest refreshRequest, string applicationId)
        {
            return service.RefreshApplicationAgent(refreshRequest, applicationId);
        }

        [HttpGet("i18nAssets")]
        [Consumes("application/json")]
        public ActionResult<I18nAssets> EnumerateI18nAssets()
        {
            return service.EnumerateI18nAssets();
        }

        [HttpGet("i18nAssets/{i18nAssetId}")]
        [Consumes("application/json")]
        public ActionResult<I18nAsset> GetI18nAsset(string i18nAssetId)
        {
            return service.GetI18nAsset(i18nAssetId);
        }

        [HttpGet("applicationAccessPoints")]
        [Consumes("application/json")]
        public ActionResult<ApplicationAccessPoints> EnumerateApplicationAccessPoints()
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateApplicationAccessPoints(query);
        }

        [HttpGet("applicationAccessPoints/{accessPointId}")]
        [Consumes("application/json")]
        public ActionResult<ApplicationAccessPoint> GetApplicationAccessPoints(string accessPointId)
        {
            return service.GetApplicationAccessPoint(accessPointId);
        }

        [HttpPost("applicationAccessPoints/{accessPointId}/initiateLaunch")]
        public ActionResult<ApplicationAccessPoint_InitiateLaunch> ApplicationAccessPointInitiateLaunch(string accessPointId)
        {
            dynamic startIntent = null;
            InitiateLaunchRequest initiateLaunchRequest = null;

            // Request is not a multipart request
            if (false == MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                // Ensure we got the right content-type
                if (Request.ContentType != "application/json")
                {
                    return BadRequest();
                }

                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    var body = stream.ReadToEndAsync().Result;
                    initiateLaunchRequest = JsonConvert.DeserializeObject<InitiateLaunchRequest>(body);
                }
            }
            else
            {
                // Process the  multipart request
                var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

                var reader = new MultipartReader(boundary, HttpContext.Request.Body);

                var section = reader.ReadNextSectionAsync().Result;
                while (section != null)
                {
                    ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                    MediaTypeHeaderValue mt = MediaTypeHeaderValue.Parse(section.ContentType);

                    if (null != cd && null != mt)
                    {
                        // Ensure we got the right content-type. Both content and startIntent should be application/json
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }

                        if (cd.Name == "content")
                        {
                            // Create the InitiateLaunch arg
                            using (var streamReader = new StreamReader(
                                       section.Body, System.Text.Encoding.UTF8,
                                       detectEncodingFromByteOrderMarks: false,
                                       bufferSize: 1024,
                                       leaveOpen: true))
                            {
                                // The value length limit is enforced by
                                // MultipartBodyLengthLimit
                                var textValue = streamReader.ReadToEndAsync().Result;
                                initiateLaunchRequest = JsonConvert.DeserializeObject<InitiateLaunchRequest>(textValue);
                            }
                        }
                        else if (cd.Name == "startIntent")
                        {
                            // Create the startIntent object
                            using (var streamReader = new StreamReader(
                                      section.Body, System.Text.Encoding.UTF8,
                                      detectEncodingFromByteOrderMarks: false,
                                      bufferSize: 1024,
                                      leaveOpen: true))
                            {
                                // The value length limit is enforced by
                                // MultipartBodyLengthLimit
                                var textValue = streamReader.ReadToEndAsync().Result;
                                startIntent = JsonConvert.DeserializeObject(textValue);
                            }
                        }
                    }

                    // Drain any remaining section body that hasn't been consumed and
                    // read the headers for the next section.
                    section = reader.ReadNextSectionAsync().Result;
                }
            }
            return service.ApplicationAccessPointInitiateLaunch(accessPointId, initiateLaunchRequest, startIntent);
        }

        [HttpGet("messageCenterAgents")]
        [Consumes("application/json")]
        public ActionResult<MessageCenterAgents> EnumerateMessageCenterAgents()
        {
            return service.EnumerateMessageCenterAgents();
        }

        [HttpGet("messageCenterAgents/{agentId}")]
        [Consumes("application/json")]
        public ActionResult<MessageCenterAgent> GetMessageCenterAgent(string agentId)
        {
            return service.GetMessageCenterAgent(agentId);
        }

        [HttpGet("messageCenterAgents/{agentId}/messages")]
        [Consumes("application/json")]
        public ActionResult<Messages> EnumerateMessages(string agentId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateMessages(agentId, query);
        }

        [HttpPost("messageCenterAgents/{agentId}/messages")]
        [Consumes("application/json")]
        public ActionResult<Message> CreateMessage(string agentId, Message_Create messageCreate)
        {
            return service.CreateMessage(agentId, messageCreate);
        }

        [HttpGet("messageCenterAgents/{agentId}/messages/{messageId}")]
        [Consumes("application/json")]
        public ActionResult<Message> GetMessage(string agentId, string messageId)
        {
            return service.GetMessage(agentId, messageId);
        }

        [HttpDelete("messageCenterAgents/{agentId}/messages/{messageId}")]
        [Consumes("application/json")]
        public ActionResult<DeleteContent> DeleteMessage(string agentId, string messageId)
        {
            return service.DeleteMessage(agentId, messageId);
        }
    }
}
