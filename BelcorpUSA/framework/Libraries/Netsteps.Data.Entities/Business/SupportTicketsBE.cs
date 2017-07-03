using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
  public   class SupportTicketsBE
    {
            public  int  SupportTicketID	{get;set;}
            public  string SupportTicketNumber	{get;set;}
            public int AccountID	{get;set;}
            public int AssignedUserID	{get;set;}
            public string Title	{get;set;}

            /*CS.20AG2016.Inicio*/
            public string OrderNumber { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string InvoiceNumber { get; set; }
            /*CS.20AG2016.Fin*/

            public   string  	 Description	{get;set;}
            public Int16  SupportTicketCategoryID	{get;set;}
            public Int16 SupportTicketPriorityID	{get;set;}
            public Int16 SupportTicketStatusID	{get;set;}
            public Boolean IsVisibleToOwner	{get;set;}
            public int    CreatedByUserID	 {get;set;}
            public int  ModifiedByUserID	 {get;set;}
            public DateTime? DateCreatedUTC	{get;set;}
            public DateTime?  DateLastModifiedUTC	 {get;set;}
            public int   SupportLevelID	 {get;set;}
            public DateTime? DateCloseUTC	{get;set;}
            public int  SupportMotiveID	 {get;set;}
            public int   ScalingUserID	 {get;set;}
            public Boolean  IsConfirm	  {get;set;}
            public int BlockUserID	 {get;set;}
            public string RutaDirectorio { get; set; }

            public string  FirstName {get;set;}
            public string  LastName {get;set;}
            public string  Username { get; set; }
            public bool NotEdit { get; set; }
            public string SupportLevelMotive { get; set; }
            public string BlockUserName { get; set; }

            public string NameStatuses { get; set; }

            public string UserNameAsigned { get; set; }

            public int UserTypeID { get; set; }
            public Boolean? IsSiteDWS { get; set; }
    }
}
