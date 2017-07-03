using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using NetSteps.Common;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Sites.Common.Configuration;

namespace NetSteps.Data.Entities
{
	/// <summary>
	/// Author: John Egbert
	/// Description: An extension of ApplicationContextCommon
	/// Created: 2/11/2011
	/// </summary>
	public class ApplicationContext
	{
		#region Singleton
		private static object _syncRoot = new object();
		public static ApplicationContext Instance
		{
			get { return Singleton.instance; }
		}

		private ApplicationContext() { }

		private abstract class Singleton
		{
			static Singleton() { } // DO NOT REMOVE: Static constructor required to prevent beforefieldinit flag from being set
			public static readonly ApplicationContext instance = new ApplicationContext();
		}
		#endregion

		#region Common Passthrus
		public bool IsDebug { get { return ApplicationContextCommon.Instance.IsDebug; } }
		public bool IsDeveloperEnvironment { get { return ApplicationContextCommon.Instance.IsDeveloperEnvironment; } }
		public bool IsLocalHost { get { return ApplicationContextCommon.Instance.IsLocalHost; } }
		public bool SupportEmailFunctionality { get { return ApplicationContextCommon.Instance.SupportEmailFunctionality; } }
		public short ApplicationID { get { return ApplicationContextCommon.Instance.ApplicationID; } set { ApplicationContextCommon.Instance.ApplicationID = value; } }
		public bool IsWebApp { get { return ApplicationContextCommon.Instance.IsWebApp; } set { ApplicationContextCommon.Instance.IsWebApp = value; } }
		public int CurrentUserID { get { return ApplicationContextCommon.Instance.CurrentUserID; } }
		public int ApplicationDefaultLanguageID { get { return ApplicationContextCommon.Instance.ApplicationDefaultLanguageID; } }
		public int CurrentLanguageID { get { return ApplicationContextCommon.Instance.CurrentLanguageID; } set { ApplicationContextCommon.Instance.CurrentLanguageID = value; } }
  		public int GetDefaultLanguageID() { return ApplicationContextCommon.Instance.GetDefaultLanguageID(); }
		public int ApplicationDefaultMarketID { get { return ApplicationContextCommon.Instance.ApplicationDefaultMarketID; } }
		public int CurrentMarketID { get { return ApplicationContextCommon.Instance.CurrentMarketID; } set { ApplicationContextCommon.Instance.CurrentMarketID = value; } }
		public int ApplicationDefaultCountryID { get { return ApplicationContextCommon.Instance.ApplicationDefaultCountryID; } }
		public int CurrentCountryID { get { return ApplicationContextCommon.Instance.CurrentCountryID; } set { ApplicationContextCommon.Instance.CurrentCountryID = value; } }
		public bool EnableApplicationUsageLogging { get { return ApplicationContextCommon.Instance.EnableApplicationUsageLogging; } set { ApplicationContextCommon.Instance.EnableApplicationUsageLogging = value; } }
		public string GetServerIPAddress() { return ApplicationContextCommon.Instance.GetServerIPAddress(); }
		public Dictionary<IPAddress, IPAddress> GetAllNetworkInterfaceIpv4Addresses() { return ApplicationContextCommon.Instance.GetAllNetworkInterfaceIpv4Addresses(); }
		public string NewLine { get { return ApplicationContextCommon.Instance.NewLine; } }
		public CultureInfo ApplicationDefaultCultureInfo { get { return ApplicationContextCommon.Instance.ApplicationDefaultCultureInfo; } }
		public DateTime DateTimeNow { get { return ApplicationContextCommon.Instance.DateTimeNow; } set { ApplicationContextCommon.Instance.DateTimeNow = value; } }
		public void SetDateTimeNow(DateTime? date) { ApplicationContextCommon.Instance.SetDateTimeNow(date); }
		public BasicUser DevelopmentCorpHelperLogin { get { return ApplicationContextCommon.Instance.DevelopmentCorpHelperLogin; } }
		public BasicUser DevelopmentDistributorHelperLogin { get { return ApplicationContextCommon.Instance.DevelopmentDistributorHelperLogin; } }
		public TimeZoneInfo ClientTimeZoneInfo { get { return ApplicationContextCommon.Instance.ClientTimeZoneInfo; } }
		public int ClientTimeZoneOffset { get { return ApplicationContextCommon.Instance.ClientTimeZoneOffset; } }
		public int CorporateAccountID { get { return ApplicationContextCommon.Instance.CorporateAccountID; } }
		public bool UseDefaultBundling { get { return ApplicationContextCommon.Instance.UseDefaultBundling; } }
		public bool AllowBundleQuantityUpdate { get { return ApplicationContextCommon.Instance.AllowBundleQuantityUpdate; } }
		public TimeZoneInfo CorporateTimeZoneInfo { get { return ApplicationContextCommon.Instance.CorporateTimeZoneInfo; } }
		#endregion

		public virtual Market CurrentMarket
		{
			get
			{
				return SmallCollectionCache.Instance.Markets.GetById(CurrentMarketID);
			}
		}

      
		protected Account _currentAccount = null;
		public virtual Account CurrentAccount
		{
			get
			{
				if (IsWebApp)
				{
					if (HttpContext.Current != null && HttpContext.Current.Session != null)
						return (Account)HttpContext.Current.Session["CurrentAccount"];
					else
						return null;
				}
				else
					return _currentAccount;
			}
			set
			{
				if (IsWebApp)
				{
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session["CurrentAccount"] = value;
				}
				else
					_currentAccount = value;
			}
		}

		public virtual IUser CurrentUser
		{
			get
			{
				if (IsWebApp)
				{
					return (IUser)Create.New<IExecutionContext>().CurrentUser;
				}

				return ApplicationContextCommon.Instance.CurrentUser;
			} 
			set
			{
				if (IsWebApp)
				{
					Create.New<IExecutionContext>().CurrentUser = value;
				}

				ApplicationContextCommon.Instance.CurrentUser = value;
			}
		}

		protected AutoshipOrder _currentAutoship = null;
		public virtual AutoshipOrder CurrentAutoship
		{
			get
			{
				if (IsWebApp)
				{
					if (HttpContext.Current != null && HttpContext.Current.Session != null)
						return (AutoshipOrder)HttpContext.Current.Session["CurrentAutoship"];
					else
						return null;
				}
				else
					return _currentAutoship;
			}
			set
			{
				if (IsWebApp)
				{
					if (HttpContext.Current.Session != null)
						HttpContext.Current.Session["CurrentAutoship"] = value;
				}
				else
					_currentAutoship = value;
			}
		}

		protected Role _anonymousRole = null;
		public virtual Role AnonymousRole
		{
			get
			{
				if (_anonymousRole == null)
				{
					lock (_syncRoot)
					{
						if (_anonymousRole == null)
						{
							var roles = Role.LoadAll();
							_anonymousRole = roles.FirstOrDefault(r => r.Name == "Anonymous");
							if (_anonymousRole != null)
								_anonymousRole = Role.LoadFull(_anonymousRole.RoleID);

							SmallCollectionCache.Instance.Roles.DataChanged -= new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Roles.DataChanged += new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Functions.DataChanged -= new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Functions.DataChanged += new System.EventHandler(Functions_DataChanged);
						}
					}
				}

				return _anonymousRole;
			}
		}

		protected Role _workstationUserRole = null;
		public virtual Role WorkstationUserRole
		{
			get
			{
				if (_workstationUserRole == null)
				{
					lock (_syncRoot)
					{
						if (_workstationUserRole == null)
						{
							var roles = Role.LoadAll();
							_workstationUserRole = roles.FirstOrDefault(r => r.TermName == "WorkstationUser");
							if (_workstationUserRole != null)
								_workstationUserRole = Role.LoadFull(_workstationUserRole.RoleID);

							SmallCollectionCache.Instance.Roles.DataChanged -= new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Roles.DataChanged += new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Functions.DataChanged -= new System.EventHandler(Functions_DataChanged);
							SmallCollectionCache.Instance.Functions.DataChanged += new System.EventHandler(Functions_DataChanged);
						}
					}
				}

				return _workstationUserRole;
			}
		}

		void Functions_DataChanged(object sender, System.EventArgs e)
		{
			_anonymousRole = null;
			_workstationUserRole = null;
		}

		protected short _siteTypeID = 0;
		public virtual short SiteTypeID
		{
			get
			{
				if (_siteTypeID == 0)
				{
					_siteTypeID = Create.New<ISitesConfiguration>().SiteTypeID;
				}

				return _siteTypeID;
			}
		}

		protected int? _defaultStoreFrontID = null;
		public virtual int StoreFrontID
		{
			get
			{
				int? storeFrontID = null;

				if (IsWebApp
					&& HttpContext.Current != null
					&& HttpContext.Current.Session != null)
				{
					storeFrontID = HttpContext.Current.Session["StoreFrontID"] as int?;

					// Validate market
					if (storeFrontID != null)
					{
						var marketStoreFront = SmallCollectionCache.Instance.MarketStoreFronts
							.FirstOrDefault(x => x.StoreFrontID == storeFrontID.Value && x.SiteTypeID == SiteTypeID);
						if (marketStoreFront != null
							&& marketStoreFront.MarketID != CurrentMarketID)
						{
							// Markets did not match, reset storefront
							storeFrontID = null;
						}
					}

					if (storeFrontID == null)
					{
						var marketStoreFront = SmallCollectionCache.Instance.MarketStoreFronts
							.FirstOrDefault(x => x.MarketID == CurrentMarketID && x.SiteTypeID == SiteTypeID);
						if (marketStoreFront != null)
						{
							HttpContext.Current.Session["StoreFrontID"] = storeFrontID = marketStoreFront.StoreFrontID;
						}
					}
				}

				if (storeFrontID == null && _defaultStoreFrontID == null)
				{
					_defaultStoreFrontID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.StoreFrontID, 1);
				}

				return storeFrontID ?? _defaultStoreFrontID.Value;
			}
		}

		protected string _currentCultureInfo = string.Empty;
		public virtual string CurrentCultureInfo
		{
			get
			{
				if (IsWebApp)
				{
                   
					return Create.New<IExecutionContext>().CurrentCultureInfo;
				}

				return _currentCultureInfo;
			}
			set
			{
				if (IsWebApp)
				{
					Create.New<IExecutionContext>().CurrentCultureInfo = value;
				}

				_currentCultureInfo = value;
			}
		}

		private bool? _usesEncoreCommissions;
		public bool UsesEncoreCommissions
		{
			get
			{
				return _usesEncoreCommissions ??
					(_usesEncoreCommissions = ConfigurationManager.GetAppSetting<bool>("UsesEncoreCommissions", true)).Value;
			}
			set
			{
				_usesEncoreCommissions = value;
			}
		}

        public int EnvironmentCountry
        {
            get
            {
                return ConfigurationManager.GetAppSetting<int>("EnvironmentCountry_Aux", 1);
            }
        }
	}
}
