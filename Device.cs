/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OXPd2ExamplesHost.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OXPd2ExamplesHost.Models
{
    public enum TokenStatus
    {
        None,
        Granted,
        Invalid
    }

    [JsonConverter(typeof(StringEnumConverter))]
    // Order matters here. Must be in ascending order by scope.
    public enum AccessTokenType
    {
        UI_Context,
        Authentication_Context,
        Solution,
        Admin
    }

    public struct AccessTokenInfo
    {
        public AccessTokenInfo(AccessTokenType type, Token token, TokenStatus status)
        {
            Type = type;
            Token = token;
            Status = status;
        }
        public AccessTokenType Type { get; }
        public Token Token { get; }
        public TokenStatus Status { get; }
    }

    public class DeviceModel
    {
        [JsonProperty("networkAddress")]
        public string NetworkAddress { get; set; }

        [JsonProperty("bindStatus")]
        public string BindStatus { get; set; }

        [JsonProperty("adminAccessTokenStatus")]
        public TokenStatus AdminAccessTokenStatus { get; set; } = TokenStatus.None;

        [JsonProperty("solutionAccessTokenStatus")]
        public TokenStatus SolutionAccessTokenStatus { get; set; } = TokenStatus.None;

        public DateTime SolutionAccessTokenTimeGranted { get; set; }

        [JsonProperty("uiContextAccessTokenStatus")]
        public TokenStatus UiContextAccessTokenStatus { get; set; } = TokenStatus.None;

        [JsonProperty("authenticationContextAccessTokenStatus")]
        public TokenStatus AuthenticationContextAccessTokenStatus { get; set; } = TokenStatus.None;
    }

    public class Device : DeviceModel
    {

        private IDeviceManagementService deviceManagementService = null;

        public Device(IDeviceManagementService deviceManagementService)
        {
            this.deviceManagementService = deviceManagementService;
        }

        [JsonIgnore]
        public Token AdminAccessToken { get; set; } = null;

        [JsonIgnore]
        public Token SolutionAccessToken { get; set; } = null;

        [JsonIgnore]
        public Token UiContextAccessToken { get; set; } = null;

        [JsonIgnore]
        public Token AuthenticationContextAccessToken { get; set; } = null;

        public string GetToken(
            params AccessTokenType[] accessTokenTypes
        )
        {
            if (accessTokenTypes == null || accessTokenTypes.Count() == 0)
            {
                return string.Empty;
            }
            else
            {
                var grantedTokenTypes = GetTokens().Where(t => t.Status == TokenStatus.Granted);
                if (grantedTokenTypes.Count() == 0)
                {
                    return string.Empty;
                }

                var matchingTokens = accessTokenTypes.Where(a => grantedTokenTypes.Any(g => g.Type == a));
                if (matchingTokens.Count() == 0)
                {
                    return string.Empty;
                }

                switch (matchingTokens.Min())
                {
                    case AccessTokenType.UI_Context: return UiContextAccessToken.AccessToken;
                    case AccessTokenType.Authentication_Context: return AuthenticationContextAccessToken.AccessToken;
                    case AccessTokenType.Solution:
                        {
                            RefreshTokenIfNeeded();
                            return SolutionAccessToken.AccessToken;
                        }
                    case AccessTokenType.Admin: return AdminAccessToken.AccessToken;
                    default: return string.Empty; // Should never reach this case
                }
            }
        }

        private void RefreshTokenIfNeeded()
        {
            DateTime expiryTime = SolutionAccessTokenTimeGranted.AddSeconds(SolutionAccessToken.ExpiresIn);
            if (DateTime.Compare(expiryTime, DateTime.UtcNow) < 0)
            {
                //Solution Provider-Owner token is expired.  Get a new token with the refresh token
                IDeviceManagementService service = deviceManagementService;
                service.RefreshGrant(SolutionAccessToken.RefreshToken);
            }
        }

        public IEnumerable<AccessTokenInfo> GetTokens()
        {
            return new[]{
                new AccessTokenInfo(AccessTokenType.Admin, AdminAccessToken, AdminAccessTokenStatus),
                new AccessTokenInfo(AccessTokenType.Solution, SolutionAccessToken, SolutionAccessTokenStatus),
                new AccessTokenInfo(AccessTokenType.UI_Context, UiContextAccessToken, UiContextAccessTokenStatus),
                new AccessTokenInfo(AccessTokenType.Authentication_Context, AuthenticationContextAccessToken, AuthenticationContextAccessTokenStatus)
            };
        }
    }
}
