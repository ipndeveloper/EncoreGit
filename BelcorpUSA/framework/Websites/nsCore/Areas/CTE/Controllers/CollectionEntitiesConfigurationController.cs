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

namespace nsCore.Areas.CTE.Controllers
{
    public class CollectionEntitiesConfigurationController : BaseController
    {
        //
        // GET: /CTE/CollectionEntitiesConfiguration/

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult BrowseCollections()
        {
            return View();
        }

        public virtual ActionResult CreateCollectionEntities(int? id)
        {
            ViewBag.CollectionId = id;
            var collection = id.HasValue ? NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionEntities().Find(x => x.CollectionEntityID == id) : new CollectionEntitiesData();

            return View(collection);
        }

        public virtual ActionResult RestrictCollections(int? id)
        {
            var collection = id.HasValue ? NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionEntities().Find(x => x.CollectionEntityID == id) : new CollectionEntitiesData();

            return View(collection);

        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetCollections(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, 
            int? companyID, int? paymentTypeID,
            string collectionEntityName, string status)
        {

            if (string.IsNullOrEmpty(collectionEntityName)) collectionEntityName = "";
            if (string.IsNullOrEmpty(status)) status = "";

            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var collections = NetSteps.Data.Entities.Business.CollectionEntities.SearchDetails(new CollectionEntitiesSearchParameter()
                {
                    CompanyID = companyID,
                    PaymentTypeID = paymentTypeID,
                    CollectionEntityID = collectionEntityName,
                    status = status,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var collection in collections)
                {
                    builder.Append("<tr>");

                    builder
                        //.AppendCheckBoxCell(value: rule.FineAndInterestRulesID.ToString())
                        .AppendLinkCell("~/CTE/CollectionEntitiesConfiguration/CreateCollectionEntities/" + collection.CollectionEntityID.ToString(), collection.CollectionEntityID.ToString())
                        .AppendCell(collection.CollectionEntityName.ToString())
                        .AppendCell(collection.Location.ToString())
                        .AppendCell(collection.PaymentType.ToString())
                        .AppendCell(collection.Status.ToString())                                             
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

        public virtual ActionResult GetCities(string state)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    Cities = (NetSteps.Data.Entities.TaxCache.SearchCityFromState(state)),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetCounty(string state, string city)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    County = (NetSteps.Data.Entities.TaxCache.SearchCountyFromCity(state,city)),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetCollectionType(int BankID)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    CollectionType = (NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionTypesPerBank().Where(x => x.BankID == BankID).Select(pp=> new{ CollectionTypesPerBankID = pp.CollectionTypesPerBankID, Name =pp.Name })),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetCollectionDocument(int BankID)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    CollectionDocument = (NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionDocumentsPerBank().Where(x => x.BankID == BankID).Select(pp => new { CollectionDocumentsPerBankID = pp.CollectionDocumentsPerBankID, Name = pp.Name })),
                    
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        //Zones
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetZones(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int collectionEntityID)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                int count = 0;

                var collections = NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionZones(collectionEntityID);
                                    

                foreach (var collection in collections)
                {
                    builder.Append("<tr class=\"Alt hover\">");

                    builder
                        //.AppendCell("<input type=\"checkbox\"  ")
                        //.Append(collection.ScopeLevelID)
                        //.Append(" />")
                        .AppendCheckBoxCell(value: collection.ScopeLevelID.ToString())
                        .AppendCell(collection.Value)
                        .AppendCell(collection.Name)
                        .AppendCell(collection.Except ? "true" : "false")
                        .Append("</tr>");
                    ++count;
                }


                return Json(new { result = true, totalPages = collections.Count, page = collections.Count == 0 ? "<tr><td colspan=\"7\">There are no zones</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //Similar to PaymentMethods to get ScopeLevels
        public virtual ActionResult GetScopeLevels(int scopeLevelid)
        {
            try
            {
                var name = NetSteps.Data.Entities.Repositories.PaymentMethodsRepository.SearchPaymentsZones(scopeLevelid).First().Name;
                return Json(new
                {
                    result = true,
                    scopeLevels = name

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //
        public virtual ActionResult SaveCollectionEntitiesCreditCard(int collectionID, int paymentTypeID, int collectionEntityID,
	                            int location, string collectionEntityName, bool chkStatus)
        {

            try
            {
                int status;
                if (chkStatus) status = 1;
                else status = 0;
                int id = CollectionEntitiesRepository.SaveCollectionEntityCreditCard(collectionID, paymentTypeID,  collectionEntityID,
	                                                     location,  collectionEntityName,  status);

                return Json(new { result = true, collectionId = id > 0 ? id.ToString() : String.Empty });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        public virtual ActionResult SaveCollectionEntitiesPaymentTicket(int collectionID, int paymentTypeID, int collectionEntityID,
                               string bankAgencie, string bankAccountNumber, int accountType, int location, int collectionType, int collectionDocument,
                               string collectionAgreement, string collectionEntityName, bool chkStatus
                               , string fileNameBankCollection, int initialPositionName, int finalPositionName, int codeDetail)
        {

            try
            {
                int status;
                if (chkStatus) status = 1;
                else status = 0;

                int id = CollectionEntitiesRepository.SaveCollectionEntityPaymentTicket(collectionID, paymentTypeID, collectionEntityID,
                                 bankAgencie, bankAccountNumber, accountType, location, collectionType, collectionDocument,
                                 collectionAgreement, collectionEntityName, status,
                                 fileNameBankCollection, initialPositionName, finalPositionName, codeDetail);


                return Json(new { result = true, collectionId = id > 0 ? id.ToString() : String.Empty });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        public virtual ActionResult SaveZones(List<CollectionZonesData> zones, int collectionEntityID)
        {

            try
            {
                CollectionEntitiesRepository.ClearCollectionZones(collectionEntityID);

                foreach (var pzone in zones)
                {
                    if (!pzone.Name.Equals("null"))
                    {
                        var zone = new ZonesData
                        {
                            Name = Convert.ToString(pzone.Name),
                            Value = Convert.ToString(pzone.Value),
                            Except = Convert.ToBoolean(pzone.Except)
                        };
                        CollectionEntitiesRepository.InsertCollectionZones(pzone, collectionEntityID);
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

    }
}
