using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class SponsorDataAccountStatus
    {
        //Obtencion de datos de AccountStatuses para opcion RestrictAccountStatuses
        //Developed by Kelvin Lopez C. - CSTI
        public int AccountStatusID { get; set; }
        public string Name { get; set; }
        
    }
}
