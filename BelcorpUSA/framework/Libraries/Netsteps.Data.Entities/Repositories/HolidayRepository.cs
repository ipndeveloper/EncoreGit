using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Repositorio para los metodos utilizados con la entidad Holiday
    /// </summary>
    public class HolidayRepository : IDisposable
    {
        /// <summary>
        /// Insertar nueva entidad tipo Holiday
        /// </summary>
        /// <param name="holiday">Entidad Holiday</param>
        /// <returns>Retorna la cantidad de elementos afectados</returns>
        public int InserHoliday(Holiday holiday)
        {
            using (var context = new EntityDBContext())
            {
                //holiday.LastUpdatedUTC = DateTime.UtcNow;
                context.Holidays.Add(holiday);

                return context.SaveChanges();
            }
        }

        /// Modificar entidad tipo Holiday
        /// </summary>
        /// <param name="holiday">Entidad Holiday</param>
        /// <returns>Retorna la cantidad de elementos afectados</returns>
        public int UpdateHoliday(Holiday holiday)
        {
            using (var context = new EntityDBContext())
            {
                var entity = context.Holidays.FirstOrDefault(x => x.HolidayID == holiday.HolidayID);

                entity.CountryID = holiday.CountryID;
                entity.StateID = holiday.StateID;
                entity.DateHoliday = holiday.DateHoliday;
                entity.IsIterative = holiday.IsIterative;
                entity.Reason = holiday.Reason;
                entity.Active = holiday.Active;

                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina logicamente una entidad tipo Holiday
        /// </summary>
        /// <param name="holidayID">Primary key de la tabla en la DB</param>
        /// <returns>Cantidad de elementos afectados</returns>
        public int DeleteteHoliday(int holidayID)
        {
            using (var context = new EntityDBContext())
            {
                var entity = context.Holidays.FirstOrDefault(x => x.HolidayID == holidayID);
                entity.Active = false;

                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Obtener todos por medio de la expresion enviada
        /// </summary>
        /// <param name="predicate">expresion Linq</param>
        /// <returns>coleccion de elementos tipo Holiday</returns>
        public IEnumerable<Holiday> Filter(Expression<Func<Holiday, bool>> predicate)
        {
            using (var context = new EntityDBContext())
            {
                return context.Holidays.Where(predicate);
            }
        }

        /// <summary>
        /// Obtener todos por medio de la expresion enviada con paginación
        /// </summary>
        /// <param name="predicate">expresion Linq</param>
        /// <returns>coleccion de elementos tipo Holiday</returns>
        public List<Holiday> Filter(Expression<Func<Holiday, bool>> predicate, int pageIndex, int pageSize, string orderBy, int direction, out int totalRows)
        {
            int rowIndex = pageIndex < 2 ? 0 : pageIndex * pageSize;

            using (var context = new EntityDBContext())
            {
                totalRows = context.Holidays.Where(predicate).Count();

                return context.Holidays.Where(predicate).OrderByDescending(x => x.LastUpdatedUTC).Skip(rowIndex).Take(pageSize).ToList();              
            }
        }

       

        public static PaginatedList<HolidaySearchData> SearchDetails(HolidaySearchParameter searchParameter)
        {
            List<HolidaySearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<HolidaySearchData>("Core", "upsGetHolidays",

                new SqlParameter("CountryID", SqlDbType.Int) { Value = (object)searchParameter.CountryID ?? DBNull.Value },
                new SqlParameter("StateID", SqlDbType.Int) { Value = (object)searchParameter.StateID ?? DBNull.Value },
                new SqlParameter("DateHoliday", SqlDbType.VarChar) { Value = searchParameter.DateHoliday }
                ).ToList();

            IQueryable<HolidaySearchData> matchingItems = paginatedResult.AsQueryable<HolidaySearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<HolidaySearchData>(searchParameter, resultTotalCount);
        }

        public static bool ValidateHoliday(HolidaySearchParameter searchParameter)
        {
           

            bool rpta = false;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@HolidayID", searchParameter.HolidayID },
                                                                                           { "@StateID", searchParameter.StateID },
                                                                                            { "@DateHoliday", string.IsNullOrEmpty(searchParameter.DateHoliday) ? null : (DateTime?) Convert.ToDateTime(searchParameter.DateHoliday) }};
                SqlCommand cmd = DataAccess.GetCommand("upsValidateHolidays", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = true;
                }
            }
            catch (Exception ex)
            {
                rpta = true;
              //  throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
           
            
        }

        /// <summary>
        /// Obtiene un elemento segun la expresion enviada
        /// </summary>
        /// <param name="predicate">Expresion Linq</param>
        /// <returns>Entidad tipo Holiday</returns>
        public Holiday Single(Expression<Func<Holiday, bool>> predicate)
        {
            using (var context = new EntityDBContext())
            {
                return context.Holidays.Where(predicate).FirstOrDefault();
            }
        }

        /// <summary>
        /// cambia el estado de una entidad tipo Holiday
        /// </summary>
        /// <param name="holidayID">Primary key de la tabla en la DB</param>
        /// <returns>Cantidad de elementos afectados</returns>
        public int ChangeStatusHoliday(int holidayID, bool status)
        {
            using (var context = new EntityDBContext())
            {
                var entity = context.Holidays.FirstOrDefault(x => x.HolidayID == holidayID);
                entity.Active = status;

                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Implementa la interface IDisposable por que vamos a utilizar Using
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
            //throw new NotImplementedException();
        }
    }
}
