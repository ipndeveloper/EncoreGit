using System.Text;
using System;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Support.Infrastructure;
using nsCore.Areas.Support.Models.Ticket;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Diagnostics.Utilities;
using nsCore.Areas.Products.Models;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Repositories;
using System.Transactions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;



namespace nsCore.Areas.Support.Controllers
{
    public class MotiveController : BaseController
    {
        //
        // GET: /Support/Motive/
        [FunctionFilter("Support", "~/Accounts")]
        public ActionResult Index()
        {
            try
            {
                //SupportMotiveSearchParameters parameters = SupportMotives.SetDefaultValuesToFilters(status, name);
                //return View(parameters);
                List<System.Tuple<int, string, int>> lstSupportLevelParent = SupportTicket.GetLevelSupportLevel(0);
                ViewBag.lstSupportLevelParent = lstSupportLevelParent;

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult GetSupportMotives(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            bool? status, string name, string SupportLevelIDs="0;")
        {
            List<string> lstSupportLevelIDs = 
                                             SupportLevelIDs.Split(';')
                                            .Where((str) => { return str != ""; })
                                            .ToList();
           
            Action<DataTable,List<string>> fcCargarTabla = ( dt,lst) => {
                dt.Columns.Add("SupportLevelID");
                lst.ForEach((obj) => {
                
                     dt.Rows.Add( new object[]{obj});
                });
            };
            DataTable dtSupportLevelIDs = new DataTable("SupportLevelIDs");
            fcCargarTabla(dtSupportLevelIDs, lstSupportLevelIDs);

            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                int _id = 0;
                string _name = "";
                _name = name;
                try { _id = int.Parse(name); if (_id > 0 )  _name = ""; }
                catch { _id = 0; _name = name; }

                var supportMotives = SupportMotives.SearchFilter(new SupportMotiveSearchParameters()
                {
                    Active = status,
                    Name = _name,
                    SupportMotiveID = _id,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    dtSupportLevelIDs = dtSupportLevelIDs
                });

                foreach (var supportMotive in supportMotives)
                {
                    builder.Append("<tr>");

                    builder
                        .AppendLinkCell("~/Support/Motive/Edit/" + supportMotive.SupportMotiveID, supportMotive.SupportMotiveID.ToString())                       
                        .AppendCell(supportMotive.Name)
                        .AppendCell(supportMotive.Description)
                        .AppendCell(supportMotive.Active ? "Active" : "Desactive")
                       
                        .AppendLinkCell("~/Support/Level/EditTree/"+supportMotive.SupportLevelID,supportMotive.LevelName)
                        
                        .AppendCell(supportMotive.DateCreatedUTC.Year.Equals(0001)? "":supportMotive.DateCreatedUTC.ToShortDateString())
                        .AppendCell(supportMotive.DateLastModifiedUTC.Year.Equals(0001) ? "" : supportMotive.DateLastModifiedUTC.ToShortDateString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = supportMotives.TotalPages, page = supportMotives.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no periods</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                txtLog("Metodo GetSupportMotives - Motive Contoller");
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                var supportMotive = id.HasValue ? SupportMotives.Search().Find(x => x.SupportMotiveID == id) : new SupportMotiveSearchData();
                int SupportLevelID = 0;
                //txtLog("Metodo Edit - Motive Contoller - supportMotive");
                if (id == null) id = 0;
                try
                {
                    List<MarketSearchData> markets = SupportMotives.GetMarketsbySupportMotive(id.ToString());
                    supportMotive.listaMarket = markets;
                    List<int> lstSupporMotiveLevelIds = SupportMotives.GetMotiveLevelBySupportMotive(id.Value);
                    ViewBag.lstSupporMotiveLevelIds =JsonConvert.SerializeObject(lstSupporMotiveLevelIds ?? new List<int>() { 0});
                    SupportLevelID = lstSupporMotiveLevelIds.Count() > 0 ? lstSupporMotiveLevelIds.First() : 0;

                    System.Data.DataTable dtSupportLevel = new  System.Data.DataTable("dtSupportLevel");
                    dtSupportLevel.Columns.Add( new System.Data.DataColumn(){ColumnName="SupportLevelID",DataType=typeof( Int32 )});

                    lstSupporMotiveLevelIds.Each((El) =>
                    {
                        dtSupportLevel.Rows.Add(new object[] { El });
                    });

                    List<string> values = new List<string>();
                    List<int> Keys = new List<int>();

                    Dictionary<int, string > lstSupportMotivoSelecionados = SupportMotives.GetSuportLevelConcats(dtSupportLevel);
                    foreach (var dc in lstSupportMotivoSelecionados)
                    {
                        values.Add(dc.Value);
                        Keys.Add(dc.Key);
                    }
                


                    ViewBag.values = values;
                    ViewBag.Claves = Keys;
                
                }
                catch (Exception ex)
                {
                    txtLog("Metodo Edit - Motive Contoller -  data - markets"+"-"+ DateTime.Now.ToString() + "-" + ex.Message);
                    InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    throw exception;
                }

                //txtLog("Metodo Edit - Motive Contoller - markets");
                // Default values, new Period
                //SupportMotives.SetDefaultValuesToViewNewPeriod(supportMotive, id);
                
                try
                {

                    var data = SupportLevels.TraerJeraquiaSupportLevel(0) ?? new List<SupportLevelSearchData>();

                    ViewData["LevelTree"] = BuildCategoryTree(supportMotive.Edit, data.Where((obj) => obj.ParentSupportLevelID == 0).ToList(), new List<int>(), data.Where((obj) => obj.ParentSupportLevelID != 0).ToList(), SupportLevelID);
                   

                }
                catch (Exception ex)
                {
                    txtLog("Metodo Edit - Motive Contoller -  data - SupportLevels.Search(0)" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                    InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    throw exception;
                }
                //txtLog("Metodo Edit - Motive Contoller - data");
                //var datosss = BuildCategoryTree(data, new List<int>());
                //txtLog("Metodo Edit - Motive Contoller - datosss");
              
                return View(supportMotive);
            }
            catch (Exception ex)
            {
                txtLog("Metodo Edit - Motive Contoller - supportMotive" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        protected virtual string BuildCategoryTree(Boolean Edit, List<SupportLevelSearchData> categories, IEnumerable<int> selectedCategories, List<SupportLevelSearchData> TodaJerarquias, int SupportLevelID)
        {
            var builder = new StringBuilder();
           
            try
            {
                
                if (categories.Count > 0)
                {
                    builder.Append("<div id=\"categoryContainer").Append(categories.First().ParentSupportLevelID).Append("\" style=\"margin-left:15px;\">");
                    foreach (SupportLevelSearchData category in categories)
                    {

                        builder.Append("<input type=\"radio\" " + (((category.HasChild || !Edit) && (SupportLevelID != category.SupportLevelID)) ? " disabled='disabled'" : "") + "  name=\"storeFronts02\" class=\"category\" value=\"")
                                 .Append(category.SupportLevelID)
                                 .Append(selectedCategories.Contains(category.SupportLevelID) ? "\" checked=\"checked" : "")
                                 .Append("\" /><span id=\"category")
                                 .Append(category.SupportLevelID)
                                 .Append("\">")
                                 .Append(category.Name)
                                 .Append("</span><br />");

                        var data = (TodaJerarquias.Where((obj) => obj.ParentSupportLevelID == category.SupportLevelID)).OrderBy((obj) => obj.SortIndex).ToList(); //SupportLevels.Search(childCategory.SupportLevelID);
                      
                       
                        if (data.Count > 0)
                        {

                            builder.Append(BuildCategoryTree(Edit, data, selectedCategories, TodaJerarquias, SupportLevelID));
                        }
                    }

                    builder.Append("</div>");
                }
            }
            catch(Exception ex)
            {
                txtLog("Metodo BuildCategoryTree - Motive Contoller - supportMotive" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            
            }
            return builder.ToString();
        }



        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Save(
            int? supportMotiveId,
            string name,
            string description,
            int motiveSLA,
            bool active,
            bool isVisibleToWorkStation,
            bool hasConfirmation,
            string[] SupportLevelIDs,
          
            int Edit
            )
        {
            try
            {

                string[] MarketIDs = null;

                byte resultadoEliminarSupproMotiveLevel = 0;
                byte resultadoInsertarSupportLevelMotive = 0;
                List<SupportMotiveSearchData> supportMotives = SupportMotives.Search();

                var motive = supportMotiveId.HasValue ? supportMotives.Find(x => x.SupportMotiveID == supportMotiveId) : new SupportMotiveSearchData();

                motive.SupportMotiveID = supportMotiveId.HasValue ? motive.SupportMotiveID : 0;
                motive.Name = name;
                motive.MotiveSLA = motiveSLA;
                motive.Description = description;
                motive.Active = active;
                motive.IsVisibleToWorkStation = isVisibleToWorkStation;
                motive.HasConfirmation = hasConfirmation;

                Site oSite = CurrentSite;

                MarketIDs = new string[1] { Convert.ToString( oSite.MarketID  )};

                //SupportMotives.SetDefaultValuesToSave(motive);  colocar los valores por defecto, pendientesdy

               var ID=  SupportMotives.Save(motive,
                                            Edit==1? SupportLevelIDs: new String []{},
                                            MarketIDs,
                                            out resultadoEliminarSupproMotiveLevel,
                                            out resultadoInsertarSupportLevelMotive
                                            );

                    return Json(
                                    new 
                                        { 
                                                result = true, 
                                                supportMotiveID = ID,
                                                resultadoInsertarSupportLevelMotive = resultadoInsertarSupportLevelMotive
                                        }
                                 );

            }
            catch (Exception ex)
            {
                txtLog("Metodo Save - Motive Contoller" +"-"+ DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        // Property
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult AddSupportMotiveProperty(
            int supportMotiveId,
            string name,
            string dataType,
            bool required,
            bool editable,
            bool visible ,
            bool FieldSolution,
            int SupportMotivePropertyTypeID, int SupportMotivePropertyDinamicID
            )
        {
            try
            {
                var motiveProperty = new NetSteps.Data.Entities.Business.SupportMotivePropertyTypeSearchData()
                {
                    Name = name,
                    DataType = dataType,
                    SupportMotiveID = supportMotiveId,
                    Required = required,
                    Editable = editable,
                    IsVisibleToWorkStation = visible,
                    SupportMotivePropertyTypeID = SupportMotivePropertyTypeID,
                    FieldSolution = FieldSolution,
                    SupportMotivePropertyDinamicID= SupportMotivePropertyDinamicID
                };

                SupportMotives.SaveMotiveProperty(motiveProperty);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetSupportMotivePropertyDinamic()
        {

            var propertyItems = SupportMotives.GetSupportMotivePropertyDinamic();
            ViewBag.MotivePropertyDinamic = propertyItems;
            return View();
        }
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetPropertyItems(string Disabled,int Enabled, int page, int pageSize, int supportMotiveID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var propertyItems = SupportMotives.SearchMotiveProperty(new SupportMotiveSearchParameters()
                {
                    SupportMotiveID=supportMotiveID,
                    PageIndex = page,
                    PageSize = 100,
                    OrderBy = "SupportMotivePropertyType.SortIndex",
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                });
                int i = 0;
                foreach (SupportMotivePropertyTypeSearchData item in propertyItems)
                {
                    //Product product = Inventory.GetProduct(item.ProductID);
                    //builder.Append("<tr id=\"catalogItem").Append(item.SupportMotivePropertyTypeID).Append("\">")
                        //.Append(string.Format("<tr data-id=\"{0}\" data-type=\"{1}\">", newsletterCampaign.CampaignActionID, campaignActionType))
                    builder.Append(string.Format("<tr data-id=\"{0}\">", item.SupportMotivePropertyTypeID))
                            .AppendCheckBoxCell(value: item.SupportMotivePropertyTypeID.ToString(), disabled: Enabled==0)
                            //.AppendLinkCell(string.Format("~/Products/Products/Overview/{0}/{1}", product.ProductBaseID, product.ProductID), product.SKU)
                            //.AppendCell(product.Translations.Name())
                            .AppendCell("<span>"+(item.Name)+"</span>")
                            .AppendCell("<span>" + (item.DataType) + "</span>")
                            .AppendCell("<span>" + (item.Required ? "Yes" : "No" )+ "</span>")
                            .AppendCell("<span>" + (item.Editable ? "Yes" : "No" ) + "</span>")
                            .AppendCell(  item.SortIndex.ToString())
                            .AppendCell("<span>" + (item.IsVisibleToWorkStation ? "Yes" : "No") + "</span>")
                            .AppendCell("<span>" + (item.FieldSolution ? "Yes" : "No") + "</span>")
                            //.AppendLinkCell(string.Format("~/Support/Motive/PropertyValues/{0}", item.SupportMotivePropertyTypeID), ( item.DataType=="List")?"View Values":"",null,null,null,"btnOpenValuesSelector"+i.ToString())     
                            .AppendLinkCell("javascript:void(0);", (item.DataType == "List") ? "View Values" : "", linkCssClass: "btnEditCampaignAction")

                            .AppendCell(Enabled == 1 ? ("<img style='cursor:pointer;' class='Read' src='/content/Images/pencil-12-trans.png' width='15' heigth='15' index='" + i.ToString() + "' onclick='Edit(this)' /> <img  style='cursor:pointer; display:none' src='/content/Images/CancelEdit.PNG' width='15' heigth='15' index='" + i.ToString() + "' onclick='Cancel(this)' />") : "")
                        
                            .Append("</tr>");
                            i++;
                }

                var lisProperties = SupportMotives.GetPropertyTypesBySupportMotive(supportMotiveID);
               var stringBuilder = BuildPropertyItems(lisProperties, Disabled);
               //ViewData["PropertiesSelect"] = lisProperties;

               return Json(new { result = true, resultCount = propertyItems.TotalCount /*catalogItems.Count */, propertyItems = builder.ToString(), select= stringBuilder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



        protected virtual string BuildPropertyItems(List<SupportMotivePropertyTypeSearchData> properties, string Disabled)
        {
            var builder = new StringBuilder();
            if (properties.Count > 0)
            {
                builder.Append("<div id=\"propertyContainer").Append(properties.First().SupportMotivePropertyTypeID).Append("\" style=\"margin-left:15px;\">");
                builder.Append("<select " + Disabled + " id=\"sTaskItemProperty\">");
                builder.Append("<option value=\"0\" selected=\"selected\">Select Item Property</option>");
                foreach (SupportMotivePropertyTypeSearchData item in properties)
                {
                    builder.Append("<option value="+item.SupportMotivePropertyTypeID.ToString()+">"+item.Name+"</option>");
                }

                builder.Append("</select>");
                builder.Append("</div>");
            }
            return builder.ToString();
        }
       

     

        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult EditPropertyValuesModal(int SupportMotivePropertyTypeID)
        {
            SupportMotivePropertyValuesSearchData data = new NetSteps.Data.Entities.Business.SupportMotivePropertyValuesSearchData();
            data.SupportMotivePropertyTypeID = SupportMotivePropertyTypeID;
            return View(data);
        }


        // TASK
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult AddSupportMotiveTask(
            int supportMotiveId,
            string name,
            string url,
            int propertyTypeID,
            int SupportMotiveTaskID
            )
        {
            try
            {
                var motiveTask = new NetSteps.Data.Entities.Business.SupportMotiveTaskSearchData()
                {
                    Name = name,
                    Link = url,
                    SupportMotiveID = supportMotiveId,
                    SupportMotivePropertyTypeID = propertyTypeID,
                    SupportMotiveTaskID = SupportMotiveTaskID
                };

                SupportMotives.SaveMotiveTask(motiveTask);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetTaskItems(int Enabled,int page, int pageSize, int supportMotiveID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var taskItems = SupportMotives.SearchMotiveTask(new SupportMotiveSearchParameters()
                {
                    SupportMotiveID = supportMotiveID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = "SupportMotiveTask.SortIndex",
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                });
                int i = 0;
                foreach (SupportMotiveTaskSearchData item in taskItems)
                {
                    //Product product = Inventory.GetProduct(item.ProductID);
                    builder.Append("<tr data-id='" + item.SupportMotiveTaskID + "' id=\"taskItem").Append(item.SupportMotivePropertyTypeID).Append("\">")
                        .AppendCheckBoxCell(value: item.SupportMotiveTaskID.ToString(), disabled: Enabled==0)
                        //.AppendLinkCell(string.Format("~/Products/Products/Overview/{0}/{1}", product.ProductBaseID, product.ProductID), product.SKU)
                        //.AppendCell(product.Translations.Name())
                            .AppendCell("<span>" + item.Name + "</span>")
                            .AppendCell("<span>"+item.Link+ "</span>")
                            .AppendCell("<span>"+item.NameProperty+ "</span>")
                            .AppendCell(  item.SortIndex.ToString()  )
                            .AppendCell(Enabled == 1 ? ("<img style='cursor:pointer;' class='Read' src='/content/Images/pencil-12-trans.png' width='15' heigth='15' index='" + i.ToString() + "' onclick='EditTask(this)' /> <img  style='cursor:pointer; display:none' src='/content/Images/CancelEdit.PNG' width='15' heigth='15' index='" + i.ToString() + "' onclick='CancelTask(this)' />") : "")
                        .Append("</tr>");
                    i++;
                }
                return Json(new { result = true, resultCount = taskItems.TotalCount /*catalogItems.Count */, taskItems = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult RemoveTaskItems(string[] MarketIDs)
        {
            try
            {
                SupportMotives.DeleteTaskItems(MarketIDs);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult RemovePropertyItems(string[] PropertyIDs)
        {
            try
            {
                SupportMotives.DeletePropertyItems(PropertyIDs);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        // PROPERTY VALUES

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetPropertyValueItems(int page, int pageSize, int supportMotivePropertyTypeID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var taskItems = SupportMotives.SearchPropertyValue(new SupportMotiveSearchParameters()
                {
                    SupportMotiveID = supportMotivePropertyTypeID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = "SupportMotivePropertyValues.SortIndex",
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                });

                foreach (SupportMotivePropertyValuesSearchData item in taskItems)
                {
                    //Product product = Inventory.GetProduct(item.ProductID);
                    builder.Append("<tr id=\"valueItem").Append(item.SupportMotivePropertyTypeID).Append("\">")
                        .AppendCheckBoxCell(value: item.SupportMotivePropertyValueID.ToString())
                        //.AppendLinkCell(string.Format("~/Products/Products/Overview/{0}/{1}", product.ProductBaseID, product.ProductID), product.SKU)
                        //.AppendCell(product.Translations.Name())
                        .AppendCell(item.Value)
                        .AppendCell(item.SortIndex.ToString())
                        .Append("</tr>");
                }
                return Json(new { result = true, resultCount = taskItems.TotalCount /*catalogItems.Count */, taskItems = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult AddPropertyValue(
            int supportMotivePropertyTypeID,
            string value
            )
        {
            try
            {
                var propertyValue = new NetSteps.Data.Entities.Business.SupportMotivePropertyValuesSearchData()
                {
                    SupportMotivePropertyTypeID = supportMotivePropertyTypeID,
                    Value = value
                };

                SupportMotives.SavePropertyValue(propertyValue);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult RemovePropertyValueItems(string[] PropertyValueIDs)
        {
            try
            {
                SupportMotives.DeletePropertyValueItems(PropertyValueIDs);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public static void InsErrorLog(int UserID, DateTime LogDateUTC, string Message)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "upsInsertErrorLog",
                 new System.Data.SqlClient.SqlParameter("UserID", System.Data.SqlDbType.Int) { Value = UserID },
                 new System.Data.SqlClient.SqlParameter("LogDateUTC", System.Data.SqlDbType.DateTime) { Value = LogDateUTC },
                 new System.Data.SqlClient.SqlParameter("Message", System.Data.SqlDbType.VarChar) { Value = Message });
        }

        public void  txtLog(string mensaje)
        {
            var pathTxt = Path.Combine(Server.MapPath("~/FileUploads"), Guid.NewGuid().ToString() + "Log.txt");

            txtLog2(pathTxt, mensaje);

        }

        public static void txtLog2(string pathTxt, string mensaje)
        {
            if (!System.IO.File.Exists(pathTxt))
            {
                // Create a file to write to. 
                using (StreamWriter sw = System.IO.File.CreateText(pathTxt))
                {
                    sw.WriteLine(mensaje);
                }
            }
        }



        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Get(int levelId)
        {
            try
            {

               string  NivelesConcatenados = SupportMotives.ConcatenarSupportLevel(levelId);
                return Json(new
                {
                    NivelesConcatenados = NivelesConcatenados 

                });
            }
            catch (Exception ex)
            {
                txtLog("Metodo Get - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        public ActionResult ListarSupporLevelConcatenados(  List<int>lstSupportLevelSeleccionados)
        {
            System.Data.DataTable dtSupportLevel = new System.Data.DataTable("dtSupportLevel");
            dtSupportLevel.Columns.Add(new System.Data.DataColumn() { ColumnName = "SupportLevelID", DataType = typeof(Int32) });

            lstSupportLevelSeleccionados.Each((El) =>
            {
                dtSupportLevel.Rows.Add(new object[] { El });
            });

            List<string> values = new List<string>();
            List<int> Keys = new List<int>();

            Dictionary<int, string> lstSupportMotivoSelecionados = SupportMotives.GetSuportLevelConcats(dtSupportLevel);
            foreach (var dc in lstSupportMotivoSelecionados)
            {
                values.Add(dc.Value);
                Keys.Add(dc.Key);
            }


            return Json(new {
                Values = values ,
                Keys = Keys
            });
        }


    }
}
