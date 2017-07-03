namespace NetSteps.Data.Entities.Business.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Repositories;

    /// <summary>
    /// Metodos de Title
    /// </summary>
    public class TitleBusinessLogic
    {
        /// <summary>
        /// Obtiene todas los titulos activos
        /// </summary>
        /// <returns>Lista de Titulos</returns>
        public IEnumerable<Title> GetAllActives()
        {
            return from r in repository.GetAllActives() select DtoToBO(r);
        }

        /// <summary>
        /// Obtiene una instancia singleton de la clase TitleBusinessLogic
        /// </summary>
        public static TitleBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TitleBusinessLogic();
                    //IOC
                    repository = new TitleRepository();
                }

                return instance;
            }
        }

        #region Constructor - singleton - Inyection

        /// <summary>
        /// Convierte dto a Title
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private Title DtoToBO(NetSteps.Data.Entities.Dto.TitleDto dto)
        {
            return new Title()
            {
                Active = dto.Active,
                ClientCode = dto.ClientCode,
                ClientName = dto.ClientName,
                Name = dto.Name,
                ReportVisibility = dto.ReportVisibility,
                SortOrder = dto.SortOrder,
                TermName = dto.TermName,
                TitleCode = dto.TitleCode,
                TitleId = dto.TitleId
            };
        }

        /// <summary>
        /// Previene una instancia por defecto de la clase TitleBusinessLogic
        /// </summary>
        private TitleBusinessLogic()
        { }

        /// <summary>
        /// Establece u obtiene una implementacion de ITitleRepository
        /// </summary>
        private static ITitleRepository repository;

        /// <summary>
        /// Establece u optiene el valor de TitleBusinessLogic
        /// </summary>
        private static TitleBusinessLogic instance;
        #endregion
    }
}
