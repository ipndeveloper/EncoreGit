using System;
using System.Collections.Generic;
using System.Net;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.DeviceNotifications
{
    public class DeviceNotificationSender : IDeviceNotificationSender
    {
        public bool SendDeviceNotifications(List<DeviceNotification> notifications)
        {
			var uriBase = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.DeviceNotificationBaseUri);
			var uriString = string.Empty;
			var notificationString = string.Empty;
			var format = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.DeviceNotificationUriFormat);

			NetSteps.Data.Entities.Exceptions.EntityExceptionHelper.GetAndLogNetStepsException(String.Format("Sending {0} device notifications", notifications.Count), Constants.NetStepsExceptionType.NetStepsApplicationException);

			try
			{
				notifications.ForEach(notification =>
				{
					var device = notification.AccountDevice;
					if (device == null)
						device = AccountDevice.Load(notification.AccountDeviceID);

					var deviceType = device.DeviceType;
					if (deviceType == null)
						deviceType = DeviceType.Load(device.DeviceTypeID);

					var serviceName = deviceType.Name == "Android" ? "android" : "iphone";

					notificationString = string.Concat(notificationString, string.Format(format, serviceName, device.DeviceID, notification.Body));
				});

				uriString = string.Concat(uriBase, notificationString);

				NetSteps.Data.Entities.Exceptions.EntityExceptionHelper.GetAndLogNetStepsException(String.Format("About to post to {0}", uriString), Constants.NetStepsExceptionType.NetStepsApplicationException);

				var request = WebRequest.Create(uriString);
				request.Method = "POST";

				var response = request.GetResponse();

				NetSteps.Data.Entities.Exceptions.EntityExceptionHelper.GetAndLogNetStepsException(String.Format("Post to {0} completed", uriString), Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
			catch (Exception e)
			{
				NetSteps.Data.Entities.Exceptions.EntityExceptionHelper.GetAndLogNetStepsException("Failure sending device notifications: uriString: " + uriString, Constants.NetStepsExceptionType.NetStepsApplicationException);
				NetSteps.Data.Entities.Exceptions.EntityExceptionHelper.GetAndLogNetStepsException(e, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return false;
			}
			
            return true;
        }
    }
}
