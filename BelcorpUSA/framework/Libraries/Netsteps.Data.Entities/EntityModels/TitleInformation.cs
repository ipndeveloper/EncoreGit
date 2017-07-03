using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class TitleInformation
    {
        public int TitleIDCareer { get; set; }
        public string CareerAsTitle { get; set; }
        public string TermCareer { get; set; }
        public int TitleIDPaid { get; set; }
        public string PaidAsTitle { get; set; }
        public string TermPaid { get; set; }
    } 
}
