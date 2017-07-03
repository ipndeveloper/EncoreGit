using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class RequirementTypesBusinessLogic
    {
        public IEnumerable<dynamic> GetAllRequirements()
        {
            var table = new RequirementTypes();
            var lista = table.All();
            return lista;
        }

    }
}
