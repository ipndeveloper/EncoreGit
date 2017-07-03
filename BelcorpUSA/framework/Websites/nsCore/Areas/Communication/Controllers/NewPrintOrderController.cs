using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using nsCore.Areas.Communication.Models;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace nsCore.Areas.Communication.Controllers
{
    public class NewPrintOrderController : BaseController
    {
        //
        // GET: /Communication/NewPrintOrder/

        #region New Print Order

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Index()
        {
            TempData["Status"] = NewPrintOrder.ListStatus();
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Search(string query)
        {
            try
            {
                query = query.ToCleanString();
                var searchResults = NewPrintOrder.SlimSearch(query);
                return Json(searchResults.Select(t => new { id = t.GeneralTemplateID, text = t.Name }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int? status, int? section, string nameTemplate, int? period, DateTime? startDate, DateTime? endDate, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var GeneralTemplates = NewPrintOrder.Search(new NewPrintOrderSearchParameters()
                {
                    StatusID = status,
                    SectionID = section,
                    EndDate = endDate,
                    Nombre = nameTemplate,
                    PediodoID = period,
                    StartDate = startDate,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });
                foreach (var GeneralTemplate in GeneralTemplates)
                {
                    builder.Append("<tr>");
                    builder
                       .AppendLinkCell("~/Communication/NewPrintOrder/Edit/" + GeneralTemplate.GeneralTemplateID, GeneralTemplate.Name)
                       .AppendCell(GeneralTemplate.Section)
                       .AppendCell(GeneralTemplate.Order.ToString())
                       .AppendCell(GeneralTemplate.Statu == true ? "Active" : "Desactive")
                       .AppendCell(DataConvertUI.StringToShortDate(GeneralTemplate.DateStar))
                       .AppendCell(DataConvertUI.StringToShortDate(GeneralTemplate.DateEnd))
                       .AppendCell(DataConvertUI.IntToString(GeneralTemplate.Period));
                    builder.Append("</tr>");
                    ++count;
                }
                
                return Json(new { result = true, totalPages = GeneralTemplates.TotalPages, page =  builder.ToString() });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            GeneralTemplateModel emailTemplateViewModel = new GeneralTemplateModel();
            
            if (id.HasValue && id.Value > 0)
            {
                
                NewPrintOrderSearchData _NewPrintOrderSearchData = NewPrintOrder.GetGeneralTemplate(id.Value);
                _NewPrintOrderSearchData.GeneralTemplateTranslations = NewPrintOrder.GetGeneralTemplateTranslations(id.Value);

                var availableLanguages = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), _NewPrintOrderSearchData.GeneralTemplateTranslations);
                
                emailTemplateViewModel = new GeneralTemplateModel
                {
                    NewPrintOrderSearchData = _NewPrintOrderSearchData,
                    Languages = availableLanguages,
                    IsNewEmailTemplate = false,
                };
            }
            else
            {
                emailTemplateViewModel = new GeneralTemplateModel
                {
                    NewPrintOrderSearchData = new NewPrintOrderSearchData(),
                    
                    Languages = TermTranslation.GetTranslatedLanguages(),
                    IsNewsLetterType = false,
                    IsNewEmailTemplate = true
                };
            }

            return View(emailTemplateViewModel);
        }

        /// <summary>
        /// Saves a new  template - now allowing users to add email template translations for each
        /// </summary>
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult Save(int GeneralTemplateID, string Name, short GeneralTemplateSectionID, int Order, string DateStar, string DateEnd, int? PeriodID, bool Active)
        {
            try
            {
                string redirectUrl = "";
                var _GeneralTemplateID = GeneralTemplateID;
                /* If it's a new template - simply save it */
                if (GeneralTemplateID == 0)
                {
                    _GeneralTemplateID = NewPrintOrder.InsertGeneralTemplate(new NewPrintOrderSearchData()
                    {
                        Name = Name,
                        GeneralTemplateSectionID = GeneralTemplateSectionID,
                        Order = Order,
                        DateStar = DataConvertUI.CadenaToDatetime(DateStar),
                        DateEnd = DataConvertUI.CadenaToDatetime(DateEnd),
                        Period =  PeriodID,
                        Statu = Active
                    });
                }
                else
                {
                    /* Update the current  template */
                     NewPrintOrder.UpdateGeneralTemplate(new NewPrintOrderSearchData()
                    {
                        GeneralTemplateID = GeneralTemplateID,
                        Name = Name,
                        GeneralTemplateSectionID = GeneralTemplateSectionID,
                        Order = Order,
                        DateStar = DataConvertUI.CadenaToDatetime(DateStar),
                        DateEnd = DataConvertUI.CadenaToDatetime(DateEnd),
                        Period = PeriodID,
                        Statu = Active
                    });
                }

                return Json(new { result = true, GeneralTemplateID = _GeneralTemplateID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual ActionResult DeleteTemplateTranslation(int id, int generalTemplateID)
        {
            try
            {
                NewPrintOrder.DelGeneralTemplateTranslations(id);

                return RedirectToAction("Edit", new { id = generalTemplateID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual JsonResult AllLanguages(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                NewPrintOrderSearchData _NewPrintOrderSearchData = NewPrintOrder.GetGeneralTemplate(id.Value);
                _NewPrintOrderSearchData.GeneralTemplateTranslations = NewPrintOrder.GetGeneralTemplateTranslations(id.Value);

                var languageTaken = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), _NewPrintOrderSearchData.GeneralTemplateTranslations);

                var languages = from lang in languageTaken
                                select new
                                {
                                    ID = lang.Key,
                                    Name = lang.Value
                                };

                return Json(languages, JsonRequestBehavior.AllowGet);
            }

            var allLang = from lang in TermTranslation.GetTranslatedLanguages()
                          select new
                          {
                              ID = lang.Key,
                              Name = lang.Value
                          };

            return Json(allLang, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        protected virtual Dictionary<int, string> LanguagesLeft(Dictionary<int, string> allLanguages, IList<GeneralTemplateTranslations> templates, int? currentLanguageID = null)
        {
            Dictionary<int, string> stillAvailable = new Dictionary<int, string>();

            // Add all the languages in the list - later we'll remove the one's that already exist
            foreach (var eachLanguages in allLanguages)
            {
                stillAvailable.Add(eachLanguages.Key, eachLanguages.Value);
            }

            // Loop through each emailTemplates and remove languages from the list that already exists
            foreach (var eachLanguages in allLanguages)
            {
                foreach (var template in templates)
                {
                    if (eachLanguages.Key == template.LanguageID && (currentLanguageID == null || (eachLanguages.Key != currentLanguageID)))
                    {
                        stillAvailable.Remove(eachLanguages.Key);
                    }
                }
            }

            return stillAvailable;
        }

        [ValidateInput(false)]
        [FunctionFilter("Communication", "~/Accounts")]
        public virtual JsonResult SaveGeneralTemplateTranslation(GeneralTemplateTranslations template)
        {
            try
            {
                // int Count = 0;
                if (template.GeneralTemplateTranslationID == 0)
                {
                    NewPrintOrder.InsertGeneralTemplateTranslations(template);
                }
                else
                {
                    NewPrintOrder.EditGeneralTemplateTranslations(template);
                }
       
                return Json(new { result = true, message = Translation.GetTerm("SavedSuccessfully", "Saved successfully!"), generalTemplateID = template.GeneralTemplateID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetGeneralTemplateTranslation(int id)
        {
            //var translation = EmailTemplateTranslation.Load(id);
            GeneralTemplateTranslations objGeneralTemplateTranslations = NewPrintOrder.GetGeneralTemplateTranslation(id);

            string body;
            bool active;

            body = objGeneralTemplateTranslations.Body;
            active = objGeneralTemplateTranslations.Active;
            
            return Json(new
            {

                body = body,
                active = active

            });
        }

        public virtual ActionResult AvailableLanguages(int generalTemplateTranslationID, int id)
        {
            // Load the current generalTemplateTranslation details
            GeneralTemplateTranslations objGeneralTemplateTranslations = NewPrintOrder.GetGeneralTemplateTranslation(generalTemplateTranslationID);
            Language currentLanguage = Language.Load(objGeneralTemplateTranslations.LanguageID);
            List<GeneralTemplateTranslations> lstGeneralTemplateTranslations = NewPrintOrder.GetGeneralTemplateTranslations(id);

            var available = LanguagesLeft(TermTranslation.GetTranslatedLanguages(), lstGeneralTemplateTranslations, currentLanguage.LanguageID);

            var availableLanguages = from lang in available
                                     select new
                                     {
                                         ID = lang.Key,
                                         Name = lang.Value
                                     };

            return Json(new { languages = availableLanguages, selected = currentLanguage.LanguageID }, JsonRequestBehavior.AllowGet);
        }

       #endregion
    }
}
