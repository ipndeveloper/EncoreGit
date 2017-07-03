using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class SupportTicketsPropertyBusinessLogic
    {
        public static dynamic Insert(SupportTicketsPropertySearchParameters model)
        {
            var table = new SupportTicketsProperty();
            try
            { 
                if (model.SupportTicketsPropertyValueID != null && model.SupportTicketsPropertyValueID != 0)
                {
                    return table.Insert(new
                    {
                        SupportTicketID = model.SupportTicketID,
                        SupportMotivePropertyTypeID = model.SupportMotivePropertyTypeID,
                        SupportTicketsPropertyValueID = model.SupportTicketsPropertyValueID                        
                    });
                }
                else {
                    return table.Insert(new
                    {
                        SupportTicketID = model.SupportTicketID,
                        SupportMotivePropertyTypeID = model.SupportMotivePropertyTypeID,
                        PropertyValue = model.PropertyValue
                    });
                
                }
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
