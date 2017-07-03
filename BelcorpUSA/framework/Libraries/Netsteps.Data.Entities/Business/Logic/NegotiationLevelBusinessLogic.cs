using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class NegotiationLevelBusinessLogic
    {
        public IEnumerable<dynamic> GetAllNegotiationLevel()
        {
            var table = new NegotiationLevels();
            return table.All();
        }
    }
}