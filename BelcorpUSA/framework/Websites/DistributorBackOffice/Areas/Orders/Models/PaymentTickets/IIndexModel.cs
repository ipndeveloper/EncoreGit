using DistributorBackOffice.Areas.Orders.Models.Shared;
using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Areas.Orders.Models.PaymentTickets
{
    [DTO]
    public interface IIndexModel
    {
        IOrderEntryModel OrderEntryModel { get; set; }
    }
}