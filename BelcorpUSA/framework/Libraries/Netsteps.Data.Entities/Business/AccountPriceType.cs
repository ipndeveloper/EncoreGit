using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AccountPriceType
    {
        public static List<AccountPriceType> LoadAllByStoreFront(int storeFrontID)
        {
            try
            {
                return Repository.LoadAllByStoreFront(storeFrontID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static ProductPriceType GetPriceType(int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int? orderTypeID)
        {
            return BusinessLogic.GetPriceType(SmallCollectionCache.Instance.AccountPriceTypes, accountTypeID, priceRelationshipType, ApplicationContext.Instance.StoreFrontID, orderTypeID);
        }

        public static ProductPriceType GetPriceType(SmallCollectionCache.AccountPriceTypeCache list, int accountTypeID, Generated.ConstantsGenerated.PriceRelationshipType priceRelationshipType, int storeFrontID, int? orderTypeID)
        {
            return BusinessLogic.GetPriceType(list, accountTypeID, priceRelationshipType, storeFrontID, orderTypeID);
        }
    }
}
