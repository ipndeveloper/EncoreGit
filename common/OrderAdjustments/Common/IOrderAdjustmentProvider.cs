using System.Collections.Generic;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common
{
    public interface IOrderAdjustmentProvider : IDataObjectExtensionProvider
    {
        IEnumerable<IOrderAdjustmentProfile> GetApplicableAdjustments(IOrderContext order);
        IOrderAdjustmentProfile GetOrderAdjustmentProfile(IOrderContext orderContext, int adjustmentID);
        bool IsInstanceOfProfile(IOrderAdjustment adjustment, IOrderAdjustmentProfile adjustmentProfile);
        IDataObjectExtension CreateOrderAdjustmentDataObjectExtension(IOrderAdjustmentProfile profile);
        void NotifyOfRemoval(IOrderContext orderContext, IOrderAdjustment adjustment);
        void CommitAdjustment(IOrderAdjustment adjustment, IOrderContext orderContext);
    }
}
