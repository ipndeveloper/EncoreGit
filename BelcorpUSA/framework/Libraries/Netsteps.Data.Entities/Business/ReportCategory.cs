using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{
    public class ReportCategory
    {
        //private ReportingService2005SoapClient _rService;
        public int ReportCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string IconUrl { get; set; }
        public string Function { get; set; }

        public List<Report> ListReports()
        {
            return Report.LoadReports(this);
        }

        //private void Authenticate()
        //{
        //    var basicHttpBinding = new WSHttpBinding();
        //    basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
        //    basicHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
        //    _rService = new ReportingService2005SoapClient(basicHttpBinding, new EndpointAddress("http://10.100.0.66/ReportServer/ReportService2005.asmx?wsdl"));
        //    _rService.ClientCredentials.HttpDigest.ClientCredential = new NetworkCredential("908590-jdobson", "W8zaZ8bR", "IAD");
        //    _rService.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Identification;
        //    _rService.ClientCredentials.Windows.ClientCredential = new NetworkCredential("908590-jdobson", "W8zaZ8bR", "IAD");
        //}
    }

    public class ReportCategoryCollection : List<ReportCategory>, ICloneable
    {
        public static ReportCategoryCollection LoadAll()    
        {
            return ReportRepository.LoadAllByCategory();
        }

        public object Clone()
        {
            var result = new ReportCategoryCollection();
            result.AddRange(this);  

            return result;
        }
    }
}
