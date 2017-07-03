using System.Collections.Generic;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 08/18/2010
    /// </summary>
    public interface IPostalCodeLookupProvider
    {
        List<PostalCodeData> LookupPostalCode(int countryId, string postalCode);
        List<PostalCodeData> LookupPostalCodeByAccount(int countryId, string postalCode, int accountID, int? addressID);
        string CleanPostalCode(int countryId, string postalCode);
        bool IsValidPostalCode(int countryId, string postalCode);
    }
}
