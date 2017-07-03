using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Fasterflect;
using NetSteps.Accounts.Downline.Common.Models;
using NetSteps.Accounts.Downline.UI.Common;
using NetSteps.Accounts.Downline.UI.Common.Configuration;
using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Accounts.Downline.UI.Common.Search;
using NetSteps.Accounts.Downline.UI.Common.TreeView;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Hierarchy;
using NetSteps.Encore.Core.IoC;
using NDCR = NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Accounts.Downline.UI.Service
{
	public class DownlineUIService : IDownlineUIService
	{
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>(); //Developed by WCS - CSTI

		#region Members

		protected readonly NDCR.IDownlineRepository _downlineRepository;
		protected readonly IDownlineUIConfiguration _downlineUIConfiguration;
		private Lazy<IDictionary<string, IDownlineInfoCardItemProvider>> _downlineInfoCardItemProvidersFactory;
		public IDictionary<string, IDownlineInfoCardItemProvider> DownlineInfoCardItemProviders { get { return _downlineInfoCardItemProvidersFactory.Value; } }
        
        public int AccountId { get; set; } //Developed by WCS - CSTI
        public int paidAsTitleID { get; set; } //Developed by WCS - CSTI
        public int currentTitleID { get; set; } //Developed by WCS - CSTI
        public string pv { get; set; } //Developed by WCS - CSTI
        public string gv { get; set; } //Developed by WCS - CSTI
        public string dv { get; set; } //Developed by WCS - CSTI
        
        #endregion

		#region Constructors

		public DownlineUIService(NDCR.IDownlineRepository downlineRepository,IDownlineUIConfiguration downlineUIConfiguration)
		{
			Contract.Requires<ArgumentNullException>(downlineRepository != null);
			Contract.Requires<ArgumentNullException>(downlineUIConfiguration != null);

			_downlineRepository = downlineRepository;
			_downlineUIConfiguration = downlineUIConfiguration;

			_downlineInfoCardItemProvidersFactory = new Lazy<IDictionary<string, IDownlineInfoCardItemProvider>>(() =>
			{
				var providers = new Dictionary<string, IDownlineInfoCardItemProvider>(StringComparer.OrdinalIgnoreCase);
				InitializeDownlineInfoCardItemProviders(providers);
				return providers;
			});
		}

		#endregion

		#region IDownlineUIService

		public virtual ITreeNodeModel GetTreeNodes(int rootAccountId, int? maxLevels = null)
		{
			// We get one extra level from the repo in order to know if the "last" level has children.
			var getDownlineParameters = Create.New<IGetDownlineContext>();
			getDownlineParameters.RootAccountId = rootAccountId;
			getDownlineParameters.MaxLevels = maxLevels.HasValue ? maxLevels + 1 : null;
			getDownlineParameters.AccountTypeIds = SmallCollectionCache.Instance.AccountTypes.Where(x => x.Active).Select(y => (short)y.AccountTypeID);

			var data = _downlineRepository.GetDownline(getDownlineParameters);

			var rootAccountData = data.FirstOrDefault(x => x.AccountId == rootAccountId);
			// Safety check
			if (rootAccountData == null)
			{
				return null;
			}

			// A quick lookup to know which nodes have children.
			var accountIdsWithChildren = new HashSet<int>(
				data
					.Where(x => x.ParentAccountId.HasValue)
					.Select(x => x.ParentAccountId.Value)
					.Distinct()
			);

			// Filter off the last level, if necessary.
			var accountDataItems = maxLevels.HasValue
				? data
					.Where(x => x.TreeLevel <= rootAccountData.TreeLevel + maxLevels)
					.ToArray()
				: data;

			var customTreeData = GetCustomTreeData(accountDataItems);

			var nodes = accountDataItems
				.ToDictionary(x => x.AccountId, x =>
				{
					dynamic customData;
					customTreeData.TryGetValue(x.AccountId, out customData);

					return (ITreeNodeModel)CreateTreeNodeModel(
						accountData: x,
						isRootNode: x.TreeLevel == rootAccountData.TreeLevel,
						hasChildren: accountIdsWithChildren.Contains(x.AccountId),
						customData: customData
					);
				})
				.AddChildrenToParents(
					x => x.ParentId.GetValueOrDefault(),
					x => x.ChildNodes
				);

			return nodes[rootAccountId];
		}

		public virtual IList<ISearchDownlineResultModel> SearchDownline(int rootAccountId, string query)
		{
			var searchDownlineParameters = Create.New<ISearchDownlineContext>();
			searchDownlineParameters.RootAccountId = rootAccountId;
			searchDownlineParameters.Query = query;

			var data = _downlineRepository.Search(searchDownlineParameters);

			return data
				.Select(CreateSearchDownlineResultModel)
				.ToList();
		}

		public virtual IList<string> GetTreePath(int rootAccountId, int targetAccountId)
		{
			var data = _downlineRepository.GetUplineAccountIds(targetAccountId);

			return data
				.SkipWhile(x => x != rootAccountId)
				.Select(x => "#" + x.ToString())
				.ToList();
		}

		public virtual void LoadDownlineInfoCardModelOptions(
			dynamic optionsBag,
			string getDataUrl,
			string baseEmailUrl)
		{
			// Code contracts rewriter doesn't work with dynamics
			if (optionsBag == null)
			{
				throw new ArgumentNullException("optionsBag");
			}

			optionsBag.HideCardText = Translation.GetTerm("AccountInfoCard-HideCardText", "Hide card");
			optionsBag.ShowCardText = Translation.GetTerm("AccountInfoCard-ShowCardText", "Show card");
			optionsBag.UpdatingText = Translation.GetTerm("AccountInfoCard-UpdatingText", "Updating...");
			optionsBag.IsEmailEnabled = ApplicationContext.Instance.SupportEmailFunctionality;
			optionsBag.BaseEmailUrl = baseEmailUrl;
			optionsBag.GetDataUrl = getDataUrl;
		}

		public virtual void LoadDownlineInfoCardModelData(
			dynamic dataBag,
			int rootAccountId,
			int accountId)
		{
			// Code contracts rewriter doesn't work with dynamics
			if (dataBag == null)
			{
				throw new ArgumentNullException("dataBag");
			}

            IDownlineAccountInfo accountInfo = null;
            IDownlineAccountInfo accountInfoPrueba = null;

            if (dataBag.PeriodID != null)
            {
                //accountInfo = new NetSteps.Data.Entities.Repositories.DownlineRepository().GetDownlineAccountInfo(rootAccountId, accountId, dataBag.PeriodID);
            }
            else
            {
                DownlineRepository objN = new DownlineRepository();
                accountInfoPrueba = objN.GetDownlineAccountInfo(rootAccountId, accountId);
                accountInfo = objN.GetDownlineAccountInfo(rootAccountId, accountId);  //_downlineRepository.GetDownlineAccountInfo(rootAccountId, accountId);
            }
            

			if (accountInfo == null)
			{
				dataBag.AccountId = null;
				dataBag.TitleText = ErrorNoDataForAccount();
				dataBag.Email = string.Empty;
				dataBag.ListItems = null;
				return;
			}

			var context = CreateDownlineInfoCardContext(rootAccountId, accountInfo);

			dataBag.AccountId = accountInfo.AccountId;
			dataBag.TitleText = FormatDownlineInfoCardTitleText(context);
			dataBag.Email = accountInfo.EmailAddress;
			dataBag.ListItems = GetDownlineInfoCardListItems(context).ToArray();
            dataBag.HiddenListItems = GetDownlineInfoCardHiddenListItems(context).ToArray(); //Developed by WCS - CSTI
		}

		#endregion

		#region Static Helpers
		/// <summary>
		/// Template names to be passed in the client-side JS model.
		/// </summary>
		public static class DownlineInfoCardItemTemplates
		{
			public static string TwoColumnListItem { get { return "DownlineInfoCard-TwoColumnListItemTemplate"; } }
		}

		/// <summary>
		/// Standard list item keys supported by the <see cref="DownlineUIService"/>.
		/// </summary>
		public static class DownlineInfoCardListItemKeys
		{
            //public static string Name { get { return "Name"; } }
            //public static string TreeLevel { get { return "TreeLevel"; } }
            //public static string DownlineCount { get { return "DownlineCount"; } }
            //public static string AccountNumber { get { return "AccountNumber"; } }
            //public static string Sponsor { get { return "Sponsor"; } }
            //public static string Enroller { get { return "Enroller"; } }
            //public static string EnrollmentDate { get { return "EnrollmentDate"; } }
            //public static string RenewalDate { get { return "RenewalDate"; } }
            //public static string AccountType { get { return "AccountType"; } }
            //public static string AccountStatus { get { return "AccountStatus"; } }
            //public static string EmailAddress { get { return "EmailAddress"; } }
            //public static string PhoneNumber { get { return "PhoneNumber"; } }
            //public static string PwsUrl { get { return "PwsUrl"; } }
            //public static string Address { get { return "Address"; } }
            //public static string CityStatePostalCode { get { return "CityStatePostalCode"; } }
            //public static string Country { get { return "Country"; } }
            //public static string LastOrderCommissionDate { get { return "LastOrderCommissionDate"; } }
            //public static string NextAutoshipRunDate { get { return "NextAutoshipRunDate"; } }

            public static string Name { get { return "Name"; } }
            public static string TreeLevel { get { return "TreeLevel"; } }
            public static string DownlineCount { get { return "DownlineCount"; } }
            public static string AccountNumber { get { return "AccountNumber"; } }
            public static string Sponsor { get { return "Sponsor"; } }
            public static string Enroller { get { return "Enroller"; } }
            public static string EnrollmentDate { get { return "EnrollmentDate"; } }
            public static string RenewalDate { get { return "RenewalDate"; } }
            public static string AccountType { get { return "AccountType"; } }
            public static string AccountStatus { get { return "AccountStatus"; } }
            public static string EmailAddress { get { return "EmailAddress"; } }
            public static string PhoneNumber { get { return "PhoneNumber"; } }
            public static string PwsUrl { get { return "PwsUrl"; } }
            public static string Address { get { return "Address"; } }
            public static string CityStatePostalCode { get { return "CityStatePostalCode"; } }
            public static string Country { get { return "Country"; } }
            public static string LastOrderCommissionDate { get { return "LastOrderCommissionDate"; } }
            public static string NextAutoshipRunDate { get { return "NextAutoshipRunDate"; } }

            public static string PV { get { return "PV"; } }
            public static string GV { get { return "GV"; } }
            public static string DV { get { return "DV"; } }
            public static string CareerTitle { get { return "CareerTitle"; } }
            public static string PaidAsTitle { get { return "PaidAsTitle"; } }

		}

		/// <summary>
		/// Returns a two-column list item object with term-translated label.
		/// </summary>
		/// <param name="defaultLabel">The default (non-term-translated) label to be displayed in the first column.</param>
		/// <param name="value">The value to be displayed in the second column.</param>
		public static object CreateDownlineInfoCardTwoColumnListItem(string defaultLabel, string value)
		{
			Contract.Ensures(Contract.Result<object>() != null);

			var termName = Regex.Replace(defaultLabel ?? string.Empty, @"[\W]", string.Empty);
			if (!string.IsNullOrEmpty(termName))
			{
				defaultLabel = Translation.GetTerm("DownlineInfoCard-" + termName, defaultLabel);
			}

			return new
			{
				Template = DownlineInfoCardItemTemplates.TwoColumnListItem,
				Label = defaultLabel,
				Value = string.IsNullOrWhiteSpace(value) ? Translation.GetTerm("N/A") : value
			};
		}
		#endregion

		#region Helpers




        #region GetAccouts 

        #endregion

        public virtual IDictionary<int, dynamic> GetCustomTreeData(
			IEnumerable<IDownlineData> accountDataItems)
		{
			Contract.Requires<ArgumentNullException>(accountDataItems != null);
			Contract.Ensures(Contract.Result<IDictionary<int, dynamic>>() != null);

            var commissionsService = Create.New<ICommissionsService>();

			var treeAccountIds = accountDataItems
				.Select(x => x.AccountId)
				.ToArray();
			
			var customDataDict = new Dictionary<int, dynamic>();

			// Uses the downline cache for now.
			if (ApplicationContext.Instance.UsesEncoreCommissions)
			{
				var downline = DownlineCache.GetDownline(0);
				if (downline != null)
				{
					foreach (var accountId in treeAccountIds)
					{
						dynamic downlineNode;
						if (downline.Lookup.TryGetValue(accountId, out downlineNode))
						{
							dynamic customData = new DynamicDictionary();
							MemberGetter getter;

							if (downline.DynamicTypeGetters.TryGetValue("CurrentTitle", out getter))
							{
								int? currentTitleId = getter(downlineNode);
								customData.CurrentTitle = currentTitleId.HasValue
									? commissionsService.GetTitle(currentTitleId.Value).TermName
									: string.Empty;
							}

							if (downline.DynamicTypeGetters.TryGetValue("IsCommissionQualified", out getter))
							{
								bool? isCommissionQualified = getter(downlineNode);
								customData.CommissionQualifiedStatus = isCommissionQualified ?? false
									? "Qualified"
									: "UnQualified";
							}

							customDataDict[accountId] = customData;
						}
					}
				}
			}

			return customDataDict;
		}

		public virtual ITreeNodeModel CreateTreeNodeModel(
			IDownlineData accountData,
			bool isRootNode,
			bool hasChildren,
			dynamic customData = null)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<ITreeNodeModel>() != null);

			var node = Create.New<ITreeNodeModel>();
			node.Id = accountData.AccountId;
			node.ParentId = accountData.ParentAccountId;
			node.Text = FormatTreeNodeText(accountData, customData);
			node.HoverText = FormatTreeNodeHoverText(accountData, customData);
			node.NodeClass = FormatTreeNodeClass(accountData, isRootNode, hasChildren, customData);
			node.LinkHtmlAttributes = FormatTreeNodeLinkHtmlAttributes(accountData, customData);
			return node;
		}

		public virtual string FormatTreeNodeText(
			IDownlineData accountData,
			dynamic customData = null)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<string>() != null);

			return string.Format("{0} {1} - {2}", accountData.FirstName, accountData.LastName, accountData.AccountNumber);
		}

		public virtual string FormatTreeNodeHoverText(
			IDownlineData accountData,
			dynamic customData = null)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<string>() != null);

			var hoverText = string.Format("{0} {1}", accountData.FirstName, accountData.LastName);

			if (customData != null)
			{
				string title = customData.CurrentTitle;
				if (!string.IsNullOrEmpty(title))
				{
					hoverText = string.Format("{0} - {1}", hoverText, title);
				}
			}

			return hoverText;
		}

		public virtual string FormatTreeNodeClass(
			IDownlineData accountData,
			bool isRootNode,
			bool hasChildren,
			dynamic customData = null)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<string>() != null);

			// Nested ternaries are bad, but...
			return hasChildren
				? isRootNode
					? "jstree-open"
					: "jstree-closed"
				: "jstree-leaf";
		}

		public virtual IDictionary<string, object> FormatTreeNodeLinkHtmlAttributes(
			IDownlineData accountData,
			dynamic customData = null)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

			var htmlAttributes = new Dictionary<string, object>();

			Constants.AccountType accountType;
			if (Enum.TryParse<Constants.AccountType>(accountData.AccountTypeId.ToString(), out accountType))
			{
				htmlAttributes["data-CustomerType"] = accountType.ToString();
			}

			if (customData != null)
			{
				string commissionQualifiedStatus = customData.CommissionQualifiedStatus;
				if (!string.IsNullOrEmpty(commissionQualifiedStatus))
				{
					htmlAttributes["data-CommissionQualified"] = commissionQualifiedStatus;
				}
			}

			return htmlAttributes;
		}

		public virtual ISearchDownlineResultModel CreateSearchDownlineResultModel(
			IDownlineData accountData)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<ISearchDownlineResultModel>() != null);

			var model = Create.New<ISearchDownlineResultModel>();
			model.id = accountData.AccountId;
			model.text = FormatSearchDownlineResultText(accountData);
			return model;
		}

		public virtual string FormatSearchDownlineResultText(
			IDownlineData accountData)
		{
			Contract.Requires<ArgumentNullException>(accountData != null);
			Contract.Ensures(Contract.Result<string>() != null);

			return string.Format("{0} {1} (#{2})", accountData.FirstName, accountData.LastName, accountData.AccountNumber);
		}

		public virtual IDownlineInfoCardContext CreateDownlineInfoCardContext(int rootAccountId, IDownlineAccountInfo accountInfo)
		{
			Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
			Contract.Requires<ArgumentNullException>(accountInfo != null);

			var context = Create.New<IDownlineInfoCardContext>();
			context.RootAccountId = rootAccountId;
			context.AccountInfo = accountInfo;
			context.CustomData = new DynamicDictionary();
			context.CustomDataDictionary = context.CustomData.AsDictionary();

			foreach (var infoCardExtension in _downlineUIConfiguration.InfoCardExtensions)
			{
				try
				{
					infoCardExtension.LoadData(context);
				}
				catch (Exception ex)
				{
					ex.Log(accountID: context.AccountInfo.AccountId);
				}
			}
			
			return context;
		}

        //protected virtual string FormatData(string dataType, string data, string columnName)
        //{
        //    try
        //    {
        //        if (String.IsNullOrWhiteSpace(data))
        //            Translation.GetTerm("N/A");
        //        if (dataType == "System.String")
        //            return Translation.GetTerm(data);
        //        else if (dataType == "System.Boolean")
        //            return data == "True" ? Translation.GetTerm("Yes") : Translation.GetTerm("No");
        //        else if (dataType == "System.DateTime")
        //        {
        //            DateTime parsedValue = DateTime.Parse(data);
        //            return parsedValue.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
        //        }
        //        else if (dataType == "System.Int32")
        //        {
        //            int parsedValue = Int32.Parse(data);
        //            return parsedValue.ToString();
        //        }
        //        else if (dataType == "System.Decimal")
        //        {
        //            if (columnName.Contains("Percentage"))
        //            {
        //                decimal parsedValue = Decimal.Parse(data);
        //                return parsedValue.ToPercentageString(NetSteps.Web.Mvc.Helpers.CoreContext.CurrentCultureInfo);
        //            }
        //            else if (columnName.Contains("Volume"))
        //            {
        //                decimal parsedValue = Decimal.Parse(data);
        //                return parsedValue.ToMoneyString(CoreContext.CurrentCultureInfo);
        //            }
        //            else
        //            {
        //                decimal parsedValue = Decimal.Parse(data);
        //                return Convert.ToDouble(parsedValue).TruncateDoubleInsertCommas();
        //            }
        //        }
        //        else
        //            return (data == null) ? Translation.GetTerm("N/A") : data;
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        throw exception;
        //    }
        //}

		/// <summary>
		/// Initializes the item providers for the info card.
		/// </summary>
		public virtual void InitializeDownlineInfoCardItemProviders(IDictionary<string, IDownlineInfoCardItemProvider> providers)
		{
			Contract.Requires<ArgumentNullException>(providers != null);

            #region ComissionService

            var currentTitle = _commissionsService.GetTitle(currentTitleID);
            var paidAsTitle = _commissionsService.GetTitle(paidAsTitleID);

            #endregion

			providers[DownlineInfoCardListItemKeys.Name] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Name", context.AccountInfo.FirstName + " " + context.AccountInfo.LastName)
			);

            providers[DownlineInfoCardListItemKeys.AccountNumber] = new DelegateDownlineInfoCardItemProvider(context =>
                CreateDownlineInfoCardTwoColumnListItem("Account Number", context.AccountInfo.AccountNumber)
            );

			providers[DownlineInfoCardListItemKeys.TreeLevel] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Tree Level", context.AccountInfo.RelativeTreeLevel.ToString())
			);

            //Commented code by WCS - CSTI
            //providers[DownlineInfoCardListItemKeys.DownlineCount] = new DelegateDownlineInfoCardItemProvider(context =>
            //    CreateDownlineInfoCardTwoColumnListItem("Downline Count", context.AccountInfo.DownlineCount.ToString())
            //);

			providers[DownlineInfoCardListItemKeys.Sponsor] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Sponsor", context.AccountInfo.SponsorFirstName + " " + context.AccountInfo.SponsorLastName)
			);

			providers[DownlineInfoCardListItemKeys.Enroller] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Enroller", context.AccountInfo.EnrollerFirstName + " " + context.AccountInfo.EnrollerLastName)
			);

			providers[DownlineInfoCardListItemKeys.EnrollmentDate] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Enrollment Date", context.AccountInfo.EnrollmentDateUtc.UTCToLocal().ToShortDateStringDisplay(Thread.CurrentThread.CurrentUICulture))
			);
            
            //Commented code by WCS - CSTI
            //providers[DownlineInfoCardListItemKeys.RenewalDate] = new DelegateDownlineInfoCardItemProvider(context =>
            //    CreateDownlineInfoCardTwoColumnListItem("Renewal Date", context.AccountInfo.NextRenewalDateUtc.UTCToLocal().ToShortDateStringDisplay(Thread.CurrentThread.CurrentUICulture))
            //);

			providers[DownlineInfoCardListItemKeys.AccountType] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Account Type", SmallCollectionCache.Instance.AccountTypes.GetById(context.AccountInfo.AccountTypeId).GetTerm())
			);
            
            //Commented code by WCS - CSTI
            //providers[DownlineInfoCardListItemKeys.AccountStatus] = new DelegateDownlineInfoCardItemProvider(context =>
            //    CreateDownlineInfoCardTwoColumnListItem("Account Status", SmallCollectionCache.Instance.AccountStatuses.GetById(context.AccountInfo.AccountStatusId).GetTerm())
            //);

			providers[DownlineInfoCardListItemKeys.EmailAddress] = new DelegateDownlineInfoCardItemProvider(context =>
                CreateDownlineInfoCardTwoColumnListItem("E-mail", context.AccountInfo.EmailAddress)
			);

			providers[DownlineInfoCardListItemKeys.PhoneNumber] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Phone Number", context.AccountInfo.PhoneNumber.FormatPhone(Thread.CurrentThread.CurrentUICulture))
			);

			providers[DownlineInfoCardListItemKeys.PwsUrl] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("URL", context.AccountInfo.PwsUrl)
			);

			providers[DownlineInfoCardListItemKeys.Address] = new DelegateDownlineInfoCardItemProvider(context =>
                CreateDownlineInfoCardTwoColumnListItem("Main Address", context.AccountInfo.Address1)
			);

			providers[DownlineInfoCardListItemKeys.CityStatePostalCode] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("City, State", FormatCityStatePostalCode(context))
			);

			providers[DownlineInfoCardListItemKeys.Country] = new DelegateDownlineInfoCardItemProvider(context =>
				CreateDownlineInfoCardTwoColumnListItem("Country", SmallCollectionCache.Instance.Countries.GetById(context.AccountInfo.CountryId ?? 0).GetTerm())
			);

			providers[DownlineInfoCardListItemKeys.LastOrderCommissionDate] = new DelegateDownlineInfoCardItemProvider(context =>
                //CreateDownlineInfoCardTwoColumnListItem("Date of Last order", context.AccountInfo.LastOrderCommissionDateUtc.UTCToLocal().ToShortDateStringDisplay(Thread.CurrentThread.CurrentUICulture))
                CreateDownlineInfoCardTwoColumnListItem("Date of Last order", context.AccountInfo.LastOrderCommissionDateUtc.ToShortDateStringDisplay(Thread.CurrentThread.CurrentUICulture))
            );
            
            //Commented code by WCS - CSTI
            //providers[DownlineInfoCardListItemKeys.NextAutoshipRunDate] = new DelegateDownlineInfoCardItemProvider(context =>
            //    CreateDownlineInfoCardTwoColumnListItem("Autoship Process Date", context.AccountInfo.NextAutoshipRunDate.ToShortDateStringDisplay(Thread.CurrentThread.CurrentUICulture))
            //);

            //********************************************* Developed by WCS - CSTI ***********************************************
            
            providers[DownlineInfoCardListItemKeys.PV] = new DelegateDownlineInfoCardItemProvider(context =>
                //CreateDownlineInfoCardTwoColumnListItem("Personal Volume", string.Format("${0}", pv)) //EL QV NO DEBE TENER SIGNO $
                CreateDownlineInfoCardTwoColumnListItem("Personal Volume", string.Format("{0}", pv))
            );

            providers[DownlineInfoCardListItemKeys.GV] = new DelegateDownlineInfoCardItemProvider(context =>
                //CreateDownlineInfoCardTwoColumnListItem("Group Volume", string.Format("${0}", gv)) //EL QV NO DEBE TENER SIGNO $
                CreateDownlineInfoCardTwoColumnListItem("Group Volume", string.Format("{0}", gv))
            );

            providers[DownlineInfoCardListItemKeys.DV] = new DelegateDownlineInfoCardItemProvider(context =>
                //CreateDownlineInfoCardTwoColumnListItem("Downline Volume", string.Format("${0}", dv)) //EL QV NO DEBE TENER SIGNO $
                CreateDownlineInfoCardTwoColumnListItem("Downline Volume", string.Format("{0}", dv))
            );

            providers[DownlineInfoCardListItemKeys.CareerTitle] = new DelegateDownlineInfoCardItemProvider(context =>
                CreateDownlineInfoCardTwoColumnListItem("Career Title", currentTitle != null ? Translation.GetTerm(currentTitle.TermName) : String.Empty)
            );
            
            providers[DownlineInfoCardListItemKeys.PaidAsTitle] = new DelegateDownlineInfoCardItemProvider(context =>
                CreateDownlineInfoCardTwoColumnListItem("Paid As Title", paidAsTitle != null ? Translation.GetTerm(paidAsTitle.TermName) : String.Empty)
			);
            //**********************************************************************************************************************

			foreach (var infoCardExtension in _downlineUIConfiguration.InfoCardExtensions)
			{
				try
				{
					infoCardExtension.InitializeItemProviders(providers);
				}
				catch (Exception ex)
				{
					ex.Log();
				}
			}
		}

		public virtual IEnumerable<object> GetDownlineInfoCardListItems(IDownlineInfoCardContext context)
		{
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);

			IEnumerable<string> itemKeys = _downlineUIConfiguration.GetInfoCardListItemKeysToDisplay(context, DownlineInfoCardItemProviders.Keys);
            List<string> result = itemKeys.ToList().FindAll(x => !x.Equals("Sponsor") && !x.Equals("Enroller")
                                                              && !x.Equals("EnrollmentDate") && !x.Equals("EmailAddress")
                                                              && !x.Equals("PhoneNumber") && !x.Equals("PwsUrl")
                                                              && !x.Equals("Address") && !x.Equals("CityStatePostalCode")
                                                              && !x.Equals("Country") && !x.Equals("AccountType"));
                
            foreach (var itemKey in result)
			{
				IDownlineInfoCardItemProvider provider;
				if (DownlineInfoCardItemProviders.TryGetValue(itemKey, out provider))
				{
					yield return provider.GetItem(context);
				}
			}
		}

        public virtual IEnumerable<object> GetDownlineInfoCardHiddenListItems(IDownlineInfoCardContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);

            string MuestraURL = OrderExtensions.GeneralParameterVal(Account.Load(context.RootAccountId).MarketID, "MUR");

            IEnumerable<string> itemKeys = _downlineUIConfiguration.GetInfoCardListItemKeysToDisplay(context, DownlineInfoCardItemProviders.Keys);
            List<string> result = itemKeys.ToList().FindAll(x => x.Equals("Sponsor") || x.Equals("Enroller") 
                                                              || x.Equals("EnrollmentDate") || x.Equals("EmailAddress")
                                                              || x.Equals("PhoneNumber") || x.Equals("PwsUrl")
                                                              || x.Equals("Address") || x.Equals("CityStatePostalCode")
                                                              || x.Equals("Country") || x.Equals("AccountType"));

            foreach (var itemKey in result)
            {
                if (itemKey == "PwsUrl")
                {
                    if (MuestraURL == "S")
                    {
                        IDownlineInfoCardItemProvider provider;
                        if (DownlineInfoCardItemProviders.TryGetValue(itemKey, out provider))
                        {
                            yield return provider.GetItem(context);
                        }
                    }
                }
                else
                {
                    IDownlineInfoCardItemProvider provider;
                    if (DownlineInfoCardItemProviders.TryGetValue(itemKey, out provider))
                    {
                        yield return provider.GetItem(context);
                    }
                }
            }
        }

		public virtual string FormatDownlineInfoCardTitleText(IDownlineInfoCardContext context)
		{
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Ensures(Contract.Result<string>() != null);

			return context.AccountInfo.FirstName + " " + context.AccountInfo.LastName;
		}

		/// <summary>
		/// Makes the City, State, Postal Code look pretty.
		/// </summary>
		public virtual string FormatCityStatePostalCode(IDownlineInfoCardContext context)
		{
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Ensures(Contract.Result<string>() != null);

			string city = context.AccountInfo.City;
			string state = context.AccountInfo.StateAbbreviation;
			string postalCode = context.AccountInfo.PostalCode;

			bool hasCity = !string.IsNullOrWhiteSpace(city);
			bool hasState = !string.IsNullOrWhiteSpace(state);
			bool hasPostalCode = !string.IsNullOrWhiteSpace(postalCode);

			if (!hasCity && !hasState && !hasPostalCode)
			{
				return string.Empty;
			}

			var sb = new StringBuilder();

			if (hasCity)
			{
				sb.Append(city);
			}

			if (hasState || hasPostalCode)
			{
				if (hasCity)
				{
					sb.Append(", ");
				}

				if (hasState)
				{
					sb.Append(state);
				}

				if (hasPostalCode)
				{
					if (hasState)
					{
						sb.Append(" ");
					}

					sb.Append(postalCode);
				}
			}

			return sb.ToString();
		}

		protected string ErrorNoDataForAccount() { return Translation.GetTerm("ErrorNoDataForAccount", "No data available for the requested account."); }
		#endregion
	}
}