using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Data.Common.Context
{
	[DTO]
	public interface ICouponCode
	{
		int AccountID { get; set; }
		string CouponCode { get; set; }
	}
}
