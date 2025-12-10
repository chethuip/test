/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;
using System.IO;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationAgentController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IDeviceManagementService deviceManagementService = null;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ApplicationAgentController(IDeviceManagementService deviceManagementService, IWebHostEnvironment webHostEnvironment)
        {
            this.deviceManagementService = deviceManagementService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("index.html")]
        [ResponseCache(Duration = 0, NoStore = true)]
        public IActionResult GetApplication()
        {
            // @StartCodeExample:GetApplication
            // By defining a custom-header in the Application Agent Target we are able to receive the
            // OXPd2 UI-Context session token in the initial application load request from the device.
            // The UI-Context access token provides exclusive access to certain Application Runtime
            // operations. The OXPd2 Example solution's Application Agent defines the custom header as:
            //
            //   {
            //      "headerName": "x-oxpd2-uiContext",
            //      "headerValue": {
            //         "expression": {
            //            "expressionPattern": "$SOLUTION_SESSION(UI_CONTEXT_TOKEN)$"
            //         }
            //      }
            //   }
            //
            // And the following code will look for this header and extract its value.

            if (Request.Headers.ContainsKey("x-oxpd2-uiContext"))
            {
                var token = Request.Headers["x-oxpd2-uiContext"];
                Token uiContextToken = new Token()
                {
                    AccessToken = token,
                    TokenType = "Bearer"
                };

                // Stash the UI-Context session token with the our device manager for later use
                deviceManagementService.SetUiContextAccessToken(uiContextToken);
            }

            // We can now continue on and return the application content in the response...
            // @EndCodeExample

            var file = Path.Combine(webHostEnvironment.ContentRootPath, "WebContent", "solutionApplicationExample.html");
            return PhysicalFile(file, "text/html");
        }
    }
}
