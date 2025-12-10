/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Types.SolutionManager;

namespace OXPd2ExamplesHost.Models
{
    public class SolutionAgentNotification
    {
        public string Timestamp { get; set; }

        public string SolutionId { get; set; }

        public string DeviceId { get; set; }

        public SolutionNotification SolutionNotification { get; set; }

        public bool authCodeExchanged { get; set; }
    }
}
