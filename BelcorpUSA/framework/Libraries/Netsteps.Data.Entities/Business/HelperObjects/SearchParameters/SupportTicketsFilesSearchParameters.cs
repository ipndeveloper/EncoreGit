using NetSteps.Common.Base;
using System;

namespace NetSteps.Data.Entities.Business
{

    public class SupportTicketsFilesSearchParameters 
    {
        public int SupportTicketsFileID { get; set; }
        public int SupportTicketID { get; set; }
        public int UserID { get; set; }
        public string FilePath { get; set; }
        public DateTime   DateCreatedUTC{get;set;}      
    }
}
