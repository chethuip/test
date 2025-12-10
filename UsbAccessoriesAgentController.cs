/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.UsbAccessories;
using HP.Extensibility.Types.Target;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsbAccessoriesAgentController : Controller
    {
        private IUsbAccessoriesAgentService usbAccessoriesAgentService;

        public UsbAccessoriesAgentController(IUsbAccessoriesAgentService service)
        {
            usbAccessoriesAgentService = service;
        }
       
        // Registration target defined in the solution manifest.
        // This end-point will be called when solution owned USB accessories
        // (as described in the solution manifest file) are attached or detached from the device.
        [HttpPost("registration")]
        [Consumes("application/json")]
        public IActionResult RegistrationTarget([FromBody] HttpPayloadWrapper payload)
        {
            foreach (JObject payloadItemJson in payload.Payloads)
            {
                UsbRegistrationPayload payloadItem = payloadItemJson.ToObject<UsbRegistrationPayload>();
                usbAccessoriesAgentService.HandleUsbRegistrationNotification(payloadItem);
            }
            return Ok();
        }

        // AsyncIOTarget defined when opening the accessory.
        // This end-point will be called when an async read or write occurs to the open accessory
        // and when the accessory is closed.
        [HttpPost("asyncio")]
        [Consumes("application/json")]
        public IActionResult AsyncIOTarget(HttpPayloadWrapper payload)
        {
            foreach (JObject payloadItemJson in payload.Payloads)
            {
                UsbCallbackEnvelope payloadItem = payloadItemJson.ToObject<UsbCallbackEnvelope>();
                usbAccessoriesAgentService.HandleUsbCallback(payloadItem);
            }
            return Ok();
        }
    }
}
