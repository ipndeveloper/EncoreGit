using System.Collections.Generic;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using System.Diagnostics.Contracts;
using System;
using NetSteps.Promotions.Plugins.Common.Rewards;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Promotions.Plugins.Common.PromotionKinds.Base
{
	[ContractClass(typeof(ProductPromotionContract))]
	public interface IProductPromotion : IPromotion
	{
		/// <summary>
		/// Gets the promoted product IDs.
		/// </summary>
		/// <value>The promoted product IDs.</value>
		IEnumerable<int> PromotedProductIDs { get; }

		/// <summary>
		/// Gets the promoted price types for product ID.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <returns></returns>
		IEnumerable<IPriceType> GetPromotedPriceTypesForProductID(int productID);

		/// <summary>
		/// Gets the promotion adjustment amount.
		/// </summary>
		/// <param name="productID">The product ID.</param>
		/// <param name="marketID">The market ID.</param>
		/// <param name="originalPrice">The original price.</param>
		/// <param name="productPriceType">Type of the product price.</param>
		/// <returns></returns>
		decimal GetPromotionAdjustmentAmount(int productID, int marketID, decimal originalPrice, IPriceType productPriceType);
		
	}

	[ContractClassFor(typeof(IProductPromotion))]
	public abstract class ProductPromotionContract : IProductPromotion
	{

		public IEnumerable<int> PromotedProductIDs
		{
			get
			{
				Contract.Ensures(Contract.Result<IEnumerable<int>>() != null);
				throw new NotImplementedException();
			}
		}

		public IEnumerable<IPriceType> GetPromotedPriceTypesForProductID(int productID)
		{
			Contract.Requires<ArgumentOutOfRangeException>(productID > 0);
            Contract.Ensures(Contract.Result<IEnumerable<IPriceType>>() != null);
			throw new NotImplementedException();
		}

		public decimal GetPromotionAdjustmentAmount(int productID, int marketID, decimal originalPrice, IPriceType productPriceType)
		{
			Contract.Requires<ArgumentOutOfRangeException>(productID > 0);
			Contract.Requires<ArgumentOutOfRangeException>(marketID > 0);
			Contract.Requires<ArgumentOutOfRangeException>(originalPrice >= 0);
			Contract.Requires<ArgumentNullException>(productPriceType != null);
			throw new NotImplementedException();
		}

		public abstract IEnumerable<string> AssociatedPropertyNames { get; }

		public abstract string Description { get; set; }

		public abstract DateTime? EndDate { get; set; }

		public abstract int PromotionID { get; set; }

		public abstract string PromotionKind { get; }

		public abstract IDictionary<string, IPromotionQualificationExtension> PromotionQualifications { get; set; }

		public abstract IDictionary<string, IPromotionReward> PromotionRewards { get; set; }

		public abstract int PromotionStatusTypeID { get; set; }

		public abstract DateTime? StartDate { get; set; }

		public abstract bool ValidFor<TProperty>(string propertyName, TProperty value);

		public abstract bool ValidateConstruction();
	}
}
