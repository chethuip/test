/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.Supplies;
using HP.Extensibility.Service.Supplies;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Supplies Service examples
    /// </summary>
    public interface ISuppliesService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service Capabilities resource
        /// </summary>
        /// <returns>Service.Supplies.Capabilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service SuppliesAgents resource
        /// Retrieves the SuppliesAgents registered on the device.
        /// </summary>
        /// <returns>Service.Supplies.SuppliesAgents</returns>
        SuppliesAgents EnumerateSuppliesAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service SuppliesAgent resource
        /// Retrieves the SuppliesAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the suppliesAgent</param>
        /// <returns>Service.Supplies.SuppliesAgent</returns>
        SuppliesAgent GetSuppliesAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service SuppliesConfiguration resource
        /// Retrieves the SuppliesConfiguration information for certain supplies and consumables.
        /// </summary>
        /// <param name="agentId">The agentId of the suppliesAgent</param>
        /// <returns>Service.Supplies.SuppliesConfiguration</returns>
        SuppliesConfiguration GetSuppliesConfiguration(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service SuppliesInfo resource
        /// Retrieves the SuppliesInfo information for certain supplies and consumables.
        /// </summary>
        /// <param name="agentId">The agentId of the suppliesAgent</param>
        /// <returns>Service.Supplies.SuppliesInfo</returns>
        SuppliesInfo GetSuppliesInfo(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Supplies Service SuppliesUsage resource
        /// Retrieves the SuppliesUsage information for certain supplies and consumables.
        /// </summary>
        /// <param name="agentId">The agentId of the suppliesAgent</param>
        /// <returns>Service.Supplies.SuppliesUsage</returns>
        SuppliesUsage GetSuppliesUsage(string agentId);
    }


    /// <summary>
    /// Implements the business logic of the Supplies examples
    /// </summary>
    public class SuppliesService : ISuppliesService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public SuppliesService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private SuppliesService() { }

        #endregion // Constructor


        #region ISuppliesService implementation

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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        public SuppliesAgents EnumerateSuppliesAgents()
        {
            // @StartCodeExample:EnumerateSuppliesAgents
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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SuppliesAgents suppliesAgents = client.SuppliesAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return suppliesAgents;
        }

        public SuppliesAgent GetSuppliesAgent(string agentId)
        {
            // @StartCodeExample:GetSuppliesAgent
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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SuppliesAgent suppliesAgent = client.SuppliesAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return suppliesAgent;
        }

        public SuppliesConfiguration GetSuppliesConfiguration(string agentId)
        {
            // @StartCodeExample:GetSuppliesConfiguration
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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SuppliesConfiguration suppliesConfiguration = client.SuppliesAgents[agentId].SuppliesConfiguration.GetAsync(accessToken).Result;
            // @EndCodeExample

            return suppliesConfiguration;
        }

        public SuppliesInfo GetSuppliesInfo(string agentId)
        {
            // @StartCodeExample:GetSuppliesInfo
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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SuppliesInfo suppliesInfo = client.SuppliesAgents[agentId].SuppliesInfo.GetAsync(accessToken).Result;
            // @EndCodeExample

            return suppliesInfo;
        }

        public SuppliesUsage GetSuppliesUsage(string agentId)
        {
            // @StartCodeExample:GetSuppliesUsage
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

            // Construct the SuppliesServiceClient
            SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SuppliesUsage suppliesUsage = client.SuppliesAgents[agentId].SuppliesUsage.GetAsync(accessToken).Result;
            // @EndCodeExample

            return suppliesUsage;
        }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            SuppliesServiceClient client = null;

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
            client = new SuppliesServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockSupplies");
            client = new SuppliesServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion // ISuppliesExamplesService implementation

        public void ExceptionHandlingExample()
        {
            Device currentDevice = deviceManagementService.CurrentDevice;
            AccessToken accessToken = string.Empty;

            // @StartCodeExample:ExceptionHandlingExample
            try
            {
                // Fetch the discovery tree
                DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
                ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

                // Construct the SuppliesServiceClient
                SuppliesServiceClient client = new SuppliesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

                // Execute the Get operation
                Capabilities capabilities = client.Capabilities.GetAsync().Result;
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
