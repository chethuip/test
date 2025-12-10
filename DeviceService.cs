using HP.Extensibility.Client.Device;
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Service.Device;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Net.Http;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    public interface IDeviceService
    {
        Capabilities GetCapabilities();
        DeploymentInformation GetDeploymentInformation();
        Email GetEmail();
        Identity GetIdentity();
        Scanner GetScanner();
        Status GetStatus();
    }

    public class DeviceService : IDeviceService
    {
        private IHttpClientFactory httpClientFactory;
        private IDeviceManagementService deviceManagementService;

        public DeviceService(IHttpClientFactory httpClientFactory, IDeviceManagementService deviceManagementService) : this()
        {
            this.httpClientFactory = httpClientFactory;
            this.deviceManagementService = deviceManagementService;
        }

        private DeviceService() { }

        public void ConstructorExamples()
        {
            string deviceNetworkAddress = string.Empty;
            ServicesDiscovery discoveryTree = null;
            DeviceServiceClient client = null;

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
            client = new DeviceServiceClient(httpClient, deviceNetworkAddress, discoveryTree);

            // Here's a test/debug constructor that constructs a client that will use the provided service URI
            // as the root endpoint for all resource interactions
            Uri mockServiceUri = new Uri("http://localhost:5000/mockApplication");
            client = new DeviceServiceClient(httpClient, mockServiceUri);

            // @EndCodeExample
        }

        public Capabilities GetCapabilities()
        {
            // @StartCodeExample:GetCapabilities
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get operation            
            Capabilities result = deviceServiceClient.Capabilities.GetAsync().Result;
            // @EndCodeExample

            return result;
        }

        public DeploymentInformation GetDeploymentInformation()
        {
            // @StartCodeExample:GetDeploymentInformation
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get Deployment Information            
            DeploymentInformation result = deviceServiceClient.DeploymentInformation.GetAsync().Result;
            // @EndCodeExample

            return result;
        }

        public Identity GetIdentity()
        {
            // @StartCodeExample:GetIdentity
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get Identity            
            Identity result = deviceServiceClient.Identity.GetAsync().Result;
            // @EndCodeExample

            return result;
        }

        public Status GetStatus()
        {
            // @StartCodeExample:GetStatus
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get Status            
            Status result = deviceServiceClient.Status.GetAsync().Result;
            // @EndCodeExample

            return result;
        }

        public Email GetEmail()
        {
            // @StartCodeExample:GetEmail
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get Email            
            Email result = deviceServiceClient.Email.GetAsync().Result;
            // @EndCodeExample

            return result;
        }

        public Scanner GetScanner()
        {
            // @StartCodeExample:GetScanner
            // Fetch the current device
            Device currentDevice = deviceManagementService.CurrentDevice;

            if (null == currentDevice)
            {
                throw new InvalidOperationException("There is no bound device.");
            }

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the DeviceServiceClient
            DeviceServiceClient deviceServiceClient = new DeviceServiceClient(httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Execute the Get Scanner            
            Scanner result = deviceServiceClient.Scanner.GetAsync().Result;
            // @EndCodeExample

            return result;
        }
    }
}
