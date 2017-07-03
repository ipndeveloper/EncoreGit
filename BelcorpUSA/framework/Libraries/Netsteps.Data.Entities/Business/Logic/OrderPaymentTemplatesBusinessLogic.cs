using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderPaymentTemplatesBusinessLogic
    {

        public static OrderPaymentTemplatesSearchParameters Edit(string id)
        {

            var table = new OrderPaymentTemplates();
            
                try
                {
                    var all = table.All();
                    var reg=all.Where(e => e.OrderPaymentTemplateId == int.Parse(id)).FirstOrDefault();
                    OrderPaymentTemplatesSearchParameters ret =  new OrderPaymentTemplatesSearchParameters {
                        OrderPaymentTemplateId = reg.OrderPaymentTemplateId,
                        Description = reg.Description,
                        Days = reg.Days,
                        MinimalAmount = reg.MinimalAmount,
                        EmailTemplateNameId = reg.EmailTemplateNameId,
                        Type = reg.Type
                    };

                    return ret;
                }
                catch
                {
                    return new OrderPaymentTemplatesSearchParameters();
                }
           
        }
        
    }
}
