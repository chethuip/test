/***********************************************************
* (C) Copyright 2021 HP Development Company, L.P.
* All rights reserved.
***********************************************************/
using HP.Extensibility.Client.Discovery;
using HP.Extensibility.Client.OAUTH2;
using HP.Extensibility.Types.Common;
using OXPd2ExamplesHost.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IHttpClientFactory = OXPd2ExamplesHost.Utilities.IHttpClientFactory;

namespace OXPd2ExamplesHost.Services
{
    /// <summary>
    /// Defines the interface to the service that provides the business logic behind the DeviceManagement controller
    /// </summary>
    public interface IDeviceManagementService
    {
        Device CurrentDevice { get; set; }

        DeviceModel BindDevice(string networkAddress);

        DeviceModel UnbindDevice();

        ServicesDiscovery GetServicesDiscovery();

        Task<string> GetDeviceInformation();

        Token PasswordGrant(string username, string password);

        Token AuthorizationCodeGrant(string code);

        Token RefreshGrant(string refreshToken);

        void SetUiContextAccessToken(Token token);

        void SetAuthContextAccessToken(Token token);

        IEnumerable<AccessTokenInfo> GetTokens();
    }

    /// <summary>
    /// Implements the business logic of the DeviceManagement controller
    /// </summary>
    public class DeviceManagementService : IDeviceManagementService
    {
        private IHttpClientFactory _httpClientFactory;
        #region Construction

        public DeviceManagementService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        private DeviceManagementService() { }


        #endregion // Construction


        #region IDeviceManagementService implementation
        public Device CurrentDevice { get; set; } = null;

        public Token PasswordGrant(string username, string password)
        {
            // @StartCodeExample:PasswordGrant
            // Fetch the current device we're working with
            Device currentDevice = this.CurrentDevice;

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the OAUTH2ServiceClient
            OAUTH2ServiceClient oauth2Client = new OAUTH2ServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Create a PasswordGrantRequest with provided admin credentials
            PasswordGrantRequest grantRequest = new PasswordGrantRequest()
            {
                Username = username,
                Password = password
            };

            // Execute the PasswordGrant operation using the Token Resource Facade
            Token token = oauth2Client.Token.PasswordGrantAsync(grantRequest).Result;
            // @EndCodeExample

            currentDevice.AdminAccessTokenStatus = TokenStatus.Granted;
            currentDevice.AdminAccessToken = token;

            return token;
        }

        public Token AuthorizationCodeGrant(string code)
        {
            // @StartCodeExample:AuthorizationCodeGrant
            // Fetch the current device we're working with
            Device currentDevice = this.CurrentDevice;

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the OAUTH2ServiceClient
            OAUTH2ServiceClient oauth2Client = new OAUTH2ServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Create a AuthorizationCodeGrantRequest with provided code
            AuthorizationCodeGrantRequest grantRequest = new AuthorizationCodeGrantRequest()
            {
                Code = code
            };

            // Execute the AuthorizationCodeGrant operation using the Token Resource Facade
            Token token = oauth2Client.Token.AuthorizationCodeGrantAsync(grantRequest).Result;
            // @EndCodeExample

            currentDevice.SolutionAccessTokenTimeGranted = DateTime.UtcNow;
            currentDevice.SolutionAccessToken = token;
            currentDevice.SolutionAccessTokenStatus = TokenStatus.Granted;

            return token;
        }

        public Token RefreshGrant(string refreshToken)
        {
            // @StartCodeExample:RefreshGrant
            // Fetch the current device we're working with
            Device currentDevice = this.CurrentDevice;

            // Fetch the discovery tree
            DiscoveryServiceClient discoveryClient = new DiscoveryServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress);
            ServicesDiscovery discoveryTree = discoveryClient.ServicesDiscovery.GetAsync().Result;

            // Construct the OAUTH2ServiceClient
            OAUTH2ServiceClient oauth2Client = new OAUTH2ServiceClient(_httpClientFactory.HttpClient, currentDevice.NetworkAddress, discoveryTree);

            // Create a RefreshTokenGrantRequest with provided refreshToken
            RefreshTokenGrantRequest grantRequest = new RefreshTokenGrantRequest()
            {
                RefreshToken = refreshToken
            };

            // Execute the RefreshTokenGrant operation using the Token Resource Facade
            Token token = oauth2Client.Token.RefreshTokenGrantAsync(grantRequest).Result;
            // @EndCodeExample

            currentDevice.SolutionAccessTokenTimeGranted = DateTime.UtcNow;
            currentDevice.SolutionAccessToken = token;
            currentDevice.SolutionAccessToken.RefreshToken = refreshToken;
            currentDevice.SolutionAccessTokenStatus = TokenStatus.Granted;

            return token;
        }

        public void SetUiContextAccessToken(Token uiContextToken)
        {
            // Fetch the current device we're working with
            Device currentDevice = this.CurrentDevice;

            if (currentDevice != null)
            {
                currentDevice.UiContextAccessToken = uiContextToken;
                currentDevice.UiContextAccessTokenStatus = (uiContextToken != null) ? TokenStatus.Granted : TokenStatus.None;
            }
        }

        public void SetAuthContextAccessToken(Token authContextToken)
        {
            // Fetch the current device we're working with
            Device currentDevice = this.CurrentDevice;

            if (currentDevice != null)
            {
                currentDevice.AuthenticationContextAccessToken = authContextToken;
                currentDevice.AuthenticationContextAccessTokenStatus = (authContextToken != null) ? TokenStatus.Granted : TokenStatus.None;
            }
        }

        public DeviceModel BindDevice(string networkAddress)
        {
            // @StartCodeExample:BindDevice
            // https://docs.microsoft.com/en-us/dotnet/api/system.uri.iswellformeduristring?view=net-5.0
            if (Uri.IsWellFormedUriString(networkAddress, UriKind.RelativeOrAbsolute) && !string.IsNullOrEmpty(networkAddress))
            {
                var uriBuilder = new UriBuilder($"https://{networkAddress}");
                CurrentDevice = new Device(this) { NetworkAddress = uriBuilder.Uri.Authority, BindStatus = "bound" };

                try
                {
                    // Let's make sure can can communicate with the device
                    GetServicesDiscovery();
                    return CurrentDevice;
                }
                catch
                {
                    CurrentDevice = null;
                    throw new InvalidOperationException("Unable to access the device's ServicesDiscovery resource");
                }
            }

            throw new InvalidOperationException("Device Network address is not a valid URI.");
            // @EndCodeExample
        }

        public DeviceModel UnbindDevice()
        {
            // @StartCodeExample:UnbindDevice
            CurrentDevice = null;
            // @EndCodeExample

            return CurrentDevice;
        }

        public ServicesDiscovery GetServicesDiscovery()
        {
            // @StartCodeExample:GetServicesDiscovery
            DiscoveryServiceClient dsc = new DiscoveryServiceClient(_httpClientFactory.HttpClient, CurrentDevice.NetworkAddress);
            return dsc.ServicesDiscovery.GetAsync().Result;
            // @EndCodeExample
        }

        public async Task<string> GetDeviceInformation()
        {
            // @StartCodeExample:GetDeviceInformation
            return await _httpClientFactory.HttpClient.GetStringAsync("https://" + CurrentDevice.NetworkAddress + "/cdm/system/v1/identity");
            // @EndCodeExample
        }

        public IEnumerable<AccessTokenInfo> GetTokens()
        {
            return CurrentDevice.GetTokens();
        }
        #endregion // IDeviceManagementService implementation
    }
}
