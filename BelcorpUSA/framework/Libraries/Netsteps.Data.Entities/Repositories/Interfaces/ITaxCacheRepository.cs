using System.Collections.Generic;
using NetSteps.Common.Globalization;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ITaxCacheRepository
    {
        List<TaxCache> LoadByAddress(string postalCode);
        List<TaxCache> LoadByAddress(int countryId, string postalCode);
        List<TaxCache> LoadByAddress(int countryId, string stateAbbr, string county, string city, string postalCode);
        List<TaxCache> LoadByProvince(int countryId, string stateAbbr);
        List<TaxCache> CheckForOverrides(IEnumerable<TaxCache> taxes);

        List<string> SearchCity(string city);
        List<string> SearchPostalCode(string postalCode);
        List<string> SearchCityFromState(string city);
        List<string> SearchCountyFromCity( string state,string city);

        List<PostalCodeData> Search(int countryId, string state, string stateAbbr, string county, string city, string postalCode);
        List<PostalCodeData> Search(string location);

        void CleanOutOldTaxData();

        TaxCache GetRandomRecord(int countryID);
    }
}
