using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace NetSteps.OrderAdjustments.Service
{
    public class OrderAdjustmentsConnection
    {
        public SqlConnection Cnx { get; private set; } 
        public OrderAdjustmentsConnection()
        {
            Cnx = new SqlConnection("Data Source=10.12.6.187;Initial Catalog=BelcorpBRACore;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usrencorebrasilqas;pwd=Belcorp2016%");              
        }
    }
}
