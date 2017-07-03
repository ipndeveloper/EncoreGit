using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Support.Models.Ticket
{
    public class SupportTicketDetaillModel
    {
        public List<SupportMotivePropertyTypes> LstSupportMotivePropertyTypes { get; set; }
        public List<SupportMotivePropertyValues> LstSupportMotivePropertyValues { get; set; }
        public List<SupportMotiveTask> LstSupportMotiveTask { get; set; }
        public List<SupportTicketsFilesBE> LstSupportTicketsFilesBE { get; set; }
        public Boolean IsSiteDWS { get; set; }

        public bool ModoEdicion{ get; set; }
      
        public SupportTicketDetaillModel
            (
                List<SupportMotivePropertyTypes> pLstSupportMotivePropertyTypes,
                List<SupportMotivePropertyValues> pLstSupportMotivePropertyValues,
                List<SupportMotiveTask> pSupportMotiveTask,
                List<SupportTicketsFilesBE> pLstSupportTicketsFilesBE ,
                Boolean IsSiteDWS=false,
                Boolean ModoEdicion=true
                
            )
        {
            this.LstSupportMotivePropertyTypes = pLstSupportMotivePropertyTypes;
            this.LstSupportMotivePropertyValues = pLstSupportMotivePropertyValues;
            this.LstSupportMotiveTask = pSupportMotiveTask;
            this.LstSupportTicketsFilesBE = pLstSupportTicketsFilesBE;
            this.ModoEdicion = ModoEdicion;
            this.IsSiteDWS = IsSiteDWS;
          
        
        }
    
     
    }
}