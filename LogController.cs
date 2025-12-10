/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using Microsoft.AspNetCore.Mvc;
using OXPd2ExamplesHost.Services;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : Controller
    {
        private ILogService service;

        public LogController(ILogService service)
        {
            this.service = service;
        }

        [HttpGet("{logName}")]
        public IActionResult GetLog(string logName)
        {
            if (logName != null)
            {
                return Ok(service.GetLog(logName));
            }
            else return BadRequest();
        }

        [HttpDelete("{logName}")]
        public IActionResult DeleteLog(string logName)
        {
            if (logName != null)
            {
                service.ClearLog(logName);
                return NoContent();
            }
            else return BadRequest();
        }
    }
}
