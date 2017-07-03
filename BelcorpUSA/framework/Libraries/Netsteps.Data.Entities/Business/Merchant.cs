
namespace NetSteps.Data.Entities
{
    public partial class Merchant
    {
        /// <summary>
        /// Attempts to find the merchant by the  merchantnumber, returns null
        /// </summary>
        /// <param name="merchantNumber"></param>
        /// <returns></returns>
        public static Merchant Load(string merchantNumber)
        {
            return Repository.GetByNumber(merchantNumber);
        }

        public static Merchant GetById(int merchantId)
        {
            return Repository.GetById(merchantId);
        }

        /// <summary>
        /// Gets the first merchant which contains an address in its address list matching the provided addressID
        /// </summary>
        /// <param name="addressID">The desired AddressID to match </param>
        /// <returns>null if not found, merchant if found</returns>
        public static Merchant GetByAddressID(int addressID)
        {
            return Repository.GetByAddressID(addressID);
        }
    }
}
