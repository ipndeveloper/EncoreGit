namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    public interface ILogisticShippingRulesRepository
    {
        List<ShippingRateGroupDto> ListShippingRates(int page, int pageSize, string column, string order, int shippingRuleId, int shippingMethodId, int statusId, int warehouseId, int logisticProviderId);
    }
}
