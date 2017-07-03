using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Common.Tests.Mocks
{
    public class MockPromotionKind : BasePromotion
    {
        public const string PromotionKindName = "mocked";

        public override string PromotionKind
        {
            get { return PromotionKindName; }
        }
	}
}
