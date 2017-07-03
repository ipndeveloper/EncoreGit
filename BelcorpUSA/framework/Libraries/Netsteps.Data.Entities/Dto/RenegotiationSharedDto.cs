using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class RenegotiationSharedDto
    {
        public string TotalAmount { get; set; }
        public string Discount { get; set; }// descuento
        public string TotalPay { get; set; }//  total a Pagar        
        public string ModifiesValues { get; set; }
        public string FirstDateExpirated { get; set; }//PrimeraFecha
        public string SharesInterval { get; set; }
        public string ModifiesDates { get; set; }
        public string LastDateExpirated { get; set; }//UltimaFecha
        public string NumShared { get; set; }//Numero de Cuotas
        public string ValShared { get; set; }//  valor de cuotas
        public string DayValidate { get; set; }

        public List<RenegotiationSharedDetDto> ListShared = new List<RenegotiationSharedDetDto>();
    }
}
