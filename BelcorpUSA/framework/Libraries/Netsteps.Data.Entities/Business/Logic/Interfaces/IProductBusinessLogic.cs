using NetSteps.Common.Base;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IProductBusinessLogic
	{
		PaginatedList<AuditLogRow> GetAuditLog(Repositories.IProductRepository repository, Product fullyLoadedProduct, AuditLogSearchParameters param);
		bool CanBeAddedToDynamicKitGroup(Product entity, int kitProductId, int dynamicKitGroupId);
		decimal GetPrice(Product entity, int accountTypeID, int? orderTypeID, int currencyID);
		decimal GetAdjustedPrice(Product entity, int accountTypeID, int? orderTypeID, int currencyID);
		decimal GetPrice(Product entity, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID, int currencyID);
		decimal GetAdjustedPrice(Product product, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID, int currencyID);
		string GetShopTerm(Product entity, bool isCurrentlyABundle);
		void AddChildProductRelation(Product entity, int relationTypeID, Product childProduct);
		Product LoadWithRelations(int productID, Repositories.IProductRepository repository);
		decimal GetCurrentPromotionalPrice(IOrderContext orderContext, Product product, Constants.ProductPriceType priceType, int currencyID, int marketID);
		decimal GetCurrentPromotionalPrice(IOrderContext orderContext, Product product, int priceTypeID, int currencyID, int marketID);
	}
}
