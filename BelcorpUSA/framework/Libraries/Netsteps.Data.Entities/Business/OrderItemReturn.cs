using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities
{
	public partial class OrderItemReturn : IDateLastModified
	{
		#region Methods
		public static OrderItemReturn LoadByOrderItemID(int orderItemID)
		{
			try
			{
				return Repository.LoadByOrderItemID(orderItemID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static List<OrderItemReturn> LoadAllByOrderItemID(int orderItemID)
		{
			try
			{
				return Repository.LoadAllByOrderItemID(orderItemID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
        //public static List<OrderReturnTable> GetOrderItemReturnByParentOrderID(int orderItemID)
        //{
        //    try
        //    {
        //        return Repository.GetOrderItemReturnByParentOrderID(orderItemID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //    }
        //}
		#endregion
	}
}
