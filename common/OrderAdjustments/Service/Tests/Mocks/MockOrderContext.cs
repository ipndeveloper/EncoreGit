using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
	public class MockOrderContext : IOrderContext
	{
		public MockOrderContext(int accountID)
		{
			Order = new MockOrder(accountID);
			_couponCodes = new List<MockCouponCode>();
			_injectedOrderSteps = new List<IOrderStep>();
		}
		public IOrder Order { get; set; }

		public List<MockCouponCode> _couponCodes;
		public IList<ICouponCode> CouponCodes
		{
			get
			{
				return _couponCodes.Cast<ICouponCode>().ToList();
			}
		}

		private List<IOrderStep> _injectedOrderSteps;
		public IList<IOrderStep> InjectedOrderSteps
		{
			get
			{
				if (_injectedOrderSteps == null)
				{
					_injectedOrderSteps = new List<IOrderStep>();
				}
				return _injectedOrderSteps;
			}
			set
			{
				_injectedOrderSteps = (List<IOrderStep>)value;
			}
		}

		public int[] ValidOrderStatusIdsForOrderAdjustment
		{
			get { return new int[] { 3, 5, 7 }; }
		}


		public decimal? Subtotal { get; set; }

		public IList<IOrderStepResponse> InjectedOrderStepResponses
		{
			get { throw new NotImplementedException(); }
			set
			{
				throw new NotImplementedException();
			}
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public IList<IProduct> SortedDynamicKitProducts
		{
			get { throw new NotImplementedException(); }
		}
	}
}
