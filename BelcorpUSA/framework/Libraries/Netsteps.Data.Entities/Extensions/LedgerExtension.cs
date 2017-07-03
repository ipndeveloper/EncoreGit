using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Extensions
{
    public class LedgerExtension
    {
        public class ProductCreditLedger
        {
            public int AccountID { get; set; }
            public string EntryDescription { get; set; }
            public int EntryReasonID { get; set; }
            public int EntryOriginID { get; set; }
            public int EntryTypeID { get; set; }
            public int UserID { get; set; } 
            public decimal EntryAmount { get; set; }
            public DateTime EntryDate { get; set; }
            public DateTime EffectiveDate { get; set; }
            public int? BonusTypeID { get; set; } 
            public int BonusValueID { get; set; }
            public int CurrencyTypeID { get; set; }
            public int OrderID { get; set; }
            public int? OrderPaymentID { get; set; }
            public string ETLNaturalKey { get; set; }
            public string ETLHash { get; set; }
            public string ETLPhase { get; set; }
            public DateTime ETLDate { get; set; }
            public int SupportTicketID { get; set; }
           
        }

        public class AccountLedger
        {
            public decimal EntryAmount { get; set; }
            public string EntryDescription { get; set; }

            public int EntryReasonID { get; set; }
            public int EntryTypeID { get; set; }
            public int? BonusTypeID { get; set; }
            public int EntryOriginID { get; set; }
            public decimal EndingBalance { get; set; }
            public DateTime EntryDate { get; set; }
            public int UserID { get; set; }
            public DateTime EffectiveDate { get; set; }
            public int CurrencyTypeID { get; set; }

        }


        public static int GetSupporTicketByID(string supportTicketNumber)
        {
            var result = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspListSupporTicketId",
                new SqlParameter("SupportTicketNumber", SqlDbType.VarChar) { Value = supportTicketNumber }                
                );
            return result;
        }

        public static List<AccountLedger> ListAccountLedger(int AccountID)
        {
            List<AccountLedger> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureListParam<AccountLedger>("Commissions", "uspListAccountLedger",
                new SqlParameter("Accountid", SqlDbType.Int) { Value = AccountID }
                ).ToList();
            return WareHouseMaterialResult;
        }
        public static int InsProductCreditLedger(ProductCreditLedger Model)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspInsProductCreditLedger",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = Model.AccountID },
                new SqlParameter("EntryDescription", SqlDbType.VarChar) { Value = Model.EntryDescription },
                new SqlParameter("EntryReasonID", SqlDbType.Int) { Value = Model.EntryReasonID },
                new SqlParameter("EntryOriginID", SqlDbType.Int) { Value = Model.EntryOriginID },
                new SqlParameter("EntryTypeID", SqlDbType.Int) { Value = Model.EntryTypeID },
                new SqlParameter("UserID", SqlDbType.Int) { Value = Model.UserID },
                new SqlParameter("EntryAmount", SqlDbType.Decimal) { Value = Model.EntryAmount },
                new SqlParameter("EntryDate", SqlDbType.DateTime) { Value = Model.EntryDate },
                new SqlParameter("EffectiveDate", SqlDbType.DateTime) { Value = Model.EffectiveDate },
                new SqlParameter("BonusTypeID", SqlDbType.Int) { Value = (object)Model.BonusTypeID ?? DBNull.Value},
                //new SqlParameter("BonusValueID", SqlDbType.Int) { Value = Model.BonusValueID },
                //new SqlParameter("CurrencyTypeID", SqlDbType.Int) { Value = Model.CurrencyTypeID },
                new SqlParameter("OrderID", SqlDbType.Int) { Value = Model.OrderID },
                new SqlParameter("OrderPaymentID", SqlDbType.Int) { Value = (object)Model.OrderPaymentID ?? DBNull.Value },
                //new SqlParameter("ETLNaturalKey", SqlDbType.VarChar) { Value = Model.ETLNaturalKey },
                //new SqlParameter("ETLHash", SqlDbType.VarChar) { Value = Model.ETLHash },
                //new SqlParameter("ETLPhase", SqlDbType.VarChar) { Value = Model.ETLPhase },
                //new SqlParameter("ETLDate", SqlDbType.DateTime) { Value = Model.ETLDate },
                new SqlParameter("SupportTicketID", SqlDbType.Int) { Value = Model.SupportTicketID } 
                );
            return result;
        }

        //public static int InsAccountLedger(ProductCreditLedger Model)
        //{
        //    var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspInsAccountLedger",
        //        new SqlParameter("AccountID", SqlDbType.Int) { Value = Model.AccountID },
        //        new SqlParameter("EntryDescription", SqlDbType.VarChar) { Value = Model.EntryDescription },
        //        new SqlParameter("EntryReasonID", SqlDbType.Int) { Value = Model.EntryReasonID },
        //        new SqlParameter("EntryOriginID", SqlDbType.Int) { Value = Model.EntryOriginID },
        //        new SqlParameter("EntryTypeID", SqlDbType.Int) { Value = Model.EntryTypeID },
        //        new SqlParameter("UserID", SqlDbType.Int) { Value = Model.UserID },
        //        new SqlParameter("EntryAmount", SqlDbType.Decimal) { Value = Model.EntryAmount },
        //        new SqlParameter("EntryDate", SqlDbType.DateTime) { Value = Model.EntryDate },
        //        new SqlParameter("EffectiveDate", SqlDbType.DateTime) { Value = Model.EffectiveDate },
        //        new SqlParameter("BonusTypeID", SqlDbType.Int) { Value = (object)Model.BonusTypeID ?? DBNull.Value },
                
        //        //new SqlParameter("OrderID", SqlDbType.Int) { Value = Model.OrderID },
        //        //new SqlParameter("OrderPaymentID", SqlDbType.Int) { Value = (object)Model.OrderPaymentID ?? DBNull.Value },

        //        //new SqlParameter("SupportTicketID", SqlDbType.Int) { Value = Model.SupportTicketID }
        //        );
        //    return result;
        //}

        public static Dictionary<string, string> GetEntryReason()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetEntryReason");
        }

        public static Dictionary<string, string> GetLedgerEntryType()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetLedgerEntryType");
        }

        public static Dictionary<string, string> GetBonusTypes()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetBonusTypes");
        }

        public static List<LedgerSupportSearchData> GetLedgerSupportTicketByID(int accountID, int orderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<LedgerSupportSearchData>("Commissions", "GetLedgerSupportTicketByID",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID },
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderID }).ToList();
        
      }
 
    }
}
