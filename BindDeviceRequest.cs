/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using Newtonsoft.Json;

namespace OXPd2ExamplesHost.Models
{
    public class BindDeviceRequest
    {
        [JsonProperty("networkAddress")]
        public string NetworkAddress { get; set; }
    }
}
