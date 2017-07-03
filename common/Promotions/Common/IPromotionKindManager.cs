using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common
{
	public interface IPromotionKindManager
	{
		/// <summary>
		/// Registers the kind of the promotion.
		/// </summary>
		/// <typeparam name="TPromotion">The type of the promotion.</typeparam>
		/// <param name="promotionKind">Kind of the promotion.</param>
		void RegisterPromotionKind<TPromotion>(string promotionKind) where TPromotion : IPromotion;

		/// <summary>
		/// Unregisters the adjustment provider.
		/// </summary>
		/// <param name="promotionKind">Kind of the promotion.</param>
		/// <returns></returns>
		bool UnregisterAdjustmentProvider(string promotionKind);

		/// <summary>
		/// Creates the promotion.
		/// </summary>
		/// <param name="promotionKind">Kind of the promotion.</param>
		/// <returns></returns>
		IPromotion CreatePromotion(string promotionKind);
		
		/// <summary>
		/// Creates the promotion.
		/// </summary>
		/// <typeparam name="TPromotion">The type of the promotion.</typeparam>
		/// <param name="promotionKind">Kind of the promotion.</param>
		/// <returns></returns>
		TPromotion CreatePromotion<TPromotion>(string promotionKind) where TPromotion : IPromotion;

		/// <summary>
		/// Gets the specific promotion kind string.
		/// </summary>
		/// <typeparam name="TPromotion">The type of the promotion.</typeparam>
		/// <returns></returns>
		string GetSpecificPromotionKindString<TPromotion>() where TPromotion : IPromotion;

		/// <summary>
		/// Gets the promotion kind strings.
		/// </summary>
		/// <typeparam name="TPromotion">The type of the promotion.</typeparam>
		/// <returns></returns>
		IEnumerable<string> GetPromotionKindStrings<TPromotion>() where TPromotion : IPromotion;
	}
}
