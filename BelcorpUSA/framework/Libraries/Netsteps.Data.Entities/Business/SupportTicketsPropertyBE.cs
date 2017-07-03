using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public    class SupportTicketsPropertyBE 
    {

            public int    SupportTicketsPropertyID{get;set;}
            public int    SupportTicketID{get;set;}
            public int    SupportMotivePropertyTypeID{get;set;}
            public int	  SupportTicketsPropertyValueID{get;set;}
            public string PropertyValue { get; set; }

       
    }
}
