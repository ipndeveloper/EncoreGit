using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{

    public class Report
    {
        private ReportCategory _parentCategory;
        public string Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ReportCategory ParentCategory
        {
            get { return _parentCategory; }
        }

        public Report(ReportCategory category)
        {
            _parentCategory = category;
        }

        public static List<Report> LoadReports(ReportCategory category)
        {
            return ReportRepository.LoadAllByCategory(category);
        }

    }
}
