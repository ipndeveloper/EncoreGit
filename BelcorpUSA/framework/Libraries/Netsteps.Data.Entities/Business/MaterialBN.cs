using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{

    // Creador de la Clase : IPN
    public class MaterialBN
    {
        // Function Lookup Materials
        // Developed  by IPN
        public  PaginatedList<MaterialsSearchData> Search(MaterialsSearchParameters orderSearchParameters)
        {


            try
            {

                MaterialRepository Repository = new MaterialRepository();

                return Repository.Search(orderSearchParameters);
                //return BusinessLogic.Search(Repository, orderSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }




        public static MaterialsSearchData MaterialData(int Id)
        {
            try
            {
                return MaterialRepository.MaterialData(Id);
               
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public void Register(MaterialsSearchData oMaterialsSearchData)
        {


            try
            {

                MaterialRepository Repository = new MaterialRepository();

                Repository.MovementsMaterial(oMaterialsSearchData);
                //return BusinessLogic.Search(Repository, orderSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

       public static System.Collections.Generic.Dictionary <int, string> GetMaterialSearchByTextResults(string text)
        {
            Dictionary<int, string> results;
            try
            {
                    results = MaterialRepository.SearchMaterialsByText(text);

                    return results;
            }
            catch (Exception ex)
            {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

       }
       public static void ChangeState( int id)
       {

           try
           {
               MaterialRepository.ChangeState(id);
           }
           catch (Exception ex)
           {
               throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
           }
       }
       public static MaterialsSearchData ValidateEAN(MaterialsSearchData ObjMaterial)
       {
           try
           {
               return MaterialRepository.ValidateEAN(ObjMaterial);
           }
           catch (Exception ex)
           {
               throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
           }
       }

       public static MaterialsSearchData ValidateEAN2(MaterialsSearchData ObjMaterial)
       {
           try
           {
               return MaterialRepository.ValidateEAN2(ObjMaterial);
           }
           catch (Exception ex)
           {
               throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
           }
       }

       public static string ActiveDeactive(List<int> ListMaterials, bool Active)
       {
           try
           {
               return MaterialRepository.ActiveDeactive(ListMaterials, Active);
           }
           catch (Exception ex)
           {
               throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
           }
       }

       public static List<BrandSP> GetBrand(int languageID)
       {
           try
           {
               return MaterialRepository.GetBrand(languageID);
           }
           catch (Exception ex)
           {
               throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
           }
       }


    }
}
