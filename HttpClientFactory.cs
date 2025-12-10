/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using System.Net.Http;

namespace OXPd2ExamplesHost.Utilities
{

    public interface IHttpClientFactory
    {
        HttpClient HttpClient { get; }
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        System.Net.Http.IHttpClientFactory _httpClientFactory;

        public HttpClientFactory(System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient HttpClient
        {
            get { return _httpClientFactory.CreateClient("oxpd"); }
        }
    }
}
