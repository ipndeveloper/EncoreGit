using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Web;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;

namespace NetSteps.Common
{
	public class ApplicationContextCommon
	{
		private static Lazy<ApplicationContextCommon> __singleton = new Lazy<ApplicationContextCommon>(() => new ApplicationContextCommon(), LazyThreadSafetyMode.ExecutionAndPublication);

		public static ApplicationContextCommon Instance
		{
			get { return __singleton.Value; }
		}

		private ApplicationContextCommon() { /* disabled except for singleton */ }

		public bool IsDebug
		{
			get
			{
#if DEBUG
				return true;
#else
								return false;
#endif
			}
		}

		public bool IsDeveloperEnvironment
		{
			get { return ConfigurationManager.GetAppSetting<bool>("IsDeveloperEnvironment", false); }
		}

		public bool IsLocalHost
		{
			get
			{
				try
				{
					if (!AreServerVariablesAvailable())
						return false;

					if (HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString().IndexOf("127.0.0.1") >= 0 ||
							HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString().ToUpper().IndexOf("LOCALHOST") >= 0 ||
							HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString().ToUpper().Replace("WWW.", string.Empty).StartsWith("LOCALHOST"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
		}
		private bool AreServerVariablesAvailable()
		{
			if (HttpContext.Current != null &&
					HttpContext.Current.Request != null &&
					HttpContext.Current.Request.ServerVariables != null)
				return true;
			else
				return false;
		}

		public bool SupportEmailFunctionality
		{
			get { return ConfigurationManager.GetAppSetting<bool>("SupportEmailFunctionality", false); }
		}

		protected short _applicationID = 0;
		public virtual short ApplicationID
		{
			get
			{
				if (_applicationID == 0)
					_applicationID = ConfigurationManager.GetAppSetting<short>("ApplicationSourceID");
				return _applicationID;
			}
			set
			{
				_applicationID = value;
			}
		}

		private bool _isWebApp = true;
		public bool IsWebApp
		{
			get { return _isWebApp; }
			set { _isWebApp = value; }
		}

		protected IUser _currentUser = null;
		public virtual IUser CurrentUser
		{
			get
			{
				if (IsWebApp)
				{
					if (HttpContext.Current != null && HttpContext.Current.Session != null)
						return (IUser)HttpContext.Current.Session["CurrentUser"];
					else
						return null;
				}
				else
					return _currentUser;
			}
			set
			{
				if (IsWebApp)
				{
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session["CurrentUser"] = value;
				}
				else
					_currentUser = value;
			}
		}

		public virtual int CurrentUserID
		{
			get
			{
				return (CurrentUser != null) ? CurrentUser.UserID : 0;
			}
		}

		#region Language
		public int ApplicationDefaultLanguageID
		{
			get
			{
				return ConfigurationManager.GetAppSetting<int>("ApplicationDefaultLanguageID", 5); // Default to English (1) - JHE
			}
		}
		protected int? _currentLanguageID = null;
		public int CurrentLanguageID
		{
			get
			{
				int result = GetDefaultLanguageID();
				if (IsWebApp)
				{
					if (HttpContext.Current != null)
					{
						var ctx = HttpContext.Current;
						if (ctx.Request != null && ctx.Request.Cookies.AllKeys.Contains("LanguageID"))
						{
							string reqLang = ctx.Request.Cookies["LanguageID"].Value;
							int lang;
							if (Int32.TryParse(reqLang, out lang))
							{
								result = lang;
							}
						}
					}
				}
				else
				{
					if (!_currentLanguageID.HasValue)
					{
						_currentLanguageID = GetDefaultLanguageID();
					}
					result = _currentLanguageID.Value;
				}
				return result;
			}
			set
			{
				if (IsWebApp)
				{
					if (HttpContext.Current != null)
					{
						var ctx = HttpContext.Current;
						var c = new HttpCookie("LanguageID", value.ToString());
						if (ctx.Response.Cookies.AllKeys.Contains("LanguageID"))
						{
							ctx.Response.Cookies.Set(c);
						}
						else
						{
							ctx.Response.Cookies.Add(c);
						}
					}
				}
				else
				{
					_currentLanguageID = value;
				}
			}
		}
		public int GetDefaultLanguageID()
		{
            //if (CurrentUser != null)
            //    //return CurrentUser.LanguageID == 0 ? ApplicationDefaultLanguageID : CurrentUser.LanguageID;
            //else
				return ApplicationDefaultLanguageID;
		}
		#endregion

		#region Market
		public int ApplicationDefaultMarketID
		{
			get
			{
				return ConfigurationManager.GetAppSetting<int>("ApplicationDefaultMarketID", 1); // Default to US (1)
			}
		}
		protected int? _currentMarketID = null;
		public int CurrentMarketID
		{
			get
			{
				if (IsWebApp)
				{
					if (HttpContext.Current.Session["CurrentMarketID"] == null)
						HttpContext.Current.Session["CurrentMarketID"] = ApplicationDefaultMarketID;

					return System.Convert.ToInt32(HttpContext.Current.Session["CurrentMarketID"]);
				}
				else
				{
					if (_currentMarketID == null)
						_currentMarketID = ApplicationDefaultMarketID;
					return _currentMarketID.Value;
				}
			}
			set
			{
				if (IsWebApp
						&& HttpContext.Current != null
						&& HttpContext.Current.Session != null)
				{
					HttpContext.Current.Session["CurrentMarketID"] = _currentMarketID = value;
				}
				else
				{
					_currentMarketID = value;
				}
			}
		}
		#endregion

		#region Country
		public int ApplicationDefaultCountryID
		{
			get
			{
				return ConfigurationManager.GetAppSetting<int>("ApplicationDefaultCountryID", 1); // Default to English (1) - JHE
			}
		}
		protected int? _currentCountryID = 0;
		public int CurrentCountryID
		{
			get
			{
				if (IsWebApp)
				{
					if (HttpContext.Current.Session == null)
					{
						return ApplicationDefaultCountryID;
					}
					else
					{
						if (HttpContext.Current.Session["CurrentCountryID"] == null)
							HttpContext.Current.Session["CurrentCountryID"] = ApplicationDefaultCountryID;

						return System.Convert.ToInt32(HttpContext.Current.Session["CurrentCountryID"]);
					}
				}
				else
				{
					if (_currentCountryID == null)
						_currentCountryID = ApplicationDefaultCountryID;
					return _currentCountryID ?? 0;
				}
			}
			set
			{
				if (IsWebApp)
					HttpContext.Current.Session["CurrentCountryID"] = value;
				else
					_currentCountryID = value;
			}
		}
		#endregion

		// TODO: Make this setting pull from the webconfig - JHE
		protected bool _enableApplicationUsageLogging = true;
		public bool EnableApplicationUsageLogging
		{
			get { return _enableApplicationUsageLogging; }
			set { _enableApplicationUsageLogging = value; }
		}


		public string GetServerIPAddress()
		{
			string strHostName = System.Net.Dns.GetHostName();
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			return ipAddress.ToString();
		}

		public Dictionary<IPAddress, IPAddress> GetAllNetworkInterfaceIpv4Addresses()
		{
			var map = new Dictionary<IPAddress, IPAddress>();

			foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (var uipi in ni.GetIPProperties().UnicastAddresses)
				{
					if (uipi.Address.AddressFamily != AddressFamily.InterNetwork) continue;

					if (uipi.IPv4Mask == null) continue; //ignore 127.0.0.1 
					map[uipi.Address] = uipi.IPv4Mask;
				}
			}
			return map;
		}

		public string NewLine
		{
			get
			{
				if (HttpContext.Current != null)
					return "<br />";
				return Environment.NewLine;
			}
		}

		protected CultureInfo _applicationDefaultCultureInfo = new CultureInfo("en-US"); // Default to English (United States) - JHE
		public CultureInfo ApplicationDefaultCultureInfo
		{
			get
			{
				return _applicationDefaultCultureInfo;
			}
		}

		/// <summary>
		///	Used to simulate different different views of content/objects - JHE
		///	All Entities/Methods should key off this property instead of DateTime.Now - JHE
		///	If DateTimeNow get set; The Date of that value is combined with the current DateTime.Now time for the simulated DateTime - JHE
		/// </summary>
		protected DateTime? _dateTimeNow = null;
		public DateTime DateTimeNow
		{
			get
			{
				if (IsWebDateTimeNowSet())
					return ((DateTime)HttpContext.Current.Session["DateTimeNow"]).AddTime(DateTime.Now);
				else if (IsAppDateTimeNowSet())
					return _dateTimeNow.Value.AddTime(DateTime.Now);
				else
					return DateTime.Now;
			}
			set
			{
				if (IsWebApp)
					HttpContext.Current.Session["DateTimeNow"] = value;
				else
					_dateTimeNow = value;
			}
		}

		private bool IsWebDateTimeNowSet()
		{
			bool value = IsWebApp && HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["DateTimeNow"] != null;
			return value;
		}

		private bool IsAppDateTimeNowSet()
		{
			bool value = _dateTimeNow.HasValue;
			return value;
		}

		/// <summary>
		/// Returns if an override DateTimeNow value has been set
		/// </summary>
		/// <returns></returns>
		public bool IsDateTimeNowSet()
		{
			return IsWebDateTimeNowSet() || IsAppDateTimeNowSet();
		}

		public void SetDateTimeNow(DateTime? date)
		{
			if (IsWebApp)
				HttpContext.Current.Session["DateTimeNow"] = date;
			else
				_dateTimeNow = date;
		}

		public BasicUser DevelopmentCorpHelperLogin
		{
			get
			{
				var user = new BasicUser
									 {
										 Username =
											 ConfigurationManager.GetAppSetting("DevelopmentHelperCorpLoginUsername",
																				"nsadmin"),
										 Password =
											 ConfigurationManager.GetAppSetting("DevelopmentHelperCorpLoginPassword",
																				"n3tst3psd3m0")
									 };
				return user;
			}
		}

		public BasicUser DevelopmentDistributorHelperLogin
		{
			get
			{
				var user = new BasicUser
									 {
										 Username =
											 ConfigurationManager.GetAppSetting(
												 "DevelopmentHelperDistributorLoginUsername", "1312"),
										 Password =
											 ConfigurationManager.GetAppSetting(
												 "DevelopmentHelperDistributorLoginPassword", "sunshine")
									 };
				return user;
			}
		}



		/// <summary>
		/// This TimeZone is not completely accurate since we can't figure out which TimeZone the Web Client is using.
		/// All we can get is an TimeZoneOffset - JHE
		/// </summary>
		private const string CookieName = "TimeZoneOffset";
		public TimeZoneInfo ClientTimeZoneInfo
		{
			get
			{
				var timeZoneOffset = ClientTimeZoneOffset;

				var timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
				var clientTimeZone = timeZoneInfos.FirstOrDefault(t => t.BaseUtcOffset.TotalHours == timeZoneOffset);
				return clientTimeZone;
			}
		}
		public int ClientTimeZoneOffset
		{
			get
			{
				var request = HttpContext.Current.Request;
				if (request[CookieName] != null)
				{
					var minOffset = GetUtcOffset(request);
					return minOffset / 60; // return offset in hours, not minutes    
				}
				return 0;
			}
		}

		private int GetUtcOffset(HttpRequest request)
		{
			var cookie = request.Cookies[CookieName];
			var offset = (cookie == null) ? 0 : int.Parse(cookie.Value);
			return offset * -1;
		}

		public int CorporateAccountID
		{
			get
			{
				return ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateAccountID, 1);
			}
		}

		public bool UseDefaultBundling
		{
			get
			{
				return ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseDefaultBundling,
																												false);
			}
		}

		public bool AllowBundleQuantityUpdate
		{
			get
			{
				return ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AllowBundleQuantityUpdate,
																												false);
			}
		}

		private TimeZoneInfo _corporateTimeZoneInfo;
		public TimeZoneInfo CorporateTimeZoneInfo
		{
			get
			{
				if (_corporateTimeZoneInfo == null)
				{
					var corporateTimeZoneString = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.CorporateTimeZone);
					if (!string.IsNullOrWhiteSpace(corporateTimeZoneString))
					{
						_corporateTimeZoneInfo =
								TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id.EqualsIgnoreCase(corporateTimeZoneString))
								?? TimeZoneInfo.Local;
					}
					else
					{
						_corporateTimeZoneInfo = TimeZoneInfo.Local;
					}
				}
				return _corporateTimeZoneInfo;
			}
		}
	}
}
