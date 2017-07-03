using System;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    [ContainerRegister(typeof(IOrderAdjustmentOrderModification), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public partial class OrderAdjustmentOrderModification : IOrderAdjustmentOrderModification, IDateLastModified
    {
        void IOrderAdjustmentOrderModification.MarkAsDeleted()
        {
            this.MarkAsDeleted();
        }

        public decimal CalculatedValue(decimal targetPropertyValue)
        {
            if (ModificationDecimalValue.HasValue)
            {
                switch (ModificationOperationID)
                {
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderOperationKind.FlatAmount:
                        return ModificationDecimalValue.Value;
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderOperationKind.Multiplier:
                        return ModificationDecimalValue.Value * targetPropertyValue;
                    default:
                        throw new InvalidOperationException(String.Format("Unable to calculate order modification value with unknown ModificationOperationID {0}.", ModificationOperationID));
                }

            }

            return 0;
        }
    }
}
