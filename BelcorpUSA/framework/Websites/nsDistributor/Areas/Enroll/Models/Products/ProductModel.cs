using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using nsDistributor.Areas.Enroll.Models.Interfaces;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class ProductModel : IProductModel
    {
        public int ProductID { get; set; }
		public decimal? RetailPrice { get; set; }
		public decimal? CVPrice { get; set; }
		public decimal? QVPrice { get; set; }
		public string Name { get; set; }
		public string SKU { get; set; }
		public bool Active { get; set; }
        public List<IPriceTypeInfoModel> AdditionalPriceTypeValues { get; set; }

        public ProductModel()
		{
			AdditionalPriceTypeValues = new List<IPriceTypeInfoModel>();
		}

        public ProductModel LoadResources(int productID, IEnumerable<int> additionalAffectedPriceTypeIDs = null, int? currencyID = null)
		{
			return LoadResources(Product.LoadFull(productID), additionalAffectedPriceTypeIDs, currencyID);
		}

		public ProductModel LoadResources(Product product, IEnumerable<int> additionalAffectedPriceTypeIDs = null, int? currencyID = null)
		{
			ProductID = product.ProductID;
			Name = product.Name;
			SKU = product.SKU;
			Active = product.Active;

			if (currencyID.HasValue)
			{
				RetailPrice = product.Prices.GetPriceByPriceType(Constants.ProductPriceType.Retail, currencyID.Value);
				QVPrice = product.Prices.GetPriceByPriceType(Constants.ProductPriceType.QV, currencyID.Value);
				CVPrice = product.Prices.GetPriceByPriceType(Constants.ProductPriceType.CV, currencyID.Value);

                if (RetailPrice == null)
                    RetailPrice = 0;
                if (QVPrice == null)
                    QVPrice = 0;
                if (CVPrice == null)
                    CVPrice = 0;

				if (additionalAffectedPriceTypeIDs != null)
				{
					foreach (var priceType in additionalAffectedPriceTypeIDs)
					{
						var value = new PromotionPriceTypeInfoModel(priceType, product.Prices.GetPriceByPriceType(priceType, currencyID.Value));
						AdditionalPriceTypeValues.Add(value);
					}
				}
			}
			return this;
		}
	}

	class PromotionPriceTypeInfoModel : IPriceTypeInfoModel
	{
		public string PriceTypeName { get; set; }
		public decimal? Value { get; set; }
		public PromotionPriceTypeInfoModel(int priceTypeId, decimal? value)
		{
			PriceTypeName = SmallCollectionCache.Instance.ProductPriceTypes.GetById(priceTypeId).GetTerm();
			Value = value;
		}
	}
}
