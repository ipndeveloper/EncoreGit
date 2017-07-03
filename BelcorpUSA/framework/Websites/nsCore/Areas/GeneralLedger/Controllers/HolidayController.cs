using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business;
using nsCore.Areas.GeneralLedger.Models.ViewModels;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Logic;
using System.Globalization;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.GeneralLedger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HolidayController : BaseController//Controller
    {
        /// <summary>
        /// 
        /// </summary>
        enum countries
        {
            Brazil = 73
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            HolidayViewModel viewModel = new HolidayViewModel
            {
                Holiday = new Holiday(),
                Countries = Country.LoadBatch(new List<int> { (int)countries.Brazil }),
                StateProvinces = StateProvince.LoadStatesByCountry((int)countries.Brazil)
            };
            return View(viewModel);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNew(HolidayViewModel viewModel)
        {
            //CultureInfo provider = CultureInfo.InvariantCulture;

            //viewModel.Holiday.DateHoliday = DateTime.ParseExact(viewModel.dateHidden, "d", provider);

            viewModel.Holiday.DateHoliday = DateTime.ParseExact(viewModel.dateHidden, "d", CoreContext.CurrentCultureInfo);

            HolidayBusinessLogic.Instance.InserHoliday(viewModel.Holiday);

            return RedirectToAction("Browse");
        }

        public virtual ActionResult ValidateHoliday(string HolidayID, DateTime ? DateHoliday, string StateProvinceID)//HolidayViewModel viewModel)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            //var fec = DateTime.ParseExact(DateHoliday, "d", provider);

            var param = new HolidaySearchParameter()
            {
                HolidayID =Convert.ToInt32( HolidayID),
                StateID = Convert.ToInt32(StateProvinceID),
                DateHoliday = DateHoliday.HasValue ? DateHoliday.Value.ToShortDateString() : ""
                //DateHoliday = fec.ToString()
            };
             var result_= HolidayRepository.ValidateHoliday(param);

             return Json(new
             {
                 result = result_
                 ,
                 msg = Translation.GetTerm("UnicoHoliday", "the Holiday must be unique, check the state and date.")
                
             });
        }

        [HttpPost]
        public ActionResult Update(HolidayViewModel viewModel)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            //viewModel.Holiday.DateHoliday = DateTime.Parse(viewModel.dateHidden, provider);

            viewModel.Holiday.DateHoliday = Convert.ToDateTime(GetCulturaFormat(viewModel.dateHidden), CoreContext.CurrentCultureInfo);
            
            HolidayBusinessLogic.Instance.UpdateHoliday(viewModel.Holiday);

            return RedirectToAction("Browse");
        }

        public ActionResult Browse(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                ViewData["Query"] = query;
                //var products = Product.SlimSearch(query);
                //if (products.Count() == 1)
                //    return RedirectToAction("Overview", new { productId = products.First().ProductID });
            }

            return View();
        }
        public string GetCulturaFormat(string valor)
        {
            var fomatos = new List<System.Globalization.CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };
            bool correcto = false;
            decimal numero = 0;
            DateTime fecha = DateTime.Now;
            //style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //NumberStyles.AllowParentheses; 

            foreach (var item in fomatos)
            {

                if (DateTime.TryParse(valor, item, System.Globalization.DateTimeStyles.None, out fecha) == true)
                {
                    correcto = true;
                    break;
                }



            }
            if (correcto)
            {

                return fecha.ToString(CoreContext.CurrentCultureInfo);
            }
            else
                return valor;


        }
        [OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult Get(string query, string bpcs, string sapSku, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        public virtual ActionResult Get(NetSteps.Common.Constants.SortDirection orderByDirection, int countryID = 0, int stateID = 0, DateTime? dateHoliday = null, int page = 0, int pageSize = 0, string orderBy = "")
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                //dateHoliday = dateHoliday.Equals("Select a Date") ? "" : dateHoliday;

                //DateTime dateQuery = DateTime.Now;
                //if (dateHoliday != string.Empty)
                //{
                //    dateQuery = DateTime.ParseExact(dateHoliday, "d", provider);
                //    dateHoliday = dateQuery.ToString();
                //}

                int total = 0;


                var holidays = HolidayRepository.SearchDetails(new HolidaySearchParameter()
                {
                    CountryID = countryID,
                    StateID = stateID,
                    DateHoliday =dateHoliday.HasValue ? dateHoliday.Value.ToString("d", CoreContext.CurrentCultureInfo) : "",
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                StringBuilder builder = new StringBuilder();
                foreach (var holiday in holidays)
                {
                    builder.Append("<tr>")
                         .AppendCheckBoxCell(value: holiday.HolidayID.ToString())
                         .AppendLinkCell("~/GeneralLedger/Holiday/Overview?holidayID=" + holiday.HolidayID, holiday.HolidayID.ToString())
                         //.AppendCell(holiday.HolidayID.ToString())
                         .AppendCell(holiday.CountryName)
                         .AppendCell(holiday.StateProvinceName)
                         .AppendCell(holiday.DateHoliday.ToString("d", CoreContext.CurrentCultureInfo))
                         .AppendCell(holiday.IsIterative ? "YES" : "NO")
                         .AppendCell(holiday.Reason)
                         .Append("</tr>");
                }
               
                return Json(new
                {
                    result = true,
                    totalPages = holidays.TotalPages,
                    page = holidays.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no collections</td></tr>" : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public ActionResult Overview(int? holidayID)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            Holiday holiday = HolidayBusinessLogic.Instance.Single(x => x.HolidayID == holidayID);
            HolidayViewModel viewModel = new HolidayViewModel
            {
                Holiday =holiday,
                //http://localhost:40000/GeneralLedger/Holiday/Overview?holidayID=9
                //dateHidden = holiday.DateHoliday.ToString("d", provider),

                dateHidden = holiday.DateHoliday.ToString("d", CoreContext.CurrentCultureInfo),
                Countries = Country.LoadBatch(new List<int> { (int)countries.Brazil }),
                StateProvinces = StateProvince.LoadStatesByCountry((int)countries.Brazil)
            };

            if (viewModel.Holiday == null)
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        HolidayBusinessLogic.Instance.DeleteteHoliday(item);
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            try
            {
                foreach (int id in items)
                {
                    HolidayBusinessLogic.Instance.ChangeStatusHoliday(id, active);
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
