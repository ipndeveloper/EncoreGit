using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
	public class MockCouponCode : ICouponCode
	{
		public int AccountID { get; set; }

		public string CouponCode { get; set; }
	}
}
