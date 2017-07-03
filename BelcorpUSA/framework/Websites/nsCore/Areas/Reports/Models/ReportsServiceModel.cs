namespace nsCore.Areas.Reports.Models
{
    public class ReportsServiceModel
    {
        public string reportID { get; set; }
        public string reportName { get; set; }
        public string reportURL { get; set; }
    }

    public class ReportsServiceProcModel
    {
        public string reportID { get; set; }
        public string reportName { get; set; }
        public string reportURL { get; set; }
        public string RolesReport { get; set; }
    }

    public class ReportsServiceROLModel
    {
        public int listRolesReport { get; set; }
    }

}