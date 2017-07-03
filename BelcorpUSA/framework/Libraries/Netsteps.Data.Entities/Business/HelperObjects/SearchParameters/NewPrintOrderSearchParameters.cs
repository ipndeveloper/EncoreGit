using System;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;


namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class NewPrintOrderSearchParameters : FilterDateRangePaginatedListParameters<NewPrintOrder>
    {
        public int? ID { get; set; }
        public int? StatusID { get; set; }
        public int? SectionID { get; set; }
        public string Nombre { get; set; }
        public int? PediodoID { get; set; }
        public int? TemplateID { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
    }
}
