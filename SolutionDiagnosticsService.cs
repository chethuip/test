/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.SolutionDiagnostics;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Service.SolutionDiagnostics;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    public interface ISolutionDiagnosticsService
    {
        /// <summary>
        /// Demosntrate SDK use-case of interacting with the OXPd2 Solution Diagnostics Service Capabilities resource
        /// </summary>
        /// <returns>Service.SolutionDiagnostics.Capabilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrates SDK use-case of interacting with the SolutionDiagnostics Service SolutionDiagnosticsAgents resource
        /// Retrieves the SolutionDiagnosticsAgents registered on the device
        /// </summary>
        /// <returns></returns>
        SolutionDiagnosticsAgents EnumerateSolutionDiagnosticsAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the SolutionDiagnostics Service SolutionDiagnosticsAgent resource
        /// Retrieves the SolutionDiagnosticsAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the SolutionDiagnosticsAgent</param>
        /// <returns></returns>
        SolutionDiagnosticsAgent GetSolutionDiagnosticsAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the SolutionDiagnostics Service Log resource
        /// Retireveds the Log for a SolutionDiagnosticsAgent with the specified agentId
        /// </summary>
        /// <param name="agentId">The agentId of the SolutionDiagnosticsAgent</param>
        /// <returns></returns>
        Tuple<Log, byte[]> GetAgentLog(string agentId);
    }

    public class SolutionDiagnosticsService : ISolutionDiagnosticsService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;

        public SolutionDiagnosticsService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private SolutionDiagnosticsService() { }


        #endregion // Constructor

        #region ISolutionDiagnosticsService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Solution Diagnostics Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.SolutionDiagnostics.Capablilities</returns>
        public Capabilities GetCapabilities()
        {
            // @StartCodeExample:GetCapabilities
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionDiagnosticsServiceClient client = new SolutionDiagnosticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;

        }

        public SolutionDiagnosticsAgents EnumerateSolutionDiagnosticsAgents()
        {
            // @StartCodeExample:EnumerateSolutionDiagnosticsAgents
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionDiagnosticsServiceClient
            SolutionDiagnosticsServiceClient client = new SolutionDiagnosticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SolutionDiagnosticsAgents solutionDiagnosticsAgents = client.SolutionDiagnosticsAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return solutionDiagnosticsAgents;
        }

        public SolutionDiagnosticsAgent GetSolutionDiagnosticsAgent(string agentId)
        {
            // @StartCodeExample:GetSolutionDiagnosticsAgent
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider Owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionDiagnosticsServiceClient
            SolutionDiagnosticsServiceClient client = new SolutionDiagnosticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SolutionDiagnosticsAgent solutionDiagnosticsAgent = client.SolutionDiagnosticsAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return solutionDiagnosticsAgent;
        }

        public Tuple<Log, byte[]> GetAgentLog(string agentId)
        {
            // @StartCodeExample:GetSolutionDiagnosticsAgentLog
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider Owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionDiagnosticsServiceClient
            SolutionDiagnosticsServiceClient client = new SolutionDiagnosticsServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Tuple<Log, byte[]> solutionDiagnosticsAgentLog = client.SolutionDiagnosticsAgents[agentId].Log.GetAsync(accessToken).Result;
            // @EndCodeExample

            return solutionDiagnosticsAgentLog;
        }

        #endregion // ISolutionDiagnosticsService implementation

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            SolutionDiagnosticsServiceClient client = null;

            // @StartCodeExample:ConstructorExamples
            // Construct the HttpClientHandler and HttpClient. In this case we are using a handler
            // that will not be performing server certificate validation...
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            HttpClient httpClient = new HttpClient(clientHandler, true);

            // Here's the standard constructor using the device's network address and a "discovery tree"
            // instance that's already been fetched from the device (not shown)
            client = new SolutionDiagnosticsServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockApplication");
            client = new SolutionDiagnosticsServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }
    }
}
