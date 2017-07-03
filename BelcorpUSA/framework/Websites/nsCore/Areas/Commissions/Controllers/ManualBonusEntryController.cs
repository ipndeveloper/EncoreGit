using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.Extensions;
using Newtonsoft.Json;
using System.Text;
using NetSteps.Web.Extensions;
using nsCore.Areas.Commissions.Models;
using System.Web;
using OfficeOpenXml;
using System.Data;
using System.IO;

namespace nsCore.Areas.Commissions.Controllers
{
    public class ManualBonusEntryController : BaseCommissionController
    {

        public virtual ActionResult Index()
        {
            try
            {
                var model = new ManualBonusEntryModel();
                model.OpenPeriod = Periods.GetOpenPeriodID();

                if (TempData["ManualBonus"] != null)
                {
                    var manualBonus = JsonConvert.DeserializeObject<ManualBonusEntryModel>((string) TempData["ManualBonus"]);

                    model.Errors = manualBonus.Errors;
                    model.IsValid = manualBonus.Errors.Where(x => x.Period && x.BonusType && x.Account && x.BonusAmount && x.Duplicated == false).Count() == model.Errors.Count() && manualBonus.Errors.Count > 0 ? true : false; 
                    model.Message = manualBonus.Message;

                    TempData["ManualBonus"] = JsonConvert.SerializeObject(manualBonus);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [HttpPost]
        public ActionResult Proccess(HttpPostedFileBase file)
        {
            var manualBonus = new ManualBonusEntryModel();

            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        // get the first worksheet in the workbook
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        var start = workSheet.Dimension.Start;
                        var end = workSheet.Dimension.End;

                        DataTable validationTable = GenerateValidationTable();
                        manualBonus.Errors = new List<ManualBonusEntrySearchData>();

                        for (int row = start.Row + 1; row <= end.Row; row++)
                        {
                            List<string> objRow = new List<string>();

                            for (int col = start.Column; col <= end.Column; col++)
                            {
                                objRow.Add(workSheet.Cells[row, col].Text);
                            }

                            bool validateFields = false;

                            foreach(var field in objRow){
                                if (!string.IsNullOrEmpty(field))
                                {
                                    validateFields = true;
                                    break;
                                }
                            }

                            //skips when there is nothing to validate
                            if (validateFields)
                            {
                                //Basic Validation per Row
                                var Error = ValidateRow(row, objRow);

                                manualBonus.Errors.Add(Error);

                                if (Error.Period && Error.BonusType && Error.Account && Error.BonusAmount)
                                {
                                    validationTable.Rows.Add(row, Error.PeriodID, Error.BonusTypeID, Error.AccountID, Error.BonusAmountVal.ToString().Replace(',', '.'));
                                }
                            }
                        }

                        if (validationTable.Rows.Count > 0)
                        {
                            List<ManualBonusEntrySearchData> DataBaseErrors = ManualBonusEntryBusinessLogic.Instance.ManualBonusEntryValidation(validationTable);

                            foreach (var dbError in DataBaseErrors)
                            {
                                int errorIndex = manualBonus.Errors.FindIndex(x => x.RowNumber == dbError.RowNumber);
                                manualBonus.Errors[errorIndex].RowNumber = dbError.RowNumber;
                                manualBonus.Errors[errorIndex].Period = dbError.Period;
                                manualBonus.Errors[errorIndex].BonusType = dbError.BonusType;
                                manualBonus.Errors[errorIndex].Account = dbError.Account;
                                manualBonus.Errors[errorIndex].BonusAmount = dbError.BonusAmount;
                                manualBonus.Errors[errorIndex].Duplicated = dbError.Duplicated;
                            }
                        }
                    }
                }
                else
                {
                    manualBonus.Message = Translation.GetTerm("SelectFile", "Select a File");
                }

            }
            catch (Exception ex)
            {
                TempData["ManualBonus"] = null;
                manualBonus.Message = Translation.GetTerm("FileProccessError", "An error ocurred while processing the file") + ": " + ex.Message;
            }

            TempData["ManualBonus"] = JsonConvert.SerializeObject(manualBonus);

            return RedirectToAction("Index");
        }

        public ActionResult Load()
        {
            bool result = false;
            string message = string.Empty;

            try
            {
                if (TempData["ManualBonus"] != null)
                {
                    var manualBonus = JsonConvert.DeserializeObject<ManualBonusEntryModel>((string)TempData["ManualBonus"]);
                    DataTable loadTable = GenerateValidationTable();

                    foreach (var entry in manualBonus.Errors)
                    {
                        loadTable.Rows.Add(entry.RowNumber, entry.PeriodID, entry.BonusTypeID, entry.AccountID, entry.BonusAmountVal.ToString().Replace(',', '.'));
                    }

                    Tuple<int, string> loadResult = ManualBonusEntryBusinessLogic.Instance.ManualBonusEntryLoad(loadTable);

                    if (loadResult.Item1 == 1)
                    {
                        result = true;
                        message = Translation.GetTerm("ProccessEnd", "The proccess has successfully ended.");
                    }
                    else
                    {
                        message = Translation.GetTerm("ProccessError", "An error occurred while proccessing the bonus.") + ": " + loadResult.Item2;
                    }
                }
                else
                {
                    throw new Exception(Translation.GetTerm("TempDataError", "Could not find TempData"));
                }
            }
            catch (Exception ex)
            {
                TempData["ManualBonus"] = null;
                message = Translation.GetTerm("FileLoadError", "An error ocurred while loading the bonus") + ": " + ex.Message;
            }

            return Json(new { result = result, message = message });
        }

        private ManualBonusEntrySearchData ValidateRow(int RowNumber, List<string> Row)
        {
            ManualBonusEntrySearchData Error = new ManualBonusEntrySearchData();

            int PeriodID = 0;
            int BonusTypeID = 0;
            int AccountID = 0;
            decimal BonusAmountVal = 0M;

            Error.Period = int.TryParse(Row[0], out PeriodID) && PeriodID > 0 ? true : false;
            Error.BonusType = int.TryParse(Row[1], out BonusTypeID) && BonusTypeID > 0 ? true : false;
            Error.Account = int.TryParse(Row[3], out AccountID) && AccountID > 0 ? true : false;
            Error.BonusAmount = decimal.TryParse(Row[5], out BonusAmountVal) && BonusAmountVal > 0 ? true : false;

            Error.RowNumber = RowNumber;
            Error.PeriodID = PeriodID;
            Error.BonusTypeID = BonusTypeID;
            Error.AccountID = AccountID;
            Error.BonusAmountVal = BonusAmountVal;

            return Error;
        }

        private DataTable GenerateValidationTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("RowNumber");
            table.Columns.Add("PeriodID");
            table.Columns.Add("BonusTypeID");
            table.Columns.Add("AccountID");
            table.Columns.Add("BonusAmount");

            return table;
        }

        public ActionResult DownloadTemplate()
        {
            string path = Server.MapPath("~/FileUploads/ExcelTemplates/ManualBonusEntryTemplate.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = "ManualBonusEntryTemplate.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
