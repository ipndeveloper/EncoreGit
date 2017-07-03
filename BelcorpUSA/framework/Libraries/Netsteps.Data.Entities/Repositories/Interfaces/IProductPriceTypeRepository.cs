namespace NetSteps.Data.Entities.Repositories
{
    using NetSteps.Data.Entities.Dto;
    using System.Collections.Generic;

	public partial interface IProductPriceTypeRepository
	{
		ProductPriceType LoadPriceType(int accountTypeID, Constants.PriceRelationshipType relationshipType, int storeFrontID);
        bool GetMandatory(int id);
        List<ProductPriceTypeDto> ListProductPriceTypes();
	}
}
