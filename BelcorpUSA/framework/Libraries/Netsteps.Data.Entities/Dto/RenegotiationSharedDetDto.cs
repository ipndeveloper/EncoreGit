using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class RenegotiationSharedDetDto
    {
        public string Parcela { get; set; }
        public string ValShared { get; set; }//  Cuota
        public string ExpirationDate { get; set; }// Vencimiento
       
    }
}
