using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
    [DTO]
    public interface IKitItemsModel
    {
        IList<IKitItemModel> KitItemModels { get; set; }
    }
}