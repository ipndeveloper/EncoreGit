using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetstepsDataAccess.DataEntities;

namespace NetSteps.AccountExport.Data
{
    public static class Repository
    {
        public static string GetAccounts()
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                List<uspIntegrationsAccountExportResult> res = db.uspIntegrationsAccountExport().ToList();
                AccountExport.AccountExportCollection col = new AccountExportCollection();
                foreach (uspIntegrationsAccountExportResult acct in res)
                {
                    Account xmlAcct = new Account();
                    xmlAcct.AccountNumber = acct.AccountNumber;
                    xmlAcct.Address = new Address
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

        private static Country GetCountryByID(int? code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    if (code == null)
                        return Country.USA; // default to USA
                    string country = (from c in db.Countries
                                      where c.CountryID == Convert.ToInt32(code)
                                      select c.CountryCode3).Single();
                    switch (country)
                    {
                        case "AUS": return Country.AUS;
                        case "BEL": return Country.BEL;
                        case "CAN": return Country.CAN;
                        case "GBR": return Country.GBR;
                        case "IRL": return Country.IRL;
                        case "NLD": return Country.NLD;
                        case "SWE": return Country.SWE;
                        case "USA": return Country.USA;
                        default: return Country.USA;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetCountryByID method, Repository class: ", ex.Message);
                    return Country.USA;
                }
            }
        }
    }
}

           
