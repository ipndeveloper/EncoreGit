using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.Orders.UI.Common.Models;

namespace NetSteps.Orders.UI.Models
{
	[ContainerRegister(typeof(IOrderDetailItemModel), RegistrationBehaviors.Default)]
	public class OrderDetailItemModel : IOrderDetailItemModel
	{
		#region Properties

		public IList<IOrderDetailItemModel> ChildItems { get; private set; }

		public IList<IOrderDetailItemMessageModel> Messages { get; private set; }

		public string SKU { get; set; }

		public string ProductImageUrl { get; set; }

		public string ProductName { get; set; }

		public decimal Price { get; set; }

		public decimal AdjustedPrice { get; set; }

		public int Quantity { get; set; }

		public decimal CV { get; set; }

		public decimal Total { get; set; }

		public bool IsDynamicKit { get; set; }

		public bool IsExclusiveProduct { get; set; }

		public bool IsBundleFull { get; set; }

		public Guid Guid { get; set; }

		public string ExclusiveProductsIndicatorText { get; set; }

		#endregion

		#region Constructor

		public OrderDetailItemModel()
		{
			ChildItems = new List<IOrderDetailItemModel>();
			Messages = new List<IOrderDetailItemMessageModel>();
		}

		#endregion
	}
}
