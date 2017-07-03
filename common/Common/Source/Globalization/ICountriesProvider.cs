using System.Collections.Generic;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 08/18/2010
    /// </summary>
    public interface ICountriesProvider
    {
        List<CountryData> GetCountries();
    }
}
