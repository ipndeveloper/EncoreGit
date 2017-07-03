using System.Collections.Generic;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Globalization
{
    [ContainerRegister(typeof(IStatesProvidencesProvider), RegistrationBehaviors.Default)]
    public class StatesProvidencesProvider : IStatesProvidencesProvider, IDefaultImplementation
    {
        public List<StateProvidenceData> GetStates(int countryID)
        {
            List<StateProvidenceData> matchingResults = new List<StateProvidenceData>();

            var states = StateProvince.LoadStatesByCountry(countryID);
            foreach (var result in states)
            {
                matchingResults.Add(new StateProvidenceData
                {
                    Name = result.Name,
                    StateID = result.StateProvinceID,
                    StateAbbr = result.StateAbbreviation,
                    CountryID = countryID
                });
            }

            return matchingResults;
        }
    }
}
