using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;

namespace nsDistributor.Extensions
{
	public static class ProductExtensions
	{
		public static string GetPrice(this Product product)
		{
			var account = CoreContext.CurrentAccount;

			short accountTypeID = account != null ? account.AccountTypeID : (short)Constants.AccountType.RetailCustomer;

			return product.GetPrice(accountTypeID, _defaultCurrencyID, null).ToString(_defaultCurrencyID);
		}

		public static string GetPrice(this Product product, ConstantsGenerated.ProductPriceType requestedPriceType)
		{
			return product.GetPrice(requestedPriceType, _defaultCurrencyID).ToString(_defaultCurrencyID);
		}

		/* 2011-11-09, JWL, Added this override method to pass in client-specific product price types that don't exist in 
		 * the main Encore ConstantsGenerated enum
		 */
		public static string GetPrice(this Product product, int requestedPriceType)
		{
			return product.GetPriceByPriceType(requestedPriceType, _defaultCurrencyID).ToString(_defaultCurrencyID);
		}

		public static string GetPrice(this Product product, int requestedPriceType, Currency currency)
		{
			return product.GetPriceByPriceType(requestedPriceType, _defaultCurrencyID).ToString(currency.CurrencyID);
		}

		public static string GetAdjustedPrice(this Product product)
		{
			var account = CoreContext.CurrentAccount;

			short accountTypeID = account != null ? account.AccountTypeID : (short)Constants.AccountType.RetailCustomer;

			return product.GetAdjustedPrice(accountTypeID, _defaultCurrencyID, null).ToString(_defaultCurrencyID);
		}

		public static bool ContainsPrice(this Product product)
		{
			var account = CoreContext.CurrentAccount;

			short accountTypeID = account != null ? account.AccountTypeID : (short)Constants.AccountType.RetailCustomer;

			return product.ContainsPrice(accountTypeID, Constants.PriceRelationshipType.Products, _defaultCurrencyID, null);
		}

		private static int _defaultCurrencyID
		{
			get
			{
				return CoreContext.CurrentOrder == null ? SmallCollectionCache.Instance.Markets.GetById(CoreContext.CurrentMarketId).GetDefaultCurrencyID() : CoreContext.CurrentOrder.CurrencyID;
			}
		}

		public static bool IsPricePromotionallyDiscounted(this Product product)
		{
			var currentAccount = CoreContext.CurrentAccount;
			var currentOrder = CoreContext.CurrentOrder;
			var accountTypeId = currentAccount != null ? currentAccount.AccountTypeID : (int)Constants.AccountType.RetailCustomer;
			var priceType = AccountPriceType.GetPriceType(accountTypeId, Constants.PriceRelationshipType.Products, currentOrder.OrderTypeID);
			var adjustedPrice = product.GetCurrentPromotionalPrice(CoreContext.CurrentOrderContext, (ConstantsGenerated.ProductPriceType)priceType.ProductPriceTypeID, currentOrder.CurrencyID, CoreContext.CurrentMarketId);
			var originalPrice = product.GetPrice(accountTypeId, currentOrder.CurrencyID);
			return adjustedPrice < originalPrice;
		}
	}
}
