using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Extensions;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Generated;

namespace nsDistributor.Areas.Enroll.Models.Products
{
	public class EnrollmentVariantKitModel : EnrollmentKitModel
	{
		public virtual bool HasVariants { get; private set; }
		public virtual IEnumerable<Product> Variants { get; private set; }
		public virtual int SelectedVariantProductID { get; set; }

		#region Infrastructure
		public virtual EnrollmentKitModel LoadResources(
			Product product,
			IEnrollmentContext enrollmentContext)
		{
			base.LoadResources(product, enrollmentContext);

			var enrollmentItems = GetVariants(enrollmentContext);
			//var variants = Product.GetVariants(product.ProductID);
			//this.Variants = variants.Join(enrollmentItems, v => v.ProductID, e => e.ProductID, (v, e) => v);
			this.Variants = Product.GetVariants(product.ProductID);
			this.HasVariants = this.Variants.CountSafe() > 1;

			return this;
		}

		IEnumerable<Product> GetVariants(IEnrollmentContext enrollmentContext)
		{
			int catalogType = (int)ConstantsGenerated.CatalogType.EnrollmentKits;

			var inventory = Create.New<InventoryBaseRepository>();
			var activeCatalogs = inventory.GetActiveCatalogs(enrollmentContext.StoreFrontID);

			var catalogRepo = Create.New<ICatalogRepository>();
			var filteredCatalogs = activeCatalogs.Where(ac => IsValidEnrollmentCatalog(ac, catalogRepo, catalogType, enrollmentContext.AccountTypeID));
			var filteredProducts = filteredCatalogs.SelectMany(ca => inventory.GetActiveProductsForCatalog(enrollmentContext.StoreFrontID, ca.CatalogID, (short)enrollmentContext.AccountTypeID));
			var returnValue = filteredProducts.Cast<Product>().ToList();

			return returnValue;
		}

		private bool IsValidEnrollmentCatalog(Catalog catalog, ICatalogRepository catalogRepo, int catalogType, int accountTypeID)
		{
			if (catalog.CatalogTypeID != catalogType)
			{
				return false;
			}

			var catalogAccountTypes = catalogRepo.GetAccountTypeIDsForCatalog(catalog);
			if (catalogAccountTypes == null || !catalogAccountTypes.Any(at => at == accountTypeID))
			{
				return false;
			}

			return true;
		}

		#endregion

	}
}
