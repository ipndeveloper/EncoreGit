using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Excel;
using NetSteps.Common.Extensions;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using nsCore.Areas.GeneralLedger.Models;
using nsCore.Controllers;
using System.IO;

namespace nsCore.Areas.GeneralLedger.Controllers
{
    public class ArrearsDefaultsReportsController : BaseController
    {
        
        #region Views

        public ActionResult SpcReport()
        {
            var ddl = new Dictionary<int, string>();
            ddl.Add(1, "Regular Process");
            ddl.Add(2, "Alternative Process");
            ViewBag.ProcessTypes = ddl;

            return View();
        }

        public ActionResult SpcCreatedReport()
        {
            Dictionary<string, string> dates = GetCreationDates();
            ViewBag.CreationDates = dates;

            return View();
        }

        #endregion

        #region Actions

        public ActionResult Report(int TypeProcess, string FileSequentialCode)
        {
            string message = String.Empty;

            OverduePaymentReport report = OverduePaymentBusinessLogic.Instance.ExecuteProcess(TypeProcess, FileSequentialCode, ref message);
            if (!String.IsNullOrEmpty(message.Trim()))
                return Json(new { success = false, message = message }, JsonRequestBehavior.AllowGet);
            //
            string prefix = System.Configuration.ConfigurationManager.AppSettings["FilePrefix"].Trim();
            string fileName = String.Concat(prefix, FileSequentialCode);
            SaveReportFile(fileName, report);
            //
            return Json(new { success = true, message = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadTextFile(string fileName)
        {
            string file = Path.Combine(Server.MapPath(@"~/FileUploads/OverduePayments/SPCReports"),
                                                                        String.Concat(fileName, ".txt"));
            string contentType = "text/plain";
            //
            return File(file, contentType, Server.UrlEncode(file));
        }

        public ActionResult ListFilePaginate(Constants.SortDirection orderByDirection, int page = 0, int pageSize = 0, string orderBy = "",
                                                    string creationDate = "", string fileId = "")
        {
            int totalPages = 0;
            StringBuilder builder = new StringBuilder();
            List<FileClass> files = GetFilesFromDirectory(page, pageSize, orderBy, orderByDirection,
                                                                    creationDate, fileId, ref totalPages);

            foreach (FileClass file in files)
            {
                builder.Append("<tr>")
                    .AppendCell(file.CreationDate)
                    .AppendLinkCell("~/GeneralLedger/ArrearsDefaultsReports/DownloadTextFile?FileName=" + file.FileName, file.FileName)
                    .Append("</tr>");
            }

            return Json(new { totalPages = 1, page = builder.ToString() });
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetFileNames(string query)
        {
            try
            {
                query = query.ToCleanString();
                var searchResults = GetFileNames().Where(f => f.Value.ContainsIgnoreCase(query));

                return Json(searchResults.Select(p => new { id = p.Key, text = p.Value }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        public ActionResult UploadExcelData()
        {
            HttpPostedFileBase file = null;
            if (Request.Files.Count > 0) file = Request.Files[0];
            if (file != null)
            {
                List<int> accountNumbers;
                string extension = System.IO.Path.GetExtension(file.FileName);
                if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                {
                    try
                    {
                        accountNumbers = read(file, extension);
                        bool answer = OverduePaymentBusinessLogic.Instance.LoadOverdueErrors(accountNumbers);

                        if (!answer)
                            return Json(new { result = false, ids = accountNumbers, message = "There was an error reading the file" });

                        return Json(new { result = true, ids = accountNumbers, message = "The Accounts were loaded" });
                    }
                    catch
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(new NetStepsApplicationException("There was an error reading the file"), NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
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

        #endregion

        #region Private Methods

        private void SaveReportFile(string fileName, OverduePaymentReport report)
        {
            string path = Path.Combine(Server.MapPath(@"~/FileUploads/OverduePayments/SPCReports"), String.Concat(fileName, ".txt"));

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(report.HeaderToString());
                sw.WriteLine(String.Empty);
                sw.WriteLine(String.Empty);
                if (report.OverduePaymentDetails != null && report.OverduePaymentDetails.Count() > 0)
                {
                    foreach (OverduePaymentDetail detail in report.OverduePaymentDetails)
                    {
                        sw.WriteLine(detail.BodyDetailToString());
                        if (detail.OverduePaymentOcurrences != null && detail.OverduePaymentOcurrences.Count() > 0)
                        {
                            foreach (OverduePaymentOccurrence occur in detail.OverduePaymentOcurrences)
                            {
                                sw.WriteLine(occur.BodyOccurrenceToString());
                            }
                        }
                        sw.WriteLine(String.Empty);
                    }

                    sw.WriteLine(String.Empty);
                }
                sw.Write(report.FooterToString());
            }
        }

        private List<int> read(HttpPostedFileBase file, string fileExtension)
        {
            List<int> accountNumbers = new List<int>();
            IExcelDataReader reader = (fileExtension.Equals(".xlsx")) ? ExcelReaderFactory.CreateOpenXmlReader(file.InputStream) : ExcelReaderFactory.CreateBinaryReader(file.InputStream);
            reader.IsFirstRowAsColumnNames = true;
            DataSet dataset = reader.AsDataSet();
            DataTable dt = dataset.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    accountNumbers.Add(int.Parse(dt.Rows[i]["CÓDIGO"].ToString()));
                }
                catch { }
            }

            reader.Close();
            return accountNumbers.Distinct().ToList();

        }

        private List<FileClass> GetFilesFromDirectory(int page, int pageSize, string orderBy, Constants.SortDirection orderDirection,
                                                            string creationDate, string fileName, ref int totalPages)
        {
            List<FileClass> files = new List<FileClass>();
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/FileUploads/OverduePayments/SPCReports"));
            IEnumerable<FileInfo> list = directory.GetFiles().ToList();

            list = list.Where(f => (creationDate.Equals("") || f.CreationTime.ToShortDateString().Equals(creationDate)) &&
                                    (fileName.Equals("") || f.Name.Contains(fileName)));

            switch (orderBy)
            {
                case "CreationDate":
                    list = list.OrderBy(f => f.CreationTime); break;
                case "FileName":
                    list = list.OrderBy(f => f.Name); break;
                default:
                    break;
            }

            if (orderDirection == Constants.SortDirection.Descending)
                list.Reverse();

            list = list.Skip(pageSize * page).Take(pageSize).ToList();


            totalPages = (int)Math.Ceiling((double)list.Count() / pageSize);

            FileClass fileClass = null;
            foreach (var file in list)
            {
                fileClass = new FileClass();

                fileClass.CreationDate = file.CreationTime.ToString("dd/MM/yyyy");
                fileClass.FileName = file.Name.Split('.')[0];

                files.Add(fileClass);
            }

            return files;
        }

        private Dictionary<string, string> GetCreationDates()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("", "-- Seleccione --");

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/FileUploads/OverduePayments/SPCReports"));
            var list = directory.GetFiles().Select(f => f.CreationTime.ToShortDateString()).Distinct().ToList();

            foreach (var dt in list)
            {
                dictionary.Add(dt, dt);
            }

            return dictionary;
        }

        private Dictionary<string, string> GetFileNames()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/FileUploads/OverduePayments/SPCReports"));
            var list = directory.GetFiles().Select(f => f.Name).Distinct().ToList();

            foreach (var dt in list)
            {
                dictionary.Add(dt.Split('.')[0], dt.Split('.')[0]);
            }

            return dictionary;
        }

        #endregion

    }
}
