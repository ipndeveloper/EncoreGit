
using NetSteps.Data.Entities.Cache;
namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAccountPriceTypeBusinessLogic
    {
        ProductPriceType GetPriceType(int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID);
        ProductPriceType GetPriceType(SmallCollectionCache.AccountPriceTypeCache list, int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int storeFrontID, int? orderTypeID);
    }
}
