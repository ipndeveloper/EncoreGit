using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration; /*CGI(JCT) MLM - 010*/

namespace NetSteps.Commissions.Service
{
    public class CommissionsConstants
    {
        public class ConnectionStringNames
        {
            public static string KnownFactorsDataWarehouse = "KnownFactorsDataWarehouse";
            public static string CommissionsPrep = "CommissionsPrep";
            public static string Commissions = NetSteps.Foundation.Common.ConnectionStringNames.Commissions;
        }

        /*CGI(JCT) - Inicio MLM - 010*/
        public class ConnectionGetEnvironment
        {
            public static int EnvironmentCountry = Convert.ToInt32(ConfigurationManager.AppSettings["EnvironmentCountry"]);
        }

        public enum EnvironmentList
        {
            USA = 1,
            Brazil = 73
        }

        public static string GetObjectNameParsed(CommissionsConstants.EnvironmentList pEnvironment, string pObjectName, string pSchema)
        {
            string ResultName;

            ResultName = pObjectName;

            //switch (pEnvironment)
            //{
            //    case CommissionsConstants.EnvironmentList.USA:
            //        ResultName = pSchema + "." + pObjectName;
            //        break;
            //    case CommissionsConstants.EnvironmentList.Brazil:
            //        ResultName = pObjectName;
            //        break;
            //    default:
            //        ResultName = pSchema + "." + pObjectName;
            //        break;
            //}
            return ResultName;
        }
        /*CGI(JCT) - Fin MLM - 010*/
    }
}
