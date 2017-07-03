using System;
using System.Collections.Generic;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class WarehouseProduct
    {
        #region Basic Crud
        public static List<WarehouseProduct> LoadAll()
        {
            try
            {
                var list = Repository.LoadAll();
                list.Each(item =>
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                });
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static new WarehouseProduct Load(int warehouseProductID)
        {
            try
            {
                var repo = new WarehouseProductRepository();
                return repo.Load(warehouseProductID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        public static List<WarehouseProduct> GetFullInventory()
        {
            try
            {
                return Repository.GetFullInventory();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static WarehouseProduct GetWarehouseProduct(int warehouseID, int productID)
        {
            try
            {
                return Repository.GetWarehouseProduct(warehouseID, productID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static WarehouseProduct GetWarehouseProduct(IAddress address, int productID)
        {
            try
            {
                
                return Repository.GetWarehouseProduct(address, productID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
