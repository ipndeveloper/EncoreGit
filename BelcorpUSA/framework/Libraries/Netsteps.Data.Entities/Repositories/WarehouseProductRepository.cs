using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class WarehouseProductRepository
	{
		protected override Func<NetStepsEntities, IQueryable<WarehouseProduct>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<WarehouseProduct>>(
				 (context) => context.WarehouseProducts.Include("Warehouse").Include("Product"));
			}
		}

		public List<WarehouseProduct> GetFullInventory()
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var warehouseProducts = context.WarehouseProducts;
					var newWarehouseProducts = context.Products.SelectMany(p => context.Warehouses, (p, w) => new { p.ProductID, w.WarehouseID }).Except(context.WarehouseProducts.Select(wp => new { wp.ProductID, wp.WarehouseID }));

					return warehouseProducts.Union(newWarehouseProducts.Select(wp => new WarehouseProduct()
					{
						WarehouseID = wp.WarehouseID,
						ProductID = wp.ProductID,
						IsAvailable = false,
						QuantityBuffer = 0,
						QuantityOnHand = 0
					})).ToList();
				}
			});
		}

		public List<WarehouseProduct> GetInventoryForWarehouse(int warehouseID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var warehouseProducts = context.WarehouseProducts;
					var newWarehouseProducts = context.Products.Select(p => new { p.ProductID, warehouseID }).Except(context.WarehouseProducts.Where(wp => wp.WarehouseID == warehouseID).Select(wp => new { wp.ProductID, warehouseID }));

					return warehouseProducts.Union(newWarehouseProducts.Select(wp => new WarehouseProduct()
					{
						WarehouseID = warehouseID,
						ProductID = wp.ProductID,
						IsAvailable = false,
						QuantityBuffer = 0,
						QuantityOnHand = 0
					})).ToList();
				}
			});
		}

	    public new WarehouseProduct Load(int warehouseProductID)
	    {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    return context.WarehouseProducts.FirstOrDefault(wp => wp.WarehouseProductID == warehouseProductID);
                }
            });
	    }
		public WarehouseProduct GetWarehouseProduct(int warehouseID, int productID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.WarehouseProducts.FirstOrDefault(wp => wp.WarehouseID == warehouseID && wp.ProductID == productID);
				}
			});
		}

		public WarehouseProduct GetWarehouseProduct(IAddress address, int productID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var inventory = Create.New<InventoryBaseRepository>();

					Product product = null;
					try
					{
						product = inventory.GetProduct(productID);
					}
					catch (Exception)
					{
						throw new NetStepsException("Product could not be found.")
						{
							PublicMessage = Translation.GetTerm("ProductNotFound", "Product could not be found.")
						};
					}

					Warehouse warehouse = GetWarehouse(context, address, new List<int> { productID });

					return context.WarehouseProducts.FirstOrDefault(wp => wp.WarehouseID == warehouse.WarehouseID && wp.ProductID == productID);
				}
			});
		}

		internal static Warehouse GetWarehouse(NetStepsEntities context, IAddress address, IEnumerable<int> productIDs = null)
		{
			StateProvince stateProvince = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(sp => sp.StateProvinceID == address.StateProvinceID) 
				?? SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(
						sp => sp.StateAbbreviation == address.State && sp.CountryID == address.CountryID);
			if (stateProvince == null)
			{
				throw new NetStepsException(string.Format("StateProvince could not be found for StateProvinceID: {0}.", address.StateProvinceID))
				{
					PublicMessage = Translation.GetTerm("StateProvinceNotFound", "StateProvince could not be found.")
				};
			}

			if(!stateProvince.ShippingRegionID.HasValue)
			{
				throw new NetStepsException(string.Format("There is no shipping region for StateProvinceID: {0}", stateProvince.StateProvinceID))
				{
					PublicMessage = Translation.GetTerm("NoShippingRegionForStateProvince", "There is no shipping region for the selected StateProvince")
				};
			}

			var shippingRegion = SmallCollectionCache.Instance.ShippingRegions.FirstOrDefault(sr => sr.ShippingRegionID == stateProvince.ShippingRegionID);
			if(shippingRegion == null)
			{
				throw new NetStepsException(string.Format("ShippingRegion could not be found for ShippingRegionID: {0}, StateProvinceID {1}", stateProvince.ShippingRegionID, stateProvince.StateProvinceID))
				{
					PublicMessage = Translation.GetTerm("ShippingRegionNotFound", "Shipping region could not be found.")
				};
			}

			//Warehouse warehouse = null;
			var warehouse = SmallCollectionCache.Instance.Warehouses.FirstOrDefault(w => w.WarehouseID == shippingRegion.WarehouseID);
			if(warehouse == null)
			{
				throw new NetStepsException(string.Format("Warehouse could not be found for WarehouseID: {0}, ShippingRegionID: {1}", shippingRegion.WarehouseID, shippingRegion.ShippingRegionID))
				{
					PublicMessage = Translation.GetTerm("WarehouseNotFound", "Warehouse could not be found")
				};
			}
			return warehouse;
		}
	}
}
