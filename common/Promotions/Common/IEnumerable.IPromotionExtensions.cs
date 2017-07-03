using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common
{
    public static class IEnumerableOfIPromotionExtensions
    {

        /// <summary>
        /// Where clause filtering an IEnumerable of IPromotion to those containing a qualification of some type.
        /// </summary>
        /// <typeparam name="QualificationType">The type of the qualification.</typeparam>
        /// <param name="promotions">The promotions.</param>
        /// <returns></returns>
        public static IEnumerable<IPromotion> WithQualification<QualificationType>(this IEnumerable<IPromotion> promotions) where QualificationType : IPromotionQualificationExtension
        {
            return promotions.Where((promo) => promo.PromotionQualifications.Values.Any((qual) => qual is QualificationType));
        }

        /// <summary>
        /// Where clause filtering an IEnumerable of IPromotion to those containing a qualification of some type that matches a provided order context.
        /// </summary>
        /// <typeparam name="QualificationType">The type of the qualification.</typeparam>
        /// <param name="promotions">The promotions.</param>
        /// <param name="orderContext">The order context.</param>
        /// <returns></returns>
        public static IEnumerable<IPromotion> WithQualificationPassingForOrder<QualificationType>(this IEnumerable<IPromotion> promotions, IOrderContext orderContext) where QualificationType : IPromotionQualificationExtension
        {

            var extensionRegistry = Create.New<IDataObjectExtensionProviderRegistry>();

            return promotions.Where((promo) =>
                    {
                        return promo.PromotionQualifications.Values.Any((qual) =>
                            {
                                if (!(qual.GetType().IsAssignableFrom(typeof(QualificationType))))
                                    return false;
                                var handler = extensionRegistry.RetrieveExtensionProvider<IPromotionQualificationHandler>(qual.ExtensionProviderKey);
                                return handler.Matches(promo, qual, orderContext);
                            });
                    });

        }

    }
}
