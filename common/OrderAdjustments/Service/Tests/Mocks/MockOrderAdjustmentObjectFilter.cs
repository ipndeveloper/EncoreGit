using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.OrderAdjustments.Common;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustmentObjectFilter : IObjectFilter<IOrderAdjustmentProfile>
    {

        public IQueryable<IOrderAdjustmentProfile> BuildQueryFrom(IQueryable<IOrderAdjustmentProfile> QueryBase)
        {
            // don't do this in a real query....
            List<IOrderAdjustmentProfile> adjustments = QueryBase.ToList();
            adjustments.ForEach((x) => x.Description += "Here is my handle, here is my spout.");
            return adjustments.AsQueryable();
        }
    }
}
