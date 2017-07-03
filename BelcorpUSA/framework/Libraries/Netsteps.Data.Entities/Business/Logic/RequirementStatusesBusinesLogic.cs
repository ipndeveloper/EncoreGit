using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NetSteps.Data.Entities.Business.Logic
{
    public class RequirementStatusesBusinesLogic
    {

        public IEnumerable<dynamic> GetAllStatus()
        {
            var table = new RequirementStatuses();
            var lista = table.All();
            return lista;
        }


    }
}
