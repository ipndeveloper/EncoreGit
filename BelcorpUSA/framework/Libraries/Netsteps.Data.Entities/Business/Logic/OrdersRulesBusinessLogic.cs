using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf.qrcode;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrdersRulesBusinessLogic
    {

        public bool HasOrderRules()
        {
            return GetAllOrdersRules().Any();
        }

        public IEnumerable<dynamic> GetAllOrdersRules()
        {
            var table = new OrdersRules();
            var lista = table.All();

            return lista;
        }

        public int Insert(string name, string termName)
        {
            var table = new OrdersRules();

            try
            {
                var newID = table.Insert(new
                {
                    Name = name,
                    TermName = termName
                });
                return (int)((dynamic)((System.Dynamic.ExpandoObject)(newID))).ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}