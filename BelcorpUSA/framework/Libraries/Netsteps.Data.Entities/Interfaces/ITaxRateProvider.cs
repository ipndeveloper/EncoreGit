using System.Collections.Generic;
using NetSteps.Data.Entities.Tax;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface ITaxRateProvider
    {
        List<TaxRateInfo> GetTaxInfo(string postalCode);
        List<TaxRateInfo> GetTaxInfo(int countryId, string stateAbbr, string county, string city, string postalCode);
        bool GetDefaultChargeShippingTax(string stateAbbr);
    }
}
