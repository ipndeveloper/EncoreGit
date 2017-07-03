using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IOrderItemPropertyTypeRepository : ISearchRepository<FilterPaginatedListParameters<OrderItemPropertyType>, PaginatedList<OrderItemPropertyType>>
    {
        OrderItemPropertyType GetByName(string name);
    }
}
