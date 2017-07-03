using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
	public partial class AutoshipSchedule
	{
		/// <summary>
		/// Determine whether this is a subscription that has all virtual skus or not - DES
		/// </summary>
		public bool IsVirtualSubscription
		{
			get
			{
				var inventory = Create.New<InventoryBaseRepository>();
				return AutoshipScheduleTypeID != (int)Constants.AutoshipScheduleType.Normal 
					&& AutoshipScheduleProducts.Select(p => inventory.GetProduct(p.ProductID)).All(p => !p.ProductBase.IsShippable);
			}
		}

		/// <summary>
		/// Determine whether this is a subscription type or not - DES
		/// </summary>
		public bool IsSubscription
		{
			get
			{
				return AutoshipScheduleTypeID != (int)Constants.AutoshipScheduleType.Normal;
			}
		}

		public static List<AutoshipSchedule> LoadByAccountTypeID(int accountTypeID)
		{
			try
			{
				return Repository.LoadByAccountTypeID(accountTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<AutoshipSchedule> LoadFullByAccountTypeID(int accountTypeID)
		{
			try
			{
				return Repository.LoadFullByAccountTypeID(accountTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static List<AutoshipSchedule> GetByActive(bool getIsActive)
        {
            try
            {
                // Returning list to prevent delayed queries
                return (from x in SmallCollectionCache.Instance.AutoshipSchedules
                        where x.Active == getIsActive
                        select x).ToList();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
	}
}
