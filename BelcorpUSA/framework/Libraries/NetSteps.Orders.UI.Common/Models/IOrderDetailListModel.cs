using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace NetSteps.Orders.UI.Common.Models
{
	[ContractClass(typeof(Contracts.OrderDetailListModelContract))]
	public interface IOrderDetailListModel
	{
		#region Properties

		ReadOnlyCollection<IOrderDetailModel> Orders { get; }

		decimal SubTotal { get; }

		#endregion

		#region Methods

		void Init(IList<IOrderDetailModel> fromOrderDetails);

		#endregion
	}

	namespace Contracts
	{
        [ContractClassFor(typeof(NetSteps.Orders.UI.Common.Models.IOrderDetailListModel))]
        internal abstract class OrderDetailListModelContract : NetSteps.Orders.UI.Common.Models.IOrderDetailListModel
		{
			public ReadOnlyCollection<IOrderDetailModel> Orders
			{
				get { throw new NotImplementedException(); }
			}

			public decimal SubTotal
			{
				get { throw new NotImplementedException(); }
			}

			public void Init(IList<IOrderDetailModel> fromOrderDetails)
			{
				Contract.Requires<ArgumentNullException>(fromOrderDetails != null);

				throw new NotImplementedException();
			}
		}
	}
}
