using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ProductPropertyType
	{
		public static PaginatedList<ProductPropertyType> Search(FilterPaginatedListParameters<ProductPropertyType> searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
