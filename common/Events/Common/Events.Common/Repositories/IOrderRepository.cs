using System;
using System.Collections.Generic;

namespace NetSteps.Events.Common.Repositories
{
	public interface IOrderRepository
	{
		IEnumerable<DateTime> GetNewestCompletedPartyDates(int accountID, int numberToTake);

		int? GetRepresentativeAccountIdForOrder(int orderID);
	}

	public static class IOrderRepositoryExtensions
	{
		public static IEnumerable<DateTime> GetNewestCompletedPartyDates(this IOrderRepository orderRepository, int accountID)
		{
			return orderRepository.GetNewestCompletedPartyDates(accountID, 0);
		}
	}

	namespace Contracts
	{
		using System.Diagnostics.Contracts;

		internal abstract class IOrderRepositoryContracts : IOrderRepository
		{
			public IEnumerable<DateTime> GetNewestCompletedPartyDates(int accountID, int numberToTake)
			{
				Contract.Requires(accountID > 0);
				throw new NotImplementedException();
			}

			public int? GetRepresentativeAccountIdForOrder(int orderID)
			{
				Contract.Requires(orderID > 0);
				throw new NotImplementedException();
			}
		}
	}
}
