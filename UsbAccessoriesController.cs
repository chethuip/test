/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.UsbAccessories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OXPd2ExamplesHost.Services;
using System.Web;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsbAccessoriesController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IUsbAccessoriesService service = null;

        public UsbAccessoriesController(IUsbAccessoriesService usbAccessoriesService)
        {
            this.service = usbAccessoriesService;
        }

        [HttpGet("capabilities")]
        [Consumes("application/json")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("usbAccessoriesAgents")]
        public ActionResult<UsbAccessoriesAgents> EnumerateUsbAccessoriesAgents()
        {
            return service.EnumerateUsbAccessoriesAgents();
        }

        [HttpGet("usbAccessoriesAgents/{agentId}")]
        public ActionResult<UsbAccessoriesAgent> GetUsbAccessoriesAgent(string agentId)
        {
            return service.GetUsbAccessoriesAgent(agentId);
        }

        [HttpGet("accessories")]
        public ActionResult<Accessories> EnumerateUsbAccessories()
        {
            return service.EnumerateUsbAccessories();
        }

        [HttpGet("accessories/{accessoryId}")]
        public ActionResult<Accessory> GetUsbAccessory(string accessoryId)
        {
            return service.GetUsbAccessory(accessoryId);
        }

        [HttpGet("accessories/{accessoryId}/hid")]
        public ActionResult<Hid> GetUsbAccessoryHid(string accessoryId)
        {
            return service.GetUsbAccessoryHid(accessoryId);
        }

        [HttpPost("accessories/{accessoryId}/hid/open")]
        public ActionResult<Hid_Open> OpenAccessoryHid(string accessoryId, Accessories_Accessory_Hid_Open_Params hidOpenParams)
        {

            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                if (isOwned)
                {
                    return service.OpenOwnedAccessoryHid(accessoryId, hidOpenParams);
                }
            }

            return service.OpenSharedAccessoryHid(accessoryId, hidOpenParams);
        }

        [HttpGet("accessories/{accessoryID}/hid/{openHIDAccessoryID}")]
        public ActionResult<OpenHIDAccessory> GetOpenAccessoryHid(string accessoryID, string openHIDAccessoryID)
        {
            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                return service.GetOpenHIDAccessory(accessoryID, openHIDAccessoryID, isOwned);
            }
            return null;
        }

        [HttpDelete("accessories/{accessoryID}/hid/{openHIDAccessoryID}")]
        public ActionResult DeleteOpenAccessoryHid(string accessoryID, string openHIDAccessoryID)
        {
            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                service.DeleteOpenHIDAccessory(accessoryID, openHIDAccessoryID, isOwned);
                return NoContent();
            }
            return null;
        }

        [HttpPatch("accessories/{accessoryID}/hid/{openHIDAccessoryID}")]
        public ActionResult<OpenHIDAccessory> ModifyOpenAccessoryHid(string accessoryID, string openHIDAccessoryID, [FromBody] OpenHIDAccessory_Modify modifyRequest)
        {
            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                return service.ModifyOpenHIDAccessory(accessoryID, openHIDAccessoryID, modifyRequest, isOwned);
            }
            return null;
        }

        [HttpPost("accessories/{accessoryID}/hid/{openHIDAccessoryID}/readReport")]
        public ActionResult<OpenHIDAccessory_ReadReport> ReadReportOpenAccessoryHid(string accessoryID, string openHIDAccessoryID, [FromBody] Accessories_Accessory_Hid_OpenHIDAccessory_ReadReport_Params readReportParams)
        {
            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                return service.ReadReportOpenHIDAccessory(accessoryID, openHIDAccessoryID, readReportParams, isOwned);
            }
            return null;
        }

        [HttpPost("accessories/{accessoryID}/hid/{openHIDAccessoryID}/writeReport")]
        public ActionResult<OpenHIDAccessory_WriteReport> WriteReportOpenAccessoryHid(string accessoryID, string openHIDAccessoryID, [FromBody] Accessories_Accessory_Hid_OpenHIDAccessory_WriteReport_Params writeReportParams)
        {
            if (Request.QueryString.HasValue)
            {
                var query = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var isOwned = query["isOwned"] != null ? bool.Parse(query["isOwned"]) : false;
                return service.WriteReportOpenHIDAccessory(accessoryID, openHIDAccessoryID, writeReportParams, isOwned);
            }
            return null;
        }
    }
}
