using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ReadyShipperIntegrationService
{
    public partial class NetstepsDataContext : System.Data.Linq.DataContext
    {
        public static NetstepsDataContext DB
        {
            get { return new NetstepsDataContext(ConfigurationManager.ConnectionStrings["ItWorksCoreDevConnectionString"].ConnectionString); }
        }
    }
}
