using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class Titles
    {
        public int TitleId { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; } 
    }

    public class GetTitle
    {
        public bool Active { get; set; }
        public int SortOrder { get; set; }
        public string TermName { get; set; }
        public string TitleCode { get; set; }
        public int TitleID { get; set; }
    }
}
