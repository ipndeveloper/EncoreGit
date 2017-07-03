using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Utility;
using System.Xml;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Configuration; 

namespace NetSteps.Data.Entities.Business.Logic
{
    public class XmlProductMaterialBusinessLogic
    {
        private XmlProductMaterialBusinessLogic()
        { 
        }

        private static XmlProductMaterialBusinessLogic instance;

        private static IXmlProductMaterialRepository repositoryXmlProductMaterial;

        public static XmlProductMaterialBusinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new XmlProductMaterialBusinessLogic();
                    repositoryXmlProductMaterial = new XmlProductMaterialRepository();
                }
                return instance;
            }
        }

        public int ExistMaterialBySKU(string SKU)
        {
            try
            {
                return repositoryXmlProductMaterial.ExistMaterialBySKU(SKU);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public int ValidarConfirmacionPagos(string COP, string VTD1, string VTD2)
        {
            try
            {
                return repositoryXmlProductMaterial.ValidarConfirmacionPagos(COP, VTD1, VTD2);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public string ReturnXMLB090(string fechaMov)
        {
            try
            {
                return repositoryXmlProductMaterial.ReturnXmlB090(fechaMov);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public string ReturnXMLE080WSAutenticacion(string Login, string Password)
        {
            try
            {
                return repositoryXmlProductMaterial.ReturnXMLE080WSAutenticacion(Login,Password);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public string ReturnXMLE080WSDatosConsultores(string Token, string Login, string CodConsultor)
        {
            try
            {
                return repositoryXmlProductMaterial.ReturnXMLE080WSDatosConsultores(Token, Login, CodConsultor);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public int ExistWareHouseByExternalCode(string externalCode)
        {
            try
            {
                return repositoryXmlProductMaterial.ExistWareHouseByExternalCode(externalCode);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public int ExisWareHouseMaterialByWareHouseMaterial(int WarehouseID, int MaterialID)
        {
            try
            {
                return repositoryXmlProductMaterial.ExisWareHouseMaterialByWareHouseMaterial(WarehouseID, MaterialID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public int ExisWareHouseMaterialBySKU_ExternalCode(string SKU, string ExternalCode)
        {
            try
            {
                return repositoryXmlProductMaterial.ExisWareHouseMaterialBySKU_ExternalCode(SKU, ExternalCode);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public int InsertWareHouseMaterial(int wareHouseID, int materialID, decimal saldo)
        {
            try
            {
                return repositoryXmlProductMaterial.InsertWareHouseMaterial(wareHouseID, materialID, saldo);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        public int UpdateInsertWareHouseMaterial(string SKU, string ExternalCode, string Saldo)
        {
            try
            {
                return repositoryXmlProductMaterial.UpdateInsertWareHouseMaterial(SKU, ExternalCode, Saldo);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        public List<DisbursementsSearchData> ObtenerDisbursementsService(int? periodo)
        {
            try
            {
                return repositoryXmlProductMaterial.ObtenerDisbursementsService(periodo);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public List<DisbursementProfilesSearchData> ObtenerDisbursementProfilesService(int? periodo)
        {
            try
            {
                return repositoryXmlProductMaterial.ObtenerDisbursementProfilesService(periodo);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public string GenerateXmlForProductMaterial(XmlDocument xmlString)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            //string TemplateProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateProducts"));
            string TemplateErrorProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateErrorProducts"));
            string TemplateErrorProductDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateDetailErrorProduct"));
            string TemplateErrorProductHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateHeaderErrorProduct"));
            
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.Load(string.Format("{0}{1}.xml", xmlPath, "TemplateProducts"));
            XmlNodeList saldos = xmlString.GetElementsByTagName("saldos");
            XmlNodeList product = ((XmlElement)saldos[0]).GetElementsByTagName("producto");
            StringBuilder XmlErrorDetail = new StringBuilder();
            XMLProductSearchData material = new XMLProductSearchData();
            Errores hijo = new Errores();
            material.Errores = new List<Errores>();
            string XmlReturn = "";
            StringBuilder XmlProduct = new StringBuilder();
            StringBuilder XmlProductDetail = new StringBuilder();  
            foreach (XmlElement nodo in product)
            { 
                int i = 0;
                XmlNodeList nSKU = nodo.GetElementsByTagName("SKU");
                XmlNodeList nSaldo = nodo.GetElementsByTagName("Saldo");
                XmlNodeList nCentro = nodo.GetElementsByTagName("Centro"); 
                if (ExistMaterialBySKU(nSKU[i].InnerText) > 0)
                {
                    //XMLProduct.Add(objE);
                    int WareHouseID = ExistWareHouseByExternalCode(nSaldo[i].InnerText);
                    if (WareHouseID > 0)
                    {
                        if (ExisWareHouseMaterialByWareHouseMaterial(Convert.ToInt32(nSKU[i].InnerText), WareHouseID) > 0)
                        {
                            //Actualización de tabla WareHouseMaterial
                            //repositoryXmlProductMaterial.InsertWareHouseMaterial(WareHouseID, Convert.ToInt32(nSKU[i].InnerText), Convert.ToDecimal(nSaldo[i].InnerText));

                        }
                        else
                        {
                            hijo.CampoError = "No existe WarehouseMaterials";
                            hijo.DescError = "WareHouseMaterials";
                        }
                    }
                    else
                    {
                        hijo.CampoError = "Centro de distribución no existe";
                        hijo.DescError = "WareHouses";
                    }
                }
                else
                {
                    hijo.CampoError = "Producto no existe";
                    hijo.DescError = "Products";
                }
                material.Errores.Add(hijo);
                foreach (var item in material.Errores)
                {
                    XmlProductDetail.AppendFormat(TemplateErrorProductDetail, item.DescError, item.CampoError);
                }

                XmlReturn += string.Format(TemplateErrorProductHeader, nSKU[i].InnerText, XmlProductDetail);
                material.Errores.Clear();
                XmlProductDetail.Clear();
            }
            string hola = string.Format(TemplateErrorProduct, XmlReturn);

            return hola;
        }

        //public string GenerateXmlForProductMaterialB020(string xml)
        //{

        //    string TemplateProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateProducts"));
        //    string TemplateErrorProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateErrorProducts"));
        //    string TemplateErrorProductDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateDetailErrorProduct"));
        //    string TemplateErrorProductHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateHeaderErrorProduct"));

        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(string.Format("{0}{1}.xml", xmlPath, "TemplateProducts"));
        //    XmlNodeList saldos = xDoc.GetElementsByTagName("saldos");
        //    XmlNodeList product = ((XmlElement)saldos[0]).GetElementsByTagName("producto");
        //    StringBuilder XmlErrorDetail = new StringBuilder();
        //    XMLProductSearchData material = new XMLProductSearchData();
        //    Errores hijo = new Errores();
        //    material.Errores = new List<Errores>();
        //    string XmlReturn = "";
        //    StringBuilder XmlProduct = new StringBuilder();
        //    StringBuilder XmlProductDetail = new StringBuilder();
        //    foreach (XmlElement nodo in product)
        //    {
        //        int i = 0;
        //        XmlNodeList nSKU = nodo.GetElementsByTagName("SKU");
        //        XmlNodeList nSaldo = nodo.GetElementsByTagName("Saldo");
        //        XmlNodeList nCentro = nodo.GetElementsByTagName("Centro");
        //        if (ExistMaterialBySKU(Convert.ToInt32(nSKU[i].InnerText)) > 0)
        //        {
        //            //XMLProduct.Add(objE);
        //            int WareHouseID = ExistWareHouseByExternalCode(Convert.ToInt32(nSaldo[i].InnerText));
        //            if (WareHouseID > 0)
        //            {
        //                if (ExisWareHouseMaterialByWareHouseMaterial(Convert.ToInt32(nSKU[i].InnerText), WareHouseID) > 0)
        //                {
        //                    //Actualización de tablas.
        //                }
        //                else
        //                {
        //                    hijo.CampoError = "No existe WarehouseMaterials";
        //                    hijo.DescError = "WareHouseMaterials";
        //                }
        //            }
        //            else
        //            {
        //                hijo.CampoError = "Centro de distribución no existe";
        //                hijo.DescError = "WareHouses";
        //            }
        //        }
        //        else
        //        {
        //            hijo.CampoError = "Producto no existe";
        //            hijo.DescError = "Products";
        //        }
        //        material.Errores.Add(hijo);
        //        foreach (var item in material.Errores)
        //        {
        //            XmlProductDetail.AppendFormat(TemplateErrorProductDetail, item.DescError, item.CampoError);
        //        }

        //        XmlReturn += string.Format(TemplateErrorProductHeader, nSKU[i].InnerText, XmlProductDetail);
        //        material.Errores.Clear();
        //        XmlProductDetail.Clear();
        //    }
        //    string hola = string.Format(TemplateErrorProduct, XmlReturn);

        //    return hola;
        //}
    }
}
