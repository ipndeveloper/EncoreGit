using DistributorBackOffice.Areas.Orders.Models.Shared;
using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Areas.Orders.Models.OrderEntry
{
    [DTO]
    public interface IIndexModel
    {
        IOrderEntryModel OrderEntryModel { get; set; }
    }
}