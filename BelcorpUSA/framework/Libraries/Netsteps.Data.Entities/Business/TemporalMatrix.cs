using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business
{
    public class TemporalMatrix
    {
         
        public  List<ProductTemporalSearchData>  Lista()
        {
            List<ProductTemporalSearchData> oList = new List<ProductTemporalSearchData> ();
            oList = DataAccess.ExecWithStoreProcedureLists<ProductTemporalSearchData>("Core", "uspGetProducts").ToList();
            return oList.ToList();
        }

        public static int InsTemporalMatrix(TemporalMatrixSearchParameters parameters)
        {
            try
            {
                return TemporalMatrixEstensions.InsTemporalMatrix(parameters);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleteTemporalMatrix()
        {
            try
            {
                TemporalMatrixEstensions.DeleteTemporalMatrix();
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            } 
        }

        
    }
}
