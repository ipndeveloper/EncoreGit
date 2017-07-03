using NetSteps.Encore.Core.Dto;

namespace AddressValidator.Common
{
    [DTO]
    public interface IValidationAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        /// <summary>
        /// Gets and sets the address' main division; state, province, territory, etc.
        /// </summary>
        string MainDivision { get; set; }
        /// <summary>
        /// Gets and sets the address' sub division; county, parish, burough, etc.
        /// </summary>
        string SubDivision { get; set; }
        string PostalCode { get; set; }
        string Country { get; set; }
    }
}