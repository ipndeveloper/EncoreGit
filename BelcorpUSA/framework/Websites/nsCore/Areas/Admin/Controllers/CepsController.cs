using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using nsCore.Controllers;
using nsCore.Areas.Admin.Models;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;

namespace nsCore.Areas.Admin.Controllers
{
    public class CepsController : BaseController
    {
        bool success = false;

        public virtual ActionResult Index()
        {

            return View();
        }

        public virtual ActionResult Register(TaxCacheSearchData objTaxt)
        {
            try
            {
                TaxCache.MovementsTaxCache(objTaxt);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                if (ex.Message.Contains("UNIQUE KEY 'UQ__TaxCache"))
                    return Json(new { result = false, message = Translation.GetTerm("RepeatedPostalCode", "Repeated Postal Code") });
                else
                    return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        public virtual ActionResult ListCombosTax(TaxCacheSearchData objTaxt)
        {
            List<TaxCacheSearchData> lenTaxCacheSearchData = new List<TaxCacheSearchData>();
            StringBuilder strResultado = new StringBuilder();
            strResultado.Append(ArmaComboOpcion("", "[SELECCIONE]"));

            lenTaxCacheSearchData = TaxCacheRepository.dllTaxCache(objTaxt);

            if (objTaxt.CountyDefault == true)
            {
                foreach (TaxCacheSearchData TaxCache in lenTaxCacheSearchData)
                {
                    strResultado.Append(ArmaComboOpcion(TaxCache.City.ToString(), TaxCache.City.ToString()));
                }

            }
            else
            {
                foreach (TaxCacheSearchData TaxCache in lenTaxCacheSearchData)
                {
                    strResultado.Append(ArmaComboOpcion(TaxCache.County.ToString(), TaxCache.County.ToString()));
                }
            }

            //return Json(new { result = true, listado = strResultado.ToString() , j});
            var serializer = new JavaScriptSerializer();
            // For simplicity just use Int32's max value. 
            // You could always read the value from the config section mentioned above. 
            serializer.MaxJsonLength = Int32.MaxValue;
            var resultData = new { result = true, listado = strResultado.ToString() };
            var result = new ContentResult { Content = serializer.Serialize(resultData), ContentType = "application/json" };
            return result;


        }

        public virtual ActionResult ListCombosState(TaxCacheSearchData objTaxt)
        {
            StringBuilder strResultado = new StringBuilder();
            strResultado.Append(ArmaComboOpcion("", "[SELECCIONE]"));
            List<StateProvince> lenProvince = new List<StateProvince>();
            lenProvince = TaxCache.ListState(objTaxt);

            foreach (StateProvince Province in lenProvince)
            {
                strResultado.Append(ArmaComboOpcion(Province.StateAbbreviation.ToString(), Province.Name.ToString()));
            }
            return Json(new { result = true, listado = strResultado.ToString() });
        }
        public static string ArmaComboOpcion(string pstrCodigo, string pstrNombre)
        {
            string strOpcion = "";
            strOpcion = "<option value='" + pstrCodigo + "'>" + pstrNombre + "</option>";
            return strOpcion;
        }



        public ActionResult LoadBulk()
        {

            if (success)
            {
                Response.Write("uploaded");
                Response.End();
                success = false;
            }

            return View();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        ///
        public ActionResult LoadBulkCeps()
        {


            HttpPostedFileBase file = Request.Files["fileUpload"];

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var ext = Path.GetExtension(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/FileUploads"), Guid.NewGuid().ToString() + ext);
                    var pathTxt = Path.Combine(Server.MapPath("~/FileUploads"), Guid.NewGuid().ToString() + "Log.txt");
                    file.SaveAs(path);

                    TaxCache.BulkLoad(path, pathTxt);

                    success = true;
                    return RedirectToAction("LoadBulk");// Json(new { result = true });
                }
                catch (Exception ex)
                {
                    InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                    //return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
                }
            }


            return View();
        }
        public static void InsErrorLog(int UserID, DateTime LogDateUTC, string Message)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "upsInsertErrorLog",
                 new System.Data.SqlClient.SqlParameter("UserID", System.Data.SqlDbType.Int) { Value = UserID },
                 new System.Data.SqlClient.SqlParameter("LogDateUTC", System.Data.SqlDbType.DateTime) { Value = LogDateUTC },
                 new System.Data.SqlClient.SqlParameter("Message", System.Data.SqlDbType.VarChar) { Value = Message });
        }
        /*
        
        [HttpPost]
        public ActionResult LoadBulkCeps(HttpPostedFileBase file)
        {
            var par = Request.QueryString["desc"];

            try
            {
                if (file.ContentLength > 0)
                {
                    // Get the uploaded image from the Files collection

                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        // Validate the uploaded image(optional)

                        // Get the complete file path
                        var fileSavePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);

                        file.SaveAs(fileSavePath);


                        TaxCache.BulkLoad(fileSavePath);
                        return Json(new { result = true });
                        //return new KeyValuePair<bool, string>(true, "File uploaded successfully.");
                    }
                    return Json(new { result = true });
                    //return new KeyValuePair<bool, string>(true, "Could not get the uploaded file.");
                }

                //return new KeyValuePair<bool, string>(true, "No file found to upload.");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
                //return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
            return View();
            //return RedirectToAction("Index");
        }
        */
    }
}
