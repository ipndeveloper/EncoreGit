using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities.Exceptions;
using System.Data.OleDb;
using System.Data;
using NetSteps.Data.Entities;
using Excel;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.IO;
using NetSteps.Web.Mvc.Helpers;
using System.Configuration;
using System.Text;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Base;

namespace nsCore.Areas.Products.Controllers
{
    public class FileController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //
        // GET: /Products/Default1/
        [HttpPost]
        public ActionResult SubmitFileAndaddAccounts()
        {
            HttpPostedFileBase file = null;
            if (Request.Files.Count > 0) file = Request.Files[0];
            if (file != null)
            {
                DataTable accountIds;
                string accountIdsNotFoundMsg = string.Empty;
                string extension = System.IO.Path.GetExtension(file.FileName);
                if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                {
                    try
                    {
                        accountIds = read(file, extension);
                        List<string> names = new List<string>();
                        DataSet ds = Account.ExistsAccount(accountIds);

                        //Cuentas que si existen
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            names.Add(Convert.ToString(row["FullName"]));
                        }

                        //Cuentas que no existen
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            string acc = string.Empty;

                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                acc += string.Format("{0},", row["AccountID"]);
                            }
                            accountIdsNotFoundMsg = string.Format(" .but the following accounts: {0} were not loaded because they do not exist.", acc);
                        }
                        return Json(new
                        {
                            result = true,
                            ids = ds.Tables[0].Rows.OfType<DataRow>().Select(dr => dr.Field<int>("AccountID")).ToList(),
                            names = names,
                            message = "The Accounts were loaded" + accountIdsNotFoundMsg
                        });
                    }
                    catch (Exception ex)
                    {
                        //var exception = EntityExceptionHelper.GetAndLogNetStepsException(new NetStepsApplicationException("There was an error reading the file"), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(new NetStepsApplicationException(ex.Message), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        return Json(new { result = false, message = exception.PublicMessage });
                    }
                }
                else
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(new NetStepsApplicationException("The type is not supported"), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            else
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(new NetStepsApplicationException("The file was not provided"), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private DataTable read(HttpPostedFileBase file, string fileExtension)
        {
            //List<int> AccountIds = new List<int>();
            IExcelDataReader reader = (fileExtension.Equals(".xlsx")) ? ExcelReaderFactory.CreateOpenXmlReader(file.InputStream) : ExcelReaderFactory.CreateBinaryReader(file.InputStream);
            reader.IsFirstRowAsColumnNames = true;
            DataSet dataset = reader.AsDataSet();
            DataTable dt = dataset.Tables[0];
            reader.Close();
            return dt;
        }

        [HttpPost]
        public ActionResult LoadImage()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var postedFile = Request.Files[i];
                        var filePath = TemporalMatrixEstensions.GetSystemConfigValue("FilePath");
                        string imagePath = Path.Combine(filePath, postedFile.FileName);

                        using (FileStream file = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                        {
                            postedFile.InputStream.CopyTo(file);
                        }
                    }
                    return Json(new { result = true, message = "Carga exitosa" });
                }
                else return Json(new { result = false, message = "No se hizo ninguna carga" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SubmitFileTemporalMatrix()
        {
            try
            {
                #region Variables

                HttpPostedFileBase file = null;
                DataTable objParameter = new DataTable();
                int PeriodID = Convert.ToInt32(Request["PeriodID"]);
                int CatalogID = Convert.ToInt32(Request["CatalogID"]);
                objParameter = null;

                #endregion

                if (Request.Files.Count > 0) file = Request.Files[0];
                if (file != null)
                {
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                    {

                        TemporalMatrix.DeleteTemporalMatrix();
                        try
                        {
                            objParameter = readTemporalMatrix(file, extension);
                            TemporalMatrixEstensions.ProcesarMatriz(objParameter, PeriodID, CatalogID, CoreContext.CurrentUser.UserID);
                            return Json(new { result = true, message = "Carga exitosa" });
                        }
                        catch (Exception ex)
                        {
                            return Json(new { result = false, message = ex.Message });
                            throw;
                        }
                    }
                    else return Json(new { result = false, message = "Formato de archivo no es el correcto" });
                }
                else return Json(new { result = false, message = "No se hizo ninguna carga" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        #region GetMatrixErrorLog

        public ActionResult GetMatrixErrorLog(int page,
                                              int pageSize,
                                              string orderBy,
                                              NetSteps.Common.Constants.SortDirection orderByDirection,
                                              string CUV,
                                              int? MaterialID)
        {
            var matrixErrorLog = ProductMatrixBusinessLogic.GetMatrixErrorLog(new ProductMatrixParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                CUV = CUV,
                MaterialID = Convert.ToInt32(MaterialID),
                LanguageID = CoreContext.CurrentLanguageID
            });

            StringBuilder builder = new StringBuilder();
            int rowCount = matrixErrorLog.Count();

            if (matrixErrorLog.Any())
            {
                foreach (var item in matrixErrorLog)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td style='width:10%'>{0}</td>", item.CUV.ToString()));
                    builder.Append(String.Format("<td style='width:10%'>{0}</td>", item.MaterialID.ToString()));
                    builder.Append(String.Format("<td style='width:10%'>{0}</td>", item.Descripcion));
                    builder.Append(String.Format("<td style='width:70%'>{0}</td>", item.Mensaje));
                    builder.Append("</tr>");
                }
                return Json(new { result = true, totalPages = matrixErrorLog.TotalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"4\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult MatrixErrorLogExport()
        {
            try
            {
                #region GetDataFromStore

                var data = ProductMatrixBusinessLogic.GetMatrixErrorLog(new ProductMatrixParameters()
                {
                    PageSize = -1,
                    CUV = string.Empty,
                    MaterialID = 0,
                    LanguageID = CoreContext.CurrentLanguageID
                });

                #endregion

                #region Set Columns

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("MatrixErrorLog", "MatrixErrorLog Export"));
                var columns = new Dictionary<string, string>
                {
                    {"CUV", Translation.GetTerm("SKU")},
                    {"MaterialID", Translation.GetTerm("Material","Material")},
                    {"Descripcion", Translation.GetTerm("Status","Status")},
                    {"Mensaje", Translation.GetTerm("Message","Message")},
                };

                #endregion
                
                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ProductMatrix>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
        #endregion

        [HttpPost]
        public ActionResult FileAccounts()
        {
            try
            {
                #region Variables

                HttpPostedFileBase file = null;
                DataTable objParameter = new DataTable();

                #endregion

                if (Request.Files.Count > 0) file = Request.Files[0];
                if (file != null)
                {
                    string extension = System.IO.Path.GetExtension(file.FileName);

                    string accountIdsNotFoundMsg = string.Empty;
                    if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                    {

                        TemporalMatrix.DeleteTemporalMatrix();
                        try
                        {
                            objParameter = readAccounts(file, extension);
                            //TemporalMatrixEstensions.ProcesarMatriz(objParameter, PeriodID, CatalogID, CoreContext.CurrentUser.UserID);
                            List<string> names = new List<string>();
                            DataSet ds = Account.ExistsAccountByAccountNumber(objParameter);

                            //Cuentas que si existen 
                            List<GetBackAccountSeachData> account = new List<GetBackAccountSeachData>();
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                GetBackAccountSeachData objE = new GetBackAccountSeachData();
                                objE.AccountNumber = Convert.ToString(row["AccountNumber"]);
                                objE.Name = Convert.ToString(row["FullName"]);
                                account.Add(objE);
                            }

                            //Cuentas que no existen
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                string acc = string.Empty;

                                foreach (DataRow row in ds.Tables[1].Rows)
                                {
                                    acc += string.Format("{0},", row["AccountNumber"]);
                                }
                                accountIdsNotFoundMsg = string.Format(" .but the following accounts: {0} were not loaded because they do not exist.", acc);
                            }
                            return Json(new
                            {
                                result = true,
                                accounts = account,
                                message = "The Accounts were loaded" + accountIdsNotFoundMsg
                            });

                            #region Comentado
                            //foreach (var item in objParameter)
                            //{
                            //    TemporalMatrixSearchParameters parameters = new TemporalMatrixSearchParameters();
                            //    parameters.ProductID = item.ProductID;
                            //    parameters.ProductName = item.ProductName;
                            //    parameters.NumDeKit = item.NumDeKit;
                            //    parameters.IsKit = item.IsKit;
                            //    parameters.ParticipationKit = item.ParticipationKit;
                            //    parameters.SKUSAP = item.SKUSAP;
                            //    parameters.TO = item.TO;
                            //    parameters.PrecioSinDto = item.PrecioSinDto;
                            //    parameters.PrecioMatriz =item.PrecioMatriz;
                            //    parameters.CodCatalog = item.CodCatalog;
                            //    parameters.Catalog = item.Catalog;
                            //    parameters.Page = item.Page;
                            //    parameters.Points = item.Points;
                            //    parameters.Type = item.Type;
                            //    int result = TemporalMatrix.InsTemporalMatrix(parameters);
                            //    if (result > 0)
                            //    {
                            //        msg = "Ingreso correcto";
                            //    }
                            //    else
                            //    {
                            //        msg = "Error";
                            //    }
                            //}
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            return Json(new { result = false, message = ex.Message });
                            throw;
                        }
                    }
                    else return Json(new { result = false, message = "Formato de archivo no es el correcto" });
                }
                else return Json(new { result = false, message = "No se hizo ninguna carga" });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        private DataTable readAccounts(HttpPostedFileBase file, string fileExtension)
        {

            List<GetBackAccountSeachData> temp = new List<GetBackAccountSeachData>();
            IExcelDataReader reader = (fileExtension.Equals(".xlsx")) ? ExcelReaderFactory.CreateOpenXmlReader(file.InputStream) : ExcelReaderFactory.CreateBinaryReader(file.InputStream);
            reader.IsFirstRowAsColumnNames = true;
            DataSet dataset = reader.AsDataSet();
            DataTable dt = dataset.Tables[0];


            reader.Close();
            //return AccountIds;
            return dt;
        }

        private DataTable readTemporalMatrix(HttpPostedFileBase file, string fileExtension)
        {
            List<TemporalMatrixSearchData> temp = new List<TemporalMatrixSearchData>();
            IExcelDataReader reader = (fileExtension.Equals(".xlsx")) ? ExcelReaderFactory.CreateOpenXmlReader(file.InputStream) : ExcelReaderFactory.CreateBinaryReader(file.InputStream);
            reader.IsFirstRowAsColumnNames = true;
            DataSet dataset = reader.AsDataSet();
            DataTable dt = dataset.Tables[0];

            #region Se valida que las filas tengan data

            IEnumerable<DataRow> query = from row in dt.AsEnumerable() where row["CAMPAÑA"] != System.DBNull.Value select row;
            DataTable result = query.CopyToDataTable<DataRow>();

            #endregion

            #region Se convierte el formato del año de ser necesario

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["MustConvertTheDate"]))
            {
                foreach (DataRow item in result.Rows)
                {
                    string PeriodID = Convert.ToString(item["CAMPAÑA"]);
                    decimal PartPerc = item[7] != DBNull.Value ? Decimal.Round(Convert.ToDecimal(item[7]), 2) : 0;

                    item["CAMPAÑA"] = string.Format("{0}{1}", PeriodID.Substring(2, 4), PeriodID.Substring(0, 2));
                    item[7] = item[7] != DBNull.Value ? PartPerc : item[7];
                }
            }

            #endregion

            reader.Close();
            //return dt;
            return result;
        }

        public virtual FileResult DownloadTemplate()
        {
            string file = Server.MapPath("~/FileUploads/Templates/Accounts.xlsx");
            string contentType = "application/vnd.ms-excel";
            return File(file, contentType, "Template-Accounts.xlsx");
        }

    }
}
