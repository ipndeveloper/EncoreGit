﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface ICustomerKitResult
	{
		List<IKit> Kits { get; set; }
		List<IBundleItem> KitlessOrderItems { get; set; }
		int AccountID { get; set; }
	}
}
