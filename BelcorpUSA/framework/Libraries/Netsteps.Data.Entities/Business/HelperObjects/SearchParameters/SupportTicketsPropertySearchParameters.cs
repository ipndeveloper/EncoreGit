using NetSteps.Common.Base;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class SupportTicketsPropertySearchParameters 
    {
        public int SupportTicketsPropertyID { get; set; }
        
        public decimal SupportTicketID { get; set; }

        public int SupportMotivePropertyTypeID { get; set; }

        public int SupportTicketsPropertyValueID { get; set; }

        public string PropertyValue { get; set; }
        
    }
}
