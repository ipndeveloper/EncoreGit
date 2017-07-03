using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
    [DTO]
    public interface IKitItemModel
    {
        string SKU { get; set; }
        string ProductName { get; set; }
        int Quantity { get; set; }
    }
}