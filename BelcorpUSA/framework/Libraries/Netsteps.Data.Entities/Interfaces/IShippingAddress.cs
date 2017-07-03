using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface IShippingAddress : IAddress
    {
        string ShipToEmailAddress { get; set; }
    }
}
