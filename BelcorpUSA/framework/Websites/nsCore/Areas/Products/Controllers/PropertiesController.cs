using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Products.Controllers
{
    public class PropertiesController : BaseController
    {
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
        {
            var properties = ProductPropertyType.Search(new FilterPaginatedListParameters<ProductPropertyType>()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection
            });

            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (ProductPropertyType property in properties)
            {
                builder.Append("<tr>");
                if (property.Editable)
                    builder.AppendCheckBoxCell(value: property.ProductPropertyTypeID.ToString());
                else
                    builder.Append("<td></td>");
                builder.AppendLinkCell("~/Products/Properties/Edit/" + property.ProductPropertyTypeID, property.GetTerm())
                    .AppendCell(property.Required ? Translation.GetTerm("Yes") : Translation.GetTerm("No"))
                    .AppendCell(property.ProductPropertyValues.Select(v => v.Value).Join(", "))
                    .Append("</tr>");
                ++count;
            }
            return Json(new { result = true, totalPages = properties.TotalPages, page = builder.ToString() });
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                foreach (int productPropertyTypeId in items)
                {
                    ProductPropertyType.Delete(productPropertyTypeId);
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Edit(int? id)
        {
            ProductPropertyType property;
            if (id.HasValue && id.Value > 0)
            {
                property = ProductPropertyType.LoadFull(id.Value);
            }
            else
                property = new ProductPropertyType() { Editable = true };
            return View(property);
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult DeleteValue(int valueId)
        {
            try
            {
                ProductPropertyValue.Delete(valueId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult Save(int? propertyTypeId, string name, bool required, bool isProductVariantProperty, bool isMaster, bool showNameAndThumbnail, string dataType, List<ProductPropertyValue> values, short? htmlInputTypeId = null)
        {
            try
            {
                ProductPropertyType property;
                if (propertyTypeId.HasValue && propertyTypeId.Value > 0)
                {
                    property = ProductPropertyType.LoadFull(propertyTypeId.Value);
                }
                else
                {
                    var existingPropertyWithSameName = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.ProductPropertyTypes.Where(p => p.Name == name.ToCleanString()).FirstOrDefault();
                    if (existingPropertyWithSameName != null)
                        return Json(new { result = false, message = Translation.TermTranslation.GetTerm("PropertyAlreadyExists", "A Property with the same name already exists") });

                    property = new ProductPropertyType();
                    property.Editable = true;
                }

                if (name.IsNullOrEmpty())
                    return Json(new { result = false, message = "Please enter a property name." });

                property.Required = required;
                property.IsProductVariantProperty = isProductVariantProperty;
                property.IsMaster = isMaster;
                property.ShowNameAndThumbnail = showNameAndThumbnail;
                property.DataType = string.IsNullOrEmpty(dataType) ? "System.String" : dataType;
                property.HtmlInputTypeID = htmlInputTypeId;

                if (property is ITermName)
                {
                    if (property.Name.IsNullOrEmpty())
                        property.Name = name.ToCleanString();

                    var termNameObj = property as ITermName;
                    if (termNameObj != null)
                    {
                        string typeName = termNameObj.GetType().Name;
                        if (termNameObj.TermName.IsNullOrEmpty() || !termNameObj.TermName.ToCleanString().StartsWith(typeName))
                            termNameObj.TermName = string.Format("{0}_{1}", typeName, property.Name);

                        TermTranslation term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(termNameObj.TermName, CoreContext.CurrentLanguageID); // Just edit the currently selected language term for now - JHE
                        if (term != null)
                        {
                            term.Term = name.ToCleanString();
                            term.Save();
                        }
                        else
                        {
                            if (CoreContext.CurrentLanguageID != Constants.Language.English.ToInt())
                            {
                                term = new TermTranslation();
                                term.LanguageID = Constants.Language.English.ToInt();
                                term.TermName = termNameObj.TermName;
                                term.Term = name.ToCleanString();
                                term.Active = true;
                                term.Save();
                            }
                            term = new TermTranslation();
                            term.LanguageID = CoreContext.CurrentLanguageID;
                            term.TermName = termNameObj.TermName;
                            term.Term = name.ToCleanString();
                            term.Active = true;
                            term.Save();
                        }
                    }
                }
                else
                    property.Name = name;

                if (values != null)
                {
                    foreach (ProductPropertyValue value in values)
                    {
                        if (value.ProductPropertyValueID > 0)
                        {
                            ProductPropertyValue existingProductPropertyValue = property.ProductPropertyValues.First(v => v.ProductPropertyValueID == value.ProductPropertyValueID);
                            existingProductPropertyValue.Value = value.Value;
                            existingProductPropertyValue.FilePath = value.FilePath;
                            //<!--filepath-->
                        }

                        else
                            property.ProductPropertyValues.Add(new ProductPropertyValue()
                            {
                                Value = value.Value,
                                Name = "",
                                FilePath = value.FilePath
                            });
                    }
                    List<ProductPropertyValue> removed = property.ProductPropertyValues.Where(v => !values.Any(val => val.ProductPropertyValueID == v.ProductPropertyValueID)).ToList();
                    foreach (ProductPropertyValue removedValue in removed)
                        property.ProductPropertyValues.RemoveAndMarkAsDeleted(removedValue);
                }

                property.Save();

                return Json(new { result = true, propertyTypeId = property.ProductPropertyTypeID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult UploadImage(int productPropertyTypeId, int valueId)
        {
            try
            {
                string newFileName = string.Empty;
                string newFullFileName = string.Empty;
                if (Request.Files.Count > 0)
                {

                    FileInfo fileInfo = new FileInfo(Request.Files[0].FileName);
                    string fileName = fileInfo.Name,
                        folder = "ProductProperties",
                        absoluteFolder = ConfigurationManager.GetAbsoluteFolder(folder),
                        path = ConfigurationManager.GetWebFolder(folder) + fileName;

                    if (System.IO.File.Exists(absoluteFolder + fileName))
                        fileName.Insert(fileName.LastIndexOf('.'), Guid.NewGuid().ToString("N"));

                    newFileName = fileName;
                    newFullFileName = absoluteFolder + fileName;
                    Request.Files[0].SaveAs(newFullFileName);
                    if (valueId > 0)
                    {
                        ProductPropertyValue ppv = ProductPropertyValue.LoadFull(valueId);
                        ppv.FilePath = "<!--filepath-->" + (!string.IsNullOrEmpty(folder) ? (folder + "/") : "") + fileName;
                        ppv.Save();
                    }

                    return Content(new
                    {
                        result = true,
                        imagePath = path
                    }.ToJSON(), "text/html");
                }
                else
                {
                    return Content(new
                    {
                        result = false,
                        message = Translation.GetTerm("NoFileUploaded", "No file uploaded")
                    }.ToJSON(), "text/html");
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Content(new
                {
                    result = false,
                    message = exception.PublicMessage
                }.ToJSON(), "text/html");
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult RenderPropertyValue(int productPropertyTypeId, int valueId)
        {
            try
            {
                ProductPropertyType type = null;
                if (productPropertyTypeId > 0)
                {
                    type = ProductPropertyType.LoadFull(productPropertyTypeId);
                }
                ProductPropertyValue propertyValue = new ProductPropertyValue();
                if (valueId > 0)
                {
                    propertyValue = type.ProductPropertyValues.FirstOrDefault(ppv => ppv.ProductPropertyValueID == valueId);
                }
                else
                {
                    if (productPropertyTypeId > 0)
                        type.ProductPropertyValues.Add(propertyValue);
                }
                return Json(new { result = true, valueHTML = RenderPartialToString("PropertyValue", propertyValue) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
