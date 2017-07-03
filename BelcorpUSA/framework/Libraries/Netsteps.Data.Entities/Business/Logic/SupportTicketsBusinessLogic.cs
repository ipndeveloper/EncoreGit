using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class SupportTicketsBusinessLogic
    {
        public  dynamic Insert(SupportTicketSearchParameters model)
        {
            var table = new SupportTickets();

            try
            {
                if (model.SupportTicketStatusID.Equals(Constants.SupportTicketStatus.Resolved) && model.DateCloseUTC != null)
                {
                    table.Insert(new
                    {
                      
                        AccountID = model.AccountID,
                        AssignedUserID = model.AssignedUserID,
                        Title = model.Title,
                        Description = model.Description,

                        SupportTicketPriorityID = model.SupportTicketPriorityID,
                        SupportTicketStatusID = model.SupportTicketStatusID,
                        IsVisibleToOwner = model.IsVisibleToOwner,
                        CreatedByUserID = model.CreatedByUserID,
                        ModifiedByUserID = model.ModifiedByUserID,

                        DateCreatedUTC = model.DateCreatedUTC,
                        DateLastModifiedUTC = model.DateLastModifiedUTC,
                        SupportLevelID = model.SupportLevelID,
                        DateCloseUTC = model.DateCloseUTC,
                        SupportMotiveID = model.SupportMotiveID

                    });


                }
                else
                {
                     table.Insert(new
                    {
                       
                        AccountID = model.AccountID,
                        AssignedUserID = model.AssignedUserID,
                        Title = model.Title,
                        Description = model.Description,

                        SupportTicketPriorityID = model.SupportTicketPriorityID,
                        SupportTicketStatusID = model.SupportTicketStatusID,
                        IsVisibleToOwner = model.IsVisibleToOwner,
                        CreatedByUserID = model.CreatedByUserID,
                        ModifiedByUserID = model.ModifiedByUserID,

                        DateCreatedUTC = model.DateCreatedUTC,
                        DateLastModifiedUTC = model.DateLastModifiedUTC,
                        SupportLevelID = model.SupportLevelID,                        
                        SupportMotiveID = model.SupportMotiveID

                    });
                
                }
                var table02 = new SupportTickets();
                var lista = table02.All().OrderByDescending(y => y.SupportTicketID).FirstOrDefault();

                table.Update(new
                   {
                       SupportTicketNumber = "10" +  lista.SupportTicketID
                   },
                      lista.SupportTicketID
                 );


                return lista;
              
                
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<dynamic> GetValuesByPropertyID(int propertyID)
        {
            var table = new CreditRequirements();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetSupportMotivePropertyValueByPropertyTypeID @0", new object[] { propertyID });
            return List;
        }
        

        //public bool Update(dynamic model)
        //{
        //    var table = new AccountProperties();

        //    try
        //    {
               
        //            table.Update(new
        //            {
        //                IDTypeID = model.IDTypeID,
        //                AccountID = model.AccountID,
        //                AccountSuppliedIDValue = model.AccountSuppliedIDValue,
        //                IsPrimaryID = model.IsPrimaryID
        //            },
        //               model.AccountSuppliedID
        //               );
                

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }
}
