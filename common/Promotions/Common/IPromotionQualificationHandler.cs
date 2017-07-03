using NetSteps.Data.Common.Context;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Common
{
	public interface IPromotionQualificationHandler : IDataObjectExtensionProvider
	{

		bool RequiresCommitNotification { get; }
		bool RequiresRemoveNotification { get; }

		void Commit(IPromotion promotion, IPromotionQualificationExtension qualification, IOrderContext orderContext);
		void Remove(IPromotion promotion, IPromotionQualificationExtension qualification, IOrderContext orderContext);

		bool AreEqual(IPromotionQualificationExtension qualification1, IPromotionQualificationExtension qualification2);

		bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value);

		PromotionQualificationResult Matches(IPromotion promo, IPromotionQualificationExtension qual, IOrderContext orderContext);

		void CheckValidity(string qualificationKey, IPromotionQualificationExtension qualification, IPromotionState state);
	}
}
