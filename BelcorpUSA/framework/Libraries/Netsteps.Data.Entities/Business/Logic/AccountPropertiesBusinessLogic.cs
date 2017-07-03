using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class AccountPropertiesBusinessLogic
    {

        public dynamic GetCreditRequirementsByAccount(int AccountID)
        {
            //davy
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetAdditionalInformationByAccount @0", new object[] { AccountID });
            return List.FirstOrDefault();
        }
        public static dynamic GetEmailTemplate(int TicketNumbers, string UbicacionPDFGenerado)
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            try
            {
                List = table.Query("EXEC SpConfPaymentTicketBank @0 ,  @1 ", new object[] { UbicacionPDFGenerado }, new object[] { TicketNumbers });
            return List.FirstOrDefault();
            }
            catch
            {
                List = table.Query("EXEC SpConfPaymentTicketBank  '" + UbicacionPDFGenerado + "'," + TicketNumbers.ToString() );
                return List.FirstOrDefault(); 
            }
        }
      
        public static dynamic GetValueByID(int secction, int ID)
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            try
            {
                List = table.Query("EXEC uspGetValuesByID  @0 , @1", new object[] { secction }, new object[] { ID });
                return List.FirstOrDefault();
            }
            catch {
                List = table.Query("EXEC uspGetValuesByID  " + secction.ToString() + "," + ID.ToString());
                return List.FirstOrDefault();
            }
           
        }

        public static Dictionary<int, string> GetRelationShip()
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetRelationShip");

            Dictionary<int, string> listFin = new Dictionary<int, string>(); 
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID,item.Value);
            }
            return listFin;           
        }

        public static List<CreditPaymentTable> GetGetAccountCredits()
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetAccountCredits");

            List<CreditPaymentTable> listFin = new List<CreditPaymentTable>();
            foreach (var item in List.ToList())
            {

                listFin.Add(new CreditPaymentTable()
                {
                    AccountID = item.AccountID,
                    AccountCreditAsg = item.AccountCreditAsg,
                    AccountCreditUti = item.AccountCreditUti,
                    AccountCreditAnt = item.AccountCreditAnt,
                    AccountCreditDis = item.AccountCreditDis,
                    AccountCreditEst = item.AccountCreditEst,
                    AccountCreditFec = item.AccountCreditFec
                });
            }
            return listFin;
        }
       
        public static Dictionary<int, string> GetAccountPropertiesValuesByTermName(string TermName)
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetAccountPropertiesValuesByTermName @0", new object[] { TermName });
            Dictionary<int, string> listFin = new Dictionary<int, string>();
            listFin.Add(0, NetSteps.Common.Globalization.Translation.GetTerm("Select", "Select"));
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID, item.Value);
            }
            return listFin;
        }

        public static Dictionary<int, string> GetFineBaseAmounts()
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC spListFineBaseAmounts");

            Dictionary<int, string> listFin = new Dictionary<int, string>();
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID, item.Value);
            }
            return listFin;
        }

        public static Dictionary<int, string> GetSupportTicketPriority()
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetSupportTicketPriorities");

            Dictionary<int, string> listFin = new Dictionary<int, string>();
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID, item.Value);
            }
            return listFin;
        }

        public static Dictionary<int, string> GetSupportTicketStatus()
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetSupportTicketStatuses");

            Dictionary<int, string> listFin = new Dictionary<int, string>();
            foreach (var item in List.ToList())
            {
                listFin.Add(item.ID, item.Value);
            }
            return listFin;
        }

        public dynamic Insert(AccountPropertiesParameters model)
        {
            var table = new AccountProperties();

            try
            {
                if (model.AccountPropertyValueID>0){
                return table.Insert(new
                {
                    AccountID = model.AccountID ,
                    AccountPropertyTypeID = model.AccountPropertyTypeID,
                   
                    AccountPropertyValueID = model.AccountPropertyValueID, 
                    Active = model.Active,                   
                });
                }
                else{
                 return table.Insert(new
                {
                    AccountID = model.AccountID ,
                    AccountPropertyTypeID = model.AccountPropertyTypeID,                   
                    PropertyValue = model.PropertyValue,
                    Active = model.Active,                   
                });
                }

            }
            catch
            {
                return null;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new AccountProperties();

            try
            {
                if (model.AccountPropertyValueID > 0)
                {
                    table.Update(new
                    {
                        AccountID = model.AccountID,
                        AccountPropertyTypeID = model.AccountPropertyTypeID,
                        AccountPropertyID = model.AccountPropertyID,
                        AccountPropertyValueID = model.AccountPropertyValueID,                  
                        Active = model.Active,
                    },
                    model.AccountPropertyID
                    );

                }
                else {
                    table.Update(new
                    {
                        AccountID = model.AccountID,
                        AccountPropertyTypeID = model.AccountPropertyTypeID,
                        AccountPropertyID = model.AccountPropertyID,
                        PropertyValue = model.PropertyValue,
                        Active = model.Active,
                    },
                       model.AccountPropertyID
                       );
                }


                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<dynamic> GetListByID(int secction, int ID)
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;

            List = table.Query("EXEC uspGetListByID  " + secction.ToString() + "," + ID.ToString());
            return List;
            //try
            //{
            //    List = table.Query("EXEC uspGetListByID  @0 , @1", new object[] { secction }, new object[] { ID }).;
            //    return List;
            //}
            //catch
            //{
            //    List = table.Query("EXEC uspGetListByID  " + secction.ToString() + "," + ID.ToString());
            //    return List;
            //}

        }

        public string DeleteAccountPropertiesByAccountID(int AccountID)
        {
            try
            {
                AccountPropertiesRepository repository = new AccountPropertiesRepository();
                string resultado = repository.DeleteAccountPropertiesByAccountID(AccountID);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
