using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class SupportTicketGestionBusinessLogic
    {
        public static dynamic Insert(SupportTicketGestionSearchParameters model)
        {
            var table = new SupportTicketGestion();

            try
            {
                bool estadoTicket = true;
                if (model.SupportTicketStatusID.Equals(Constants.SupportTicketStatus.Resolved))
                    estadoTicket = false;

                    return table.Insert(new
                    {
                        SupportTicketID = model.SupportTicketID,
                        UserID  = model.UserID,
                        Descripction=model.Description,
                        DateCreatedUTC = model.DateCreatedUTC,
                        SupportTicketStatusID = estadoTicket,
                        isInternal = model.IsInternal
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
