using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;

namespace NetSteps.Base.Integrations.Data
{
    public static class Repository
    {
        public static string GetOrders()
        {
            List<uspIntegrationsOrderExportResult> list = null;
            using (NetStepsEntities db = new NetStepsEntities())
                list = db.uspIntegrationsOrderExport().ToList();
            if (list.Count > 0)
            {
                foreach (uspIntegrationsOrderExportResult res in list)
                {

                }
            }
            return "";
        }

        public static string GetAccounts()
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                List<uspIntegrationsAccountExportResult> res = db.uspIntegrationsAccountExport().ToList();
                AccountExport.AccountExportCollection col = new AccountExport.AccountExportCollection();
                foreach (uspIntegrationsAccountExportResult acct in res)
                {
                    AccountExport.Account xmlAcct = new AccountExport.Account();
                    xmlAcct.AccountNumber = acct.AccountNumber;
                    xmlAcct.Address = new AccountExport.Address
                    {
                        Address1 = acct.Address1,
                        Address2 = acct.Address2,
                        City = acct.City,
                        CountryISOCode = GetCountryByID(acct.CountryID),
                        Email = acct.EmailAddress,
                        FirstName = acct.FirstName,
                        LastName = acct.LastName,
                        Phone = acct.PhoneNumber,
                        State = acct.State,
                        Zip = acct.PostalCode
                    };
                    col.Account.Add(xmlAcct);
                }
                return col.Serialize();
            }

        }

        private static AccountExport.Country GetCountryByID(int? code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    if (code == null)
                        return AccountExport.Country.USA; // default to USA
                    string country = (from c in db.Countries
                                      where c.CountryID == Convert.ToInt32(code)
                                      select c.CountryCode3).Single();
                    switch (country)
                    {
                        case "AUS": return AccountExport.Country.AUS;
                        case "BEL": return AccountExport.Country.BEL;
                        case "CAN": return AccountExport.Country.CAN;
                        case "GBR": return AccountExport.Country.GBR;
                        case "IRL": return AccountExport.Country.IRL;
                        case "NLD": return AccountExport.Country.NLD;
                        case "SWE": return AccountExport.Country.SWE;
                        case "USA": return AccountExport.Country.USA;
                        default: return AccountExport.Country.USA;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetCountryByID method, Repository class: ", ex.Message);
                    return AccountExport.Country.USA;
                }
            }
        }
    }
}
