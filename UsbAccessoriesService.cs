/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.UsbAccessories;
using HP.Extensibility.Service.UsbAccessories;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    public interface IUsbAccessoriesService
    {
        /// <summary>
        /// Demonstrate SDK use-case of interacting with the OXPd2 Usb Accessories Service Capabilities resource
        /// </summary>
        /// <returns>Service.UsbAccessories.Capablilities</returns>
        Capabilities GetCapabilities();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the UsbAccessories Service UsbAccessoriesAgents resource
        /// Retrieves the UsbAccessoriesAgents registered on the device.
        /// </summary>
        /// <returns>Types.UsbAccessories.UsbAccessoriesAgents</returns>
        UsbAccessoriesAgents EnumerateUsbAccessoriesAgents();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the UsbAccessories Service UsbAccessoriesAgent resource
        /// Retrieves the UsbAccessoriesAgent with the specified agentId.
        /// </summary>
        /// <param name="agentId">The agentId of the UsbAccessoriesAgent</param>
        /// <returns>Types.UsbAccessories.UsbAccessoriesAgent</returns>
        UsbAccessoriesAgent GetUsbAccessoriesAgent(string agentId);

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the UsbAccessories Service Accessories resource
        /// Retrieves the accessories attached to the device.
        /// </summary>
        /// <returns>Types.UsbAccessories.Accessories</returns>
        Accessories EnumerateUsbAccessories();

        /// <summary>
        /// Demonstrate SDK use-case of interacting with the UsbAccessories Service Accessory resource
        /// Retrieves the Accessory with the specified accessoryId.
        /// </summary>
        /// <param name="accessoryId">The accessoryId of the usb accessory</param>
        /// <returns>Types.UsbAccessories.Accessory</returns>
        Accessory GetUsbAccessory(string accessoryId);

        /// <summary>
        /// Returns a description of the HID accessory.
        /// </summary>
        /// <param name="accessoryId">The accessoryId of the usb accessory</param>
        /// <returns>Types.UsbAccessories.Hid</returns>
        Hid GetUsbAccessoryHid(string accessoryId);

        /// <summary>
        /// Opens the shared accessory and creates a resource representing the open accessory.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to open hid</param>
        /// <returns>Types.UsbAccessories.Hid_Open</returns>
        Hid_Open OpenSharedAccessoryHid(string accessoryId, Accessories_Accessory_Hid_Open_Params hidOpenParams);

        /// <summary>
        /// Opens the owned accessory and creates a resource representing the open accessory.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to open hid</param>
        /// <returns>Types.UsbAccessories.Hid_Open</returns>
        Hid_Open OpenOwnedAccessoryHid(string accessoryId, Accessories_Accessory_Hid_Open_Params hidOpenParams);

        /// <summary>
        /// Gets the description of the open HID.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to retrieve the accessory</param>
        /// <param name="hidAccessoryId">The HidAccessoryID to retrieve the open hid</param>
        /// <param name="isOwned">If the open HID accessory is Shared or Owned</param>
        /// <returns>Types.UsbAccessories.OpenHIDAccessory</returns>
        OpenHIDAccessory GetOpenHIDAccessory(string accessoryId, string hidAccessoryId, bool isOwned);

        /// <summary>
        /// Closes the HID accessory.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to retrieve the accessory</param>
        /// <param name="hidAccessoryId">The HidAccessoryID to retrieve the open hid</param>
        /// <param name="isOwned">If the open HID accessory is Shared or Owned</param>
        void DeleteOpenHIDAccessory(string accessoryId, string hidAccessoryId, bool isOwned);

        /// <summary>
        /// Updates values associated with the open HID accessory.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to retrieve the accessory</param>
        /// <param name="hidAccessoryId">The HidAccessoryID to retrieve the open hid</param>
        /// <param name="modificationRequest"></param>
        /// <param name="isOwned">If the open HID accessory is Shared or Owned</param>
        /// <returns>Types.UsbAccessories.OpenHIDAccessory</returns>
        OpenHIDAccessory ModifyOpenHIDAccessory(string accessoryId, string hidAccessoryId, OpenHIDAccessory_Modify modificationRequest, bool isOwned);

        /// <summary>
        /// This reads a report from the HID's control pipe.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to retrieve the accessory</param>
        /// <param name="hidAccessoryId">The HidAccessoryID to retrieve the open hid</param>
        /// <param name="readReportRequest">Read Report Params</param>
        /// <param name="isOwned">If the open HID accessory is Shared or Owned</param>
        /// <returns>Types.UsbAccessories.OpenHIDAccessory_ReadReport</returns>
        OpenHIDAccessory_ReadReport ReadReportOpenHIDAccessory(string accessoryId, string hidAccessoryId, Accessories_Accessory_Hid_OpenHIDAccessory_ReadReport_Params readReportRequest, bool isOwned);

        /// <summary>
        /// This writes a report to the HID's control pipe.
        /// </summary>
        /// <param name="accessoryId">The AccessoryID to retrieve the accessory</param>
        /// <param name="hidAccessoryId">The HidAccessoryID to retrieve the open hid</param>
        /// <param name="writeReportRequest">Write Report Params</param>
        /// <param name="isOwned">If the open HID accessory is Shared or Owned</param>
        /// <returns>Types.UsbAccessories.OpenHIDAccessory_WriteReport</returns>
        OpenHIDAccessory_WriteReport WriteReportOpenHIDAccessory(string accessoryId, string hidAccessoryId, Accessories_Accessory_Hid_OpenHIDAccessory_WriteReport_Params writeReportRequest, bool isOwned);
    }

    public class UsbAccessoriesService : IUsbAccessoriesService
    {
        #region Constructor

        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;

        public UsbAccessoriesService
        (
            IHttpClientFactory httpClientFactory,
            IDeviceManagementService deviceManagementService
        ) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private UsbAccessoriesService() { }


        #endregion // Constructor

        #region Capabilities

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

            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            Capabilities capabilities = client.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return capabilities;
        }

        #endregion //Capabilities

        #region Agents

        public UsbAccessoriesAgents EnumerateUsbAccessoriesAgents()
        {
            // @StartCodeExample:EnumerateUsbAccessoriesAgents
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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            UsbAccessoriesAgents usbAccessoriesAgents = client.UsbAccessoriesAgents.GetAsync(accessToken).Result;
            // @EndCodeExample

            return usbAccessoriesAgents;
        }

        public UsbAccessoriesAgent GetUsbAccessoriesAgent(string agentId)
        {
            // @StartCodeExample:GetUsbAccessoriesAgent
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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            UsbAccessoriesAgent usbAccessoriesAgent = client.UsbAccessoriesAgents[agentId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return usbAccessoriesAgent;
        }

        #endregion //Agents

        #region Accessories

        public Accessories EnumerateUsbAccessories()
        {
            // @StartCodeExample:EnumerateUsbAccessories
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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Accessories usbAccessories = client.Accessories.GetAsync(accessToken).Result;
            // @EndCodeExample

            return usbAccessories;
        }

        public Accessory GetUsbAccessory(string accessoryId)
        {
            // @StartCodeExample:GetUsbAccessory
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
            DiscoveryServiceClient discoveryClient = new(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Accessory usbAccessory = client.Accessories[accessoryId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return usbAccessory;
        }

        #endregion //Accessories

        #region Hid

        public Hid GetUsbAccessoryHid(string accessoryId)
        {
            // @StartCodeExample:GetUsbAccessoryHid
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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation
            Hid usbAccessoryHid = client.Accessories[accessoryId].Hid.GetAsync(accessToken).Result;
            // @EndCodeExample

            return usbAccessoryHid;
        }

        public Hid_Open OpenOwnedAccessoryHid(string accessoryId, Accessories_Accessory_Hid_Open_Params hidOpenParams)
        {
            // @StartCodeExample:OpenOwnedAccessoryHid
            // This demonstrates using the solution provider token on an owned accessory

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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the open hid operation
            Hid_Open usbAccessoryOpenHidOperation = client.Accessories[accessoryId].Hid.OpenHidOperation.ExecuteAsync(accessToken, hidOpenParams).Result;
            // @EndCodeExample

            return usbAccessoryOpenHidOperation;
        }

        public Hid_Open OpenSharedAccessoryHid(string accessoryId, Accessories_Accessory_Hid_Open_Params hidOpenParams)
        {
            // @StartCodeExample:OpenSharedAccessoryHid
            // This should only be done during an active session
            // This method demonstrates using a UI token on a shared accessory
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

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the open hid operation
            Hid_Open usbAccessoryOpenHidOperation = client.Accessories[accessoryId].Hid.OpenHidOperation.ExecuteAsync(accessToken, hidOpenParams).Result;
            // @EndCodeExample

            return usbAccessoryOpenHidOperation;
        }

        #endregion //Hid

        #region OpenHIDAccessory

        public OpenHIDAccessory GetOpenHIDAccessory(string accessoryId, string hidAccessoryId, bool isOwned)
        {
            // @StartCodeExample:GetOpenHIDAccessory
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Solution Context
            string accessToken = currentDevice.GetToken(
                isOwned ? AccessTokenType.Solution : AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Get the open hid usb Accessory
            OpenHIDAccessory openHIDUsbAccessory = client.Accessories[accessoryId].Hid[hidAccessoryId].GetAsync(accessToken).Result;
            // @EndCodeExample

            return openHIDUsbAccessory;
        }

        public void DeleteOpenHIDAccessory(string accessoryId, string hidAccessoryId, bool isOwned)
        {
            // @StartCodeExample:DeleteOpenHIDAccessory
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Solution Context
            string accessToken = currentDevice.GetToken(
                isOwned ? AccessTokenType.Solution : AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the delete open hid operation
            client.Accessories[accessoryId].Hid[hidAccessoryId].DeleteAsync(accessToken);
            // @EndCodeExample
        }

        public OpenHIDAccessory ModifyOpenHIDAccessory(string accessoryId, string hidAccessoryId, OpenHIDAccessory_Modify modificationRequest, bool isOwned)
        {
            // @StartCodeExample:ModifyOpenHIDAccessory
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Solution Context
            string accessToken = currentDevice.GetToken(
                isOwned ? AccessTokenType.Solution : AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the modify open hid accessory operation
            OpenHIDAccessory modifiedOpenHIDAccessory = client.Accessories[accessoryId].Hid[hidAccessoryId].ModifyAsync(accessToken, modificationRequest).Result;
            // @EndCodeExample

            return modifiedOpenHIDAccessory;
        }

        public OpenHIDAccessory_ReadReport ReadReportOpenHIDAccessory(string accessoryId, string hidAccessoryId, Accessories_Accessory_Hid_OpenHIDAccessory_ReadReport_Params readReportParams, bool isOwned)
        {
            // @StartCodeExample:ReadReportOpenHIDAccessory
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Solution Context
            string accessToken = currentDevice.GetToken(
                isOwned ? AccessTokenType.Solution : AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the read open hid accessory report operation
            OpenHIDAccessory_ReadReport readReportResult = client.Accessories[accessoryId].Hid[hidAccessoryId].ReadReport.ExecuteAsync(accessToken, readReportParams).Result;
            // @EndCodeExample

            return readReportResult;
        }

        public OpenHIDAccessory_WriteReport WriteReportOpenHIDAccessory(string accessoryId, string hidAccessoryId, Accessories_Accessory_Hid_OpenHIDAccessory_WriteReport_Params writeReportParams, bool isOwned)
        {
            // @StartCodeExample:WriteReportOpenHIDAccessory
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // This operation requires Solution Context
            string accessToken = currentDevice.GetToken(
                isOwned ? AccessTokenType.Solution : AccessTokenType.UI_Context
            );

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the UsbAccessoriesServiceClient
            UsbAccessoriesServiceClient client = new UsbAccessoriesServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the write open hid accessory report operation
            OpenHIDAccessory_WriteReport writeReportResult = client.Accessories[accessoryId].Hid[hidAccessoryId].WriteReport.ExecuteAsync(accessToken, writeReportParams).Result;
            // @EndCodeExample

            return writeReportResult;
        }

        #endregion //OpenHIDAccessory

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            UsbAccessoriesServiceClient client = null;

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
            client = new UsbAccessoriesServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockUsbAccessories");
            client = new UsbAccessoriesServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }
    }
}
