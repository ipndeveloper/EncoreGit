using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;

using System.Data.SqlClient;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Repositories
{
    public class ProductMatrixRepository : IProductMatrixRepository
    {
        public PaginatedList<ProductMatrix> GetMatrixErrorLog(ProductMatrixParameters searchParams)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString);

            try
            {
                #region Parameters

                List<ProductMatrixDto> lstProductMatrixDto = new List<ProductMatrixDto>();
                SqlParameter[] LstParameter = new SqlParameter[] 
                { 
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.NVarChar, Value = searchParams.CUV, ParameterName = "CUVCode"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.Int, Value = searchParams.MaterialID, ParameterName = "SAPSKU"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.Int, Value = searchParams.LanguageID, ParameterName = "LanguageID"},
                };

                #endregion

                #region GetData

                connection.Open();
                SqlDataReader dr = DataAccess.queryDatabase("[dbo].[upsGetMatrixErrorLog]", connection, LstParameter);

                while (dr.Read())
                {
                    ProductMatrixDto item = new ProductMatrixDto();
                    if (!dr.IsDBNull(dr.GetOrdinal("CUVCode"))) item.CUV = Convert.ToString(dr["CUVCode"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("SAPSKU"))) item.MaterialID = Convert.ToInt32(dr["SAPSKU"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("TermName"))) item.Descripcion = Convert.ToString(dr["TermName"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Message"))) item.Mensaje = Convert.ToString(dr["Message"]);
                    lstProductMatrixDto.Add(item);
                }

                #endregion

                #region Convert to

                List<ProductMatrix> result = new List<ProductMatrix>();

                for (int i = 0; i < lstProductMatrixDto.Count(); i++)
                {
                    result.Add((ProductMatrix)lstProductMatrixDto[i]);
                }

                #endregion

                #region Pagination

                IQueryable<ProductMatrix> matchingItems = result.AsQueryable<ProductMatrix>();
                var resultTotalCount = matchingItems.Count();
                if (searchParams.PageSize >= 0)
                {
                    matchingItems = matchingItems.ApplyPagination(searchParams);
                    return matchingItems.ToPaginatedList<ProductMatrix>(searchParams, resultTotalCount);
                }
                else
                {
                    return matchingItems.ToPaginatedList<ProductMatrix>(searchParams, resultTotalCount);
                }

                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
