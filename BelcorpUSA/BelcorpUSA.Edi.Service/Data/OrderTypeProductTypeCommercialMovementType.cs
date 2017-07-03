using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Service.Data
{
	public class OrderTypeProductTypeCommercialMovementType
	{
		public int OrderTypeProductTypeCommercialMovementTypeId { get; set; }
		public int OrderTypeId { get; set; }
		public int ProductTypeId { get; set; }
		public int? ParentOrderStatusId { get; set; }
		public int CommercialMovementTypeId { get; set; }
	}
}
