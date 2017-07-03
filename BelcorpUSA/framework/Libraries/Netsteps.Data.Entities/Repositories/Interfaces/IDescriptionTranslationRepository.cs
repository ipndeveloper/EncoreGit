using NetSteps.Common.Base;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{

	public partial interface IDescriptionTranslationRepository
	{
		IEnumerable<DescriptionTranslation> LoadByProductID(int productId);
	}
}
