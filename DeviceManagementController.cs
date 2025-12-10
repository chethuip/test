/***********************************************************
* (C) Copyright 2021 HP Development Company, L.P.
* All rights reserved.
***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Models;
using OXPd2ExamplesHost.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OXPd2ExamplesHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceManagementController : ControllerBase
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IDeviceManagementService service = null;

        public DeviceManagementController(IDeviceManagementService service)
        {
            this.service = service;
        }

        [HttpPost("bindDevice")]
        [Consumes("application/json")]
        public ActionResult<DeviceModel> BindDevice(BindDeviceRequest request)
        {
            return service.BindDevice(request.NetworkAddress);
        }

        [HttpPost("unbindDevice")]
        [Consumes("application/json")]
        public ActionResult<DeviceModel> UnbindDevice()
        {
            return service.UnbindDevice();
        }

        [HttpGet("device")]
        [Consumes("application/json")]
        public ActionResult<DeviceModel> Device()
        {
            return service.CurrentDevice;
        }

        [HttpPost("device/passwordGrant")]
        [Consumes("application/json")]
        public ActionResult<Token> PasswordGrant(PasswordGrantRequest request)
        {
            return service.PasswordGrant(request.Username, request.Password);
        }

        [HttpGet("device/servicesDiscovery")]
        [Consumes("application/json")]
        public object ServicesDiscovery()
        {
            return service.GetServicesDiscovery();
        }

        [HttpGet("device/deviceInfo")]
        [Consumes("application/json")]
        public async Task<object> DeviceInfo()
        {
            return await service.GetDeviceInformation();
        }

        [HttpGet("device/tokens")]
        [Consumes("application/json")]
        public IEnumerable<AccessTokenInfo> Tokens()
        {
            return service.GetTokens();
        }
    }
}
