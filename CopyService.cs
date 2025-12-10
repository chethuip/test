/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.Copy;
using HP.Extensibility.Service.Copy;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;
using HP.Extensibility.Types.OptionProfile;
using System.Collections.Generic;
using HP.Extensibility.Types.Imaging;
using HP.Extensibility.Types.Media;
using Newtonsoft.Json;
using System.Linq;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic of the Copy Service examples
    /// </summary>
    public interface ICopyService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service Capabilities resource
        /// </summary>
        /// <returns>Types.Copy.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service Default Options resource
        /// </summary>
        /// <returns>Types.Copy.DefaultOptions</returns>
        DefaultOptions GetDefaultOptions();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service Profile resource
        /// Retrieves the Copy Ticket Profile, which defines the valid copy ticket options and their constraints. 
        /// </summary>
        /// <returns>Types.Copy.Profile</returns>
        Profile GetProfile();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyAgents resource
        /// Retrieves the CopyAgents registered on the device.
        /// </summary>
        /// <returns>Types.Copy.CopyAgents</returns>
        CopyAgents EnumerateCopyAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyAgent resource
        /// Retrieves the CopyAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the CopyAgent</param>
        /// <returns>Types.Copy.CopyAgent</returns>
        CopyAgent GetCopyAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyJobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy jobs</param>
        /// <returns>Types.Copy.CopyJobs</returns>
        CopyJobs EnumerateCopyJobs(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the create CopyJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy jobs</param>
        /// <param name="copyJob_Create">The object to create the copyJob from</param>
        /// <returns>HP.Extensibility.Service.Copy.CopyJobs</returns>
        CopyJob CreateCopyJob(string agentId, CopyJob_Create copyJob_Create);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy agent</param>
        /// <param name="copyJobId">The copyJobId of the copy job</param>
        /// <returns>Types.Copy.CopyJob</returns>
        CopyJob GetCopyJob(string agentId, string copyJobId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyJob Cancel resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="copyJobId">The copyJobId of the copy job</param>
        /// <returns>Types.Copy.CopyJob_Cancel</returns>
        CopyJob_Cancel CancelCopyJob(string agentId, string copyJobId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyTicketHelper resource
        /// </summary>
        /// <param name="copyOptions">The copy options to be verfied by the ticket helper</param>
        /// <returns>Types.OptionProfile.OptionRuleNotification</returns>
        List<OptionRuleNotification> VerifyCopyTicket(CopyOptions copyOptions);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyTicketHelper resource
        /// </summary>
        /// <param name="copyOptions">The copy options to be verfied by the ticket helper</param>
        public void VerifyCopyTicketExamples(CopyOptions copyOptions);
    }


    /// <summary>
    /// Implements the business logic of the Copy examples
    /// </summary>
    public class CopyService : ICopyService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory = null;
        private IDeviceManagementService deviceManagementService;

        public CopyService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private CopyService() { }

        #endregion // Constructor


        #region ICopyService implementation

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service Capabilities resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Copy.Capabilities</returns>
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

            // Construct the CopyServiceClient
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service DefaultOptions resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Copy.DefaultOptions</returns>
        public DefaultOptions GetDefaultOptions()
        {
            // @StartCodeExample:GetDefaultOptions
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires SolutionProvider
            string accessToken = currentDevice.GetToken(
                AccessTokenType.Solution
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the ScanJobServiceClient
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            DefaultOptions defaultOptions = client.DefaultOptions.GetAsync(accessToken).Result;
            // @EndCodeExample

            return defaultOptions;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service Profile Resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Copy.Profile</returns>
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Profile profile = client.Profile.GetAsync(accessToken).Result;
            // @EndCodeExample

            return profile;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyAgents resource
        /// </summary>
        /// <returns>HP.Extensibility.Service.Copy.CopyAgents</returns>
        public CopyAgents EnumerateCopyAgents()
        {
            // @StartCodeExample:EnumerateCopyAgents
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            CopyAgents copyAgents = client.CopyAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return copyAgents;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyAgent Resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy agent</param>
        /// <returns>HP.Extensibility.Service.Copy.CopyAgent</returns>
        public CopyAgent GetCopyAgent(string agentId)
        {
            // @StartCodeExample:GetCopyAgent
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            CopyAgent copyAgent = client.CopyAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return copyAgent;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyJobs resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy jobs</param>
        /// <returns>Types.Copy.CopyJobs</returns>
        public CopyJobs EnumerateCopyJobs(string agentId)
        {
            // @StartCodeExample:EnumerateCopyJobs
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            CopyJobs copyJobs = client.CopyAgents[agentId].CopyJobs.GetAsync(accessToken).Result;
            // @EndCodeExample

            return copyJobs;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the create CopyJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy jobs</param>
        /// <param name="copyJob_Create">The object to create the copyJob from</param>
        /// <returns>HP.Extensibility.Service.Copy.CopyJobs</returns>
        public CopyJob CreateCopyJob(string agentId, CopyJob_Create copyJob_Create)
        {
            // @StartCodeExample:CreateCopyJob
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the create operation
            CopyJob copyJob = client.CopyAgents[agentId].CopyJobs.CreateAsync(accessToken, copyJob_Create).Result;
            // @EndCodeExample

            return copyJob;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the Copy Service CopyJob resource
        /// </summary>
        /// <param name="agentId">The agentId of the copy agent</param>
        /// <param name="copyJobId">The copyJobId of the copy job</param>
        /// <returns>Types.Copy.CopyJob</returns>
        public CopyJob GetCopyJob(string agentId, string copyJobId)
        {
            // @StartCodeExample:GetCopyJob
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            CopyJob copyJob = client.CopyAgents[agentId].CopyJobs[copyJobId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return copyJob;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyJob Cancel resource
        /// </summary>
        /// <param name="agentId">The agentId of the scan agent</param>
        /// <param name="copyJobId">The copyJobId of the copy job</param>
        /// <returns>Types.Copy.CopyJob_Cancel</returns>
        public CopyJob_Cancel CancelCopyJob(string agentId, string copyJobId)
        {
            // @StartCodeExample:CancelCopyJob
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
            CopyServiceClient client = new CopyServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Cancel operation
            CopyJob_Cancel copyJobCancel = client.CopyAgents[agentId].CopyJobs[copyJobId].Cancel.ExecuteAsync(accessToken).Result;
            // @EndCodeExample

            return copyJobCancel;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyTicketHelper resource
        /// </summary>
        /// <param name="copyOptions">The copy options to be verfied by the ticket helper</param>
        /// <returns>Types.OptionProfile.OptionRuleNotification</returns>
        public List<OptionRuleNotification> VerifyCopyTicket(CopyOptions copyOptions)
        {
            // @StartCodeExample:VerifyCopyTicket
            // Start by getting the profile options
            Profile profile = GetProfile();
            OptionProfile baseOptionProfile = new OptionProfile();
            baseOptionProfile.Definitions = new List<OptionDefinition>();
            if (profile.Definitions != null)
            {
                baseOptionProfile.Definitions = profile.Definitions;
            }

            // Create the copy ticket helper
            CopyTicketHelper target = new CopyTicketHelper(baseOptionProfile);

            // Set incoming scan options
            target.BaseCopyOptions = copyOptions;

            // Check for conflicts
            // If conflicts.Count > 0, the copy ticket is invalid
            List<OptionRuleNotification> conflicts = target.BaseCopyOptionsProfileHelper.GetConflicts();
            // @EndCodeExample

            return conflicts;
        }

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the CopyTicketHelper resource
        /// </summary>
        /// <param name="copyOptions">The copy options to be verfied by the ticket helper</param>
        public void VerifyCopyTicketExamples(CopyOptions copyOptions)
        {
            // @StartCodeExample:VerifyCopyTicketExamples
            // Start by getting the profile options
            Profile profile = GetProfile();
            OptionProfile baseOptionProfile = new OptionProfile() { Definitions = profile.Definitions };

            // Create the copy ticket helper
            CopyTicketHelper target = new CopyTicketHelper(baseOptionProfile);

            //
            // Let's assume we have a very simple group of settings in our app:
            //  * OriginalMediaSize
            //  * ColorMode
            //  * Copies
            // The first thing we ought to do is make sure these three CopyOptions are available (supported) on this device.
            // We can do this by making sure the option (names) are either IN the AvailableOptions, or NOT IN the UnavailableOptions.
            // Let's check the latter...
            //

            var unavailableOptions = target.BaseCopyOptionsProfileHelper.GetUnavailableOptions();

            if (unavailableOptions.Contains("OriginalMediaSize") || unavailableOptions.Contains("ColorMode") || unavailableOptions.Contains("Copies"))
            {
                throw new InvalidOperationException("One or more of the needed options is unavailable");
            }

            //
            // Ok. Everything we want is supported. Now, we know the first two are enumerations, so lets see what the possible-values
            // are so that we can populate our combo boxes. The OptionProfile has that information, so use the helper to extract it.
            // There are two variants - one method returns a simple list of strings, the other will return the list of the explict type
            // we want. We'll use the latter in this demo!
            //

            target.BaseCopyOptionsProfileHelper.Evaluate();

            var possibleOriginalMediaSizes = target.BaseCopyOptionsProfileHelper.GetPossibleValues<MediaSizeId>("OriginalMediaSize");
            var possibleColorModes = target.BaseCopyOptionsProfileHelper.GetPossibleValues<ColorMode>("ColorMode");

            if (possibleOriginalMediaSizes.Count != 22)
            {
                throw new InvalidOperationException("The expected original media size types are not available");
            }

            if (possibleColorModes.Count != 3 || !possibleColorModes.Contains(ColorMode.Parse("cmColor")) ||
                !possibleColorModes.Contains(ColorMode.Parse("cmGrayscale")) || !possibleColorModes.Contains(ColorMode.Parse("cmAutoDetect")))
            {
                throw new InvalidOperationException("The expected color modes are not available");
            }

            //
            // Alright, lets now validate Copies
            //

            var copiesRange = target.BaseCopyOptionsProfileHelper.GetCurrentRange("Copies");
            if (copiesRange.UpperBoundary != 999 || copiesRange.LowerBoundary != 1 || copiesRange.Step != 1)
            {
                throw new InvalidOperationException("The expected copies range rule is incorrect");
            }

            //
            // Alright, we have our ComboBoxes ready. It's time to load the helper with a "live" instance we want to 
            // be using to hold our settings from the view. So we set the BaseCopyOptions property of the helper
            // with a CopyOptions instance we created. This will be the instance, then, that is used during evaluations of the
            // BaseCopyOptionsProfileHelper member. We'll use the "default" instance from the beginning of this demo.
            //
            target.BaseCopyOptions = copyOptions;

            // Hopefully the default CopyOptions are valid! Let's check
            if (!target.BaseCopyOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The copy options should be valid");
            }

            //
            // Great! Next, let's do a check to see if any of the three options we are supporting in our view are disabled.
            // Being disabled simply means it is not relevant based on the current state of all CopyOptions.
            //

            var disabled = target.BaseCopyOptionsProfileHelper.GetDisabledOptions();

            if (disabled.Contains("OriginalMediaSize") || disabled.Contains("ColorMode") || disabled.Contains("Copies"))
            {
                throw new InvalidOperationException("The options shouldn't be disabled");
            }

            //
            // How about checking what our current *available* values are for OriginalMediaSize and ColorMode?  Because
            // recall that available-values might be a subset of total possible-values based on the optionProfile.
            // (In this demo we know there is a difference, so we'll assert it)

            var availableOriginalMediaSizes = target.BaseCopyOptionsProfileHelper.GetCurrentAvailableValues<MediaSizeId>("OriginalMediaSize");
            var availableColorModes = target.BaseCopyOptionsProfileHelper.GetCurrentAvailableValues<ColorMode>("ColorMode");

            if (availableOriginalMediaSizes.Count != 22)
            {
                throw new InvalidOperationException("The expected original media sizes are not available");
            }

            if (availableColorModes.Count != 3 || !availableColorModes.Contains(ColorMode.Parse("cmColor")) ||
                !availableColorModes.Contains(ColorMode.Parse("cmGrayscale")) || !availableColorModes.Contains(ColorMode.Parse("cmAutoDetect")))
            {
                throw new InvalidOperationException("The expected original color modes are not available");
            }

            //
            // So the above has shown that the current available values of ColorMode is Color and Grayscale, based on the
            // current state of the options instance.
            //

            //
            // Let's change the color mode to Monochrome, now that it's available.
            // (which we know in this demo will trigger a conflict and make the options no longer valid!)
            //
            copyOptions.ColorMode = ColorMode.CmMonochrome;
            if (target.BaseCopyOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The copy options should be invalid");
            }

            //
            // Why are we invalid? It's all explained by the Helper :)
            //

            var conflicts = target.BaseCopyOptionsProfileHelper.CurrentNotifications.FindAll(o => o.NotificationType == OptionRuleNotificationType.OptionConflict);
            if (!string.Equals("ColorMode", conflicts[0].OptionName, System.StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The first conflict should be for ColorMode");
            }

            //
            // Ok, then, let's change ColorMode back to cmColor
            //
            copyOptions.ColorMode = ColorMode.CmColor;
            if (!target.BaseCopyOptionsProfileHelper.IsValid())
            {
                throw new InvalidOperationException("The copy options should be valid");
            }

            //
            // Finally, let's create a ticket with these options and validate the ticket
            //
            CopyJobTicket ticket = new CopyJobTicket();
            ticket.CopyOptions = copyOptions;

            if (!target.IsTicketValid(ticket))
            {
                throw new InvalidOperationException("The copy options should be valid");
            }
            // @EndCodeExample
        }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            CopyServiceClient client = null;

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
            client = new CopyServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockCopy");
            client = new CopyServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        #endregion // ICopyExamplesService implementation
    }
}
