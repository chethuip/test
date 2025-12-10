/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using HP.Extensibility.Types.Authentication;
using HP.Extensibility.Types.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Models;
using OXPd2ExamplesHost.Services;
using System.IO;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationAgentController : Controller
    {
        // This is a very basic example of a solution implementation, see any applicable demos for more in depth examples.
        private IAuthenticationAgentService agentService = null;
        private readonly IDeviceManagementService deviceManagementService = null;
        private static string authenticationAgentLog = "authenticationAgent";
        private ILogService logService = null;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthenticationAgentController(IAuthenticationAgentService authenticationAgentService, IDeviceManagementService deviceManagementService,
            ILogService logService, IWebHostEnvironment webHostEnvironment)
        {
            agentService = authenticationAgentService;
            this.deviceManagementService = deviceManagementService;
            this.logService = logService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region AuthenticationAgent ServiceTarget Implementation
        [HttpPost("prePromptResult")]
        [Consumes("application/json")]
        public IActionResult PrePromptResult(PrePromptResultRequest request)
        {
            Token authContextToken = new Token()
            {
                AccessToken = request.SessionAccessToken,
                TokenType = "Bearer"
            };

            // Stash the Auth-Context session token with the our device manager for later use
            deviceManagementService.SetAuthContextAccessToken(authContextToken);

            //For prePromptResults, always return continue.
            PrePromptResult result = new PrePromptResult
            {
                Result = new PrePromptResultValue()
                {
                    Continue = new AuthenticationContinued()
                }
            };

            AuthenticationLogItem item = new AuthenticationLogItem("PrePromptResult", request, result);
            logService.Log(authenticationAgentLog, item);
            return Ok(result);
        }

        [HttpPost("postPromptResult")]
        public IActionResult PostPromptResult(PostPromptResultRequest request)
        {
            Token authContextToken = new Token()
            {
                AccessToken = request.SessionAccessToken,
                TokenType = "Bearer"
            };

            // Stash the Auth-Context session token with the our device manager for later use
            deviceManagementService.SetAuthContextAccessToken(authContextToken);

            PostPromptResult result = agentService.GetPostPromptResult();

            AuthenticationLogItem item = new AuthenticationLogItem("PostPromptResult", request, result);
            logService.Log(authenticationAgentLog, item);
            return Ok(result);
        }

        [ResponseCache(Duration = 0, NoStore = true)]
        [HttpGet("prompt")]
        public IActionResult Prompt()
        {
            AuthenticationLogItem item = new AuthenticationLogItem("Prompt", null, null);
            logService.Log(authenticationAgentLog, item);
            var file = Path.Combine(webHostEnvironment.ContentRootPath, "WebContent", "LoginPage.html");
            return PhysicalFile(file, "text/html");
        }

        [HttpPost("signoutNotification")]
        public IActionResult SignoutNotification(SignoutNotificationRequest request)
        {
            AuthenticationLogItem item = new AuthenticationLogItem("SignoutNotification", request, null);
            // The user has signed out, remove the authContextToken
            deviceManagementService.SetAuthContextAccessToken(null);

            logService.Log(authenticationAgentLog, item);
            return NoContent();
        }
        #endregion

        // The call the user prompt pages will make when the user presses the 'login' button.
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            PostPromptResult postPromptResult = new PostPromptResult();

            //This is the only success case.  All others are failure.
            if (login != null && login.Pin != null && login.Pin.Equals("1234"))
            {
                postPromptResult.Result = new PostPromptResultValue()
                {
                    Succeeded = GetAuthenticationSuccess()
                };
            }
            else
            {
                postPromptResult.Result = new PostPromptResultValue()
                {
                    Failed = new AuthenticationFailed()
                    {
                        Message = "Invalid PIN"
                    }
                };
            }
            agentService.SetPostPromptResult(postPromptResult);
            return NoContent();
        }

        // The call the user prompt pages will make when the user presses the 'cancel' button.
        [HttpPost("cancelLogin")]
        public IActionResult CancelLogin()
        {
            PostPromptResult postPromptResult = new PostPromptResult();
            postPromptResult.Result = new PostPromptResultValue()
            {
                Canceled = new AuthenticationCanceled()
            };
            agentService.SetPostPromptResult(postPromptResult);
            return NoContent();
        }
        private AuthenticationSuccess GetAuthenticationSuccess()
        {
            AuthenticationSuccess success = new AuthenticationSuccess()
            {
                Details = new WritableAuthenticatedUserDetails()
                {
                    DisplayName = "John Doe",
                    EmailAddress = "john.doe@hp.com",
                    FullyQualifiedUserName = "jDoe",
                    PreferredLanguage = "en-US",
                    UserName = "jDoe"
                }
            };
            return success;
        }
    }
}
