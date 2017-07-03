using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsCore.Areas.Commissions.Models
{
    public class TRNComisiones
    {
        public string RecordType { get; set; }
        public string TranType { get; set; }
        public string AlternTransType { get; set; }
        public string USACHCompanyID { get; set; }
        public string BranchCode { get; set; }
        public string OrigiAccount { get; set; }
        public string ABARoutNumUS { get; set; }
        public string OrigiAccountCurr { get; set; }
        public string PaymeCollectIndicator { get; set; }
        public string TransHandlingCode { get; set; }
        public string PostIndicator { get; set; }
        public string ConsolRef { get; set; }
        public string PriorityIndicator { get; set; }
        public string TranRef { get; set; }
        public string RecPartyMailHandlCode { get; set; }
        public string OrderPartyName { get; set; }
        public string OrderPartyEntityID { get; set; }
        public string OrderPartyAddr1 { get; set; }
        public string OrderPartyAddr2 { get; set; }
        public string OrderPartyCityName { get; set; }
        public string OrderPartyStateCode { get; set; }
        public string OrderParZipPosCode { get; set; }
        public string OrderPartyISOCountryCode { get; set; }
        public string ReceivPartyName { get; set; }
        public int    ReceivPartyID { get; set; }
        public string ReceivPartyAddr1 { get; set; }
        public string ReceivPartyAddr2 { get; set; }
        public string ReceivPartyCityName { get; set; }
        public string ReceivPartyStateCode { get; set; }
        public string ReceivParZipPosCode { get; set; }
        public string ReceivPartyISOCountryCode { get; set; }
        public string EffecEntryDate { get; set; }
        public string TransDesc { get; set; }
        public string TransCurrCode { get; set; }
        public decimal TransAmount { get; set; }
        public string ChargesIndicator { get; set; }
        public string CheckNumber { get; set; }
        public string ReceivBankName { get; set; }
        public string ReceivBankAccountypeAndReceivBankIDtype { get; set; }
        public string ReceivBankIDSortCode	{ get; set; }
        public string ReceivBankSWIFTAddr { get; set; }
        public string ReceivBankAccountNumber { get; set; }
        public string ReceivBankBranchNum { get; set; }
        public string ReceivBankCityName { get; set; }
        public string ReceivBankISOCountryCode { get; set; }

    }
}