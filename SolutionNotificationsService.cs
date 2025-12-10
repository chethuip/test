/***********************************************************
 * (C) Copyright 2021 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Client.OAUTH2;
using HP.Extensibility.Types.SolutionManager;
using OXPd2ExamplesHost.Models;

namespace OXPd2ExamplesHost.Services
{
    public interface ISolutionNotificationsService
    {
        void HandleNotification(SolutionNotification payload);
    }

    public class SolutionNotificationsService : ISolutionNotificationsService
    {
        #region Construction and singleton stuff

        private IDeviceManagementService deviceManagementService = null;

        private ILogService logService = null;

        private static string logBase = "solutionNotifications";

        public SolutionNotificationsService(IDeviceManagementService deviceManagementService, ILogService logService)
        {
            this.deviceManagementService = deviceManagementService;
            this.logService = logService;
            logService.CreateLog(logBase);
        }

        private SolutionNotificationsService() { }

        #endregion // Construction

        public void HandleNotification(SolutionNotification payload)
        {
            // @StartCodeExample:AddNotificationPayload            
            if (payload.NotificationType == NotificationType.NtAuthCode)
            {
                //exchange the authCode for an accessToken.
                if (payload.NotificationPayload != null)
                {
                    string code = payload.NotificationPayload.AuthCodeNotificationPayload.Code;

                    //Check that the current device is set in order to exchange;
                    Device currentDevice = deviceManagementService.CurrentDevice;
                    if (currentDevice != null)
                    {
                        IDeviceManagementService service = deviceManagementService;

                        //Exchange the authorization code to get a bearer access token for the solution
                        service.AuthorizationCodeGrant(code);
                    }
                }
            }
            logService.Log(logBase, new LogItem(payload.NotificationType, payload.NotificationPayload));
            // @EndCodeExample
        }
    }
}
