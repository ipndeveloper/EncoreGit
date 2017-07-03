using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Utility;
using System.Configuration;

namespace NetSteps.Common.Configuration
{
	/// <summary>
	/// Author: John Egbert
	/// Description: The original code for this was taken from Spencer's implementation on CTMH to provide a way to access
	/// config values of all different environments (Production, LocalHost, Test, ect...) in one config file. I modified the 
	/// file to include the Clients name in the config Key in hopes of making copy and pate of config values less prone to errors
	/// as the values will need to be changes for the specific client. I also added type safe GetAppSetting methods and 
	/// methods/classes to provide default and overridden values for these config settings.
	/// Created: 04-19-2010
	/// </summary>
	public class ConfigurationManager
	{
		#region Enum
		public enum EnvironmentMode
		{
			LOCAL,
			DEV,
			TEST,
			PRE,
			PROD
		}

        public enum VariableKey
        {
            BackOfficeUrl,
            AdminUrl,
            DistributorWorkstationUrl,
            AlertEmailAddresses,
            LogErrors,
            IsPaymentLiveMode,
            IsPaymentTestTransaction,
            ClientPropayTestAccountNum,
            ClientPropayTestAuthId,
            ClientPropayTerminalId,
            UsPropayUrl,
            CanadaPropayUrl,
            EnableFailSafeCreditCardCode,
            PathToSAN,
            FileUploadWebPath,
            FileUploadAbsolutePath,
            FileUploadAbsoluteWebPath,
            SecureFileUploadAbsolutePath,
            SecureFileUploadWebPath,
            ViewstateCompression,
            HandlerImages,
            TinyHandlerImages,
            CacheNavigation,
            CacheTextImages,
            TaxCachingDurationDays,
            AccountNumbersEqualIdentity,
            AccountNumbersAreNumeric,
            OrderNumbersEqualIdentity,
            OrderNumbersAreNumeric,
            ShippingCalculationMode,
            AllowPOBoxShipment,
            UseEmailAsUsername,
            UseExternalEmailForReplyTo,

			MailMessageSendAttempts,
			SmtpServer,
			SmtpPort,
			SmtpUserName,
			SmtpPassword,
			UseSmtpAuthentication,
			SmtpLicenseKey,
			MailThreadThreshold,
			EnrollmentCategoryID,

			Domains,
			CorporateMasterSiteId,
			BackOfficeMasterSiteId,
			ReplicatedMasterSiteId,
			PayForSites,
			UrlFormatIsSubdomain,

			ServerIPs,

			FlatDownlineColumns,

			ControlLibraries,

			CorporateAccountID,
			NSCoreSiteID,
			nsBackofficeSiteID,
			RestockingFeeSKU,
			UseSponsorForOrderConsultant,

            CorporateMailAccountID,
            CorporateContactMailAccountID,

			ReturnOrderTypeID,
			ReturnOrderCommissionDateAddition,
			StoreFrontID,
			Orders_Returns_RefundPercentOfShipping,

			AllowExternalEmail,
			TestEmailAccount,
			HurricaneAccountId,
			EmailEventTrackingUri,

			PartyGuestReminderProcessor_Start,
			PartyGuestReminderProcessor_WorkerThreads,
			PartyGuestReminderProcessor_PollingIntervalMs,
			PartyGuestReminderProcessor_MaxNumberToPoll,
			PartyGuestReminderProcessor_ReminderInterval,

			SendMailQueueProcessor_Start,
			SendMailQueueProcessor_WorkerThreads,
			SendMailQueueProcessor_PollingIntervalMs,
			SendMailQueueProcessor_MaxNumberToPoll,

			DomainEventQueueProcessor_Start,
			DomainEventQueueProcessor_WorkerThreads,
			DomainEventQueueProcessor_PollingIntervalMs,
			DomainEventQueueProcessor_MaxNumberToPoll,

			CampaignActionQueueItemProcessor_Start,
			CampaignActionQueueItemProcessor_WorkerThreads,
			CampaignActionQueueItemProcessor_PollingIntervalMs,
			CampaignActionQueueItemProcessor_MaxNumberToPoll,

			CampaignActionProcessor_Start,
			CampaignActionProcessor_WorkerThreads,
			CampaignActionProcessor_PollingIntervalMs,
			CampaignActionProcessor_MaxNumberToPoll,

			DeviceNotificationQueueProcessor_Start,
			DeviceNotificationQueueProcessor_WorkerThreads,
			DeviceNotificationQueueProcessor_PollingIntervalMs,
			DeviceNotificationQueueProcessor_MaxNumberToPoll,

			AutoshipReminderQueueProcessor_Start,
			AutoshipReminderQueueProcessor_WorkerThreads,
			AutoshipReminderQueueProcessor_PollingIntervalMs,
			AutoshipReminderQueueProcessor_MaxNumberToPoll,

			DeviceNotificationBaseUri,
			DeviceNotificationUriFormat,
			MobileBackOfficeUrl,

            DownlineCacheReloadIntervalMinutes,
			UseDownlineCacheForKPIs,
            ShowPerformanceVolumeWidget,
            ShowPerformanceTitleProgressionWidget,
            UseAdvancedPerformanceTitleProgressionWidget,
            CorporateTimeZone,

			AutoshipReminderDayValue,
			LoadLastPendingOrderOnLogin,

			CustomAssignDistributorAccountID,
			UseDefaultBundling,
			AllowBundleQuantityUpdate,

			ForceSSL,

			AllowInstantCommissions,
			EnablePwsPathRedirect,
			ShowSocialNetworkingLinks,
			EnableForgotPassword,

            EnvironmentLevelDomain,
            UseSimpleSiteCache,
            QuickStartWidgetDisplayDays,

			UsesEncoreCommissions,

            ShopCategoryCount,
            CategoryShopDisplayCount,
		}
		#endregion

		#region Members
		private static AppSettings _appSettings = new AppSettings();
		private static Nullable<bool> _useSqlDependencyCache;
		private static Nullable<bool> _useInventoryCache;
		private static Nullable<bool> _forceSSL;
		public const string TOKEN = "<!--filepath-->";
		#endregion

		#region Properties
		public static EnvironmentMode CurrentEnviornment
		{
			get
			{
				string returnValueString = string.Empty;
				EnvironmentMode returnValue = EnvironmentMode.LOCAL;

				try
				{
					// Get the env variable - this is always set on a webserver                    
					object webEnviornmentVariable;
					webEnviornmentVariable = Environment.GetEnvironmentVariable("WEBENV");

					returnValueString = (webEnviornmentVariable != null) ? webEnviornmentVariable.ToString() : EnvironmentMode.LOCAL.ToString();

					string webEnviornmentVariableOverride = System.Configuration.ConfigurationManager.AppSettings["webEnviornmentVariableOverride"];

					if (!string.IsNullOrEmpty(webEnviornmentVariableOverride))
					{
						returnValueString = webEnviornmentVariableOverride;
					}
				}
				catch
				{
					returnValueString = EnvironmentMode.LOCAL.ToString();
				}

				//try
				//{
				//    Array lstProvider = System.Enum.GetValues(typeof(EnvironmentMode));

				//    foreach (EnvironmentMode envEnum in lstProvider)
				//    {
				//        if (returnValueString.ToUpper().Contains(envEnum.ToString().ToUpper()))
				//        {
				//            returnValue = envEnum;
				//        }
				//    }
				//}
				//catch
				//{
				//    returnValue = EnvironmentMode.LOCAL;
				//}

				return returnValue;
			}
		}

		public static bool UseInventoryCache
		{
			get
			{
				if (_useInventoryCache.HasValue)
					return _useInventoryCache.Value;
				try
				{
					if (System.Configuration.ConfigurationManager.AppSettings["UseInventoryCache"] != null)
					{
						_useInventoryCache = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseInventoryCache"].ToString());
						return _useInventoryCache.Value;
					}
				}
				catch { }

				_useInventoryCache = true;
				return _useInventoryCache.Value;
			}
		}

		public static bool ForceSSL
		{
			get
			{
				if (_forceSSL.HasValue)
					return _forceSSL.Value;
				try
				{
					if (System.Configuration.ConfigurationManager.AppSettings["ForceSSL"] != null)
					{
						_forceSSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ForceSSL"].ToString());
						return _forceSSL.Value;
					}
				}
				catch { }

				_forceSSL = false;
				return _forceSSL.Value;
			}
		}

	    public static string ShopCategoryCount
	    {
	        get
            { 
                var result = string.Empty;
	            try
	            {
	                result = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ShopCategoryCount, "5");
	            }
	            catch { }
	            
                return result;
	        }
	    }

        public static string CategoryShopDisplayCount
        {
            get
            {
                var result = string.Empty;
                try
                {
                    result = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.CategoryShopDisplayCount, "5");
                }
                catch { }

                return result;
            }
        }

		public static string FileUploadWebPath
		{
			get
			{
				var result = string.Empty;
				try
				{
					result =
						ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadWebPath).
							AppendForwardSlash();
				}
				catch { }
				return result;
			}
		}

		public static string SecureFileUploadPath
		{
			get
			{
				var result = string.Empty;
				try
				{
					result =
					ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SecureFileUploadAbsolutePath)
						.AppendForwardSlash()
						.EldEncode();
				}
				catch { }
				return result;
			}
		}

		public static string SecureFileUploadWebPath
		{
			get
			{
				var result = string.Empty;
				try
				{
					result =
						ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SecureFileUploadWebPath)
						.AppendForwardSlash()
						.EldEncode();
				}
				catch { }
				return result;
			}
		}

		public static bool UseSqlDependencyCache
		{
			get
			{
				if (_useSqlDependencyCache.HasValue)
					return _useSqlDependencyCache.Value;
				try
				{
					if (System.Configuration.ConfigurationManager.AppSettings["UseSqlDependencyCache"] != null)
					{
						_useSqlDependencyCache = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseSqlDependencyCache"].ToString());
						return _useSqlDependencyCache.Value;
					}
				}
				catch { }

				_useSqlDependencyCache = true;
				return _useSqlDependencyCache.Value;
			}
		}

		public static Constants.Client CurrentClient
		{
			get
			{
				Constants.Client returnValue = Constants.Client.UnknownClient;

				try
				{
					if (System.Configuration.ConfigurationManager.AppSettings["CurrentClient"] != null)
					{
						string clientString = System.Configuration.ConfigurationManager.AppSettings["CurrentClient"].ToString();
						Enum.TryParse<Constants.Client>(clientString, out returnValue);
						return returnValue;
					}
				}
				catch { }

				return returnValue;
			}
		}

		public static bool UseConfigValueOverrides
		{
			get
			{
				bool returnValue = true;

				try
				{
					if (System.Configuration.ConfigurationManager.AppSettings["UseConfigValueOverrides"] != null)
						return System.Configuration.ConfigurationManager.AppSettings["UseConfigValueOverrides"].ToString().ToBool();
				}
				catch { }

				return returnValue;
			}
		}

		public static AppSettings AppSettings
		{
			get
			{
				return _appSettings;
			}
		}

		public static string MaxFileSize
		{
			get
			{
				var httpRuntimeSection = System.Configuration.ConfigurationManager.GetSection("system.web/httpRuntime") as System.Web.Configuration.HttpRuntimeSection;
				return httpRuntimeSection == null ? "" : (httpRuntimeSection.MaxRequestLength * 1024).FormatBytes();
			}
		}
		#endregion

		#region Private Methods
		private static string GetKey(string key, EnvironmentMode mode, Constants.Client client)
		{
			return string.Format("{0}_{1}_{2}", key, mode.ToString(), client.ToString());
		}
		#endregion

		#region Public Methods
		public static string GetConnectionString(string key)
		{
			return GetConnectionString(key, CurrentEnviornment, CurrentClient);
		}
		public static string GetConnectionString(string key, EnvironmentMode mode, Constants.Client client)
		{
			string returnValue = string.Empty;

			string modifiedKey = GetKey(key, mode, client);
			returnValue = System.Configuration.ConfigurationManager.ConnectionStrings[modifiedKey].ConnectionString ?? string.Empty;

			return returnValue;
		}

		public static T GetAppSetting<T>(VariableKey key, T defaultValue = default(T))
		{
			return GetAppSetting<T>(key.ToString(), CurrentEnviornment, CurrentClient, defaultValue);
		}
		public static T GetAppSetting<T>(string key, T defaultValue = default(T))
		{
			return GetAppSetting<T>(key, CurrentEnviornment, CurrentClient, defaultValue);
		}
		public static T GetAppSetting<T>(string key, EnvironmentMode mode, Constants.Client client, T defaultValue = default(T))
		{
			string returnValue = string.Empty;

			string modifiedKey = key;
			//string modifiedKey = GetKey(key, mode, client);
			returnValue = System.Configuration.ConfigurationManager.AppSettings[modifiedKey];
			//if (returnValue == null)
			//    returnValue = DefaultConfigValues.GetDefaultConfigValue(key) ?? string.Empty;

			//if (UseConfigValueOverrides)
			//{
			//    string overridenValue = GetOverridenValue(key, mode, client);
			//    if (overridenValue != null)
			//        returnValue = overridenValue;
			//}

			return VariableParser.GetVar<T>(returnValue, defaultValue);
		}

		/// <summary>
		/// This is server as a safety net to override certain values that can accidently be set wrong such as Credit Card processing settings. - JHE
		/// </summary>
		/// <param name="key"></param>
		/// <param name="mode"></param>
		/// <param name="client"></param>
		/// <returns></returns>
		private static string GetOverridenValue(string key, EnvironmentMode mode, Constants.Client client)
		{
			if (mode != EnvironmentMode.PROD)
			{
				if (key == "IsPaymentLiveMode")
					return "false";
				else if (key == "IsPaymentTestTransaction")
					return "true";
			}
			return null;
		}

		public static string GetAbsoluteFolder(string folder, VariableKey variableKey = VariableKey.FileUploadAbsolutePath)
		{
			if (string.IsNullOrEmpty(folder))
				throw new ArgumentException("You must specify a folder", "folder");
			string absoluteFolder = GetAppSetting<string>(variableKey).AppendBackSlash() + folder.AppendBackSlash();
			try
			{
				if (!Directory.Exists(absoluteFolder))
					Directory.CreateDirectory(absoluteFolder);
			}
			catch { }
			return absoluteFolder;
		}

		public static string GetWebFolder(string folder)
		{
			if (string.IsNullOrEmpty(folder))
				throw new ArgumentException("You must specify a folder", "folder");
			return FileUploadWebPath + folder.AppendForwardSlash();
		}

		/// <summary>
		/// For the secure file uploads folder
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		public static string GetSecureUploadWebFolder(string folder)
		{
			if (string.IsNullOrEmpty(folder))
				throw new ArgumentException("You must specify a folder", "folder");
			return SecureFileUploadWebPath + folder.AppendForwardSlash();
		}

		public static string GetAbsoluteUploadPath(params string[] folders)
		{
			string path = GetAppSetting<string>(VariableKey.FileUploadAbsolutePath).AppendBackSlash();

			if (folders == null)
				return path;

			path = folders.Aggregate(path, (tempPath, folder) => tempPath.AppendBackSlash() + folder).AppendBackSlash();

			return path;
		}

		public static string GetWebUploadPath(params string[] folders)
		{
			string path = FileUploadWebPath;

			if (folders == null)
				return path;

			path = folders.Aggregate(path, (tempPath, folder) => tempPath.AppendForwardSlash() + folder).AppendForwardSlash();

			return path;
		}

		public static string GetConfigValues(string sectionName, string key)
		{
			string value = string.Empty;
			try
			{
				value = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection(sectionName))[key];
			}
			catch (ConfigurationErrorsException)
			{
				value = string.Empty;
			}
			catch (NullReferenceException)
			{
				value = string.Empty;
			}
			return value;
		}
		#endregion
	}

	#region Helper Classes
	public class AppSettings : NameValueCollection
	{
		public new string this[string key]
		{
			get
			{
				return ConfigurationManager.GetAppSetting<string>(key, ConfigurationManager.CurrentEnviornment, ConfigurationManager.CurrentClient);
			}
		}
	}
	#endregion
}
