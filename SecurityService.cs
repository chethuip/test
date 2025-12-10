/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.Security;
using HP.Extensibility.Service.Security;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// This service provides access to a limited set of device security related features and data.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service Capabilities resource
        /// </summary>
        /// <returns>Service.Security.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service SecurityAgents resource
        /// Retrieves the SecurityAgents registered on the device.
        /// </summary>
        /// <returns>Service.Security.SecurityAgents</returns>
        SecurityAgents EnumerateSecurityAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service SecurityAgent resource
        /// Retrieves the SecurityAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the SecurityAgent</param>
        /// <returns>Service.Security.SecurityAgent</returns>
        SecurityAgent GetSecurityAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service ResolveSecurityExpression resource
        /// Retrieves the Security Context Attribute for the specified expression from the Security Agent.
        /// </summary>
        /// <param name="agentId">The agentId of the SecurityAgent</param>
        /// <param name="request">The request containing the expression to resolve</param>
        /// <returns>Service.Security.SecurityAgent_ResolveSecurityExpression</returns>
        SecurityAgent_ResolveSecurityExpression ResolveSecurityExpression(string agentId, ResolveSecurityExpressionRequest request);
    }

    public class SecurityService : ISecurityService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public SecurityService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private SecurityService() { }

        #endregion // Constructor

        #region ISecurityService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Security.Capabilities</returns>
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

            // Construct the SecurityServiceClient
            SecurityServiceClient client = new SecurityServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service SecurityAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Security.SecurityAgents</returns>
        public SecurityAgents EnumerateSecurityAgents()
        {
            // @StartCodeExample:EnumerateSecurityAgents
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

            // Construct the SecurityServiceClient
            SecurityServiceClient client = new SecurityServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SecurityAgents securityAgents = client.SecurityAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return securityAgents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service SecurityAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the security agent</param>
        /// <returns>HP.Extensibility.Service.Security.SecurityAgent</returns>
        public SecurityAgent GetSecurityAgent(string agentId)
        {
            // @StartCodeExample:GetSecurityAgent
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

            // Construct the SecurityServiceClient
            SecurityServiceClient client = new SecurityServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            SecurityAgent securityAgent = client.SecurityAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return securityAgent;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Security Service ResolveSecurityExpression resource
        /// Retrieves the Security Context Attribute for the specified expression from the Security Agent.
        /// </summary>
        /// <param name="agentId">The agentId of the SecurityAgent</param>
        /// <param name="request">The request containing the expression to resolve</param>
        /// <returns>Service.Security.SecurityAgent_ResolveSecurityExpression</returns>
        public SecurityAgent_ResolveSecurityExpression ResolveSecurityExpression(string agentId, ResolveSecurityExpressionRequest request)
        {
            // @StartCodeExample:ResolveSecurityExpression
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

            // Construct the SecurityServiceClient
            SecurityServiceClient client = new SecurityServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Call the Execute operation
            SecurityAgent_ResolveSecurityExpression resolvedExpression = client.SecurityAgents[agentId].ResolveSecurityExpression.ExecuteAsync(accessToken, request).Result;
            // @EndCodeExample

            return resolvedExpression;
        }

        #endregion // ISecurityService implementation


        #region Constructor Example
        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            SecurityServiceClient client = null;

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
            client = new SecurityServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockSecurity");
            client = new SecurityServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion //Constructor Example
    }
}
