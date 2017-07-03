using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using nsCore.Areas.Commissions.Models;
using NetSteps.Web.Mvc.ActionResults;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace nsCore.Areas.Commissions.Controllers
{
    public class PaymentsController : Controller
    {
        //
        // GET: /Commissions/Payments/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexCharge()
        {
            var periods = DataAccess.ExecWithStoreProcedureLists <BonusValuesModel>("Commissions", "uspGetBonusValues").ToList();

            List<BonusValuesModel> periodsReturn = new List<BonusValuesModel>();

            foreach (var itemsin in periods)
            {
                periodsReturn.Add(new BonusValuesModel() { PeriodID = itemsin.PeriodID });
            }

            return View(periodsReturn);
        }

        public ActionResult GenerateCSV(string Period)
        {
            try
            {
                

                int iPeriod = Int32.Parse(Period);
                decimal totalAmount = 0;
                int count = 0;
                
                //Carga de la Cabecera
                string head = "";
                string csvFileType = System.Configuration.ConfigurationManager.AppSettings["CSVFileType"].ToString();
                string csvCompanyName = System.Configuration.ConfigurationManager.AppSettings["CSVCompanyName"].ToString();
                string csvCompanyID = System.Configuration.ConfigurationManager.AppSettings["CSVCompanyID"].ToString();
                string csvCompanyEFTKey = System.Configuration.ConfigurationManager.AppSettings["CSVCompanyEFTKey"].ToString();
                string creationDate = DateTime.Now.ToString("yyyyMMdd");
                string fileName = "PAYMENTS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string csvIndicator = System.Configuration.ConfigurationManager.AppSettings["CSVIndicator"].ToString();

                head = csvFileType + "," + csvCompanyName + "," + csvCompanyID + "," + csvCompanyEFTKey + "," + creationDate + "," + fileName + "," + csvIndicator;

                //Carga de datos Transaccionales 
                var trnComisiones = DataAccess.ExecWithStoreProcedureListParam<TRNComisiones>("Commissions", "uspGetTRNComisiones_CSV",
                                    new SqlParameter("PeriodID", SqlDbType.Int) { Value = iPeriod }).ToList();

                count = trnComisiones.Count();


                var csvExport = new CsvExport(",", true, head, "");

                foreach (var item in trnComisiones)
                {
                    csvExport.AddRow();
                    csvExport["RecordType"] = item.RecordType;
                    csvExport["TranType"] = item.TranType;
                    csvExport["AlternTransType"] = item.AlternTransType;
                    csvExport["USACHCompanyID"] = item.USACHCompanyID;
                    csvExport["BranchCode"] = item.BranchCode;
                    csvExport["OrigiAccount"] = item.OrigiAccount;
                    csvExport["ABARoutNumUS"] = item.ABARoutNumUS;
                    csvExport["OrigiAccountCurr"] = item.OrigiAccountCurr;
                    csvExport["PaymeCollectIndicator"] = item.PaymeCollectIndicator;
                    csvExport["TransHandlingCode"] = item.TransHandlingCode;
                    csvExport["PostIndicator"] = item.PostIndicator;
                    csvExport["ConsolRef"] = item.ConsolRef;
                    csvExport["PriorityIndicator"] = item.PriorityIndicator;
                    csvExport["TranRef"] = item.TranRef;
                    csvExport["RecPartyMailHandlCode"] = item.RecPartyMailHandlCode; 
                    csvExport["OrderPartyName"] = item.OrderPartyName;
                    csvExport["OrderPartyEntityID"] = item.OrderPartyEntityID;
                    csvExport["OrderPartyAddr1"] = item.OrderPartyAddr1;
                    csvExport["OrderPartyAddr2"] = item.OrderPartyAddr2;
                    csvExport["OrderPartyCityName"] = item.OrderPartyCityName;
                    csvExport["OrderPartyStateCode"] = item.OrderPartyStateCode;
                    csvExport["OrderParZipPosCode"] = item.OrderParZipPosCode;
                    csvExport["OrderPartyISOCountryCode"] = item.OrderPartyISOCountryCode;
                    csvExport["ReceivPartyName"] = item.ReceivPartyName;
                    csvExport["ReceivPartyID"] = item.ReceivPartyID;
                    csvExport["ReceivPartyAddr1"] = item.ReceivPartyAddr1;
                    csvExport["ReceivPartyAddr2"] = item.ReceivPartyAddr2;
                    csvExport["ReceivPartyCityName"] = item.ReceivPartyCityName;
                    csvExport["ReceivPartyStateCode"] = item.ReceivPartyStateCode;
                    csvExport["ReceivParZipPosCode"] = item.ReceivParZipPosCode;
                    csvExport["ReceivPartyISOCountryCode"] = item.OrderPartyISOCountryCode;
                    csvExport["EffecEntryDate"] = item.EffecEntryDate;
                    csvExport["TransDesc"] = item.TransDesc;
                    csvExport["TransCurrCode"] = item.TransCurrCode;
                    csvExport["TransAmount"] = item.TransAmount.ToString("0.00", CultureInfo.InvariantCulture);
                    totalAmount += item.TransAmount;
                    csvExport["ChargesIndicator"] = item.ChargesIndicator;
                    csvExport["CheckNumber"] = item.CheckNumber;
                    csvExport["ReceivBankName"] = item.ReceivBankName;
                    csvExport["ReceivBankAccountypeAndReceivBankIDtype"] = item.ReceivBankAccountypeAndReceivBankIDtype; 
                    csvExport["ReceivBankIDSortCode"]= item.ReceivBankIDSortCode;
                    csvExport["ReceivBankSWIFTAddr"] = item.ReceivBankSWIFTAddr;
                    csvExport["ReceivBankAccountNumber"] = item.ReceivBankAccountNumber;
                    csvExport["ReceivBankBranchNum"] = item.ReceivBankBranchNum;
                    csvExport["ReceivBankCityName"] = item.ReceivBankCityName;
                    csvExport["ReceivBankISOCountryCode"] = item.ReceivBankISOCountryCode;

                    
                }

                //Creación de Footer CSV
                string csvRecordType = System.Configuration.ConfigurationManager.AppSettings["CSVRecordType"].ToString();
                string foot = csvRecordType + "," + count + "," + totalAmount.ToString("0", CultureInfo.InvariantCulture);
                csvExport.footCSV = foot;

                return File(csvExport.ExportToBytes(), "text/csv", fileName+".csv");

            }
            catch (Exception ex)
           {
                //var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false });//, message = exception.PublicMessage });
            }

            
        }

    }
}
