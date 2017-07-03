using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Data.Entities.Business
{
	[ContractClass(typeof(Contracts.OrderPromotionQualificationContracts))]
    public interface IOrderPromotionQualification
    {
        BasicResponse ApplyPromotion(IOrderContext context, IOrderCustomer customer, string promotionCode);
        BasicResponse CanAddPromotionCode(IOrderContext context, IOrderCustomer customer, string promotionCode);
        IEnumerable<Func<IOrderContext, string, BasicResponse>> Qualifiers { get; }
        BasicResponse RemovePromotion(IOrderContext context, IOrderCustomer customer, string promotionCode);
        IEnumerable<IPromotion> SearchPromotions(IOrderContext context, string promotionCode, int? accountID = null, Predicate<IPromotion> filter = null);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderPromotionQualification))]
		abstract class OrderPromotionQualificationContracts : IOrderPromotionQualification
		{
			public BasicResponse ApplyPromotion(IOrderContext context, IOrderCustomer customer, string promotionCode)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(context.Order != null);
				Contract.Requires<ArgumentNullException>(promotionCode != null);
				Contract.Requires<ArgumentException>(promotionCode.Length > 0);
				Contract.Ensures(Contract.Result<BasicResponse>() != null);

				throw new NotImplementedException();
			}

			public BasicResponse CanAddPromotionCode(IOrderContext context, IOrderCustomer customer, string promotionCode)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(context.Order != null);
				Contract.Requires<ArgumentNullException>(promotionCode != null);
				Contract.Requires<ArgumentException>(promotionCode.Length > 0);
				Contract.Ensures(Contract.Result<BasicResponse>() != null);

				throw new NotImplementedException();
			}

			public IEnumerable<Func<IOrderContext, string, BasicResponse>> Qualifiers
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public BasicResponse RemovePromotion(IOrderContext context, IOrderCustomer customer, string promotionCode)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(context.Order != null);
				Contract.Requires<ArgumentNullException>(promotionCode != null);
				Contract.Requires<ArgumentException>(promotionCode.Length > 0);
				Contract.Ensures(Contract.Result<BasicResponse>() != null);

				throw new NotImplementedException();
			}

			public IEnumerable<IPromotion> SearchPromotions(IOrderContext context, string promotionCode, int? accountID = null, Predicate<IPromotion> filter = null)
			{
				throw new NotImplementedException();
			}
		}
	}
}