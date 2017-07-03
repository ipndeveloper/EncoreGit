
using System;
namespace NetSteps.Promotions.Common.ModelConcrete
{
	[Serializable]
	public sealed class BasicPromotion : BasePromotion
	{
		public const string PromotionKindName = "Basic";

		public override string PromotionKind
		{
			get
			{
				return PromotionKindName;
			}
		}
	}
}
