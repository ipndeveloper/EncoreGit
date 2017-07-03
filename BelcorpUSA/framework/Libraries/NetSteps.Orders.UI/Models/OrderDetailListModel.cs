using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Orders.UI.Common.Models;

namespace NetSteps.Orders.UI.Models
{
	[ContainerRegister(typeof(IOrderDetailListModel), RegistrationBehaviors.Default)]
	public class OrderDetailListModel : IOrderDetailListModel
	{
		#region Properties

		private bool IsInitialized { get; set; }

		public ReadOnlyCollection<IOrderDetailModel> Orders { get; private set; }

		public decimal SubTotal { get; private set; }

		#endregion

		#region Constructors

		public OrderDetailListModel() { }

		#endregion

		#region Methods

		public void Init(IList<IOrderDetailModel> fromOrderDetails)
		{
			if (!IsInitialized)
			{
				Orders = new ReadOnlyCollection<IOrderDetailModel>(fromOrderDetails);
				SubTotal = Orders.Sum(o => o.OrderSubtotal);
				IsInitialized = true;
			}
		}

		#endregion
	}
}
