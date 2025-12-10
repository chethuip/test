/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 * *********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.SolutionManager;
using HP.Extensibility.Service.SolutionManager;
using HP.Extensibility.Types.Base;
using HP.Extensibility.Types.Common;
using HP.Extensibility.Types.SolutionManager;
using OXPd2ExamplesHost.Models;
using System;
using System.IO;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Solution Manager examples
    /// </summary>
    public interface ISolutionManagerService
    {

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Solution Manager Service Capabilities resource
        /// </summary>
        Capabilities GetCapabilities();

        Solutions EnumerateSolutions(string queryParams);

        Solution GetSolution(string solutionId);

        Solution_ReissueInstallCode ReissueInstallCode(string solutionId);

        Context GetSolutionContext(string solutionId);

        Context ModifySolutionContext(string solutionId, Context_Modify resource);

        Context ReplaceSolutionContext(string solutionId, Context_Replace resource);

        Configuration GetConfiguration(string solutionId, string queryParams);

        Configuration ModifyConfiguration(string solutionId, Configuration_Modify resource);

        Tuple<Data, byte[]> GetConfigurationData(string solutionId, string queryParams);

        Data ReplaceConfigurationData(string solutionId, Stream dataStream, string mimeType);

        Installer GetInstaller();

        Installer_InstallSolution InstallSolution(InstallSolutionRequest installSolutionRequest, Stream solutionBundle, string solutionBundleFilename, Context_Replace context);

        Installer_InstallRemote InstallRemote(InstallRemoteRequest installRequest, RemoteArchive remoteArchive);

        Installer_UninstallSolution UninstallSolution(UninstallSolutionRequest uninstallSolutionRequest);

        InstallerOperations EnumerateInstallerOperations(string queryParams);

        InstallerOperation GetInstallerOperation(string operationId);

        CertificateAuthorities EnumerateCertificateAuthorities(string solutionId, string queryParams);

        Tuple<CertificateAuthorities_Export, byte[]> ExportCertificateAuthorities(string solutionId);

        CertificateAuthorities_Import ImportCertificateAuthority(string solutionId, CertificateAuthoritiesImportRequest importRequest, Stream certificate, string certificateFilename);

        CertificateAuthority GetCertificateAuthority(string solutionId, string certificateId);

        DeleteContent DeleteCertificateAuthority(string solutionId, string certificateId);

        Tuple<CertificateAuthority_Export, byte[]> ExportCertificateAuthority(string solutionId, string certificateId);

        RuntimeRegistrations EnumerateRuntimeRegistrations(string solutionId, string queryParams);

        RuntimeRegistration CreateRuntimeRegistration(string solutionId, RuntimeRegistration_Create resource, string queryParams);

        RuntimeRegistration GetRuntimeRegistration(string solutionId, string recordId, string queryParams);

        DeleteContent DeleteRuntimeRegistration(string solutionId, string recordId);
    }

    /// <summary>
    /// Implements the business logic of the Solution Manager examples
    /// </summary>
    public class SolutionManagerService : ISolutionManagerService
    {

        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;
        #region Construction

        public SolutionManagerService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private SolutionManagerService() { }

        #endregion // Construction

        #region ISolutionManagerService implementation

        #region Solutions
        public Solutions EnumerateSolutions(string queryParams)
        {
            // @StartCodeExample:EnumerateSolutions
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator scope, so use the token if available
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Solutions solutions = solutionManagerClient.Solutions.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return solutions;
        }

        public Solution GetSolution(string solutionId)
        {
            // @StartCodeExample:GetSolution
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider-Owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Solution solution = solutionManagerClient.Solutions[solutionId].GetAsync(accessToken).Result;
            // @EndCodeExample
            return solution;

        }

        public Solution_ReissueInstallCode ReissueInstallCode(string solutionId)
        {
            // @StartCodeExample:ReissueInstallCode
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice || null == currentDevice.NetworkAddress)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            // Construct the HttpClientHandler and HttpClient
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Solution_ReissueInstallCode response = solutionManagerClient.Solutions[solutionId].ReissueInstallCode.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return response;
        }

        public Context GetSolutionContext(string solutionId)
        {
            // @StartCodeExample:GetSolutionContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider-Owner grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Context context = solutionManagerClient.Solutions[solutionId].Context.GetAsync(accessToken).Result;
            // @EndCodeExample

            return context;
        }

        public Context ModifySolutionContext(string solutionId, Context_Modify resource)
        {
            // @StartCodeExample:ModifySolutionContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Context context = solutionManagerClient.Solutions[solutionId].Context.ModifyAsync(accessToken, resource).Result;
            // @EndCodeExample

            return context;
        }

        public Context ReplaceSolutionContext(string solutionId, Context_Replace resource)
        {
            // @StartCodeExample:ReplaceSolutionContext
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Context context = solutionManagerClient.Solutions[solutionId].Context.ReplaceAsync(accessToken, resource).Result;
            // @EndCodeExample

            return context;
        }

        public Configuration GetConfiguration(string solutionId, string queryParams)
        {
            // @StartCodeExample:GetConfiguration
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider-Owner grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Configuration configuration = solutionManagerClient.Solutions[solutionId].Configuration.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return configuration;
        }

        public Configuration ModifyConfiguration(string solutionId, Configuration_Modify resource)
        {
            // @StartCodeExample:ModifyConfiguration
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Configuration configuration = solutionManagerClient.Solutions[solutionId].Configuration.ModifyAsync(accessToken, resource).Result;
            // @EndCodeExample

            return configuration;
        }

        public Tuple<Data, byte[]> GetConfigurationData(string solutionId, string queryParams)
        {
            // @StartCodeExample:GetConfigurationData
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider-Owner grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Tuple<Data, byte[]> data = solutionManagerClient.Solutions[solutionId].Configuration.Data.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return data;
        }

        public Data ReplaceConfigurationData(string solutionId, Stream dataStream, string mimeType)
        {
            // @StartCodeExample:ReplaceConfigurationData
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider-Owner grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Data data = solutionManagerClient.Solutions[solutionId].Configuration.Data.ReplaceAsync(accessToken, dataStream, mimeType).Result;
            // @EndCodeExample

            return data;
        }

        public RuntimeRegistrations EnumerateRuntimeRegistrations(string solutionId, string queryParams)
        {
            // @StartCodeExample:EnumerateRuntimeRegistrations
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            RuntimeRegistrations registrations = solutionManagerClient.Solutions[solutionId].RuntimeRegistrations.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return registrations;
        }

        public RuntimeRegistration CreateRuntimeRegistration(string solutionId, RuntimeRegistration_Create runtimeRegistrationCreate, string queryParams)
        {
            // @StartCodeExample:CreateRuntimeRegistration
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
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            RuntimeRegistration registration = solutionManagerClient.Solutions[solutionId].RuntimeRegistrations.CreateAsync(accessToken, runtimeRegistrationCreate, queryParams).Result;
            // @EndCodeExample

            return registration;
        }

        public RuntimeRegistration GetRuntimeRegistration(string solutionId, string recordId, string queryParams)
        {
            // @StartCodeExample:GetRuntimeRegistration
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            RuntimeRegistration registration = solutionManagerClient.Solutions[solutionId].RuntimeRegistrations[recordId].GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return registration;
        }

        public DeleteContent DeleteRuntimeRegistration(string solutionId, string recordId)
        {
            // @StartCodeExample:DeleteRuntimeRegistration
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            DeleteContent result = solutionManagerClient.Solutions[solutionId].RuntimeRegistrations[recordId].DeleteAsync(accessToken).Result;
            // @EndCodeExample

            return result;
        }

        #endregion //Solutions

        #region Installer
        public Installer GetInstaller()
        {
            // @StartCodeExample:GetInstaller
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Fetch the installer
            Installer result = solutionManagerClient.Installer.GetAsync(accessToken).Result;
            // @EndCodeExample

            return result;
        }

        public Installer_InstallSolution InstallSolution(InstallSolutionRequest installSolutionRequest, Stream solutionBundle, string solutionBundleFilename, Context_Replace context)
        {
            // @StartCodeExample:InstallSolution
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Install operation
            solutionBundle.Seek(0, SeekOrigin.Begin);
            Installer_InstallSolution result = solutionManagerClient.Installer.InstallSolution.ExecuteAsync(accessToken, installSolutionRequest, solutionBundle, solutionBundleFilename, context, null).Result;
            // @EndCodeExample

            return result;
        }

        public Installer_InstallRemote InstallRemote(InstallRemoteRequest installRemoteRequest, RemoteArchive remoteArchive)
        {
            // @StartCodeExample:InstallRemote
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Installer_InstallRemote result = solutionManagerClient.Installer.InstallRemote.ExecuteAsync(accessToken, installRemoteRequest, remoteArchive, null).Result;
            // @EndCodeExample

            return result;
        }

        public Installer_UninstallSolution UninstallSolution(UninstallSolutionRequest uninstallSolutionRequest)
        {
            // @StartCodeExample:UninstallSolution
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Uninstall operation
            Installer_UninstallSolution result = solutionManagerClient.Installer.UninstallSolution.ExecuteAsync(accessToken, uninstallSolutionRequest, null).Result;

            // Uninstalled the solution, reset the Tokens as the solutionProvider token is no longer available.
            currentDevice.SolutionAccessTokenStatus = TokenStatus.None;
            currentDevice.SolutionAccessToken = null;
            // @EndCodeExample

            return result;
        }

        public InstallerOperations EnumerateInstallerOperations(string queryParams)
        {
            // @StartCodeExample:EnumerateInstallerOperations
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            /************************************************************************************
             * Create the query parameter string using the values passed in...
             *
             *  For example, using includeMembers and contentFilter value of:
             *
             *      contentFilter=["members/(operationId, operationType, operationState, ,solutionId)", "memberIds"]
             *
             *  would result in a response something like:
             *
             *   {
             *       "memberIds": [
             *           "ad5ede57-9768-4423-a703-c166b6b99702"
             *       ],
             *       "members": [
             *           {
             *               "operationId": "ad5ede57-9768-4423-a703-c166b6b99702",
             *               "operationState": "iosSucceeded",
             *               "operationType": "iotInstall",
             *               "solutionId": "7d4c135f-948c-4cb8-b2c7-7cc9e4276be9"
             *           }
             *       ]
             *   }
             **************************************************************************************/

            // Execute the Get operation
            InstallerOperations result = solutionManagerClient.Installer.InstallerOperations.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return result;
        }

        public InstallerOperation GetInstallerOperation(string operationId)
        {
            // @StartCodeExample:GetInstallerOperation
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Administrator or SolutionProvider owner grant
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            //Execute the Get operation
            InstallerOperation result = solutionManagerClient.Installer.InstallerOperations[operationId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return result;
        }
        #endregion //Installer

        public Capabilities GetCapabilities()
        {
            // @StartCodeExample:GetCapabilities
            // Fetch the current device we're working with (this an OXPd 2.0 Examples App abstraction)
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities result = solutionManagerClient.Capabilities.GetAsync().Result;

            // The SolutionPlatforms property provides the current "status" of the extensibility platforms
            // on this device..

            foreach (var platform in result.SolutionPlatforms)
            {
                // Check for the OXPd 2.0 platform
                if (platform.PlatformType == SolutionPlatformType.SptOXPd2)
                {
                    // Check to make sure it is available
                    if (platform.PlatformStatus == SolutionPlatformStatus.SpsAvailable)
                    {
                        //...
                    }
                }
            }
            // @EndCodeExample

            return result;
        }
        #endregion // ISolutionManagerService implementation

        #region CertificateAuthorities

        public CertificateAuthorities EnumerateCertificateAuthorities(string solutionId, string queryParams)
        {
            // @StartCodeExample:EnumerateCertificateAuthorities
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            CertificateAuthorities certificateAuthorities = solutionManagerClient.Solutions[solutionId].CertificateAuthorities.GetAsync(accessToken, queryParams).Result;
            // @EndCodeExample

            return certificateAuthorities;
        }

        public Tuple<CertificateAuthorities_Export, byte[]> ExportCertificateAuthorities(string solutionId)
        {
            // @StartCodeExample:ExportCertificateAuthorities
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Tuple<CertificateAuthorities_Export, byte[]> certificateAuthorities = solutionManagerClient.Solutions[solutionId].CertificateAuthorities.Export.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return certificateAuthorities;
        }

        public CertificateAuthorities_Import ImportCertificateAuthority(string solutionId, CertificateAuthoritiesImportRequest importRequest, Stream certificate, string certificateFilename)
        {
            // @StartCodeExample:ImportCertificateAuthority
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the SolutionManagerServiceClient
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Import operation
            certificate.Seek(0, SeekOrigin.Begin);
            CertificateAuthorities_Import result = solutionManagerClient.Solutions[solutionId].CertificateAuthorities.Import.ExecuteAsync(accessToken, importRequest, certificate, certificateFilename, null).Result;
            // @EndCodeExample

            return result;
        }

        public CertificateAuthority GetCertificateAuthority(string solutionId, string certificateId)
        {
            // @StartCodeExample:GetCertificateAuthority
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            CertificateAuthority certificateAuthority = solutionManagerClient.Solutions[solutionId].CertificateAuthorities[certificateId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return certificateAuthority;
        }

        public DeleteContent DeleteCertificateAuthority(string solutionId, string certificateId)
        {
            // @StartCodeExample:DeleteCertificateAuthority
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            DeleteContent result = solutionManagerClient.Solutions[solutionId].CertificateAuthorities[certificateId].DeleteAsync(accessToken).Result;
            // @EndCodeExample

            return result;
        }

        public Tuple<CertificateAuthority_Export, byte[]> ExportCertificateAuthority(string solutionId, string certificateId)
        {
            // @StartCodeExample:ExportCertificateAuthority
            // Fetch the current device we're working with
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires administrator or solution-owner
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Admin,
                AccessTokenType.Solution
            );

            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;
            SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Tuple<CertificateAuthority_Export, byte[]> certificateAuthority = solutionManagerClient.Solutions[solutionId].CertificateAuthorities[certificateId].Export.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return certificateAuthority;
        }

        #endregion

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            SolutionManagerServiceClient client = null;

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
            client = new SolutionManagerServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockSolutionManager");
            client = new SolutionManagerServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        public void ExceptionHandlingExample()
        {
            Device currentDevice = deviceManagementService.CurrentDevice;
            string solutionId = string.Empty;
            Context_Replace resource = null;
            AccessToken accessToken = string.Empty;

            // @StartCodeExample:ExceptionHandlingExample
            try
            {
                DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);

                ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

                SolutionManagerServiceClient solutionManagerClient = new SolutionManagerServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

                Context context = solutionManagerClient.Solutions[solutionId].Context.ReplaceAsync(accessToken, resource).Result;
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
