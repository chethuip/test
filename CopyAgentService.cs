/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.Copy;
using OXPd2ExamplesHost.Models;

namespace OXPd2ExamplesHost.Services
{
    public interface ICopyAgentService
    {
        void HandleNotification(CopyNotification notification);
    }

    public class CopyAgentService : ICopyAgentService
    {
        #region Construction and singleton 

        private IDeviceManagementService deviceManagementService = null;

        private ILogService logService = null;

        private static string notificationLog = "copyNotifications";

        public CopyAgentService(IDeviceManagementService deviceManagementService, ILogService logService)
        {
            this.deviceManagementService = deviceManagementService;
            this.logService = logService;
            logService.CreateLog(notificationLog);
        }

        private CopyAgentService() { }

        #endregion // Construction


        public void HandleNotification(CopyNotification notification)
        {
            logService.Log(notificationLog, new LogItem(notification.JobNotification.CopyJobId.Value.ToString(), notification));
        }
    }
}
