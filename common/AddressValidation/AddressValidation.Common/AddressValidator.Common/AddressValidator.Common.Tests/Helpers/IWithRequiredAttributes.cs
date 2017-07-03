using System.ComponentModel.DataAnnotations;
using NetSteps.Encore.Core.Dto;

namespace AddressValidation.Common.Tests.Helpers
{
    [DTO]
    public interface IWithRequiredAttributes
    {
        [Required(ErrorMessage = "Name is required.")]
        string Name { get; set; }
    }
}
