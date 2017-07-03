using nsDistributor.Areas.Enroll.Models.Shared;
using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    [DTO]
    public interface IIndexModel
    {
        IOrderEntryModel OrderEntryModel { get; set; }
    }


}