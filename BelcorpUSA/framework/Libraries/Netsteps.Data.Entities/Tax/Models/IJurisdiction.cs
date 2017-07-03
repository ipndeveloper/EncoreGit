using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
    [DTO]
    public interface IJurisdiction
    {
        JurisdictionLevel Level { get; set; }
        string Name { get; set; }
    }
}
