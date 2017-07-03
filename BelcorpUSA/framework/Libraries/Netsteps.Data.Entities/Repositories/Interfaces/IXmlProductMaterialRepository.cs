using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IXmlProductMaterialRepository
    {
        int ExistMaterialBySKU(string SKU);
        int ValidarConfirmacionPagos(string COP, string VTD1, string VTD2);
        int ExistWareHouseByExternalCode(string externalCode);
        int ExisWareHouseMaterialByWareHouseMaterial(int WarehouseID, int MaterialID);
        int ExisWareHouseMaterialBySKU_ExternalCode(string SKU,string ExternalCode);
        int InsertWareHouseMaterial(int wareHouseID, int materialID, decimal saldo);
        int UpdateInsertWareHouseMaterial(string SKU, string ExternalCode, string Saldo);
        string ReturnXmlB090(string FechaMov);
        string ReturnXMLE080WSAutenticacion(string Login, string Password);
        string ReturnXMLE080WSDatosConsultores(string Token, string Login, string CodConsultor);
        //int ExistWareHouseByExternalCode(string externalCode);
        //int ExisWareHouseMaterialByWareHouseMaterial(int WarehouseID,int MaterialID);
        //int InsertWareHouseMaterial(int wareHouseID, int materialID, int saldo);
        List<DisbursementsSearchData> ObtenerDisbursementsService(int? periodo);
        List<DisbursementProfilesSearchData> ObtenerDisbursementProfilesService(int? periodo);
    }
}
