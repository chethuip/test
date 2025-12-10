using HP.Extensibility.Service.Device;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private IDeviceService service = null;
        public DeviceController(IDeviceService service)
        {
            this.service = service;
        }
        private DeviceController() { }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("identity")]
        public ActionResult<Identity> GetIdentity()
        {
            return service.GetIdentity();
        }

        [HttpGet("status")]
        public ActionResult<Status> GetStatus()
        {
            return service.GetStatus();
        }
        
        [HttpGet("email")]
        public ActionResult<Email> GetEmail()
        {
            return service.GetEmail();
        }

        [HttpGet("scanner")]
        public ActionResult<Scanner> GetScanner()
        {
            return service.GetScanner();
        }

        [HttpGet("deploymentInformation")]
        public ActionResult<DeploymentInformation> GetDeploymentInformation()
        {
            return service.GetDeploymentInformation();
        }
    }
}
