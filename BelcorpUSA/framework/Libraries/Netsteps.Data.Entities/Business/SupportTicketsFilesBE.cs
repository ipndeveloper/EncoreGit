using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{

    [Serializable]
   public  class SupportTicketsFilesBE
    {
        public int  SupportTicketFileID{get;set;}
        public int SupportTicketID { get; set; }
        public string FilePath { get; set; }
        public int	UserID{get;set;}
        public DateTime?   DateCreatedUTC{get;set;}
        public byte[] Content { get; set; }
        public string Extension { get; set; }
    }
}
