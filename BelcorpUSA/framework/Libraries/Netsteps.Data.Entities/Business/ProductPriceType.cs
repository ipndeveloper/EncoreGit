using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class ProductPriceType 
    {
        public static ProductPriceType LoadPriceType(int accountTypeID, Constants.PriceRelationshipType relationshipType, int storeFrontID)
        {
            try
            {
                return Repository.LoadPriceType(accountTypeID, relationshipType, storeFrontID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
