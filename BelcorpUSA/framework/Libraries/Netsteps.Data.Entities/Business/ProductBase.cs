using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ProductBase : IProductBase
	{
		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0,
			Categories = 1 << 0,
			Translations = 1 << 1,
			Products = 1 << 2,
			ProductsFull = 1 << 3,
			ProductBaseProperties = 1 << 4,
			ExcludedStateProvinces = 1 << 5,
			ProductTypes = 1 << 6,
			LoadFull = Categories | Translations | ProductsFull | ProductBaseProperties | ExcludedStateProvinces | ProductTypes,
			LoadForProducts = Categories | Translations | Products | ProductBaseProperties | ExcludedStateProvinces | ProductTypes
		};

		#region Methods
		public static PaginatedList<ProductBaseSearchData> Search(ProductBaseSearchParameters searchParams)
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

		public static void ChangeActiveStatus(int productBaseID, bool active)
		{
			try
			{
				Repository.ChangeActiveStatus(productBaseID, active);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void DeleteBatch(IEnumerable<int> productBaseIDs)
		{
			try
			{
				Repository.DeleteBatch(productBaseIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion
	}
}
