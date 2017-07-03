namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Descripcion de la interface
    /// </summary>
    public interface IPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountRepository
    {
        PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetByAmount(decimal amount);

        PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetById(int id);

        PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto GetByPromotionID(int id);
    }
}
