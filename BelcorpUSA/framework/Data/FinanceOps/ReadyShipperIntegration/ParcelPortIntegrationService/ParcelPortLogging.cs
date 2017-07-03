using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParcelPortIntegrationService
{
    static class ParcelPortLogging
    {
        public static void LogCommunication(ParcelPortDataContext dc, String Message)
        {
            LogisticsCommunication com = new LogisticsCommunication
            {
                Message = Message,
                CreatedOn = DateTime.Now.ToUniversalTime(),
                UpdatedOn = DateTime.Now.ToUniversalTime()
            };

            dc.LogisticsCommunications.InsertOnSubmit(com);

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {

            }
        }
    }
}
