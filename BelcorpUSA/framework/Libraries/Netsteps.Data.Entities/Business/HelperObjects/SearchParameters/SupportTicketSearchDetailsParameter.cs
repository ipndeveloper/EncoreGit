using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    [Serializable]
    public class SupportTicketSearchDetailsParameter : FilterDateRangePaginatedListParameters<SupportTicket>
    {
        public int TypeConsult { get; set; }

        public int? PriorityID { get; set; }

        public int? CategoryID { get; set; }

        public int? StatusID { get; set; }

        public string SupportTicket { get; set; }

        public string Title { get; set; }

        /*CS.19AG2016.Inicio.NuevasPropiedades de Filtro*/
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        /*CS.19AG2016.Fin.NuevasPropiedades de Filtro*/

        public int? AssignedUserID { get; set; }

        public int? ConsultSearchID { get; set; }

        public int? CreateByUserID { get; set; }

        public int? TypeUserID { get; set; }

        public int? TypeConsultID { get; set; }

        public int? IsConfirmID { get; set; }

        public int? CampaignID { get; set; }

        public int RowCount { get; set; }

        public string Order { get; set; }
        

        public int? SupportLevelID { get; set; }

        public int? SupportMotiveID { get; set; }


        public byte IsSiteDWS { get; set; }
    }
}
