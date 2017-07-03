using System.Collections.Generic;
using AddressValidation.Common;
using NetSteps.Encore.Core.Dto;

namespace AddressValidator.Common
{
    [DTO]
    public interface IAddressValidationResult
    {
        AddressInfoResultState Status { get; set; }
        IEnumerable<IValidationAddress> ValidAddresses { get; set; }
        string Message { get; set; }
    }
}