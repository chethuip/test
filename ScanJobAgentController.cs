/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.ScanJob;
using HP.Extensibility.Types.Target;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OXPd2ExamplesHost.Services;
using OXPd2ExamplesHost.Utilities;
using System;
using System.IO;

namespace OXPd2ExamplesHost.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ScanJobAgentController : Controller
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private IScanJobAgentService scanJobAgentService = null;

        public ScanJobAgentController(IScanJobAgentService scanJobAgentService)
        {
            this.scanJobAgentService = scanJobAgentService;
        }

        [Consumes("application/json")]
        [HttpPost("scanJobNotification")]
        public IActionResult ScanJobNotification([FromBody] HttpPayloadWrapper payload)
        {
            foreach (JObject payloadItemJson in payload.Payloads)
            {
                ScanNotification notification = payloadItemJson.ToObject<ScanNotification>();
                scanJobAgentService.HandleNotification(notification);
            }
            return Ok();
        }

        [HttpPost("scanJobReceiver")]
        public IActionResult ScanJobReceiver()
        {
            ServiceTargetContent scanJobReceived = new ServiceTargetContent();
            Stream fileStream;

            // Request is not a multipart request, read the body to get the notification
            if (false == MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                // Ensure we got the right content-type
                if (Request.ContentType != "application/json")
                {
                    return BadRequest(new { Message = "Invalid Content-Type" });
                }

                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    var body = stream.ReadToEndAsync().Result;
                    scanJobReceived = JsonConvert.DeserializeObject<ServiceTargetContent>(body);
                }
            }
            else
            {
                var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

                var reader = new MultipartReader(boundary, HttpContext.Request.Body);

                var section = reader.ReadNextSectionAsync().Result;
                while (section != null)
                {
                    ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                    MediaTypeHeaderValue mt;
                    try
                    {
                        mt = MediaTypeHeaderValue.Parse(section.ContentType);
                    }
                    catch (Exception)
                    {
                        return BadRequest(new { Message = "Can not get section content type" });
                    }

                    if (null != cd && null != mt)
                    {
                        if (cd.Name == "content")
                        {
                            //Ensure we got the right content-type
                            if (mt.MediaType != "application/json")
                            {
                                return BadRequest(new { Message = "Invalid Content-Type for content" });
                            }

                            // Create the  arg
                            using (var streamReader = new StreamReader(
                                       section.Body, System.Text.Encoding.UTF8,
                                       detectEncodingFromByteOrderMarks: false,
                                       bufferSize: 1024,
                                       leaveOpen: true))
                            {
                                // The value length limit is enforced by
                                // MultipartBodyLengthLimit
                                // read the empty content part
                                var textValue = streamReader.ReadToEndAsync().Result;
                                scanJobReceived = JsonConvert.DeserializeObject<ServiceTargetContent>(textValue);
                            }
                        }
                        else if (cd.Name == "scanFile" || cd.Name == "metadata")
                        {
                            fileStream = new MemoryStream();
                            section.Body.CopyToAsync(fileStream).Wait();
                            fileStream.Seek(0, SeekOrigin.Begin);

                            string fileName = cd.FileName.ToString().Replace("\"", "");

                            if (scanJobReceived.ScanJobId != null)
                            {
                                fileName = "./ScanFiles/" + scanJobReceived.ScanJobId.Value.ToString() + "/" + fileName;
                                Directory.CreateDirectory("./ScanFiles/" + scanJobReceived.ScanJobId.Value.ToString());
                            }
                            else
                            {
                                fileName = "./ScanFiles/unknownId/" + fileName;
                                Directory.CreateDirectory("./ScanFiles/unknownId/");
                            }

                            // Save file to ScanFiles folder
                            using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                            {
                                byte[] bytes = new byte[fileStream.Length];
                                fileStream.Read(bytes, 0, (int)fileStream.Length);
                                file.Write(bytes, 0, bytes.Length);
                                fileStream.Close();
                            }
                        }
                    }

                    // Drain any remaining section body that hasn't been consumed and
                    // read the headers for the next section.
                    section = reader.ReadNextSectionAsync().Result;
                }
            }
            scanJobAgentService.HandleScanJobReceiver(scanJobReceived);

            return Ok();
        }
    }
}
