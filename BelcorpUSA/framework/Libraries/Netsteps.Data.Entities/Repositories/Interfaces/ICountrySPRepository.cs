namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    /// <summary>
    /// Interface de Country
    /// </summary>
    public interface ICountrySPRepository
    {
        /// <summary>
        /// Obtiene el Country Id por su nombre
        /// </summary>
        /// <param name="name">Nombre Country</param>
        /// <returns>Country Id</returns>
        int CountryIdByName(string name);
    }
}
