/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.SolutionManager;
using HP.Extensibility.Types.Base;
using HP.Extensibility.Types.SolutionManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OXPd2ExamplesHost.Services;
using OXPd2ExamplesHost.Utilities;
using System;
using System.IO;

namespace OXPd2ExamplesHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionManagerController : ControllerBase
    {
        // Filters are handling exceptions/error conditions and returning appropriate responses.
        private ISolutionManagerService service = null;

        public SolutionManagerController(ISolutionManagerService service)
        {
            this.service = service;
        }

        private SolutionManagerController() { }


        [HttpGet("capabilities")]
        public ActionResult<Capabilities> GetCapabilities()
        {
            return service.GetCapabilities();
        }

        [HttpGet("solutions")]
        public ActionResult<Solutions> EnumerateSolutions()
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateSolutions(query);
        }

        [HttpGet("solutions/{solutionId}")]
        public ActionResult<Solution> GetSolution(string solutionId)
        {
            return service.GetSolution(solutionId);
        }

        [HttpPost("solutions/{solutionId}/reissueInstallCode")]
        public ActionResult<Solution_ReissueInstallCode> PostReissueInstallCode(string solutionId)
        {
            return service.ReissueInstallCode(solutionId);
        }

        [HttpGet("solutions/{solutionId}/context")]
        public ActionResult<Context> GetSolutionContext(string solutionId)
        {
            return service.GetSolutionContext(solutionId);
        }

        [HttpPatch("solutions/{solutionId}/context")]
        public ActionResult<Context> ModifySolutionContext(string solutionId, [FromBody] Context_Modify contextModify)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.ModifySolutionContext(solutionId, contextModify);
        }

        [HttpPut("solutions/{solutionId}/context")]
        public ActionResult<Context> ReplaceSolutionContext(string solutionId, [FromBody] Context_Replace contextReplace)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.ReplaceSolutionContext(solutionId, contextReplace);
        }

        [HttpGet("solutions/{solutionId}/configuration")]
        public ActionResult<Configuration> GetConfiguration(string solutionId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.GetConfiguration(solutionId, query);
        }

        [HttpPatch("solutions/{solutionId}/configuration")]
        public ActionResult<Configuration> ModifyConfiguration(string solutionId, [FromBody] Configuration_Modify configurationModify)
        {
            return service.ModifyConfiguration(solutionId, configurationModify);
        }

        [HttpGet("solutions/{solutionId}/configuration/data")]
        public ActionResult<Tuple<Data, byte[]>> GetConfigurationData(string solutionId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.GetConfigurationData(solutionId, query);
        }

        [HttpPut("solutions/{solutionId}/configuration/data")]
        public ActionResult<Data> ReplaceConfigurationData(string solutionId)
        {
            Stream dataStream = null;
            string mimeType = null;

            var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = reader.ReadNextSectionAsync().Result;
            while (section != null)
            {
                ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                MediaTypeHeaderValue mt = MediaTypeHeaderValue.Parse(section.ContentType);

                if (null != cd && null != mt)
                {
                    if (cd.Name == "content")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }
                    }
                    else if (cd.Name == "data")
                    {
                        // Set the mimeType for the configuration data being sent
                        mimeType = mt.MediaType.ToString();

                        // Create the Data args
                        // Try a new memory-stream
                        dataStream = new MemoryStream();
                        section.Body.CopyToAsync(dataStream).Wait();
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = reader.ReadNextSectionAsync().Result;
            }

            return service.ReplaceConfigurationData(solutionId, dataStream, mimeType);
        }

        [HttpGet("solutions/{solutionId}/certificateAuthorities")]
        public ActionResult<CertificateAuthorities> EnumerateCertificateAuthorities(string solutionId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateCertificateAuthorities(solutionId, query);
        }

        [HttpPost("solutions/{solutionId}/certificateAuthorities/export")]
        public ActionResult<Tuple<CertificateAuthorities_Export, byte[]>> ExportCertificateAuthorities(string solutionId)
        {
            return service.ExportCertificateAuthorities(solutionId);
        }

        [HttpPost("solutions/{solutionId}/certificateAuthorities/import")]
        public ActionResult<CertificateAuthorities_Import> ImportCertificateAuthority(string solutionId)
        {
            CertificateAuthorities_Import result = null;
            CertificateAuthoritiesImportRequest importRequest = null;
            Stream certificateFile = null;
            string certificateFileName = "certificate.pem";

            if (false == MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest();
            }

            var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = reader.ReadNextSectionAsync().Result;
            while (section != null)
            {
                ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                MediaTypeHeaderValue mt = MediaTypeHeaderValue.Parse(section.ContentType);

                if (null != cd && null != mt)
                {
                    if (cd.Name == "content")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }

                        // Create the CertificateAuthoritiesImportRequest arg
                        using (var streamReader = new StreamReader(
                                   section.Body, System.Text.Encoding.UTF8,
                                   detectEncodingFromByteOrderMarks: false,
                                   bufferSize: 1024,
                                   leaveOpen: true))
                        {
                            // The value length limit is enforced by
                            // MultipartBodyLengthLimit
                            var textValue = streamReader.ReadToEndAsync().Result;
                            importRequest = JsonConvert.DeserializeObject<CertificateAuthoritiesImportRequest>(textValue);
                        }
                    }
                    else if (cd.Name == "certificate")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/x-pem-file")
                        {
                            return BadRequest();
                        }

                        // Create the Certificate args
                        // Try a new memory-stream
                        certificateFile = new MemoryStream();
                        section.Body.CopyToAsync(certificateFile).Wait();
                        certificateFileName = cd.FileName.ToString();
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = reader.ReadNextSectionAsync().Result;
            }

            result = service.ImportCertificateAuthority(solutionId, importRequest, certificateFile, certificateFileName);

            return result;

        }

        [HttpGet("solutions/{solutionId}/certificateAuthorities/{certificateId}")]
        public ActionResult<CertificateAuthority> GetCertificateAuthority(string solutionId, string certificateId)
        {
            return service.GetCertificateAuthority(solutionId, certificateId);
        }

        [HttpDelete("solutions/{solutionId}/certificateAuthorities/{certificateId}")]
        public ActionResult<DeleteContent> DeleteCertificateAuthority(string solutionId, string certificateId)
        {
            return service.DeleteCertificateAuthority(solutionId, certificateId);
        }

        [HttpPost("solutions/{solutionId}/certificateAuthorities/{certificateId}/export")]
        public ActionResult<Tuple<CertificateAuthority_Export, byte[]>> ExportCertificateAuthority(string solutionId, string certificateId)
        {
            return service.ExportCertificateAuthority(solutionId, certificateId);
        }

        [HttpGet("installer")]
        public ActionResult<Installer> GetInstaller()
        {
            return service.GetInstaller();
        }

        [HttpPost("installer/installSolution")]
        public ActionResult<Installer_InstallSolution> InstallSolution()
        {
            InstallSolutionRequest installSolutionRequest = null;
            Stream solutionBundle = null;
            string solutionBundleFilename = "solution.bdl";
            Context_Replace context = null;

            if (false == MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest();
            }

            var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = reader.ReadNextSectionAsync().Result;
            while (section != null)
            {
                ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                MediaTypeHeaderValue mt = MediaTypeHeaderValue.Parse(section.ContentType);

                if (null != cd && null != mt)
                {
                    if (cd.Name == "content")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }

                        // Create the InstallRequest arg
                        using (var streamReader = new StreamReader(
                                   section.Body, System.Text.Encoding.UTF8,
                                   detectEncodingFromByteOrderMarks: false,
                                   bufferSize: 1024,
                                   leaveOpen: true))
                        {
                            // The value length limit is enforced by
                            // MultipartBodyLengthLimit
                            var textValue = streamReader.ReadToEndAsync().Result;
                            installSolutionRequest = JsonConvert.DeserializeObject<InstallSolutionRequest>(textValue);
                        }
                    }
                    else if (cd.Name == "context")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }
                        // Create the Context_Replace args
                        using (var streamReader = new StreamReader(
                                  section.Body, System.Text.Encoding.UTF8,
                                  detectEncodingFromByteOrderMarks: false,
                                  bufferSize: 1024,
                                  leaveOpen: true))
                        {
                            // The value length limit is enforced by
                            // MultipartBodyLengthLimit
                            var textValue = streamReader.ReadToEndAsync().Result;
                            context = JsonConvert.DeserializeObject<Context_Replace>(textValue);
                        }
                    }
                    else if (cd.Name == "solution")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/vnd.hp.solution-bundle")
                        {
                            return BadRequest();
                        }

                        // Create the SolutionBundle and SolutionBundleFilename args
                        // Try a new memory-stream

                        solutionBundle = new MemoryStream();
                        section.Body.CopyToAsync(solutionBundle).Wait();
                        solutionBundleFilename = cd.FileName.ToString();
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = reader.ReadNextSectionAsync().Result;
            }

            return service.InstallSolution(installSolutionRequest, solutionBundle, solutionBundleFilename, context); 
        }

        [HttpPost("installer/installRemote")]
        public ActionResult<Installer_InstallRemote> InstallRemote()
        {
            InstallRemoteRequest installRemoteRequest = null;
            RemoteArchive remoteArchive = null;

            if (false == MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest();
            }

            var boundary = MultipartRequestHelper.GetBoundary(Request.ContentType, 128);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = reader.ReadNextSectionAsync().Result;
            while (section != null)
            {
                ContentDispositionHeaderValue cd = ContentDispositionHeaderValue.Parse(section.ContentDisposition);
                MediaTypeHeaderValue mt = MediaTypeHeaderValue.Parse(section.ContentType);

                if (null != cd && null != mt)
                {
                    if (cd.Name == "content")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }

                        // Create the InstallRequest arg
                        using (var streamReader = new StreamReader(
                                   section.Body, System.Text.Encoding.UTF8,
                                   detectEncodingFromByteOrderMarks: false,
                                   bufferSize: 1024,
                                   leaveOpen: true))
                        {
                            // The value length limit is enforced by
                            // MultipartBodyLengthLimit
                            var textValue = streamReader.ReadToEndAsync().Result;
                            installRemoteRequest = JsonConvert.DeserializeObject<InstallRemoteRequest>(textValue);
                        }
                    }
                    else if (cd.Name == "remote")
                    {
                        //Ensure we got the right content-type
                        if (mt.MediaType != "application/json")
                        {
                            return BadRequest();
                        }
                        // Create the RemoteArchive args
                        using (var streamReader = new StreamReader(
                                  section.Body, System.Text.Encoding.UTF8,
                                  detectEncodingFromByteOrderMarks: false,
                                  bufferSize: 1024,
                                  leaveOpen: true))
                        {
                            // The value length limit is enforced by
                            // MultipartBodyLengthLimit
                            var textValue = streamReader.ReadToEndAsync().Result;
                            remoteArchive = JsonConvert.DeserializeObject<RemoteArchive>(textValue);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = reader.ReadNextSectionAsync().Result;
            }

            return service.InstallRemote(installRemoteRequest, remoteArchive);
        }

        [HttpPost("installer/uninstallSolution")]
        public ActionResult<Installer_UninstallSolution> UninstallSolution(UninstallSolutionRequest uninstallSolutionRequest)
        {
            return service.UninstallSolution(uninstallSolutionRequest);
        }

        [HttpGet("installer/installerOperations")]
        public ActionResult<InstallerOperations> EnumerateInstallerOperations()
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateInstallerOperations(query);
        }

        [HttpGet("installer/installerOperations/{operationId}")]
        public ActionResult<InstallerOperation> GetInstallerOperation(string operationId)
        {
            return service.GetInstallerOperation(operationId);
        }

        [HttpGet("solutions/{solutionId}/runtimeRegistrations")]
        public ActionResult<RuntimeRegistrations> EnumerateRuntimeRegistrations(string solutionId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.EnumerateRuntimeRegistrations(solutionId, query);
        }

        [HttpPost("solutions/{solutionId}/runtimeRegistrations")]
        public ActionResult<RuntimeRegistration> CreateRuntimeRegistration(string solutionId, RuntimeRegistration_Create runtimeRegistrationCreate)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.CreateRuntimeRegistration(solutionId, runtimeRegistrationCreate, query);
        }

        [HttpGet("solutions/{solutionId}/runtimeRegistrations/{recordId}")]
        public ActionResult<RuntimeRegistration> GetRuntimeRegistration(string solutionId, string recordId)
        {
            string query = Request.QueryString.HasValue ? Request.QueryString.Value.TrimStart('?') : null;
            return service.GetRuntimeRegistration(solutionId, recordId, query);
        }

        [HttpDelete("solutions/{solutionId}/runtimeRegistrations/{recordId}")]
        public ActionResult<DeleteContent> DeleteRuntimeRegistration(string solutionId, string recordId)
        {
            return service.DeleteRuntimeRegistration(solutionId, recordId);
        }

    }
}
