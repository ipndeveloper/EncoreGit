using System;
using System.Linq;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AccountPriceTypeBusinessLogic
	{
		public virtual ProductPriceType GetPriceType(int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID)
		{
			return AccountPriceType.GetPriceType(SmallCollectionCache.Instance.AccountPriceTypes, accountTypeID, priceRelationshipType, ApplicationContext.Instance.StoreFrontID, orderTypeID);
		}

		public virtual ProductPriceType GetPriceType(SmallCollectionCache.AccountPriceTypeCache list, int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int storeFrontID, int? orderTypeID)
		{
			if (orderTypeID.HasValue && priceRelationshipType == Generated.ConstantsGenerated.PriceRelationshipType.Products)
			{
				var orderType = SmallCollectionCache.Instance.OrderTypes.First(ot => ot.OrderTypeID == orderTypeID.Value);
				if (orderType.Name.Contains("Fundraiser"))
					return SmallCollectionCache.Instance.ProductPriceTypes.FirstOrDefault(ppt => ppt.Name.Equals("Fundraiser", StringComparison.InvariantCultureIgnoreCase));
			}
			
			AccountPriceType accountPriceType = list.FirstOrDefault(apt => apt.AccountTypeID == accountTypeID && apt.PriceRelationshipTypeID == (int)priceRelationshipType && apt.StoreFrontID == storeFrontID);
			return accountPriceType == default(AccountPriceType) ? null : SmallCollectionCache.Instance.ProductPriceTypes.GetById(accountPriceType.ProductPriceTypeID);
		}
	}
}
