using System;
using System.Collections.Generic;

namespace NetSteps.Orders.UI.Common.Models
{
	public interface IOrderDetailItemModel
	{
		IList<IOrderDetailItemModel> ChildItems { get; }
		IList<IOrderDetailItemMessageModel> Messages { get; }
		
		string SKU { get; set; }
		string ProductImageUrl { get; set; }
		string ProductName { get; set; }
		decimal Price { get; set; }
		decimal AdjustedPrice { get; set; }
		int Quantity { get; set; }
		decimal CV { get; set; }
		decimal Total { get; set; }

		bool IsDynamicKit { get; set; }
		bool IsExclusiveProduct { get; set; }
		bool IsBundleFull { get; set; }

		Guid Guid { get; set; }

		#region Terms

		string ExclusiveProductsIndicatorText { get; set; }

		#endregion
	}
}
