using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class SupportTicketGestionBE
    {
        public int SupportTicketID	{get;set;}
        public int UserID	 {get;set;}
        public string  Descripction	{ get; set; }
        public  DateTime DateCreatedUTC	{get;set;}
        public DateTime DateLastModifiedUTC	{get;set;}
        public DateTime DateCloseUTC	{get;set;}
        public int SupportTicketStatusID	{get;set;}
        public Boolean isInternal { get;set;}
        public string Username { get; set; }

        public Boolean isCLosed { get; set; }
        public string NameStatus { get; set; }
        public Boolean IsVisibleToOwner { get; set; }

        
    }
}
