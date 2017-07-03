using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.Helpers;


namespace nsCore.Areas.Products.Controllers
{
    public class MaterialsController : BaseProductsController
    {
        //
        // GET: /Products/Materials/

        public ActionResult Index()
        {

            int languageID = CurrentLanguageID;
            var listBrand = MaterialBN.GetBrand(languageID);
            Dictionary<int, string> dcBrand = new Dictionary<int, string>();
            dcBrand.Add(0, "All");
            foreach (var item in listBrand)
            {
                dcBrand.Add(item.BrandID, item.Name);
            }
            ViewBag.Brands = dcBrand;


            ViewData["PersonName"] = "Test Name";
            return View();
        }

        public ActionResult NewMaterial()
        {

            GetBrand();
            return View();
        }

        private void GetBrand()
        {
            int languageID = CurrentLanguageID;

            var catalogos = MaterialBN.GetBrand(languageID);
            if (catalogos == null)
            {
                catalogos = new List<BrandSP>();
            }
            {
                ViewData["BrandsID"] = catalogos;
            }
        
        }

        public virtual ActionResult ListMaterials(int page, int pageSize, int? accountNumberOrName, string BPCSCode, string BrandId, bool? Active, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
          

             MaterialBN ObjMaterial = new MaterialBN();

             

            StringBuilder builder = new StringBuilder();
            int count = 0;
            var Periods = ObjMaterial.Search(new  MaterialsSearchParameters ()
            {
                Active = Active,
                MaterialID = accountNumberOrName,
                Brand = BrandId,
                BPCSCode =BPCSCode,
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection
            });
            foreach (var catalog in Periods)
            {

                builder.Append("<tr>")
                            .AppendCheckBoxCell(value: catalog.MaterialID.ToString())
                            .AppendLinkCell("~/Products/Materials/EditMaterial/" + catalog.MaterialID.ToString(), catalog.SKU)
                            .AppendCell(catalog.Name)
                            //.AppendCell(catalog.SKU)
                            .AppendCell(catalog.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                            .AppendCell(catalog.EANCode.ToString())
                            .AppendCell(catalog.BPCSCode.ToString())
                            .AppendCell(catalog.UnityType.ToString())
                            .AppendCell(catalog.Weight.ToString("N",CoreContext.CurrentCultureInfo))
                            .AppendCell(catalog.Volume.ToString("N",CoreContext.CurrentCultureInfo))
                            //.AppendCell(catalog.NCM.ToString())
                            //.AppendCell(catalog.Origin.ToString())
                            //.AppendCell(catalog.OriginCountry.ToString())
                            .AppendCell(catalog.Brand.ToString())
                            .AppendCell(catalog.Group.ToString())
                         .Append("</tr>");
                ++count;



                //builder.Append("<tr>");

  
                //builder
                //    .AppendLinkCell("~/Products/Catalogs/Edit/" + catalog.MaterialID.ToString(), catalog.MaterialID.ToString())
                //    .AppendCell(!string.IsNullOrEmpty(catalog.Name.ToString()) ? catalog.Name.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.SKU.ToString()) ? catalog.SKU.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(catalog.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                //    .AppendCell(!string.IsNullOrEmpty(catalog.EANCode.ToString()) ? catalog.EANCode.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.BPCSCode.ToString()) ? catalog.BPCSCode.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.UnityType.ToString()) ? catalog.UnityType.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.Weight.ToString()) ? catalog.Weight.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")

                //    .AppendCell(!string.IsNullOrEmpty(catalog.Volume.ToString()) ? catalog.Volume.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.NCM.ToString()) ? catalog.NCM.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.Origin.ToString()) ? catalog.Origin.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.OriginCountry.ToString()) ? catalog.OriginCountry.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.Brand.ToString()) ? catalog.Brand.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .AppendCell(!string.IsNullOrEmpty(catalog.Group.ToString()) ? catalog.Group.ToString() : "<i class=\"LawyerText\">" + Translation.GetTerm("Unassigned") + "</i>")
                //    .Append("</tr>");
                ++count;
            }




            return Json(new { result = true, totalPages = Periods.TotalPages, page = Periods.TotalCount == 0 ? "<tr><td colspan=\"5\">There are no catalogs</td></tr>" : builder.ToString() });

            //return View();
        }



        public virtual ActionResult RegisterMaterials(MaterialsSearchData oenMaterial)
        {

            try
            {
                int idValidacionMaterialCode = 0, idValidacionEanCode = 0;
                

                if (MaterialBN.ValidateEAN(oenMaterial) != null)
                {
                    idValidacionMaterialCode = 1;
                }

                if (MaterialBN.ValidateEAN2(oenMaterial) != null)
                {
                    idValidacionEanCode = 2;
                }

                if (idValidacionMaterialCode == 1 && idValidacionEanCode == 2 )
                {
                    return Json(new { result = false, message = "El Material Code y el Ean Code se encuentra registrado" });
                }
                else if (idValidacionMaterialCode == 1)
                {
                    return Json(new { result = false, message = "El Material Code se encuentra registrado" });
                }
                else if (idValidacionEanCode == 2)
                {
                    return Json(new { result = false, message = "El Ean Code se encuentra registrado" });
                }
              
                MaterialBN  ObjMaterial = new MaterialBN();
                            ObjMaterial.Register(oenMaterial);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchFilter(string query)
        {
            

            try
            {
                var resultado = NetSteps.Web.Mvc.Extensions.DictionaryExtensions.ToAJAXSearchResults(MaterialBN.GetMaterialSearchByTextResults(query));
                return Json(resultado);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult EditMaterial(int id)
        {
            GetBrand();
            List<MaterialsSearchData> listObj = new List<MaterialsSearchData>();
            try
            {

                 var material =MaterialBN.MaterialData(id);
                if (material != null)
                {
                    listObj.Add(material);

                };
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
            return View(listObj.ToList());
        }

        public virtual ActionResult ToggleStatus( int Id)
        {
            try
            {
          
                    MaterialBN.ChangeState(Id);
                    return Json(new { result = true });
               
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        public virtual ActionResult ValidateEAN(MaterialsSearchData oenMaterial)
        {
            try
            {


                if (MaterialBN.ValidateEAN(oenMaterial) != null)
                {
                    return Json(new { result = true , existe =true });

                }
                else
                {
                    return Json(new { result = true, existe = false });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

          
        }

        [FunctionFilter("Products", "~/Accounts")]
        public virtual ActionResult ActiveDeactive(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    string result = MaterialBN.ActiveDeactive(items, active);

                    if (result.Length > 0)
                    {
                        result = Translation.GetTerm("NotAbleToUpdate", "Unable to Update the following Materials:") + " " + result + ".";
                        return Json(new { result = false, message = result });
                    }      
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }

            return Json(new { result = true });
        }


    }
}
