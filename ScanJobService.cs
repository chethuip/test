/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.ScanJob;
using HP.Extensibility.Service.ScanJob;
using HP.Extensibility.Types.Common;
using HP.Extensibility.Types.OptionProfile;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;
using HP.Extensibility.Types.Target;
using HP.Extensibility.Types.Imaging;
using System.Linq;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Scan Job Service examples
    /// </summary>
    public interface IScanJobService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service Capabilities resource
        /// </summary>
        /// <returns>Types.ScanJob.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service Default Options resource
        /// </summary>
        /// <returns>Types.ScanJob.DefaultOptions</returns>
        DefaultOptions GetDefaultOptions();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanAgents resource
        /// Retrieves the ScanAgents registered on the device.
        /// </summary>
        /// <returns>Types.ScanJob.ScanAgents</returns>
        ScanJobAgents EnumerateScanJobAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanJobAgent resource
        /// Retrieves the ScanAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the ScanAgent</param>
        /// <returns>Types.ScanJob.ScanAgent</returns>
        ScanJobAgent GetScanJobAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanJobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan jobs</param>
        /// <returns>Types.ScanJob.ScanJobs</returns>
        ScanJobs EnumerateScanJobs(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the create ScanJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan jobs</param>
        /// <param name="scanJob_Create">The object to create the scanJob from</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanJobs</returns>
        ScanJob CreateScanJob(string agentId, ScanJob_Create scanJob_Create);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="scanJobId">The scanJobId of the scan job</param>
        /// <returns>Types.ScanJob.ScanJob</returns>
        ScanJob GetScanJob(string agentId, string scanJobId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Cancel resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="scanJobId">The scanJobId of the scan job</param>
        /// <returns>Types.ScanJob.ScanJob_Cancel</returns>
        ScanJob_Cancel CancelScanJob(string agentId, string scanJobId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service Profile resource
        /// Retrieves the Scan Ticket Profile, which defines the valid scan ticket options and their constraints. 
        /// </summary>
        /// <returns>Types.ScanJob.Profile</returns>
        Profile GetProfile();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanTicketHelper resource
        /// </summary>
        /// <param name="scanOptions">The scan options to be verfied by the ticket helper</param>
        /// <returns>Types.OptionProfile.OptionRuleNotification</returns>
        List<OptionRuleNotification> VerifyScanTicket(ScanOptions scanOptions);
    }


    /// <summary>
    /// Implements the business logic of the ScanJob examples
    /// </summary>
    public class ScanJobService : IScanJobService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public ScanJobService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private ScanJobService() { }

        #endregion // Constructor

        #region IScanJobService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.ScanJob.Capabilities</returns>
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service DefaultOptions resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.ScanJob.DefaultOptions</returns>
        public DefaultOptions GetDefaultOptions()
        {
            // @StartCodeExample:GetDefaultOptions
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // // This operation requires SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            DefaultOptions defaultOptions = client.DefaultOptions.GetAsync(accessToken).Result;
            // @EndCodeExample

            return defaultOptions;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanJobAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanAgents</returns>
        public ScanJobAgents EnumerateScanJobAgents()
        {
            // @StartCodeExample:EnumerateScanJobAgents
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            ScanJobAgents scanJobAgents = client.ScanJobAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return scanJobAgents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Scan Job Service ScanJobAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanAgent</returns>
        public ScanJobAgent GetScanJobAgent(string agentId)
        {
            // @StartCodeExample:GetScanJobAgent
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            ScanJobAgent scanJobAgent = client.ScanJobAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return scanJobAgent;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Service ScanJobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanJobs</returns>
        public ScanJobs EnumerateScanJobs(string agentId)
        {
            // @StartCodeExample:EnumerateScanJobs
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            ScanJobs scanJobs = client.ScanJobAgents[agentId].ScanJobs.GetAsync(accessToken).Result;
            // @EndCodeExample

            return scanJobs;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the create ScanJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan jobs</param>
        /// <param name="scanJob_Create">The object to create the scanJob from</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanJobs</returns>
        public ScanJob CreateScanJob(string agentId, ScanJob_Create scanJob_Create)
        {
            // @StartCodeExample:CreateScanJob
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires UI Context
            string accessToken = currentDevice.GetToken(
                AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the create operation
            ScanJob scanJob = client.ScanJobAgents[agentId].ScanJobs.CreateAsync(accessToken, scanJob_Create).Result;
            // @EndCodeExample

            return scanJob;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Scan Job Service ScanJob Resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="scanJobId">The scanJobId of the scan job</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanJob</returns>
        public ScanJob GetScanJob(string agentId, string scanJobId)
        {
            // @StartCodeExample:GetScanJob
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            ScanJob scanJob = client.ScanJobAgents[agentId].ScanJobs[scanJobId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return scanJob;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanJob Cancel resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="scanJobId">The scanJobId of the scan job</param>
        /// <returns>HP.Extensibility.Service.ScanJob.ScanJob_Cancel</returns>
        public ScanJob_Cancel CancelScanJob(string agentId, string scanJobId)
        {
            // @StartCodeExample:CancelScanJob
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

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Cancel operation
            ScanJob_Cancel scanJobCancel = client.ScanJobAgents[agentId].ScanJobs[scanJobId].Cancel.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return scanJobCancel;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Scan Job Service Profile Resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.ScanJob.Profile</returns>
        public Profile GetProfile()
        {
            // @StartCodeExample:GetProfile
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation is available to SolutionProviders
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the ScanJobServiceClient
            ScanJobServiceClient client = new ScanJobServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Profile profile = client.Profile.GetAsync(accessToken).Result;
            // @EndCodeExample

            return profile;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanTicketHelper resource
        /// </summary>
        /// <param name="scanOptions">The scan options to be verfied by the ticket helper</param>
        /// <returns>HP.Extensibility.Types.OptionProfile.OptionRuleNotification</returns>
        public List<OptionRuleNotification> VerifyScanTicket(ScanOptions scanOptions)
        {
            // @StartCodeExample:VerifyScanTicket
            // Start by getting the profile http and base options
            Profile profile = GetProfile();
            OptionProfile baseOptionProfile = profile.Base;
            OptionProfile httpOptionProfile = profile.Http;

            // Create the scan ticket helper
            ScanTicketHelper target = new ScanTicketHelper(baseOptionProfile, httpOptionProfile);

            // Set incoming scan options
            target.HttpScanOptions = scanOptions;

            // Check for conflicts
            // If conflicts.Count > 0, the scan ticket is invalid
            List<OptionRuleNotification> conflicts = target.HttpScanOptionsProfileHelper.GetConflicts();
            // @EndCodeExample

            return conflicts;
        }

        #endregion // IScanJobExamplesService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the ScanTicketHelper resource
        /// </summary>
        /// <param name="scanOptions">The scan options to be verfied by the ticket helper</param>
        /// <returns>HP.Extensibility.Types.OptionProfile.OptionRuleNotification</returns>
        public void VerifyScanTicketExamples(ScanOptions scanOptions)
        {
            // @StartCodeExample:VerifyScanTicketExamples
            // Start by getting the profile http and base options
            Profile profile = GetProfile();
            OptionProfile baseOptionProfile = profile.Base;
            OptionProfile httpOptionProfile = profile.Http;

            // Create the scan ticket helper
            ScanTicketHelper target = new ScanTicketHelper(baseOptionProfile, httpOptionProfile);

            // Set incoming scan options
            target.HttpScanOptions = scanOptions;

            //
            // Let's assume we have a very simple group of settings in our app:
            //  * OutputFileFormat
            //  * ColorMode
            //  * FileName
            // The first thing we ought to do is make sure these three ScanOptions are available (supported) on this device.
            // We can do this by making sure the option (names) are either IN the AvailableOptions, or NOT IN the UnavailableOptions.
            // Let's check the latter...
            //
            // Check which options are unavailable
            var unavailableOptions = target.HttpScanOptionsProfileHelper.GetUnavailableOptions();

            if (unavailableOptions.Contains("ColorMode") || unavailableOptions.Contains("OutputFileFormat") || unavailableOptions.Contains("FileName"))
            {
                throw new InvalidOperationException("One or more of the needed options is unavailable");
            }

            //
            // Ok. Everything we want is supported. Now, we know the first two are enumerations, so lets see what the possible-values
            // are so that we can populate our combo boxes. The OptionProfile has that information, so use the helper to extract it.
            // There are two variants - one method returns a simple list of strings, the other will return the list of the explict type
            // we want. We'll use the latter in this demo!
            //

            scanOptions.OutputFileFormat = FileFormat.FfJpeg; ;
            target.HttpScanOptionsProfileHelper.Evaluate();

            var availableFileTypes = target.HttpScanOptionsProfileHelper.GetCurrentAvailableValues<FileFormat>("OutputFileFormat");
            var availableColorModes = target.HttpScanOptionsProfileHelper.GetCurrentAvailableValues<ColorMode>("ColorMode");

            if (availableFileTypes.Count != 5 || !availableFileTypes.Contains(FileFormat.Parse("ffJpeg")) ||
                !availableFileTypes.Contains(FileFormat.Parse("ffPdf")) || !availableFileTypes.Contains(FileFormat.Parse("ffTiff")) ||
                 !availableFileTypes.Contains(FileFormat.Parse("ffMtiff")) || !availableFileTypes.Contains(FileFormat.Parse("ffPdfa")))
            {
                throw new InvalidOperationException("The expected files types are not available");
            }

            if (availableColorModes.Count != 2 || !availableColorModes.Contains(ColorMode.Parse("cmColor")) || !availableColorModes.Contains(ColorMode.Parse("cmGrayscale")))
            {
                throw new InvalidOperationException("The expected color modes are not available");
            }

            //
            // So the above has shown that the current available values of ColorMode is Color and Grayscale, based on the
            // current state of the options instance.
            //

            // Now let's see these notifications in action. Let's change the OutputFileFormat to PDF and evaluate the result.
            // (We know that this will trigger some notifications)

            scanOptions.OutputFileFormat = FileFormat.FfPdf;
            target.HttpScanOptionsProfileHelper.Evaluate();

            availableColorModes = target.HttpScanOptionsProfileHelper.GetCurrentAvailableValues<ColorMode>("ColorMode");

            if (availableColorModes.Count != 3 || !availableColorModes.Contains(ColorMode.Parse("cmColor")) ||
                !availableColorModes.Contains(ColorMode.Parse("cmGrayscale")) || !availableColorModes.Contains(ColorMode.Parse("cmMonochrome")))
            {
                throw new InvalidOperationException("The expected color modes are not available");
            }

            //
            // The above has shown that now with OutoutFileFormat of PDF, our available ColorModes is now all 3 modes, rather
            // than the restricted set that was available when OutputFileFormat was JPEG.
            //

            // Let's verify we are still valid
            if (!target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The scan ticket should be valid at this point");
            }

            //
            // Let's change the color mode to Monochrome, now that it's available.
            //
            scanOptions.ColorMode = ColorMode.CmMonochrome;
            if (!target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The scan ticket should still be valid at this point");
            }

            //
            // Now change the OutputFileFormat back to JPEG...
            // (which we know in this demo will trigger a conflict and make the options no longer valid!)
            //
            scanOptions.OutputFileFormat = FileFormat.FfJpeg;
            if (target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The scan ticket should now be invalid");
            }

            //
            // Why are we invalid? It's all explained by the Helper
            //

            List<OptionRuleNotification> conflicts = target.HttpScanOptionsProfileHelper.GetConflicts();
            if (conflicts[0].EnforcedRule.ValidValues.Message != "JPEG file format does not support Monochrome color mode.")
            {
                throw new InvalidOperationException("The first conflict message should be that JPEG file format does not support Monochrome color mode");
            }

            //
            // Ok, then, let's change OutputFileFormat back to PDF
            //
            scanOptions.OutputFileFormat = FileFormat.FfPdf;
            if (!target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The scan ticket should now be valid again");
            }

            //
            // Finally, let's try some different FileName settings... FileName is a complex option, as it is Bindable.
            // First let's try to set it to an explict empty-string.
            scanOptions.FileName.Explicit = new ScanOptions_FileName_Value() { ExplicitValue = "" };
            target.HttpScanOptionsProfileHelper.Evaluate();
            if (target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The scan ticket should invalid with an empty string");
            }

            conflicts = target.HttpScanOptionsProfileHelper.GetConflicts();
            var conflict = conflicts.FirstOrDefault(o => string.Equals("FileName.Explicit.ExplicitValue", o.OptionName, System.StringComparison.OrdinalIgnoreCase) && o.EnforcedRule.IsRegularExpression);

            if (!string.Equals("Explicit Filename must not be empty or include invalid characters.", conflict.EnforcedRule.RegularExpression.Message))
            {
                throw new InvalidOperationException("The conflict should mean that Filename is invalid");
            }

            //
            // Since that name is invalid, let's try an empty Expression instead
            //
            scanOptions.FileName.Expression = new ScanOptions_FileName_Expression() { ExpressionPattern = "" };
            target.HttpScanOptionsProfileHelper.Evaluate();
            if (target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("Again, the scan ticket should invalid with empty string");
            }

            conflicts = target.HttpScanOptionsProfileHelper.GetConflicts();
            conflict = conflicts.FirstOrDefault(o => string.Equals("FileName.Expression.ExpressionPattern", o.OptionName, System.StringComparison.OrdinalIgnoreCase) && o.EnforcedRule.IsStringLength);

            if (!string.Equals("Filename expression must not be empty.", conflict.EnforcedRule.StringLength.Message))
            {
                throw new InvalidOperationException("The conflict should mean that Filename is invalid");
            }

            //
            // Ok, let's change the FileName back to an explicit-value with something in it!
            //
            scanOptions.FileName.Explicit = new ScanOptions_FileName_Value() { ExplicitValue = "MyScanFile" };
            if (!target.HttpScanOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("scanOptions has a fileName so the ticket should be valid");
            }

            //
            // Finally, let's create a ticket with these options and validate the ticket
            //
            ScanTicket ticket = new ScanTicket();
            ticket.ScanOptions = scanOptions;

            HttpOptions httpOptions = new HttpOptions();
            httpOptions.Destination = new HttpDestination();
            httpOptions.Destination.Scheme = "http";
            httpOptions.Destination.Host = new HttpStyleHostCommon_Host_Binding(new HttpStyleHostCommon_Host_Value());
            httpOptions.Destination.Host.Explicit.ExplicitValue = "localhost";
            httpOptions.Destination.Path = new HttpStyleClientCommon_Path_Binding(new HttpStyleClientCommon_Path_Value());
            httpOptions.Destination.Path.Explicit.ExplicitValue = "/myScanDestinationPath";

            ticket.DestinationOptions = new DestinationOptions(httpOptions);

            if (!target.IsTicketValid(ticket))
            {
                throw new InvalidOperationException("This scan ticket should be valid and ready to use!");
            }
            // @EndCodeExample
        }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            ScanJobServiceClient client = null;

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
            client = new ScanJobServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockScanJob");
            client = new ScanJobServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }
    }
}
