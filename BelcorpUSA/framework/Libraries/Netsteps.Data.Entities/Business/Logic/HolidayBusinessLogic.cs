using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using System.Linq.Expressions;

namespace NetSteps.Data.Entities.Business.Logic
{
    /// <summary>
    /// Clase para las llamadas al repositorio y logica de negocio
    /// </summary>
    public class HolidayBusinessLogic : IDisposable
    {
        #region Constructor

        protected static readonly Lazy<HolidayBusinessLogic> instance = new Lazy<HolidayBusinessLogic>(() => new HolidayBusinessLogic());

        public static HolidayBusinessLogic Instance
        {
            get { return instance.Value; }
        }

        #endregion

        /// <summary>
        /// Insertar nueva entidad tipo Holiday
        /// </summary>
        /// <param name="holiday">Entidad Holiday</param>
        /// <returns>Retorna la cantidad de elementos afectados</returns>
        public int InserHoliday(Holiday holiday)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.InserHoliday(holiday);
            }
        }

        /// Modificar entidad tipo Holiday
        /// </summary>
        /// <param name="holiday">Entidad Holiday</param>
        /// <returns>Retorna la cantidad de elementos afectados</returns>
        public int UpdateHoliday(Holiday holiday)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.UpdateHoliday(holiday);
            }
        }

        /// <summary>
        /// Elimina logicamente una entidad tipo Holiday
        /// </summary>
        /// <param name="holidayID">Primary key de la tabla en la DB</param>
        /// <returns>Cantidad de elementos afectados</returns>
        public int DeleteteHoliday(int holidayID)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.DeleteteHoliday(holidayID);
            }
        }

        /// <summary>
        /// Obtener todos por medio de la expresion enviada
        /// </summary>
        /// <param name="predicate">expresion Linq</param>
        /// <returns>coleccion de elementos tipo Holiday</returns>
        public IEnumerable<Holiday> Filter(Expression<Func<Holiday, bool>> predicate)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.Filter(predicate);
            }
        }

        /// <summary>
        /// Obtener todos por medio de la expresion enviada con paginación
        /// </summary>
        /// <param name="predicate">expresion Linq</param>
        /// <returns>coleccion de elementos tipo Holiday</returns>
        public IEnumerable<Holiday> Filter(Expression<Func<Holiday, bool>> predicate, int pageIndex, int pageSize, string orderBy, int direction, out int totalRows)
        {
            List<Holiday> listHoliday = new List<Holiday>();
            var result = new List<Holiday>();

            using (var repository = new HolidayRepository())
            {
                result = repository.Filter(predicate, pageIndex, pageSize, orderBy, direction, out totalRows);
            }

            var listCountryID = Country.LoadBatch(new List<int>() { 73 });

            listHoliday.AddRange(result.Select(x => new Holiday
            {
                HolidayID = x.HolidayID,
                CountryID = x.CountryID,
                CountryName = Country.LoadBatch(new List<int>() { 73 }).FirstOrDefault().Name,
                StateID = x.StateID,
                StateProvinceName = Convert.ToInt32(x.StateID) == 0 ? " - " : StateProvince.LoadBatch(new List<int>() { (int)x.StateID }).FirstOrDefault().Name,
                DateHoliday = x.DateHoliday,
                IsIterative = x.IsIterative,
                Reason = x.Reason
            }));

            return listHoliday;
        }

        /// <summary>
        /// Obtiene un elemento segun la expresion enviada
        /// </summary>
        /// <param name="predicate">Expresion Linq</param>
        /// <returns>Entidad tipo Holiday</returns>
        public Holiday Single(Expression<Func<Holiday, bool>> predicate)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.Single(predicate);
            }
        }

        /// <summary>
        /// cambiael estado de una entidad tipo Holiday
        /// </summary>
        /// <param name="holidayID">Primary key de la tabla en la DB</param>
        /// <returns>Cantidad de elementos afectados</returns>
        public int ChangeStatusHoliday(int holidayID, bool status)
        {
            using (var repository = new HolidayRepository())
            {
                return repository.ChangeStatusHoliday(holidayID, status);
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
