using System.Collections.Generic;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 08/18/2010
    /// </summary>
    public interface IStatesProvidencesProvider
    {
        List<StateProvidenceData> GetStates(int countryId);
    }
}
