using System;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    [ContainerRegister(typeof(IOrderAdjustmentOrderLineModification), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public partial class OrderAdjustmentOrderLineModification : IOrderAdjustmentOrderLineModification, IDateLastModified
    {

        void IOrderAdjustmentOrderLineModification.MarkAsDeleted()
        {
            this.MarkAsDeleted();

        }

        public int ProductID { get; set; }

        public int? MaximumQuantityAffected { get; set; }

        public decimal CalculatedValue(decimal targetPropertyValue)
        {
            if (ModificationDecimalValue.HasValue)
            {
                switch (ModificationOperationID)
                {
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem:
                        return ModificationDecimalValue.Value;
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.FlatAmount:
                        return ModificationDecimalValue.Value;
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.Multiplier:
                        return ModificationDecimalValue.Value * targetPropertyValue;
                    default:
                        throw new InvalidOperationException(String.Format("Unable to calculate order line modification value with unknown ModificationOperationID {0}.", ModificationOperationID));
                }

            }

            return 0;
        }

        /// <summary>
        /// Devuelve el precio aplicado por el descuento, deacuerdo al tipo de operacion
        /// </summary>
        /// <param name="targetPropertyValue">Precio original</param>
        /// <param name="modificationDiscount">Porcentaje de descuento</param>
        /// <returns>Descuento</returns>
        public decimal CalculatedValue(decimal targetPropertyValue, decimal? modificationDiscount)
        {
            if (modificationDiscount.HasValue)
            {
                switch (ModificationOperationID)
                {
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem:
                        return modificationDiscount.Value;
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.FlatAmount:
                        return modificationDiscount.Value;
                    case (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.Multiplier:
                        return modificationDiscount.Value * targetPropertyValue;
                    default:
                        throw new InvalidOperationException(String.Format("Unable to calculate order line modification value with unknown ModificationOperationID {0}.", ModificationOperationID));
                }

            }

            return 0;
        }
    }
}
