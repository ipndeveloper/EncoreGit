using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;

namespace DistributorBackOffice.Helpers
{
	public interface ICategoryHelper
	{
		IEnumerable<Category> GetDwsActiveCategories(Account account);
		StringBuilder ProductsTableForBulkAdd(int categoryID, short accountTypeID, Order order);
		IEnumerable<Product> GetValidProductsForCategory(int categoryID, int accountTypeID, int currencyID);
	}

	public class CategoryHelper : ICategoryHelper
	{
		public IEnumerable<Category> GetDwsActiveCategories(Account account)
		{
			var inventory = Create.New<InventoryBaseRepository>();

			var allActiveCategories = inventory.GetActiveCategories(ApplicationContext.Instance.StoreFrontID, account.AccountTypeID);

            //var allNormalCatalogs = inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
            //                                 .Where(c => c.CatalogTypeID == (short)Constants.CatalogType.Normal);

            var allNormalCatalogs = CatalogExtensions.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID, ApplicationContext.Instance.CurrentLanguageID)
                                             .Where(c => c.CatalogTypeID == (short)Constants.CatalogType.Normal);


			var filteredCategoriesForBulkAdd = new List<Category>();

			foreach (var catalog in allNormalCatalogs.DistinctBy(c => c.CategoryID))
			{
				Category root = inventory.GetCategoryTree(catalog.CategoryID);

				FilterActiveCategories(root, allActiveCategories, filteredCategoriesForBulkAdd);
			}

			return filteredCategoriesForBulkAdd.DistinctBy(c => c.CategoryID);
		}

		public StringBuilder ProductsTableForBulkAdd(int categoryID, short accountTypeID, Order order)
		{
			StringBuilder builder = new StringBuilder();
			int count = 0;

			var products = GetValidProductsForCategory(categoryID, accountTypeID, order.CurrencyID);

			foreach (Product product in products)
			{
				builder.Append("<tr").Append(count % 2 == 0 ? "" : " class=\"Alt\"").Append(">")
						.AppendCell(product.SKU, style: "width: 80px;")
						.AppendCell(product.Name, style: "width: 120px;")
						.AppendCell(product.Prices.First(pp => pp.ProductPriceTypeID == AccountPriceType.GetPriceType(accountTypeID, Constants.PriceRelationshipType.Products, order.OrderTypeID).ProductPriceTypeID && pp.CurrencyID == order.CurrencyID).Price.ToString(order.CurrencyID), style: "width: 50px;")
						.AppendCell("<input type=\"hidden\" value=\"" + product.ProductID + "\" class=\"productId\"/><input type=\"text\" value=\"0\" style=\"width: 20px;\" class=\"quantity\"/>", style: "width: 50px;")
						.Append("</tr>");
				++count;
			}

			return builder;
		}

		/// <summary>
		/// Ensure that the category is active and that it is a leaf level category.
		/// We don't want to show any parent categories
		/// </summary>
		protected void FilterActiveCategories(Category parent, List<Category> activeCategories, List<Category> filteredCategoryBucket)
		{
			if (parent.ChildCategories == null || parent.ChildCategories.Count == 0)
				return;

			foreach (var category in parent.ChildCategories.OrderBy(c => c.SortIndex))
			{
				if (IsCategoryInActiveList(category, activeCategories) && IsLeafCategory(category))
					filteredCategoryBucket.Add(category);

				FilterActiveCategories(category, activeCategories, filteredCategoryBucket);
			}
		}

		protected bool IsCategoryInActiveList(Category currentCategory, IEnumerable<Category> activeCategories)
		{
			return activeCategories.Any(c => c.CategoryID == currentCategory.CategoryID);
		}

		protected bool IsLeafCategory(Category category)
		{
			return category.ChildCategories.Count == 0;
		}

		public IEnumerable<Product> GetValidProductsForCategory(int categoryID, int accountTypeID, int currencyID)
		{
			var inventory = Create.New<InventoryBaseRepository>();

			IEnumerable<Product> allActiveProducts =
										categoryID > 0
										? inventory.GetActiveProductsForCategory(ApplicationContext.Instance.StoreFrontID, categoryID, (short)accountTypeID)
										: inventory.GetAllActiveProducts(ApplicationContext.Instance.StoreFrontID);

			return allActiveProducts.Where(p => !p.IsVariantTemplate && p.ContainsPrice((short)accountTypeID, currencyID));
		}

		protected IEnumerable<Product> ExcludeInvalidProducts(InventoryBaseRepository inventory, IEnumerable<Product> allActiveProducts, short accountTypeID, int currencyID)
		{
			return inventory.ExcludeInvalidProducts(allActiveProducts,
													accountTypeID,
													currencyID
				).Where(p => p.ProductBase.Products.Count == 1 || p.IsVariantTemplate).ToList();
		}

	}
}