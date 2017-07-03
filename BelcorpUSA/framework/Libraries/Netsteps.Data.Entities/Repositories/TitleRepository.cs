namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;
    
    public partial class TitleRepository : ITitleRepository
    {
        /// <summary>
        /// Gets all actives and SorOrder > 2
        /// </summary>
        /// <returns>List of Title Data Transfer Object</returns>
        public IEnumerable<TitleDto> GetAllActives()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.Titles
                            where r.Active && r.SortOrder > 2
                            orderby r.SortOrder ascending
                            select new TitleDto()
                            {
                                TitleId = r.TitleId,
                                ClientCode = r.ClientCode,
                                ClientName = r.ClientName,
                                Name = r.Name,
                                TermName = r.TermName,
                                TitleCode = r.TitleCode,
                                SortOrder = r.SortOrder,
                                ReportVisibility = r.ReportVisibility,
                                Active = r.Active
                            }).ToList();

                if (data == null)
                    throw new Exception("Titles not found");

                return data;
            }
        }
    }
}
