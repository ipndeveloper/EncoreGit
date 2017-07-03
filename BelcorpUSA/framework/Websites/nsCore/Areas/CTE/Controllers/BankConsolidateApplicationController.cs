using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.Logic;
using System.IO;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Helpers;
using System.Globalization;





namespace nsCore.Areas.CTE.Controllers
{
    public class BankConsolidateApplicationController : BaseController
    {
        bool success = false;
       // int succ = 0;
        // GET: /CTE/CollectionEntitiesConfiguration/

        public ActionResult Index(int succ=-1)
        {

            Dictionary<int, string> dcBank = new Dictionary<int, string>();
            dcBank = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(PaymentTicktesBussinessLogic.BanksActiveDrop());
                       
            ViewBag.dcBank = dcBank;
            //if (success)
            //    return View(new MensajeSearchData { Message = "ok" });
            //else
            //    return View();
            ViewBag.MsgLoad = -1;
            if (succ > -1)
            {
                ViewBag.MsgLoad = succ;
                //return Content("<script language='javascript' type='text/javascript'>alert('Save Successfully');</script>");

               // return View(new MensajeSearchData { Message = Translation.GetTerm("BankConsolidateOK", "El archivo finalizo correctamente") });
            }

            Session["orderCol"] = null;

            return View();



        }

        

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult BrowseBankConsolidateApplication(int page, int pageSize, 
            string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? Bankid,string BankConsolidateDatePro=""
            )
        {

            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var ordercol = Session["orderCol"];

                if (ordercol == null)
                {
                    orderBy = "BankConsolidatePro";
                    orderByDirection = NetSteps.Common.Constants.SortDirection.Descending;
                    Session["orderCol"] = "BankConsolidatePro";
                }

                var collections = BankConsolidateApplication.SearchBankConsolidateApplication(new BankConsolidateApplicationSearchParameter()
                {
                    Bankid = Bankid,
                    BankConsolidateDatePro = BankConsolidateDatePro,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                string color = "";
                foreach (var collection in collections)
                {

                    if (collection.BankConsolidateErr > 0)
                        color = "red";
                    else
                        color = "";

                    builder.Append("<tr>");

                    builder
                        //.AppendLinkCell("~/CTE/BankConsolidateApplication/BrowseBankPayments/BankID=" + collection.BankID.ToString() + "&&BankConsolidateSec=" + collection.BankConsolidateSec.ToString(), collection.BankConsolidateSec.ToString())
                        .AppendCell("<a href='javascript:void(0)' onclick='loadBrowseBankPayments(" + collection.BankID.ToString() + "," + collection.BankConsolidateSec.ToString() + ")'> " + collection.BankConsolidateSec.ToString() + "</a>")
                        .AppendCell(collection.BankName)
                        .AppendCell(collection.BankConsolidatePro.ToString())                        
                        .AppendCell(collection.BankConsolidateVal.ToString("N"))
                        .AppendCell(collection.BankConsolidateErr.ToString(), style: "color :" + color)
                        .AppendCell(collection.BankConsolidateFile)
                         .AppendCell(FormatDatetimeByLenguage(collection.BankConsolidateDateFile,CoreContext.CurrentCultureInfo))
                        .AppendCell(FormatDatetimeByLenguage(collection.BankConsolidateDatePro,CoreContext.CurrentCultureInfo))   
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = collections.TotalPages, 
                    page = collections.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no collections</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        public static string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(fecha))
                return "N/A";
            else
            {

            }
            {
                DateTime? fec = null;
                DateTime Temp;

                var split = fecha.Split('/');
                if (split.Length >= 3)
                {

                    if (culture.Name == "en-US")

                        return string.Format("{0}/{1}/{2}", split[1], split[0], split[2]);

                    else
                        return string.Format("{0}/{1}/{2}", split[0], split[1], split[2]);

                }

                else
                    return fecha;

            }
        }

        public ActionResult LoadBankID(int BankID, int BankConsolidateSec)
        {

            Session["BankID"] = BankID;
            Session["BankConsolidateSec"] = BankConsolidateSec;

            return Json(new
            {
                result = true
                
            });



        }
        public ActionResult BrowseBankPayments()
        {
            return View();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult getBankPayments(int page, int pageSize,
            string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection
            ,int? ticketNumber , int? accountId 
            )
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                int bankid=int.Parse(Session["BankID"].ToString());
                int bankConsolidateSec = int.Parse(Session["BankConsolidateSec"].ToString());

                var collections = BankConsolidateApplication.SearchBankPayments(new BankPaymentsSearchParameter()
                {
                    Bankid = bankid,
                    FileSequence = bankConsolidateSec,
                    TicketNumber = ticketNumber,
                    AccountCode = accountId,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                string color = "";
                foreach (var collection in collections)
                {
                    if (collection.Applied == 3)
                        color = "red";
                    else
                        color = "";


                    builder.Append("<tr>");

                    builder                       
                        .AppendCell(collection.AccountCode.ToString())
                        .AppendCell(collection.AccountName.ToString())
                        .AppendCell(collection.BankName.ToString())
                        .AppendCell(collection.TicketNumber.ToString())
                        .AppendCell(collection.OrderNumber)
                        //.AppendCell(collection.Amount.ToString("N"))
                        .AppendCell(collection.Amount.ToString("N",CoreContext.CurrentCultureInfo))
                        .AppendCell(collection.DateReceivedBank.ToString(CoreContext.CurrentCultureInfo))
                        .AppendCell(collection.DateApplied.ToString(CoreContext.CurrentCultureInfo))
                        .AppendCell(collection.FileNameBank)
                        .AppendCell(collection.FileSequence.ToString())
                        .AppendCell(collection.Applied.ToString(), style: "color :" + color)
                        .AppendCell(collection.Credito)
                        .AppendCell(collection.Residual.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new
                {
                    result = true,
                    totalPages = collections.TotalPages,
                    page = collections.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no collections</td></tr>" : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public ActionResult LoadInformation()
        {
           

            HttpPostedFileBase file = Request.Files["fileUpload"];
             try
                {
                    int respuesta = 0;
                    if (file != null && file.ContentLength > 0)
                    {


                        var NombreArchivo = Path.GetFileName(file.FileName);
                        var ext = Path.GetExtension(file.FileName);
                        // var path = Path.Combine(Server.MapPath("~/FileUploads"), Guid.NewGuid().ToString() + ext);
                        //var path = Path.Combine(Server.MapPath("~/FileUploads"), NombreArchivo + ext);
                        var path = Path.Combine(Server.MapPath("~/FileUploads"), file.FileName);
                        // var pathTxt = Path.Combine(Server.MapPath("~/FileUploads"), Guid.NewGuid().ToString() + "Log.txt");
                        file.SaveAs(path);


                        NombreArchivo.Replace(".ret", "");
                        NombreArchivo.Replace(".txt", "");

                        respuesta = Validar(NombreArchivo, path);

                            
                        // throw new Exception(Translation.GetTerm("BankConsolidate", "El archivo ya fue procesado"));

                        //    return JsonError(Translation.GetTerm("BankConsolidate", "El archivo ya fue procesado"));

                        //return Json(new { result = true, message = Translation.GetTerm("CreditoDesbloqueado", "Crédito Desbloqueado") });
                        //success = true;
                        //// return RedirectToAction("LoadBulk");// Json(new { result = true });
                        // Json(new { result = true });



                    }
                    else {respuesta = 3; }
            return RedirectToAction("Index", new { succ = respuesta });
                }
             catch (Exception ex)
             {
                 var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                 return Json(new { result = false, message = exception.PublicMessage });
             }
           
        }

        private int Validar(string NombreArchivo,  string path)
        {
             /*=================================================================================================
            Se debe acceder  a la tabla CollectionEntities  filtrando el campo PaymentTypeId =11 y Active = 1
            ==================================================================================================*/
            var ListCollectionEntities = AccountPropertiesBusinessLogic.GetListByID(1, 0);
            string NombreArchivoFijo = ""; int InitialPositionName = 0; int FinalPositionName = 0; string FileNameBankCollection = ""; int BankID = 0; string CodeDetail="";
            int longNameArchiv = NombreArchivo.Length;

            /*=================================================================================================
            Se debe recorrer el resultado de ListCollectionEntities, 
            Si @NombreArchivoFijo = @FileNameBankCollection Entonces tomar el @BankId = BankId y salir del ciclo
            ==================================================================================================*/
            foreach (var item in ListCollectionEntities)
            {
                InitialPositionName = item.InitialPositionName;
                FinalPositionName = item.FinalPositionName;
                FileNameBankCollection = item.FileNameBankCollection;

                if (InitialPositionName > 0)
                    InitialPositionName = InitialPositionName - 1;

                if (FinalPositionName <= longNameArchiv)
                {

                    NombreArchivoFijo = NombreArchivo.Substring(InitialPositionName, FinalPositionName);

                    if (NombreArchivoFijo == FileNameBankCollection)
                    {
                        CodeDetail = item.CodeDetail;
                        BankID = item.BankId;  //***
                        break;
                    }
                }
                
            }
    
            string SecuenciaArchivo = "";
            string FechaArchivo = "";

            string TicketNumber;
            string FechaPagoBanco;
            string MontoPago  ;
            string SecuenciaRegistro;
            string BankName="";
            string primerRegistro = "";             
            string linea = null;
            string CodeDetailFile = "";
            string validItau = "";
            StreamReader ArchivoLectura = new StreamReader(path);
           

            if (BankID == 0)
                return 2;
            //if (CodeDetail != "" && BankID != 0)
            else
            {
                /*==================================================================
                 Esto indica que se va a trabajar Bando Do Brasil
                 ======================================================*/
                if (BankID == 1)
                {

                    BankName = AccountPropertiesBusinessLogic.GetValueByID(6, BankID).BankName;

                    primerRegistro = ArchivoLectura.ReadLine();


                    SecuenciaArchivo = primerRegistro.Substring(100, 7);//101-107);
                    FechaArchivo = primerRegistro.Substring(94, 6);//(95, 100);

                    /*==================================================================
                    * Obtenemos Informacion de la tabla BANKCONSOLIDATEDAPPLICATION  
                    * con los valores de Bankid , BankConsolidateSec;  (utilizamos el store del  browser**)
                    ==================================================================*/
                    var listBancos = BankConsolidateApplication.SearchBankConsolidateApplication(
                           new BankConsolidateApplicationSearchParameter() { Bankid = BankID });

                    var ListBankoConsolidateApplicateion = listBancos.Where(x => x.BankConsolidateSec == int.Parse(SecuenciaArchivo)).ToList();

                    /*==================================================================
                     *Si se encuentra registro se debe desplegar mensaje en donde se indique el archivo ya fue procesado  y salir del proceso
                     ==================================================================*/
                    if (ListBankoConsolidateApplicateion.Count > 0)
                        return 1; //
                    /*==================================================================
                     * Pasamos a recorrer las siguientes lineas , la primera la sacamos con remove
                    ==================================================================*/
                    while (!ArchivoLectura.EndOfStream)
                    {
                        linea = ArchivoLectura.ReadLine();
                        if (linea != "")
                        {
                            CodeDetailFile = linea.Substring(0, 1);                           
                            if (CodeDetailFile == CodeDetail )
                            {

                                TicketNumber = linea.Substring(71, 9);//(72, 80);
                                FechaPagoBanco = linea.Substring(110, 6);//(111, 116);
                                //FechaPagoBanco = fec.Substring(2, 4) + fec.Substring(0, 2);
                                MontoPago = linea.Substring(253, 13);//(254, 266);
                                SecuenciaRegistro = linea.Substring(394, 6);//(395, 400);

                                BankConsolidateApplication.InsertBankPayments(
                                    new BankPaymentsSearchParameter()
                                    {
                                        TicketNumber = TicketNumber.ToInt(),
                                        DateReceivedBank = FechaPagoBanco,
                                        Amount = MontoPago,
                                        FileSequence = SecuenciaArchivo.ToInt(),
                                        BankName = BankName,
                                        FileNameBank = NombreArchivo,
                                        logSequence = SecuenciaRegistro.ToInt(),
                                        Bankid = BankID
                                    });
                            }
                        }
                    }
                    ArchivoLectura.Close();


                }


                /*==================================================================
               Esto indica que se va a trabajar Caixa
               ======================================================*/
                if (BankID == 2)
                {
                    BankName = AccountPropertiesBusinessLogic.GetValueByID(6, BankID).BankName;
                    //SecuenciaArchivo = ArchivoLectura.ReadLine().FirstOrDefault().ToString().Substring(390, 394);
                    //FechaArchivo = ArchivoLectura.ReadLine().FirstOrDefault().ToString().Substring(95, 100);
                    primerRegistro = ArchivoLectura.ReadLine();
                    SecuenciaArchivo = primerRegistro.Substring(389, 5);//101-107);
                    FechaArchivo = primerRegistro.Substring(94, 6);//(95, 100);
                    /*==================================================================
                    * Obtenemos Informacion de la tabla BANKCONSOLIDATEDAPPLICATION  
                    * con los valores de Bankid , BankConsolidateSec;  (utilizamos el store del  browser**)
                    ==================================================================*/
                    var listBancos = BankConsolidateApplication.SearchBankConsolidateApplication(
                           new BankConsolidateApplicationSearchParameter() { Bankid = BankID });

                    var ListBankoConsolidateApplicateion = listBancos.Where(x => x.BankConsolidateSec == int.Parse(SecuenciaArchivo)).ToList();

                    /*==================================================================
                     *Si se encuentra registro se debe desplegar mensaje en donde se indique el archivo ya fue procesado  y salir del proceso
                     ==================================================================*/
                    if (ListBankoConsolidateApplicateion.Count > 0)
                        return 1;//
                    /*==================================================================
                     * Pasamos a recorrer las siguientes lineas , la primera la sacamos con remove
                    ==================================================================*/
                    while (!ArchivoLectura.EndOfStream)
                    {
                        linea = ArchivoLectura.ReadLine();
                        if (linea != "")
                        {
                            CodeDetailFile = linea.Substring(0, 1);
                            if (CodeDetailFile == CodeDetail && CodeDetail != "")
                            {
                                TicketNumber = linea.Substring(43, 9);//(44, 52);
                                FechaPagoBanco = linea.Substring(110, 6);//(111, 116);
                                // FechaPagoBanco = fec.Substring(2, 4) + fec.Substring(0, 2);
                                MontoPago = linea.Substring(253, 13);//(254, 266);
                                SecuenciaRegistro = linea.Substring(394, 6);//(395, 400);

                                BankConsolidateApplication.InsertBankPayments(
                                    new BankPaymentsSearchParameter()
                                    {
                                        TicketNumber = TicketNumber.ToInt(),
                                        DateReceivedBank = FechaPagoBanco,
                                        Amount = MontoPago,
                                        FileSequence = SecuenciaArchivo.ToInt(),
                                        BankName = BankName,
                                        FileNameBank = NombreArchivo,
                                        logSequence = SecuenciaRegistro.ToInt(),
                                        Bankid = BankID
                                    });
                            }
                        }
                    }
                    ArchivoLectura.Close();

                }

                /*==================================================================
             Esto indica que se va a trabajar Itau
             ======================================================*/
                if (BankID == 4)
                {

                    BankName = AccountPropertiesBusinessLogic.GetValueByID(6, BankID).BankName;

                    primerRegistro = ArchivoLectura.ReadLine();

                    //Ktorres ; Fecha 14-07-2016
                    SecuenciaArchivo = primerRegistro.Substring(108, 5);//109-113 --------primerRegistro.Substring(100, 5);//101-105;
                    FechaArchivo = primerRegistro.Substring(94, 6);//(95, 100);

                    /*==================================================================
                    * Obtenemos Informacion de la tabla BANKCONSOLIDATEDAPPLICATION  
                    * con los valores de Bankid , BankConsolidateSec;  (utilizamos el store del  browser**)
                    ==================================================================*/
                    var listBancos = BankConsolidateApplication.SearchBankConsolidateApplication(
                           new BankConsolidateApplicationSearchParameter() { Bankid = BankID });

                    var ListBankoConsolidateApplicateion = listBancos.Where(x => x.BankConsolidateSec == int.Parse(SecuenciaArchivo)).ToList();

                    /*==================================================================
                     *Si se encuentra registro se debe desplegar mensaje en donde se indique el archivo ya fue procesado  y salir del proceso
                     ==================================================================*/
                    if (ListBankoConsolidateApplicateion.Count > 0)
                        return 1; //
                    /*==================================================================
                     * Pasamos a recorrer las siguientes lineas , la primera la sacamos con remove
                    ==================================================================*/
                    while (!ArchivoLectura.EndOfStream)
                    {
                        linea = ArchivoLectura.ReadLine();
                        if (linea != "")
                        {
                            CodeDetailFile = linea.Substring(0, 1); // la primera posicion del detalle
                            validItau = linea.Substring(108, 2);//  posicion 109-110  tienen  que ser diferentes a 06
                            if (CodeDetailFile == CodeDetail && CodeDetail != "" && CodeDetailFile == "1" && validItau == "06")
                            {

                                TicketNumber = linea.Substring(62, 8);//(63, 70);
                                FechaPagoBanco = linea.Substring(110, 6);//(111, 116);linea.Substring(295, 6);//(296, 301);
                                MontoPago = linea.Substring(257, 9);// posiscion 258-266
                                SecuenciaRegistro = linea.Substring(394, 6);//(395, 400);

                                BankConsolidateApplication.InsertBankPayments(
                                    new BankPaymentsSearchParameter()
                                    {
                                        TicketNumber = TicketNumber.ToInt(),
                                        DateReceivedBank = FechaPagoBanco,
                                        Amount = MontoPago,
                                        FileSequence = SecuenciaArchivo.ToInt(),
                                        BankName = BankName,
                                        FileNameBank = NombreArchivo,
                                        logSequence = SecuenciaRegistro.ToInt(),
                                        Bankid = BankID
                                    });
                            }
                        }
                    }
                    ArchivoLectura.Close();


                }

               
                    //3.6 Aplicación del recaudo 
                    string TipoCredito = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "TPC");

                    BankConsolidateApplication.ImplementationCollectionAsym(
                             new BankPaymentsSearchParameter()
                             {
                                 TipoCredito = TipoCredito,
                                 Bankid = BankID,
                                 FileSequence = int.Parse(SecuenciaArchivo),
                                 UserID = CoreContext.CurrentUser.UserID,
                                 FileNameBank = NombreArchivo,
                                 FileDate = FechaArchivo,
                                 BankName = BankName

                             });
                    //BankConsolidateApplication.ImplementationCollection(
                    //     new BankPaymentsSearchParameter()
                    //     {
                    //         TipoCredito = TipoCredito,
                    //         Bankid = BankID,
                    //         FileSequence = int.Parse(SecuenciaArchivo),
                    //         UserID = CoreContext.CurrentUser.UserID,
                    //         FileNameBank = NombreArchivo,
                    //         FileDate = FechaArchivo,
                    //         BankName = BankName

                    //     });
               
            }
            return 0;
        }

        
       
     
    }
}
