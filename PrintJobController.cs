/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.PrintJob;
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintJobController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IPrintJobService service = null;

        public PrintJobController(IPrintJobService service)
        {
            this.service = service;
        }

        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("printJobAgents")]
        public ActionResult<PrintJobAgents> EnumeratePrintJobAgents()
        {
            return service.EnumeratePrintJobAgents();
        }

        [HttpGet("printJobAgents/{agentId}")]
        public ActionResult<PrintJobAgent> GetPrintJobAgent(string agentId)
        {
            return service.GetPrintJobAgent(agentId);
        }
    }
}
