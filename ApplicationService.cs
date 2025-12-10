/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Application;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Service.Application;
using HP.Extensibility.Types.Base;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Solution Manager examples
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Capabilities resource
        /// </summary>
        /// <returns>Service.Application.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Application Runtime resource
        /// </summary>
        /// <returns>Service.Application.ApplicationRuntime</returns>
        ApplicationRuntime GetApplicationRuntime();

        /// <summary>
        /// Demonstrate SDK use-case of executing a reset with the OXPd2 Application Service Application Runtime resource
        /// </summary>
        /// <returns>Service.Application.ApplicationRuntime_Reset</returns>
        ApplicationRuntime_Reset ResetApplicationRuntime(ResetRequest resetRequest);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Application Runtime Current Context resource
        /// </summary>
        /// <returns>Service.Application.CurrentContext</returns>
        CurrentContext GetCurrentContext();

        /// <summary>
        /// Demonstrate SDK use-case of executing an exit with the OXPd2 Application Service Application Runtime Current Context resource
        /// </summary>
        /// <returns>Service.Application.CurrentContext_Exit</returns>
        CurrentContext_Exit ExitCurrentContext(ExitRequest exitRequest);


        /// <summary>
        /// Demonstrate SDK use-case of executing a resetInactivityTimer with the OXPd2 Application Service Application Runtime Current Context resource
        /// </summary>
        /// <returns>Service.Application.CurrentContext_ResetInactivityTimer</returns>
        CurrentContext_ResetInactivityTimer ResetInactivityTimerCurrentContext(ResetInactivityTimerRequest resetRequest);

        /// <summary>
        /// Demonstrates SDK use-case of obtaining the start intent on the current context.
        /// </summary>
        /// <returns>Service.Application.StartIntent</returns>
        StartIntent GetStartIntent();

        /// <summary>
        /// Demostrates SDK use-case of obtaining the runtime chrome in the current context.
        /// </summary>
        /// <returns>Service.Application.RuntimeChrome</returns>
        RuntimeChrome GetRuntimeChrome();

        /// <summary>
        /// Demonstrates SDK use-case of replacing the runtime chrome in the current context.
        /// </summary>
        /// <param name="runtimeChrome"></param>
        /// <returns>Service.Application.RuntimeChrome</returns>
        RuntimeChrome ReplaceRuntimeChrome(RuntimeChrome_Replace runtimeChrome);

        /// <summary>
        /// Demonstrates SDK use-case of modifying the runtime chrome in the current context.
        /// </summary>
        /// <param name="runtimeChrome"></param>
        /// <returns>Service.Application.RuntimeChrome</returns>
        RuntimeChrome ModifyRuntimeChrome(RuntimeChrome_Modify runtimeChrome);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgents</returns>
        ApplicationAgents EnumerateApplicationAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgent resource
        /// </summary>
        /// <param name="applicationId">The applicationId of the Application Agent</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgent</returns>
        ApplicationAgent GetApplicationAgent(string applicationId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgent Refresh resource
        /// </summary>
        /// <param name="applicationId">The applicationId of the Application Agent</param>
        /// <param name="refreshRequest">The Refresh Request to be executed</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgent_Refresh</returns>
        ApplicationAgent_Refresh RefreshApplicationAgent(RefreshRequest refreshRequest, string applicationId);

        I18nAssets EnumerateI18nAssets();

        I18nAsset GetI18nAsset(string i18nAssetId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoints resource
        /// </summary>
        /// <param name="queryParams">The query parameters that will be applied to the collection</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAccessPoints</returns>
        ApplicationAccessPoints EnumerateApplicationAccessPoints(string queryParams);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="accessPointId">The accessPointId of the Application Access Point</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAccessPoint</returns>
        ApplicationAccessPoint GetApplicationAccessPoint(string accessPointId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service InitiateLaunch resource
        /// </summary>
        /// <param name="accessPointId">The accessPointId of the Application Access Point</param>
        /// <param name="initiateLaunchRequest">The Initiate Launch Request for the application</param>
        /// <param name="startIntent">The Start Intent Value Initiate Launch Request</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAccessPoint_InitiateLaunch</returns>
        ApplicationAccessPoint_InitiateLaunch ApplicationAccessPointInitiateLaunch(string accessPointId, InitiateLaunchRequest initiateLaunchRequest, dynamic startIntent);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Message Center resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.MessageCenterAgents</returns>
        MessageCenterAgents EnumerateMessageCenterAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <returns>HP.Extensibility.Service.Application.MessageCenterAgent</returns>
        MessageCenterAgent GetMessageCenterAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <returns>HP.Extensibility.Service.Application.Messages</returns>
        Messages EnumerateMessages(string agentId, string queryParams);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageCreate">The message to create.</param>
        /// <returns>HP.Extensibility.Service.Application.Message</returns>
        Message CreateMessage(string agentId, Message_Create messageCreate);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageId">The messageId of the Message.</param>
        /// <returns>HP.Extensibility.Service.Application.Message</returns>
        Message GetMessage(string agentId, string messageId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageId">The messageId of the Message.</param>
        /// <returns>HP.Extensibility.Service.Base.DeleteContent</returns>
        DeleteContent DeleteMessage(string agentId, string messageId);
    }


    /// <summary>
    /// Implements the business logic of the Application examples
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;

        public ApplicationService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private ApplicationService() { }


        #endregion // Constructor


        #region IApplicationService implementation


        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.Capablilities</returns>
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;

        }

        #region Application RuntimeChrome
        public RuntimeChrome GetRuntimeChrome()
        {
            // @StartCodeExample:GetRuntimeChrome
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI-Context based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            RuntimeChrome runtimeChrome = client.ApplicationRuntime.CurrentContext.RuntimeChrome.GetAsync(accessToken).Result;
            // @EndCodeExample

            return runtimeChrome;
        }

        public RuntimeChrome ReplaceRuntimeChrome(RuntimeChrome_Replace runtimeChromeReplace)
        {
            // @StartCodeExample:ReplaceRuntimeChrome
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI-Context based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            RuntimeChrome runtimeChrome = client.ApplicationRuntime.CurrentContext.RuntimeChrome.ReplaceAsync(accessToken, runtimeChromeReplace).Result;
            // @EndCodeExample

            return runtimeChrome;
        }

        public RuntimeChrome ModifyRuntimeChrome(RuntimeChrome_Modify runtimeChromeModify)
        {
            // @StartCodeExample:ModifyRuntimeChrome
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI-Context based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            RuntimeChrome runtimeChrome = client.ApplicationRuntime.CurrentContext.RuntimeChrome.ModifyAsync(accessToken, runtimeChromeModify).Result;
            // @EndCodeExample

            return runtimeChrome;
        }
        #endregion //Application RuntimeChrome

        #region Application Runtime
        public ApplicationRuntime GetApplicationRuntime()
        {
            // @StartCodeExample:GetApplicationRuntime
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
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            ApplicationRuntime applicationRuntime = client.ApplicationRuntime.GetAsync(accessToken).Result;
            // @EndCodeExample

            return applicationRuntime;
        }

        public ApplicationRuntime_Reset ResetApplicationRuntime(ResetRequest resetRequest)
        {
            // @StartCodeExample:ResetApplicationRuntime
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            ApplicationRuntime_Reset applicationRuntime_Reset = client.ApplicationRuntime.Reset.ExecuteAsync(accessToken, resetRequest).Result;

            // Reset called successfully, reset the UiContext AccessToken as it is no longer available.
            deviceManagementService.SetUiContextAccessToken(null);
            // @EndCodeExample

            return applicationRuntime_Reset;
        }
        #endregion //Application Runtime

        #region Application Runtime CurrentContext
        public CurrentContext GetCurrentContext()
        {
            // @StartCodeExample:GetCurrentContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation supports Administrator or SolutionProvider based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            CurrentContext currentContext = client.ApplicationRuntime.CurrentContext.GetAsync(accessToken).Result;
            // @EndCodeExample

            return currentContext;
        }

        public CurrentContext_Exit ExitCurrentContext(ExitRequest exitRequest)
        {
            // @StartCodeExample:ExitCurrentContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI-Context based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            CurrentContext_Exit currentContext_Exit = client.ApplicationRuntime.CurrentContext.Exit.ExecuteAsync(accessToken, new ExitRequest()).Result;

            // Exit called successfully, reset the UiContext AccessToken as it is no longer available.
            deviceManagementService.SetUiContextAccessToken(null);
            // @EndCodeExample

            return currentContext_Exit;
        }

        public CurrentContext_ResetInactivityTimer ResetInactivityTimerCurrentContext(ResetInactivityTimerRequest resetRequest)
        {
            // @StartCodeExample:ResetInactivityTimerCurrentContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI-Context based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            CurrentContext_ResetInactivityTimer currentContext_ResetInactivityTimer = client.ApplicationRuntime.CurrentContext.ResetInactivityTimer.ExecuteAsync(accessToken, resetRequest).Result;
            // @EndCodeExample

            return currentContext_ResetInactivityTimer;

        }

        public StartIntent GetStartIntent()
        {
            // @StartCodeExample:GetStartIntent
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            StartIntent startIntent = client.ApplicationRuntime.CurrentContext.StartIntent.GetAsync(accessToken).Result;
            // @EndCodeExample

            return startIntent;
        }

        #endregion //Application Runtime CurrentContext

        #region Application Agents
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgents</returns>
        public ApplicationAgents EnumerateApplicationAgents()
        {
            // @StartCodeExample:EnumerateApplicationAgents
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            ApplicationAgents agents = client.ApplicationAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return agents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgent resource
        /// </summary>
        /// <param name="applicationId">The applicationId of the Application Agent</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgent</returns>
        public ApplicationAgent GetApplicationAgent(string applicationId)
        {
            // @StartCodeExample:GetApplicationAgent
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            ApplicationAgent agent = client.ApplicationAgents[applicationId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return agent;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAgent Refresh resource
        /// </summary>
        /// <param name="applicationId">The applicationId of the Application Agent</param>
        /// <param name="refreshRequest">The Refresh Request to be executed</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAgent_Refresh</returns>
        public ApplicationAgent_Refresh RefreshApplicationAgent(RefreshRequest refreshRequest, string applicationId)
        {
            // @StartCodeExample:RefreshApplicationAgent
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            ApplicationAgent_Refresh refreshResponse = client.ApplicationAgents[applicationId].RefreshOperation.ExecuteAsync(accessToken, refreshRequest).Result;
            // @EndCodeExample

            return refreshResponse;
        }
        #endregion //Application Agents

        #region I18Assets
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service I18nAssets resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.I18nAssets</returns>
        public I18nAssets EnumerateI18nAssets()
        {
            // @StartCodeExample:EnumerateI18nAssets
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            I18nAssets assets = client.I18nAssets.GetAsync(accessToken).Result;
            // @EndCodeExample

            return assets;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service I18nAsset resource
        /// </summary>
        /// <param name="i18nAssetId">The applicationId of the Application Agent</param>
        /// <returns>HP.Extensibility.Service.Application.I18nAsset</returns>

        public I18nAsset GetI18nAsset(string i18nAssetId)
        {
            // @StartCodeExample:GetI18nAsset
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            I18nAsset asset = client.I18nAssets[i18nAssetId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return asset;
        }
        #endregion //I18Assets

        #region Application Access Points
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoints resource
        /// </summary>
        /// <param name="queryParams">The query parameters that will be applied to the collection</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAccessPoints</returns>
        public ApplicationAccessPoints EnumerateApplicationAccessPoints(string queryParams)
        {
            // @StartCodeExample:EnumerateApplicationAccessPoints
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            ApplicationAccessPoints accessPoints = client.ApplicationAccessPoints.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return accessPoints;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="accessPointId">The accessPointId of the Application Access Point</param>
        /// <returns>HP.Extensibility.Service.Application.ApplicationAccessPoint</returns>
        public ApplicationAccessPoint GetApplicationAccessPoint(string accessPointId)
        {
            // @StartCodeExample:GetApplicationAccessPoint
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // This operation requires Administrator or SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            ApplicationAccessPoint accessPoint = client.ApplicationAccessPoints[accessPointId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return accessPoint;
        }

        public ApplicationAccessPoint_InitiateLaunch ApplicationAccessPointInitiateLaunch(string accessPointId, InitiateLaunchRequest initiateLaunchRequest, dynamic startIntent)
        {
            // @StartCodeExample:ApplicationAccessPointInitiateLaunch
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider based grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery servicesDiscovery = discoveryClient.ServicesDiscovery.GetAsync().Result;

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, servicesDiscovery);

            ApplicationAccessPoint_InitiateLaunch applicationAccessPoint_InitiateLaunch = client.ApplicationAccessPoints[accessPointId].InitiateLaunch.ExecuteAsync(accessToken, initiateLaunchRequest, startIntent).Result;
            // @EndCodeExample

            return applicationAccessPoint_InitiateLaunch;
        }
        #endregion // Application Access Points

        #region Message Center

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service Message Center resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Application.MessageCenterAgents</returns>
        public MessageCenterAgents EnumerateMessageCenterAgents()
        {
            // @StartCodeExample:EnumerateMessageCenterAgents
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            MessageCenterAgents agents = client.MessageCenterAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return agents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <returns>HP.Extensibility.Service.Application.MessageCenterAgent</returns>
        public MessageCenterAgent GetMessageCenterAgent(string agentId)
        {
            // @StartCodeExample:GetMessageCenterAgent
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            MessageCenterAgent agent = client.MessageCenterAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return agent;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <returns>HP.Extensibility.Service.Application.Messages</returns>
        public Messages EnumerateMessages(string agentId, string queryParams)
        {
            // @StartCodeExample:EnumerateMessages
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Messages messages = client.MessageCenterAgents[agentId].Messages.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return messages;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageCreate">The message to create.</param>
        /// <returns>HP.Extensibility.Service.Application.Message</returns>
        public Message CreateMessage(string agentId, Message_Create messageCreate)
        {
            // @StartCodeExample:CreateMessage
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Message message = client.MessageCenterAgents[agentId].Messages.CreateAsync(accessToken, messageCreate).Result;
            // @EndCodeExample

            return message;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageId">The messageId of the Message.</param>
        /// <returns>HP.Extensibility.Service.Application.Message</returns>
        public Message GetMessage(string agentId, string messageId)
        {
            // @StartCodeExample:GetMessage
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Message message = client.MessageCenterAgents[agentId].Messages[messageId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return message;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Application Service ApplicationAccessPoint resource
        /// </summary>
        /// <param name="agentId">The agentId of the Message Center Agent.</param>
        /// <param name="messageId">The messageId of the Message.</param>
        /// <returns>HP.Extensibility.Service.Base.DeleteContent</returns>
        public DeleteContent DeleteMessage(string agentId, string messageId)
        {
            // @StartCodeExample:DeleteMessage
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

            ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            DeleteContent deleteContent = client.MessageCenterAgents[agentId].Messages[messageId].DeleteAsync(accessToken).Result;
            // @EndCodeExample

            return deleteContent;
        }
        #endregion
        #endregion // IApplicationService implementation

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            ApplicationServiceClient client = null;

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
            client = new ApplicationServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockApplication");
            client = new ApplicationServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        public void ExceptionHandlingExample()
        {
            Device currentDevice = deviceManagementService.CurrentDevice;
            string queryParams = string.Empty;
            AccessToken accessToken = string.Empty;

            // @StartCodeExample:ExceptionHandlingExample
            try
            {
                DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

                ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

                ApplicationServiceClient client = new ApplicationServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

                ApplicationAccessPoints accessPoints = client.ApplicationAccessPoints.GetAsync(accessToken, queryParams).Result;
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
