using System.Collections.Generic;
using Fasterflect;
using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Accounts.Downline.UI.Service;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Extensions
{
	/// <summary>
	/// This is an example of an <see cref="IDownlineInfoCardExtension"/> that uses values from the soon-to-be-obsolete downline cache.
	/// </summary>
	public class SampleDownlineCacheInfoCardExtension : IDownlineInfoCardExtension
	{
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>();
		/// <summary>
		/// List item keys provided by this extension.
		/// </summary>
		public static class ListItemKeys
		{
			public static string CurrentTitle { get { return "CurrentTitle"; } }
			public static string PaidAsTitle { get { return "PaidAsTitle"; } }
			public static string PV { get { return "PV"; } }
			public static string GV { get { return "GV"; } }
			public static string IsCommissionQualified { get { return "IsCommissionQualified"; } }
		}

		/// <summary>
		/// Gets values from the downline cache and adds them to the CustomData bag.
		/// </summary>
		public virtual void LoadData(IDownlineInfoCardContext context)
		{
			var downline = DownlineCache.GetDownline(0);
			if (downline != null)
			{
				dynamic downlineNode;
				if (downline.Lookup.TryGetValue(context.AccountInfo.AccountId, out downlineNode))
				{
					MemberGetter getter;
					if (downline.DynamicTypeGetters.TryGetValue("CurrentTitle", out getter))
					{
						context.CustomData.CurrentTitleId = getter(downlineNode);
					}
					if (downline.DynamicTypeGetters.TryGetValue("PaidAsTitle", out getter))
					{
						context.CustomData.PaidAsTitleId = getter(downlineNode);
					}
					if (downline.DynamicTypeGetters.TryGetValue("PV", out getter))
					{
						context.CustomData.PV = getter(downlineNode);
					}
					if (downline.DynamicTypeGetters.TryGetValue("GV", out getter))
					{
						context.CustomData.GV = getter(downlineNode);
					}
					if (downline.DynamicTypeGetters.TryGetValue("IsCommissionQualified", out getter))
					{
						context.CustomData.IsCommissionQualified = getter(downlineNode);
					}
				}
			}
		}

		/// <summary>
		/// Registers providers for the custom items.
		/// </summary>
		public virtual void InitializeItemProviders(IDictionary<string, IDownlineInfoCardItemProvider> providers)
		{
			providers[ListItemKeys.CurrentTitle] = new DelegateDownlineInfoCardItemProvider(context =>
			{
				int? currentTitleId = context.CustomData.CurrentTitleId;
				string value = currentTitleId.HasValue
					? _commissionsService.GetTitle(currentTitleId.Value).TermName
					: null;
				return DownlineUIService.CreateDownlineInfoCardTwoColumnListItem("Current Title", value);
			});

			providers[ListItemKeys.PaidAsTitle] = new DelegateDownlineInfoCardItemProvider(context =>
			{
				int? paidAsTitleId = context.CustomData.PaidAsTitleId;
				string value = paidAsTitleId.HasValue
                    ? _commissionsService.GetTitle(paidAsTitleId.Value).TermName
					: null;
				return DownlineUIService.CreateDownlineInfoCardTwoColumnListItem("Paid As Title", value);
			});

			providers[ListItemKeys.PV] = new DelegateDownlineInfoCardItemProvider(context =>
			{
				decimal? pv = context.CustomData.PV;
				string value = pv.HasValue
					? pv.Value.ToString("N2")
					: null;
				return DownlineUIService.CreateDownlineInfoCardTwoColumnListItem("PV", value);
			});

			providers[ListItemKeys.GV] = new DelegateDownlineInfoCardItemProvider(context =>
			{
				decimal? gv = context.CustomData.GV;
				string value = gv.HasValue
					? gv.Value.ToString("N2")
					:null;
				return DownlineUIService.CreateDownlineInfoCardTwoColumnListItem("GV", value);
			});

			providers[ListItemKeys.IsCommissionQualified] = new DelegateDownlineInfoCardItemProvider(context =>
			{
				bool? isCommissionQualified = context.CustomData.IsCommissionQualified;
				string value = isCommissionQualified.HasValue
					? isCommissionQualified.Value
						? Translation.GetTerm("Yes")
						: Translation.GetTerm("No")
					: null;
				return DownlineUIService.CreateDownlineInfoCardTwoColumnListItem("Commission Qualified", value);
			});
		}
	}
}