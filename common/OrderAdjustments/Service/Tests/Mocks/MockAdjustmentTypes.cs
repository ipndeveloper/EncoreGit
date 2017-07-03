using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    [Flags]
    public enum MockAdjustmentTypes
    {
        AddedItem = 1,
        AddedItemWithQuantity2 = 1 << 1,
        ReducedShippingTotalBy20Percent = 1 << 2,
        ReducedShippingTotalBy10Flat = 1 << 3,
        ReducedProduct1PriceBy14Percent = 1 << 4,
        ReducedProduct1PriceBy16Flat = 1 << 5,
        ReducedSingleProduct1PriceBy23Percent = 1 << 6,
        ReducedSingleProduct1PriceBy24Flat = 1 << 7,
        All = AddedItem | AddedItemWithQuantity2 | ReducedSingleProduct1PriceBy24Flat | 
              ReducedSingleProduct1PriceBy23Percent | ReducedShippingTotalBy20Percent | 
              ReducedShippingTotalBy10Flat | ReducedProduct1PriceBy16Flat | ReducedProduct1PriceBy14Percent
    }
}
