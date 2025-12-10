/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.PrintJob;
using HP.Extensibility.Service.PrintJob;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Print Job Service examples
    /// </summary>
    public interface IPrintJobService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the PrintJob Service Capabilities resource
        /// </summary>
        /// <returns>Types.PrintJob.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the PrintJob Service PrintAgents resource
        /// Retrieves the PrintAgents registered on the device.
        /// </summary>
        /// <returns>Types.PrintJob.PrintAgents</returns>
        PrintJobAgents EnumeratePrintJobAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the PrintJob Service PrintJobAgent resource
        /// Retrieves the PrintAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the printAgent</param>
        /// <returns>Types.PrintJob.PrintAgent</returns>
        PrintJobAgent GetPrintJobAgent(string agentId);
    }


    /// <summary>
    /// Implements the business logic of the PrintJob examples
    /// </summary>
    public class PrintJobService : IPrintJobService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public PrintJobService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private PrintJobService() { }

        #endregion // Constructor


        #region IPrintJobService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the PrintJob Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.PrintJob.Capabilities</returns>
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

            // Construct the PrintJobServiceClient
            PrintJobServiceClient client = new PrintJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the PrintJob Service PrintJobAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.PrintJob.PrintAgents</returns>
        public PrintJobAgents EnumeratePrintJobAgents()
        {
            // @StartCodeExample:EnumeratePrintAgents
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

            // Construct the PrintJobServiceClient
            PrintJobServiceClient client = new PrintJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            PrintJobAgents printJobAgents = client.PrintJobAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return printJobAgents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Print Job Service PrintJobAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the print agent</param>
        /// <returns>HP.Extensibility.Service.PrintJob.PrintAgent</returns>
        public PrintJobAgent GetPrintJobAgent(string agentId)
        {
            // @StartCodeExample:GetPrintAgent
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

            // Construct the PrintJobServiceClient
            PrintJobServiceClient client = new PrintJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            PrintJobAgent printJobAgent = client.PrintJobAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return printJobAgent;
        }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            PrintJobServiceClient client = null;

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
            client = new PrintJobServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockPrintJob");
            client = new PrintJobServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion // IPrintJobExamplesService implementation

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

                // Construct the PrintJobServiceClient
                PrintJobServiceClient client = new PrintJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

                // Execute the Get operation
                PrintJobAgent printJobAgent = client.PrintJobAgents[agentId].GetAsync(accessToken).Result;
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
