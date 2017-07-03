using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Web.Mvc.Attributes;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using nsCore.Areas.Support.Models.SupportLevel;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Helpers;
using System.IO;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Support.Controllers
{
    public class LevelController : BaseController
    {

        [FunctionFilter("Support", "~/Accounts")]
        public ActionResult Index()
        {

            
            return View();
        }


        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult EditTree(int? id)
        {
            try
            {
                Translation.GetTerm("HaveChild", "It has an associated level of support");
                Translation.GetTerm("HaveMotive", "Associated have a reason");


                //Se mejora el proceso de carga solo se trae una vez toda la jerarquia de niveles de soporte -Salcedo vila G.
                var listParents = SupportLevels.TraerJeraquiaSupportLevel(0) ??  new List<SupportLevelSearchData>();
                StringBuilder categoryBuilder = new StringBuilder();
                BuildTree(listParents.Where((obj) => obj.ParentSupportLevelID == 0).ToList(), categoryBuilder, 1, listParents.Where((obj) => obj.ParentSupportLevelID !=0).ToList());
               
                
                SupportLevelSearchData levelTree = new SupportLevelSearchData();
               

                EditLevelTreeModel model = new EditLevelTreeModel()
                {
                    LevelTree = levelTree,
                    SaveURL = "~/Support/Level/Save",
                    DeleteURL = "~/Support/Level/Delete",
                    GetURL = "~/Support/Level/Get",
                    MoveURL = "~/Support/Level/Move",
                    levels = categoryBuilder.ToString(),
                     
                    levelId =id.HasValue? id.Value:0
                    
                };
                return View(model);
            }
            catch (Exception ex)
            {
                txtLog("Metodo EditTree - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Support", "~/Accounts")]
        protected virtual void BuildTree(List<SupportLevelSearchData> parent, StringBuilder builder, int level, List<SupportLevelSearchData> TodaJerarquias)
        {
            
            try
            {

                if (parent.Count > 0)
                {
                    builder.Append("<ul>");
                    foreach (SupportLevelSearchData childCategory in parent)
                    {
                        builder
                                .Append("<li Parent='" + childCategory .ParentSupportLevelID.ToString()+ "' id=\"category")
                                .Append(childCategory.SupportLevelID)
                                .Append("\"><a href=\"javascript:void(0);\"  class=\"category\" style=\"padding:0px 4px;\">")
                                .Append(childCategory.Name)
                                .Append("</a>");
                        if (level < 6)
                        {
                            builder
                                .Append("<span class=\"AddCat\"><a id=\"addChildTo")
                                .Append(childCategory.SupportLevelID)
                                .Append("\" title=\"Add Child Category\" href=\"javascript:void(0);\">+</a></span>");
                        }
                        var data = (TodaJerarquias.Where((obj) => obj.ParentSupportLevelID == childCategory.SupportLevelID)).OrderBy((obj)=>obj.SortIndex).ToList(); //SupportLevels.Search(childCategory.SupportLevelID);
                        if (data.Count > 0)
                        {
                            BuildTree(data, builder, level + 1, TodaJerarquias);
                        }
                        builder.Append("</li>");
                    }
                    builder.Append("</ul>");
                }

            }
            catch (Exception ex)
            {
                txtLog("Metodo BuildTree - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Get(int levelId)
        {
            try
            {

                SupportLevelSearchData content = new SupportLevelSearchData();
                content = SupportLevels.Get(levelId);
                return Json(new
                {

                    result = true,
                    levelId = levelId,
                    name = content.Name,
                    description = content.Description,
                    visible = content.IsVisibleToWorkStation,
                    active = content.Active,
                    parentSupportLevelID = content.ParentSupportLevelID,
                    sortIndex = content.SortIndex,

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

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Move(int parentId, List<int> levelIds)
        {
            try
            {
                for (short i = 0; i < levelIds.Count; i++)
                {
                    int SupportLevelID = levelIds[i];
                    SupportLevelSearchData content = new SupportLevelSearchData();
                    content = SupportLevels.Get(SupportLevelID);

                    if (content.SortIndex != i || content.ParentSupportLevelID != parentId)
                    {
                        content.SortIndex = i;
                        content.ParentSupportLevelID = parentId;
                        SupportLevels.Update(content);
                    }
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                txtLog("Metodo Move - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [ValidateInput(false)]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Save(int? levelId, string name, int sortIndex, string descripction, int parentId, int languageId, bool visible, bool active)
        {
            try
            {

                SupportLevelSearchData level = new SupportLevelSearchData();

                //SupportLevelSearchData level;
                //if (levelId.HasValue)
                //    level = SupportLevels.Get(levelId);
                //else
                //{
                //level = new SupportLevelSearchData();
                //};

                level.Name = name;
                level.Description = descripction;
                level.IsVisibleToWorkStation = visible;
                level.Active = active;
                level.SortIndex = sortIndex;
                level.ParentSupportLevelID = parentId;
                level.SupportLevelID =levelId.HasValue?  levelId.Value:0;


                //level.Save();
                if (levelId.HasValue)
                    SupportLevels.Update(level);
                else
                    SupportLevels.Save(level);
                //modificado por salcedo vila G.
                var listParents = SupportLevels.TraerJeraquiaSupportLevel(0);
                StringBuilder categoryBuilder = new StringBuilder();
                BuildTree(listParents.Where((obj) => obj.ParentSupportLevelID == 0).ToList(), categoryBuilder, 1, listParents.Where((obj) => obj.ParentSupportLevelID != 0).ToList());
               
              

                return Json(new
                {
                    result = true,
                    levelId = level.SupportLevelID,
                    levels = categoryBuilder.ToString()
                });
            }
            catch (Exception ex)
            {
                txtLog("Metodo Save - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
                InsErrorLog(CoreContext.CurrentUser.UserID, DateTime.Now, ex.Message);

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Delete(int levelId)
        {
            try
            {
                var TermMensaje = SupportLevels.Delete(levelId);
                string mensajeError = string.Empty;
                string[] Terms = null;
                Boolean result = true;
                if (TermMensaje.Trim() != "")
                {
                    Terms = TermMensaje.Split(';');
                    result = false;
                    if (Terms.Length==2)
                    {
                         mensajeError= String.Format("{0};{1}",Translation.GetTerm(Terms[0]),Translation.GetTerm(Terms[1]));
                    }else
                    {
                        mensajeError = String.Format("{0}", Translation.GetTerm(TermMensaje));
                    }
                }
                //Agregado por salcedo vila G.
                var listParents = SupportLevels.TraerJeraquiaSupportLevel(0);
                StringBuilder categoryBuilder = new StringBuilder();
                BuildTree(listParents.Where((obj) => obj.ParentSupportLevelID == 0).ToList(), categoryBuilder, 1, listParents.Where((obj) => obj.ParentSupportLevelID != 0).ToList());

                
//Translation.GetTerm("NoteNotSavedNoSubjectOrTextCouldBeFoundForThisNote", "Note not saved.  No subject or text could be found for this note.") });
                return Json(new { result = result, message = mensajeError, levels = categoryBuilder.ToString() });
            }
            catch (Exception ex)
            {
                txtLog("Metodo Delete - Level Contoller" + "-" + DateTime.Now.ToString() + "-" + ex.Message);
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

        public void txtLog(string mensaje)
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


    }
}
