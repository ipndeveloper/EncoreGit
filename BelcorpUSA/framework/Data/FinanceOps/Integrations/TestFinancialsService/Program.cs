using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Financials;

namespace NetSteps.Financials
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FinancialsProxy prox = new FinancialsProxy();
            string s = prox.GetGrossRevenue("miche5779", "98jklrgeasg", DateTime.Now.AddDays(-7), DateTime.Now, "USA");
            string ss = prox.GetShippedRevenue("miche5779", "98jklrgeasg", DateTime.Now.AddDays(-7), DateTime.Now, "USA");
        }
    }
}
