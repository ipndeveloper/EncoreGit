namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    
    public interface ITitleRepository
    {
        /// <summary>
        /// Gets all actives and SorOrder > 2
        /// </summary>
        /// <returns>List of Title Data Transfer Object</returns>
        IEnumerable<TitleDto> GetAllActives();
    }
}
