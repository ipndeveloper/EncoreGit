using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Common.Context
{
	public interface IOrderItemPropertyModification : IOrderItemModification
	{
		IDictionary<string, string> Properties { get; }
	}

	[ContainerRegister(typeof(IOrderItemPropertyModification), RegistrationBehaviors.Default)]
	public sealed class OrderItemPropertyModification : IOrderItemPropertyModification
	{
		public IDictionary<string, string> Properties { get; private set; }

		public int ProductID { get; set; }

		public Entities.IOrderCustomer Customer { get; set; }

		public Entities.IOrderItem ExistingItem { get; set; }

		public OrderItemPropertyModification()
		{
			Properties = new Dictionary<string, string>();
		}
	}
}
