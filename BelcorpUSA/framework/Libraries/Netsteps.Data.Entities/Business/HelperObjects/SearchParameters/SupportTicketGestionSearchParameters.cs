using NetSteps.Common.Base;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class SupportTicketGestionSearchParameters 
    {
        public decimal SupportTicketID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public DateTime   DateCreatedUTC{get;set;}
        public DateTime DateLastModifiedUTC { get; set; }
        public string PropertyValue { get; set; }
        public  int SupportTicketStatusID {get;set;}
        public bool  IsInternal{get;set;}
        // 
    }
}
