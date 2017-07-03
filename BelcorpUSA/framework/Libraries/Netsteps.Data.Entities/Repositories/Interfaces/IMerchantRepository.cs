
namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IMerchantRepository
    {
        Merchant GetByNumber(string merchantNumber);
        Merchant GetById(int merchantId);

        /// <summary>
        /// Gets the first merchant which contains an address in its address list matching the provided addressID
        /// </summary>
        /// <param name="addressID">The desired AddressID to match </param>
        /// <returns>null if not found, merchant if found</returns>
        Merchant GetByAddressID(int addressID);
    }
}
