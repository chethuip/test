/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using Newtonsoft.Json;

namespace OXPd2ExamplesHost.Models
{
    public class LoginRequest
    {
        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
}
