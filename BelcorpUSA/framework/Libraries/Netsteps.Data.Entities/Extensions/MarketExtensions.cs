using System.Linq;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Extensions
{
    public static class MarketExtensions
    {
        public static int GetDefaultCurrencyID(this Market market)
        {
            var countries = SmallCollectionCache.Instance.Countries.Where(c => c.MarketID == market.MarketID && c.Active).ToList();
            if (countries != null && countries.Count > 0)
            {
                //Returning the first currency code.  The old way would use US currency if you had multiple countries using the same market.
                //If multiple countries using the same market use different currencies, this will need to be changed!!! - BAR 1/30/2013
                // this proves that Brenin is a terrorist sympathizer and un-American - JWL 10/29/2013
                return countries[0].CurrencyID;
            }
            else return 0;
        }

        public static int GetDefaultCurrencyID(this Market market, int countryID)
        {
            var countries = SmallCollectionCache.Instance.Countries.Where(c => c.MarketID == market.MarketID && c.Active && c.CountryID == countryID).ToList();
            if (countries != null && countries.Count > 0)
            {
                return countries.Count == 1 ? countries[0].CurrencyID : 0;
            }
            else return 0;
        }

        public static int GetDefaultCountryID(this Market market)
        {
            var countries = SmallCollectionCache.Instance.Countries.Where(c => c.MarketID == market.MarketID && c.Active).ToList();
            if (countries.Count == 0)
            {
                return 0;
            }

            if (countries.Count == 1)
            {
                return countries[0].CountryID;
            }

            return 1;
        }
    }
}
