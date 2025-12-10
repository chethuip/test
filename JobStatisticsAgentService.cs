/***********************************************************
 * (C) Copyright 2023 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.JobStatistics;
using OXPd2ExamplesHost.Models;

namespace OXPd2ExamplesHost.Services
{
    public interface IJobStatisticsAgentService
    {
        StatisticsCallbackResponse HandleNotification(StatisticsCallbackPayload payload);
    }

    public class JobStatisticsAgentService : IJobStatisticsAgentService
    {
        #region Construction and singleton 

        private IDeviceManagementService deviceManagementService = null;

        private ILogService logService = null;

        private static string notificationLog = "jobStatisticsNotifications";

        public JobStatisticsAgentService(IDeviceManagementService deviceManagementService, ILogService logService)
        {
            this.deviceManagementService = deviceManagementService;
            this.logService = logService;
            logService.CreateLog(notificationLog);
        }

        private JobStatisticsAgentService() { }

        #endregion // Construction


        public StatisticsCallbackResponse HandleNotification(StatisticsCallbackPayload payload)
        {
            logService.Log(notificationLog, new LogItem(payload));

            // This is where the solution would process the data

            StatisticsCallbackResponse response = new StatisticsCallbackResponse
            {
                LastSequenceNumberNotified = payload.LastSequenceNumberNotified,
                LastSequenceNumberProcessed = payload.LastSequenceNumberProcessed
            };

            return response;
        }
    }
}
