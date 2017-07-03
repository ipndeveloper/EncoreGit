using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
   public class OrderReportBusinessLogic
    {
       public static DataSet OrderSearch(string orderNumber, int LanguageID)
       {
           return OrderReportRepository.OrderSearch(orderNumber, LanguageID);
       }
    }
}
