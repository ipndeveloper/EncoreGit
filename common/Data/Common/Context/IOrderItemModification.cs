using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Context
{
	[DTO]
	public interface IOrderItemModification
	{
		int ProductID { get; set; }
		IOrderCustomer Customer { get; set; }
		IOrderItem ExistingItem { get; set; }
	}
}
