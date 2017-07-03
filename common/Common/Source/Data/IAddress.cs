namespace NetSteps.Common.Data
{
    public interface IAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string PostalCode { get; set; }
        string Country { get; }
    }
}
