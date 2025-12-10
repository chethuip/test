using HP.Extensibility.Client.Authentication;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.OAUTH2;
using HP.Extensibility.Service.Authentication;
using HP.Extensibility.Types.Authentication;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Authentication examples
    /// </summary>
    public interface IAuthenticationService
    {
        AuthenticationAccessPoint GetAuthenticationAccessPoint(string accessPointId);

        AuthenticationAccessPoints EnumerateAuthenticationAccessPoints(string queryParams);

        AuthenticationAccessPoint_InitiateLogin AuthenticationAccessPointInitiateLogin(string accessPointId);

        AuthenticationAgent GetAuthenticationAgent(string agentId);

        AuthenticationAgents EnumerateAuthenticationAgents();

        AuthenticationAgent_Login AuthenticationAgentLogin(string agentId, PrePromptResult prePromptResult);

        Session_ForceLogout SessionForceLogout();

        Capabilities GetCapabilities();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;

        public AuthenticationService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService)
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        public AuthenticationAgents EnumerateAuthenticationAgents()
        {
            // @StartCodeExample: EnumerateAuthenticationAgents
            // Fetch the current device we're working with
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

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAgents agents = authenticationClient.AuthenticationAgents.GetAsync(accessToken).Result;

            // @EndCodeExample
            return agents;
        }

        public AuthenticationAgent GetAuthenticationAgent(string agentId)
        {
            // @StartCodeExample: GetAuthenticationAgent
            // Fetch the current device we're working with
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

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAgent agent = authenticationClient.AuthenticationAgents[agentId].GetAsync(accessToken).Result;

            // @EndCodeExample
            return agent;
        }

        public AuthenticationAgent_Login AuthenticationAgentLogin(string agentId, PrePromptResult prePromptResult)
        {
            // @StartCodeExample: AuthenticationAgentLogin
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires SolutionProvider
            string accessToken = currentDevice.GetToken(AccessTokenType.Solution);

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAgent_Login agentLogin = authenticationClient.AuthenticationAgents[agentId].Login.ExecuteAsync(accessToken, prePromptResult).Result;

            if (agentLogin.SessionAccessToken != null)
            {
                Token authContextToken = new Token()
                {
                    AccessToken = agentLogin.SessionAccessToken,
                    TokenType = "Bearer"
                };
                // Stash the Auth-Context session token with the our device manager for later use
                deviceManagementService.SetAuthContextAccessToken(authContextToken);
            }
            // @EndCodeExample
            return agentLogin;
        }


        public AuthenticationAccessPoints EnumerateAuthenticationAccessPoints(string queryParams)
        {
            // @StartCodeExample: EnumerateAuthenticationAccessPoints
            // Fetch the current device we're working with
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

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAccessPoints accessPoints = authenticationClient.AuthenticationAccessPoints.GetAsync(accessToken, queryParams).Result;

            // @EndCodeExample
            return accessPoints;
        }

        public AuthenticationAccessPoint GetAuthenticationAccessPoint(string accessPointId)
        {
            // @StartCodeExample: GetAuthenticationAccessPoint
            // Fetch the current device we're working with
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

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAccessPoint authenticationAccessPoint = authenticationClient.AuthenticationAccessPoints[accessPointId].GetAsync(accessToken).Result;

            // @EndCodeExample
            return authenticationAccessPoint;
        }

        public AuthenticationAccessPoint_InitiateLogin AuthenticationAccessPointInitiateLogin(string accessPointId)
        {
            // @StartCodeExample: AuthenticationAccessPointInitiateLogin
            // Fetch the current device we're working with
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

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            AuthenticationAccessPoint_InitiateLogin accessPointLogin = authenticationClient.AuthenticationAccessPoints[accessPointId].InitiateLogin.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return accessPointLogin;
        }

        public Session_ForceLogout SessionForceLogout()
        {
            // @StartCodeExample: SessionForceLogout
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Authentication Context
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Authentication_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            AuthenticationServiceClient authenticationClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Session_ForceLogout sessionLogout = authenticationClient.Session.ForceLogout.ExecuteAsync(accessToken).Result;

            // @EndCodeExample
            return sessionLogout;
        }

        public Capabilities GetCapabilities()
        {
            // @StartCodeExample: GetCapabilities
            // Fetch the current device we're working with (this an OXPd 2.0 Examples App abstraction)
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            AuthenticationServiceClient authenticaionClient = new AuthenticationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Capabilities capabilities = authenticaionClient.Capabilities.GetAsync().Result;

            // @EndCodeExample
            return capabilities;
        }
    }
}
