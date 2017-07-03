using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class ConsultSupportTicketDto
    {
        public int SupportTicketID { get; set; }

        public string AccountNumber { get; set; }

        public string SupportTicketNumber { get; set; }

        public string AssignedUsername { get; set; }

        public string PriorityName { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StatusName { get; set; }

        public string CategoryName { get; set; }

        /*CS.19AGO2016.Inicio.Nuevas Propiedades*/
        public string OrderNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string InvoiceNumber { get; set; }
        /*CS.19AGO2016.Fin.Nuevas Propiedades*/

        public string CreateUserName { get; set; }

        public string DateCreated { get; set; }

        public string DateLastModified { get; set; }

        public string Comfim { get; set; }

        public string Close { get; set; }

        public string Question { get; set; }

        public string Respuesta { get; set; }

        public string RowTotal  { get; set; }
        public string SupportLevelMotive { get; set; }
    }
}
