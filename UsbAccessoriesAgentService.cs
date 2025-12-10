/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using HP.Extensibility.Service.UsbAccessories;
using OXPd2ExamplesHost.Models;

namespace OXPd2ExamplesHost.Services
{
    public interface IUsbAccessoriesAgentService
    {
        public void HandleUsbRegistrationNotification(UsbRegistrationPayload payload);

        public void HandleUsbCallback(UsbCallbackEnvelope callbackEnvelope);
    }

    public class UsbAccessoriesAgentService : IUsbAccessoriesAgentService
    {
        private ILogService logService;

        public static string RegistrationLog = "usbregistration";
        public static string CallbackLog = "usbcallback"; 

        public UsbAccessoriesAgentService(ILogService logService)
        {
            this.logService = logService;
            logService.CreateLog(RegistrationLog);
            logService.CreateLog(CallbackLog);
        }

        public void HandleUsbRegistrationNotification(UsbRegistrationPayload payload)
        {
            if (payload.IsUsbAttached)
            {
                logService.Log(RegistrationLog, new LogItem(payload.UsbAttached.TypeName, payload.UsbAttached));
            }
            if (payload.IsUsbDetached)
            {
                logService.Log(RegistrationLog, new LogItem(payload.UsbDetached.TypeName, payload.UsbDetached));
            }
        }

        public void HandleUsbCallback(UsbCallbackEnvelope callbackEnvelope)
        {
            UsbCallback callback = callbackEnvelope.UsbCallback;

            if (callback.IsHidRead)
            {
                logService.Log(CallbackLog, new LogItem(callback.HidRead.TypeName, callback.HidRead));
            }
            if (callback.IsUsbRead)
            {
                logService.Log(CallbackLog, new LogItem(callback.UsbRead.TypeName, callback.UsbRead));
            }
            if (callback.IsUsbWrite)
            {
                logService.Log(CallbackLog, new LogItem(callback.UsbWrite.TypeName, callback.UsbWrite));
            }
            if (callback.IsUsbClosed)
            {
                logService.Log(CallbackLog, new LogItem(callback.UsbClosed.TypeName, callback.UsbClosed));
            }
        }
    }
}
