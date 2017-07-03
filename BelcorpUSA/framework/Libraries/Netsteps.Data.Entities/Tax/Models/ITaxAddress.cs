using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
    public enum AddressPropertyKind
    {
        None = 0,
        /// <summary>
        /// Indicates a city.
        /// </summary>
        City = 1,
        /// <summary>
        /// Indicates a state, province, or territory.
        /// </summary>
        MainDivision = 2,
        /// <summary>
        /// Indicates a county, parish, etc.
        /// </summary>
        SubDivision = 2,
        /// <summary>
        /// Indicates a postal code.
        /// </summary>
        PostalCode = 3,
        /// <summary>
        /// Indicates a country.
        /// </summary>
        Country = 4,
    }

    [DTO]
    public interface ITaxAddressProperty
    {
        AddressPropertyKind Kind { get; set; }
        string Name { get; set; }
        string Code { get; set; }
    }

    [DTO]
    public interface ITaxAddress
    {
        string StreetAddress1 { get; set; }
        string StreetAddress2 { get; set; }
        ITaxAddressProperty City { get; set; }
        /// <summary>
        /// Gets the address' main division; State, Province, or Territory.
        /// </summary>
        ITaxAddressProperty MainDivision { get; set; }
        /// <summary>
        /// Gets the address' sub division; usually a County, Parish, Burough
        /// </summary>
        ITaxAddressProperty SubDivision { get; set; }
        ITaxAddressProperty PostalCode { get; set; }
        ITaxAddressProperty Country { get; set; }
    }

}
