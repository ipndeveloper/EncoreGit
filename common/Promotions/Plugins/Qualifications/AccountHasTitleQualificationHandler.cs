using System;
using System.Linq;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.Plugins.Qualifications
{
	public class AccountHasTitleQualificationHandler : BasePromotionQualificationHandler<IAccountHasTitleQualificationExtension, IAccountHasTitleQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IAccountHasTitleQualificationHandler
	{
        private ITitleService _titleService;

        public AccountHasTitleQualificationHandler(ITitleService titleService)
        {
            _titleService = titleService;
        }

		public override string GetProviderKey()
		{
			return QualificationExtensionProviderKeys.AccountHasTitleProviderKey;
		}

		public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
		{
			Contract.Assert(promotionQualification1 != null);
			Contract.Assert(promotionQualification2 != null);

			var extension1 = promotionQualification1 as IAccountHasTitleQualificationExtension;
			var extension2 = promotionQualification2 as IAccountHasTitleQualificationExtension;

			if (extension1.AllowedTitles.Except(extension2.AllowedTitles, new ExtensionComparer()).Count() > 0)
				return false;
			if (extension2.AllowedTitles.Except(extension1.AllowedTitles, new ExtensionComparer()).Count() > 0)
				return false;
			return true;
		}

		public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
		{
			Contract.Assert(promotionQualification != null);
			Contract.Assert(orderContext != null);

			var qualification = (IAccountHasTitleQualificationExtension)promotionQualification;
			var customerAccountIDs = orderContext.Order.OrderCustomers.Select(x => x.AccountID);
			var allowedTitleIDs = qualification.AllowedTitles.Select(title => title.TitleID).ToList();
			var qualifiedAccounts = customerAccountIDs
											.Where(accountID =>
											{
												return _titleService.GetAccountTitles(accountID, null)
													.Select(title => title.Title.TitleID)
													.Intersect(allowedTitleIDs).Count() > 0;
											});
			if (qualifiedAccounts.Count() > 0)
				return PromotionQualificationResult.MatchForSelectCustomers(qualifiedAccounts);
			return PromotionQualificationResult.NoMatch;
		}

		public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
		{
			Contract.Assert(qualification != null);
			Contract.Assert(!string.IsNullOrEmpty(propertyName));
			Contract.Assert(value != null);
			Contract.Assert(typeof(IAccountHasTitleQualificationExtension).IsAssignableFrom(qualification.GetType()));
			var extension = qualification as IAccountHasTitleQualificationExtension;
			var allowedTitleIDs = extension.AllowedTitles.Select(title => title.TitleID).ToList();
							
			switch (propertyName)
			{
				case AccountHasTitleQualification.PropertyNames.AccountID:
					return _titleService.GetAccountTitles(Convert.ToInt32(value), null)
													.Select(title => title.Title.TitleID)
													.Intersect(allowedTitleIDs).Count() > 0;
				case AccountHasTitleQualification.PropertyNames.Title:
					try
					{
						var titleOptionSelected = (IAccountTitleOption)value;
						return (extension.AllowedTitles.Any(titleOption => titleOption.TitleID == titleOptionSelected.TitleID && titleOption.TitleTypeID == titleOptionSelected.TitleTypeID));
					}
					catch
					{
						return false;
					}
				case AccountHasTitleQualification.PropertyNames.TitleID:
					return allowedTitleIDs.Contains(Convert.ToInt32(value));
			}
			return true;
		}

		private class ExtensionComparer : IEqualityComparer<IAccountTitleOption>
		{

			public bool Equals(IAccountTitleOption x, IAccountTitleOption y)
			{
				return (x.TitleID == y.TitleID && x.TitleTypeID == y.TitleTypeID);
			}

			public int GetHashCode(IAccountTitleOption obj)
			{
				int hash = 31;
				hash = obj.TitleID + (hash * 37);
				hash = obj.TitleTypeID + (hash * 37);

				return hash;
			}
		}

		public override void CheckValidity(string qualificationKey, IAccountHasTitleQualificationExtension qualification, IPromotionState state)
		{
			var availableTitleIDs = _titleService.GetTitles().Select(title => title.TitleID);
			foreach (var allowedTitle in qualification.AllowedTitles)
			{
				if (!availableTitleIDs.Any(x => x == allowedTitle.TitleID))
				{
					state.AddConstructionError
						(
							String.Format("Promotion Qualification {0}", qualificationKey),
							String.Format("Invalid Title ID '{0}' specified.", allowedTitle.TitleID)
						);
				}
			}
		}
	}
}