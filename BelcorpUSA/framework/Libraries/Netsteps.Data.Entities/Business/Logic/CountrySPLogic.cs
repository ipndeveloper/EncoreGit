namespace NetSteps.Data.Entities.Business.Logic
{
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Clase CountrySPLogic
    /// </summary>
    public partial class CountrySPLogic
    {
        /// <summary>
        /// Obtiene el valor Id de Country por nombre
        /// </summary>
        /// <param name="name">Country Name</param>
        /// <returns>Country Id</returns>
        public int CountryIdByName(string name)
        {
            return repository.CountryIdByName(name);
        }

        /// <summary>
        /// Previene la creacion por defecto de la clase CountrySPLogic
        /// </summary>
        private CountrySPLogic()
        {}

        /// <summary>
        /// Obtiene o establece una instancia de CountrySPLogic
        /// </summary>
        private static CountrySPLogic instance;

        /// <summary>
        /// Obtiene o establece una implementacion de ICountrySPRepository
        /// </summary>
        public static ICountrySPRepository repository;

        /// <summary>
        /// Obtiene una instancia singleton de la clase CountrySPLogic
        /// </summary>
        public static CountrySPLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new CountrySPLogic();
                    repository = new NetSteps.Data.Entities.Repositories.CountrySPRepository();
                }

                return instance;
            }
        }
    }
}
