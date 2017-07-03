using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Extensions;


namespace NetSteps.Data.Entities.Extensions
{
    public class NewPrintOrderExtensions
    {

        public static Dictionary<int, string> ListPeriod()
        {
            List<NewPrintOrderPeriodo> PeriodResult = DataAccess.ExecWithStoreProcedureLists<NewPrintOrderPeriodo>("Core", "uspListPeriod").ToList();
            Dictionary<int, string> PeriodResultDic = new Dictionary<int, string>();
            foreach (var item in PeriodResult)
            {
                PeriodResultDic.Add(item.PeriodID, Convert.ToString(item.PeriodID));
            }
            return PeriodResultDic;
        }

        public static Dictionary<short, string> ListGeneralSection()
        {
            List<NewPrintOrderGeneralTemplateSection> SectionResult = DataAccess.ExecWithStoreProcedureLists<NewPrintOrderGeneralTemplateSection>("Core", "uspListGeneralTemplateSection").ToList();
            Dictionary<short, string> SectionResultDic = new Dictionary<short, string>();
            foreach (var item in SectionResult)
            {
                SectionResultDic.Add(item.GeneralTemplateSectionID, item.Name);
            }
            return SectionResultDic;
        }

        public static PaginatedList<NewPrintOrderSearchData> Search(NewPrintOrderSearchParameters searchParameter)
        {
            List<NewPrintOrderSearchData> paginatedResult = new List<NewPrintOrderSearchData>();
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspListSearchGeneralTemplates"))
                {
                    comando.Parameters.Add("@StatusID", SqlDbType.Int).Value = searchParameter.StatusID;
                    comando.Parameters.Add("@SectionID", SqlDbType.Int).Value = searchParameter.SectionID;
                    comando.Parameters.Add("@NamePlantilla", SqlDbType.VarChar, 510).Value = searchParameter.Nombre;
                    comando.Parameters.Add("@PeriodID", SqlDbType.Int).Value = searchParameter.PediodoID;

   
                    if (searchParameter.StartDate != null)
                        comando.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = searchParameter.StartDate;
                    else
                        comando.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = DBNull.Value;

                    if (searchParameter.StartDate != null)
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = searchParameter.EndDate;
                    else
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DBNull.Value;

                      
                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        NewPrintOrderSearchData entidad = new NewPrintOrderSearchData();
                        entidad.GeneralTemplateID = DataConvertDA.ObjectToInt32(lector["GeneralTemplateID"]);
                        entidad.Name = DataConvertDA.ObjectToString(lector["Name"]);
                        entidad.Section = DataConvertDA.ObjectToString(lector["Section"]);
                        entidad.Order = DataConvertDA.ObjectToInt32(lector["OrderTemplate"]);
                        entidad.Statu = DataConvertDA.ObjectToBool(lector["Statu"]);
                        entidad.DateStar = DataConvertDA.ObjectToDateTimeNull(lector["DateStar"]);
                        entidad.DateEnd = DataConvertDA.ObjectToDateTimeNull(lector["DateEnd"]);
                        entidad.Period = DataConvertDA.ObjectToInt32Null(lector["Period"]);
                        paginatedResult.Add(entidad);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            IQueryable<NewPrintOrderSearchData> matchingItems = paginatedResult.AsQueryable<NewPrintOrderSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<NewPrintOrderSearchData>(searchParameter, resultTotalCount);
        }

        public static List<NewPrintOrderSearchData> SlimSearch(string query, int? pageSize = 1000)
        {
            List<NewPrintOrderSearchData> SlimSearchResult = new List<NewPrintOrderSearchData>();
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspSlimSearchGeneralTemplates"))
                {
                    comando.Parameters.Add("@NameTemplate", SqlDbType.VarChar, 510).Value = query;

                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        NewPrintOrderSearchData entidad = new NewPrintOrderSearchData();
                        entidad.GeneralTemplateID = DataConvertDA.ObjectToInt32(lector["GeneralTemplateID"]);
                        entidad.Name = DataConvertDA.ObjectToString(lector["Name"]);

                        SlimSearchResult.Add(entidad);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            return SlimSearchResult;
        }

        public static int InsertGeneralTemplate(NewPrintOrderSearchData dataParameter)
        {
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.spInsertGeneralTemplate"))
                {

                    comando.Parameters.Add("@GeneralTemplateID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@GeneralTemplateSectionID", SqlDbType.SmallInt).Value = dataParameter.GeneralTemplateSectionID;
                    comando.Parameters.Add("@Name", SqlDbType.VarChar,510).Value = dataParameter.Name;
                    comando.Parameters.Add("@OrderTemplate", SqlDbType.Int).Value = dataParameter.Order;
                    if (dataParameter.DateStar != null)
                        comando.Parameters.Add("@StarDate", SqlDbType.DateTime).Value = dataParameter.DateStar;
                    else
                        comando.Parameters.Add("@StarDate", SqlDbType.DateTime).Value = DBNull.Value;

                    if (dataParameter.DateEnd != null)
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dataParameter.DateEnd;
                    else
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DBNull.Value;

                    comando.Parameters.Add("@PeriodID", SqlDbType.Int).Value = dataParameter.Period;
                    comando.Parameters.Add("@Active", SqlDbType.Bit).Value = dataParameter.Statu;

                    comando.ExecuteNonQuery();
                    dataParameter.GeneralTemplateID = int.Parse(comando.Parameters["@GeneralTemplateID"].Value.ToString());
                   
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }
            return dataParameter.GeneralTemplateID;
        }

        public static void  UpdateGeneralTemplate(NewPrintOrderSearchData dataParameter)
        {
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.spUpdateGeneralTemplate"))
                {
                    comando.Parameters.Add("@GeneralTemplateID", SqlDbType.Int).Value = dataParameter.GeneralTemplateID;
                    comando.Parameters.Add("@GeneralTemplateSectionID", SqlDbType.SmallInt).Value = dataParameter.GeneralTemplateSectionID;
                    comando.Parameters.Add("@Name", SqlDbType.VarChar, 510).Value = dataParameter.Name;
                    comando.Parameters.Add("@OrderTemplate", SqlDbType.Int).Value = dataParameter.Order;
                    if (dataParameter.DateStar != null)
                        comando.Parameters.Add("@StarDate", SqlDbType.DateTime).Value = dataParameter.DateStar;
                    else
                        comando.Parameters.Add("@StarDate", SqlDbType.DateTime).Value = DBNull.Value;

                    if (dataParameter.DateEnd != null)
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dataParameter.DateEnd;
                    else
                        comando.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DBNull.Value;

                    comando.Parameters.Add("@PeriodID", SqlDbType.Int).Value = dataParameter.Period;
                    comando.Parameters.Add("@Active", SqlDbType.Bit).Value = dataParameter.Statu;

                    comando.ExecuteNonQuery();

                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }
        }

        public static NewPrintOrderSearchData GetGeneralTemplate(int GeneralTemplateID)
        {
            NewPrintOrderSearchData entidad = new NewPrintOrderSearchData();
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspGetGeneralTemplates"))
                {
                    comando.Parameters.Add("@GeneralTemplateID", SqlDbType.Int).Value = GeneralTemplateID;

                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        entidad.GeneralTemplateID = DataConvertDA.ObjectToInt32(lector["GeneralTemplateID"]);
                        entidad.Name = DataConvertDA.ObjectToString(lector["Name"]);
                        entidad._GeneralTemplateSectionID = DataConvertDA.ObjectToInt16(lector["GeneralTemplateSectionID"]);
                        entidad.Order = DataConvertDA.ObjectToInt32(lector["OrderTemplate"]);
                        entidad.DateStar = DataConvertDA.ObjectToDateTimeNull(lector["DateStar"]);
                        entidad.DateEnd = DataConvertDA.ObjectToDateTimeNull(lector["DateEnd"]);
                        entidad.Period = DataConvertDA.ObjectToInt32Null(lector["PeriodID"]);
                        entidad.Statu = DataConvertDA.ObjectToBool(lector["Statu"]);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            return entidad;
        }

        public static GeneralTemplateTranslations GetGeneralTemplateTranslation(int GeneralTemplateTranslationsID)
        {
            GeneralTemplateTranslations objGeneralTemplateTranslations = new GeneralTemplateTranslations();

            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspGetGeneralTemplatesTranslation"))
                {
                    comando.Parameters.Add("@GeneralTemplateTranslationID", SqlDbType.Int).Value = GeneralTemplateTranslationsID;

                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        objGeneralTemplateTranslations.GeneralTemplateTranslationID = DataConvertDA.ObjectToInt32(lector["GeneralTemplateTranslationID"]);
                        objGeneralTemplateTranslations.LanguageID = DataConvertDA.ObjectToInt32(lector["LanguageID"]);
                        objGeneralTemplateTranslations.Body = DataConvertDA.ObjectToString(lector["Body"]);
                        objGeneralTemplateTranslations.Active = DataConvertDA.ObjectToBool(lector["Active"]);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            return objGeneralTemplateTranslations;
        }

        public static List<GeneralTemplateTranslations> GetGeneralTemplateTranslations(int GeneralTemplateID)
        {
            List<GeneralTemplateTranslations> lstGeneralTemplateTranslations = new List<GeneralTemplateTranslations>();
            
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.uspGetGeneralTemplatesTranslations"))
                {
                    comando.Parameters.Add("@GeneralTemplateID", SqlDbType.Int).Value = GeneralTemplateID;
                    
                    SqlDataReader lector = comando.ExecuteReader();
                    while (lector.Read())
                    {
                        GeneralTemplateTranslations objGeneralTemplateTranslations = new GeneralTemplateTranslations();
                        objGeneralTemplateTranslations.GeneralTemplateTranslationID = DataConvertDA.ObjectToInt32(lector["GeneralTemplateTranslationID"]);
                        objGeneralTemplateTranslations.LanguageID = DataConvertDA.ObjectToInt32(lector["LanguageID"]);
                        objGeneralTemplateTranslations.Body = DataConvertDA.ObjectToString(lector["Body"]);
                        objGeneralTemplateTranslations.Active = DataConvertDA.ObjectToBool(lector["Active"]);
                        lstGeneralTemplateTranslations.Add(objGeneralTemplateTranslations);
                    }
                    lector.Close();
                }
            }
            catch (SqlException e)
            {
                //notificarError(e);
            }

            return lstGeneralTemplateTranslations;
        }

        public static void DelGeneralTemplateTranslations(int GeneralTemplateTranslationID)
        {
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.spDeleteGeneralTemplateTranslation"))
                {
                    comando.Parameters.Add("@GeneralTemplateTranslationID", SqlDbType.Int).Value = GeneralTemplateTranslationID;
                    comando.ExecuteScalar();
                }

            }
            catch (SqlException e)
            {
                //notificarError(e);
            }
        }

        public static void EditGeneralTemplateTranslations(GeneralTemplateTranslations GeneralTemplateTranslations)
        {

            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.spUpdateGeneralTemplateTranslation"))
                {
                    comando.Parameters.Add("@GeneralTemplateTranslationID", SqlDbType.Int).Value = GeneralTemplateTranslations.GeneralTemplateTranslationID;
                    comando.Parameters.Add("@LanguageID", SqlDbType.Int).Value = GeneralTemplateTranslations.LanguageID;
                    comando.Parameters.Add("@Body", SqlDbType.NVarChar,-1).Value = GeneralTemplateTranslations.Body;
                    comando.Parameters.Add("@Active", SqlDbType.Bit).Value = GeneralTemplateTranslations.Active;
                    comando.ExecuteNonQuery();
                }

            }
            catch (SqlException e)
            {
                //notificarError(e);
            }
        }

        public static void InsertGeneralTemplateTranslations(GeneralTemplateTranslations GeneralTemplateTranslations)
        {
            try
            {
                using (SqlCommand comando = ConfiguracionDAC.obtenerComando("dbo.spInsertGeneralTemplateTranslation"))
                {
                    comando.Parameters.Add("@GeneralTemplateID", SqlDbType.Int).Value = GeneralTemplateTranslations.GeneralTemplateID;
                    comando.Parameters.Add("@LanguageID", SqlDbType.Int).Value = GeneralTemplateTranslations.LanguageID;
                    comando.Parameters.Add("@Body", SqlDbType.NVarChar, -1).Value = GeneralTemplateTranslations.Body;
                    comando.Parameters.Add("@Active", SqlDbType.Bit).Value = GeneralTemplateTranslations.Active;
                    comando.ExecuteNonQuery();
                }

            }
            catch (SqlException e)
            {
                //notificarError(e);
            }
        }


    }
}
