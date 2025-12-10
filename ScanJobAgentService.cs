/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using HP.Extensibility.Service.ScanJob;
using HP.Extensibility.Types.SolutionManager;
using OXPd2ExamplesHost.Models;
using System.Diagnostics;
using TransmittingState = HP.Extensibility.Service.ScanJob.TransmittingState;

namespace OXPd2ExamplesHost.Services
{
    public interface IScanJobAgentService
    {
        void HandleNotification(ScanNotification payload);
        void HandleScanJobReceiver(ServiceTargetContent payload);
    }

    public class ScanJobAgentService : IScanJobAgentService
    {
        #region Construction and singleton 

        private IDeviceManagementService deviceManagementService = null;

        private ILogService logService = null;

        private static string notificationLog = "scanNotifications";
        private static string receiverLog = "scanJobReceiver";

        public ScanJobAgentService(IDeviceManagementService deviceManagementService, ILogService logService)
        {
            this.deviceManagementService = deviceManagementService;
            this.logService = logService;
            logService.CreateLog(notificationLog);
            logService.CreateLog(receiverLog);
        }

        private ScanJobAgentService() { }

        #endregion // Construction

        
        public void HandleNotification(ScanNotification payload)
        {
            logService.Log(notificationLog, new LogItem(payload.JobNotification.ScanJobId.Value.ToString(), payload));
        }

        public void HandleScanJobReceiver(ServiceTargetContent payload)
        {
            logService.Log(receiverLog, new LogItem(payload.TransmittingState, payload));
        }
    }
}
