/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.DeviceUsage;
using HP.Extensibility.Service.DeviceUsage;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Device Usage Service examples
    /// </summary>
    public interface IDeviceUsageService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the DeviceUsage Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.DeviceUsage.Capabilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the DeviceUsage Service DeviceUsageAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.DeviceUsage.DeviceUsageAgents</returns>
        DeviceUsageAgents EnumerateDeviceUsageAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Device Usage Service DeviceUsageAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the device usage agent</param>
        /// <returns>HP.Extensibility.Service.DeviceUsage.DeviceUsageAgent</returns>
        DeviceUsageAgent GetDeviceUsageAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Device Usage Service LifetimeCounters resource
        /// </summary>
        /// <param name="agentId">The agentId of the device usage agent</param>
        /// <returns>HP.Extensibility.Service.DeviceUsage.LifetimeCounters</returns>
        LifetimeCounters GetLifetimeCounters(string agentId);
    }


    /// <summary>
    /// Implements the business logic of the DeviceUsage examples
    /// </summary>
    public class DeviceUsageService : IDeviceUsageService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public DeviceUsageService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private DeviceUsageService() { }

        #endregion // Constructor


        #region IDeviceUsageService implementation

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

            // Construct the DeviceUsageServiceClient
            DeviceUsageServiceClient client = new DeviceUsageServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        public DeviceUsageAgents EnumerateDeviceUsageAgents()
        {
            // @StartCodeExample:EnumerateDeviceUsageAgents
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

            // Construct the DeviceUsageServiceClient
            DeviceUsageServiceClient client = new DeviceUsageServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            DeviceUsageAgents deviceUsageAgents = client.DeviceUsageAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return deviceUsageAgents;
        }

        public DeviceUsageAgent GetDeviceUsageAgent(string agentId)
        {
            // @StartCodeExample:GetDeviceUsageAgent
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

            // Construct the DeviceUsageServiceClient
            DeviceUsageServiceClient client = new DeviceUsageServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            DeviceUsageAgent deviceUsageAgent = client.DeviceUsageAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return deviceUsageAgent;
        }

        public LifetimeCounters GetLifetimeCounters(string agentId)
        {
            // @StartCodeExample:GetLifetimeCounters
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

            // Construct the DeviceUsageServiceClient
            DeviceUsageServiceClient client = new DeviceUsageServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            LifetimeCounters lifetimeCounters = client.DeviceUsageAgents[agentId].LifetimeCounters.GetAsync(accessToken).Result;
            // @EndCodeExample

            return lifetimeCounters;
        }       

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            DeviceUsageServiceClient client = null;

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
            client = new DeviceUsageServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockDeviceUsage");
            client = new DeviceUsageServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion // IDeviceUsageExamplesService implementation

        public void ExceptionHandlingExample()
        {
            Device currentDevice = deviceManagementService.CurrentDevice;
            string agentId = string.Empty;
            AccessToken accessToken = string.Empty;

            // @StartCodeExample:ExceptionHandlingExample
            try
            {
                // Fetch the discovery tree
                DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
                ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

                // Construct the DeviceUsageServiceClient
                DeviceUsageServiceClient client = new DeviceUsageServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

                // Execute the Get operation
                Capabilities capabilities = client.Capabilities.GetAsync(accessToken).Result;
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException is OXPdHttpRequestException)
                    {
                        var oxpdException = (OXPdHttpRequestException)e.InnerException;
                        if (oxpdException.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            // Can inspect the errors to see what the caused the BadRequest.
                            var errors = oxpdException.ErrorResponse;
                        }
                        else if (oxpdException.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            // Possible issue with a token.
                        }
                        // ... etc.
                    }
                }
            }
            // @EndCodeExample
        }
    }
}
