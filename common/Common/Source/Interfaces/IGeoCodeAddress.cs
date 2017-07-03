
namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Interface to using when looking up Geocodes - JHE
    /// </summary>
    public interface IGeoCodeAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string City { get; set; }
        string County { get; set; }
        string State { get; set; }
        string PostalCode { get; set; }
        string Country { get; }
    }
}
