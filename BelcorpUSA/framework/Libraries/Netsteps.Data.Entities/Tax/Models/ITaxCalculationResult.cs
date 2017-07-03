using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
    public enum TaxCalculationState
    {
        Faulted = -1,
        Skipped = 0,
        Succeeded = 1,
    }

    [DTO]
    public interface ITaxCalculationResult
    {
        TaxCalculationState Status { get; set; }
        ITaxOrder Order { get; set; }
        ITaxCalculationFault Fault { get; set;}
    }

    [DTO]
    public interface ITaxCalculationFault
    {
        string Code { get; set; }
        string Message { get; set; }
    }
}
