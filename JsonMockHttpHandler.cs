/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 * *********************************************************/
using HP.Extensibility.Types.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SolutionManager = HP.Extensibility.Service.SolutionManager;

namespace OXPd2ExamplesHost.Utilities
{
    public class JsonMockHttpHandler : DelegatingHandler
    {
        private string responsePath;
        public JsonMockHttpHandler(string path)
        {
            responsePath = path;
        }

        private string GetJsonString(string filePath)
        {
            if (File.Exists(filePath))
            {
                StreamReader r = new StreamReader(filePath);
                return r.ReadToEnd();
            }
            return null;
        }
        private string GetResponseData(HttpRequestMessage request)
        {
            return GetJsonString(responsePath + request.RequestUri.AbsolutePath + "/" + request.Method.ToString() + ".json");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            string responseString = GetResponseData(request);
            if (request.RequestUri.ToString().Contains("/ext/solutionManager/v1/solutions/86dd4dd1-ae11-4572-9800-a9d301f55d17/certificateAuthorities/export", StringComparison.CurrentCultureIgnoreCase))
            {
                SolutionManager.CertificateAuthorities_Export caExport = new SolutionManager.CertificateAuthorities_Export();
                caExport.OpMeta = new OperationMeta() { ContentFilter = new ContentFilter(new List<ContentFilterString>() { "*" }) };
                caExport.Links = new List<Link>() { new Link() { Rel = "self", Href = "/self" } };
                var response = getMultipartCAResponse(caExport);
                return Task.FromResult(response);
            }
            else if (request.RequestUri.ToString().Contains("/ext/solutionManager/v1/solutions/86dd4dd1-ae11-4572-9800-a9d301f55d17/certificateAuthorities/34d911a2-2a5c-4b0b-b5dc-5eb0bde0474c/export", StringComparison.CurrentCultureIgnoreCase))
            {
                SolutionManager.CertificateAuthority_Export caExport = new SolutionManager.CertificateAuthority_Export();
                caExport.OpMeta = new OperationMeta() { ContentFilter = new ContentFilter(new List<ContentFilterString>() { "*" }) };
                caExport.Links = new List<Link>() { new Link() { Rel = "self", Href = "/self" } };
                var response = getMultipartCAResponse(caExport);
                return Task.FromResult(response);
            }
            else if (responseString != null)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(responseString);
                return Task.FromResult(response);
            }
            else
            {
                return Task.FromException<HttpResponseMessage>(new Exception("No MockHandler hasn't been provided for" + request.RequestUri.ToString()));
            }
        }

        private HttpResponseMessage getMultipartCAResponse(dynamic stringContentPart)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.TryAddWithoutValidation("Content-Type", "multipart/mixed");
            MultipartContent parts = new MultipartContent();
            StringContent contentPart = new StringContent(JsonConvert.SerializeObject(stringContentPart));
            contentPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            contentPart.Headers.ContentDisposition.Name = "content";
            contentPart.Headers.ContentType = new MediaTypeHeaderValue("application/x-pem-file");
            parts.Add(contentPart);
            StringContent dataPart = new StringContent("Mock Certificate");
            dataPart.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            dataPart.Headers.ContentDisposition.Name = "certificate";
            parts.Add(dataPart);
            response.Content = parts;
            return response;
        }
    }
}
