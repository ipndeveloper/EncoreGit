using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Transactions;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Business
{
     public class RenegotiationMethods 
    {
         public static PaginatedList<RenegotiationSearchData> BrowseRenegotiation(RenegotiationSearchParameters searchParams)
         {
             try
             {
                 return RenegotiationMethodsRepository.BrowseRenegotiation(searchParams);
             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }
         }

         //public static PaginatedList<RenegotiationDetSearchData> BrowseRenegotiationDet(RenegotiationDetSearchParameters searchParams)
         //{
         //    try
         //    {
         //        return RenegotiationMethodsRepository.BrowseRenegotiationDet(searchParams);
         //    }
         //    catch (Exception ex)
         //    {
         //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
         //    }
         //}
         

         public static RenegotiationSearchData DataRenegotiation(int id)
         {
             RenegotiationSearchData ns = new RenegotiationSearchData();
             //return ns;

             try
             {
                 return RenegotiationMethodsRepository.DataRenegotiation(id);
             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }
         }

         public static bool SaveStructuredReng(RenegotiationSearchData Renegotiation)//, List<RenegotiationDetSearchData> details)
         {
             try
             {
                 return RenegotiationMethodsRepository.SaveStructuredReng(Renegotiation);//, details);
             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }
         }

         #region Proces Renegotiation

         public static List<RenegotiationMethodDto> ListRenegotiationMethodsByOrder(RenegotiationMethodDto datRegMet)
         {
             try
             {
                 return RenegotiationMethodsRepository.ListRenegotiationMethodsByOrder(datRegMet);
             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }

         }

         public static RenegotiationSharedDto ListRenegotiationShares(RenegotiationMethodDto datRegMet)
         {
             try
             {
                 return RenegotiationMethodsRepository.ListRenegotiationShares(datRegMet);
             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }

         }

         public bool RegisterRenegotiationOrderPayment(string site, List<OrderPaymentNegotiationData> oOrderPaymentSearchData, int ultRegis, int numbercuotas, decimal DescuentoGlobal)
         {
             try
             {
                 RenegotiationMethodsRepository Repository = new RenegotiationMethodsRepository();
                 return Repository.RegisterRenegotiationOrderPayment(site, oOrderPaymentSearchData, ultRegis, numbercuotas, DescuentoGlobal);

             }
             catch (Exception ex)
             {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
             }
         }
         #endregion
    }
}
