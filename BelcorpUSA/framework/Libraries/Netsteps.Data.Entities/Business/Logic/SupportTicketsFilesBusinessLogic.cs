using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class SupportTicketsFilesBusinessLogic
    {
        public  static dynamic Insert(SupportTicketsFilesSearchParameters model)
        { 
            var table = new SupportTicketsFiles();
             
            try
            {               
                    return table.Insert(new
                    {
                        SupportTicketID = model.SupportTicketID,
                        FilePath = model.FilePath,
                        UserID = model.UserID,
                        DateCreatedUTC = model.DateCreatedUTC
                    });
                
            }
            catch
            {
                return null;
            }
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
