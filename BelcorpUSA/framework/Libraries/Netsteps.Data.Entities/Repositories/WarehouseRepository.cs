using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData; 
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class WarehouseRepository
    {
        #region Members
        protected override Func<NetStepsEntities, int, IQueryable<Warehouse>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<Warehouse>>(
                 (context, warehouseID) => from w in context.Warehouses
                                               .Include("Address")
                                               .Include("WarehouseProducts")
                                               .Include("ShippingRegions")
                                           where w.WarehouseID == warehouseID
                                           select w);
            }
        }

        protected override Func<NetStepsEntities, IQueryable<Warehouse>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Warehouse>>(
                 (context) => from w in context.Warehouses
                                               .Include("Address")
                                               .Include("WarehouseProducts")
                                               .Include("ShippingRegions")
                              select w);
            }
        }
        #endregion


        public override SqlUpdatableList<Warehouse> LoadAllFullWithSqlDependency()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
					var accountTypes = context.Warehouses
						.Include("Address")
						.Include("ShippingRegions")
						.Include("WarehouseProducts")
                        .ToList();

                    SqlUpdatableList<Warehouse> list = new SqlUpdatableList<Warehouse>();

                    list.AddRange(accountTypes);

                    return list;
                }
            });
        }

        #region CSTI -FHP
        public static PaginatedList<WareHouseMaterialSearchData> Search(string dataBase, WareHouseMaterialSearchParameters searchParameter)
        {
            List<WareHouseMaterialSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialSearchData>(dataBase, "uspListWareHouseInventory",
                new SqlParameter("SelectedWareHouse", SqlDbType.Int) { Value = searchParameter.WareHouseID },
                new SqlParameter("SelectedProductID", SqlDbType.Int) { Value = 3034 },
                new SqlParameter("SelectedMaterialID", SqlDbType.Int) { Value = 34 }
                ).ToList();

            IQueryable<WareHouseMaterialSearchData> matchingItems = paginatedResult.AsQueryable<WareHouseMaterialSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<WareHouseMaterialSearchData>(searchParameter, resultTotalCount);
        }


        public class ModelWareHouseMaterial
        {
            public int WareHouseMaterialID { get; set; }
            public string CodeMaterial { get; set; }
            public string MaterialName { get; set; }
            public string WareHouseName { get; set; }
        }

        public static List<ModelWareHouseMaterial> ListWareHouseMaterial(string dataBase, int Code)
        {
            List<ModelWareHouseMaterial> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureListParam<ModelWareHouseMaterial>(dataBase, "uspListWareHouseMaterials",
                new SqlParameter("WareHouseMaterialID", SqlDbType.Int) { Value = Code }
                ).ToList();
            return WareHouseMaterialResult;
        }

        public class InventoryMovementTypes
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int InventoiryMovementTypeID { get; set; }
        }

        public static List<InventoryMovementTypes> ListInventoryMovementTypes(string dataBase)
        {
            List<InventoryMovementTypes> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureLists<InventoryMovementTypes>(dataBase, "uspListInventoryMovementTypes").ToList();
            return WareHouseMaterialResult;
        }
        #endregion



    }
}
