using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NetSteps.Data.Entities.Services
{
	public interface IProductPricingService
	{
		bool ProductContainsPrice(Product product, short accountTypeID, int currencyID, int? orderTypeID = null);
		bool ProductIsDynamicPricedKit(Product product);
		bool ProductContainsPrice(Product product, short accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null);
		decimal GetPrice(Product product, int productPriceTypeID, int currencyID);
		decimal GetPrice(Product product, int accountTypeID, int currencyID, int? orderTypeID = null);
		decimal GetPrice(Product product, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null);
		decimal GetPrice(int productID, int productPriceTypeID, int currencyID);
        /*CS.24MAY2016.Inicio*/
        Dictionary<int, decimal> GetPriceByPriceTypes(int currencyID, int productID);
        /*CS.24MAY2016.Fin*/
	}

	[ContainerRegister(typeof(IProductPricingService), RegistrationBehaviors.Default, ScopeBehavior=ScopeBehavior.Singleton)]
	public class ProductPricingService : IProductPricingService
	{
		private IProductPriceRepository priceRepository;

		public bool ProductContainsPrice(Product product, short accountTypeID, int currencyID, int? orderTypeID = null)
		{
			return product.ContainsPrice(accountTypeID, currencyID, orderTypeID);
		}

		public bool ProductIsDynamicPricedKit(Product product)
		{
			return product.IsDynamicallyPricedKit();
		}

		public bool ProductContainsPrice(Product product, short accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null)
		{
			return product.ContainsPrice(accountTypeID, priceRelationshipType, currencyID, orderTypeID);
		}

		public decimal GetPrice(Product product, int accountTypeID, int currencyID, int? orderTypeID = null)
		{
			return product.GetPrice(accountTypeID, currencyID, orderTypeID);
		}

		public decimal GetPrice(Product product, int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null)
		{
			return product.GetPrice(accountTypeID, priceRelationshipType, currencyID, orderTypeID);
		}

		public decimal GetPrice(Product product, int productPriceTypeID, int currencyID)
		{
			return product.GetPriceByPriceType(productPriceTypeID, currencyID);
		}

		public decimal GetPrice(int productID, int productPriceTypeID, int currencyID)
		{
			if (priceRepository == null) priceRepository = Create.New<IProductPriceRepository>();

			var productPrice =
				priceRepository.FirstOrDefault(
					pp => pp.ProductID == productID && pp.ProductPriceTypeID == productPriceTypeID && pp.CurrencyID == currencyID);
			return productPrice != null ? productPrice.Price : 0m;
		}

        /*CS.24MAY2016.Inicio*/
        public Dictionary<int, decimal> GetPriceByPriceTypes(int currencyID, int productID)
        {
            Dictionary<int, decimal> result = new Dictionary<int, decimal>();
            result = (Dictionary<int, decimal>)ProductPricesExtensions.GetPriceByPriceTypes(currencyID, productID);
            return result;
        }
        /*CS.24MAY2016.Fin*/

    }
}
