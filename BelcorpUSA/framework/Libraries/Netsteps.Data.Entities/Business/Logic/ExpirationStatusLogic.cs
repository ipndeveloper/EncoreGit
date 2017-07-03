using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class ExpirationStatusLogic
    {
       public IEnumerable<dynamic> GetAllExpirationStatus()
        {
            var table = new ExpirationStatuses();
            return table.All();
        }
   }
}