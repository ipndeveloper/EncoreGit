using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Helpers
{
    public static class OrderItemMultiplierCalculator
    {
        public static decimal CalculateVolumeMultiplier(this BaseRecordPropertyCalculationHandler handler, IRecord orderItemRecord, IProductPriceType primaryVolumeType)
        {
            var primaryOrderItemPriceRecord =
                orderItemRecord.ChildRecords.SingleOrDefault(
                    x =>
                    x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItemPrice &&
                    ((int)x.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue) == primaryVolumeType.PriceTypeID);
            var originalPrice = (decimal)handler.CalculateDependentValue(primaryOrderItemPriceRecord, EncoreFieldNames.OrderItemPrice.OriginalUnitPrice);
            var calculatedPrice = (decimal)handler.CalculateDependentValue(primaryOrderItemPriceRecord, EncoreFieldNames.OrderItemPrice.UnitPrice);
            if (originalPrice != 0)
            {
                return calculatedPrice / originalPrice;
            }
            else
            {
                return 0;
            }
        }

        public static decimal CalculateCurrencyMultiplier(this BaseRecordPropertyCalculationHandler handler, IRecord orderItemRecord)
        {
            var primaryPriceTypeID = (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductPriceTypeID].OriginalValue;
            var primaryOrderItemPriceRecord =
                orderItemRecord.ChildRecords.SingleOrDefault(
                    x =>
                    x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItemPrice &&
                    ((int)x.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue) == primaryPriceTypeID);
            var originalPriceObject = handler.CalculateDependentValue(primaryOrderItemPriceRecord, EncoreFieldNames.OrderItemPrice.OriginalUnitPrice);
            var originalPrice = originalPriceObject == null ? (decimal)originalPriceObject : 0M;
            var calculatedPriceObject = handler.CalculateDependentValue(primaryOrderItemPriceRecord, EncoreFieldNames.OrderItemPrice.UnitPrice);
            var calculatedPrice = calculatedPriceObject == null ? (decimal)calculatedPriceObject : 0M;
            if (originalPrice != 0)
            {
                return calculatedPrice / originalPrice;
            }
            else
            {
                return 0;
            }
        }

        
    }
}
