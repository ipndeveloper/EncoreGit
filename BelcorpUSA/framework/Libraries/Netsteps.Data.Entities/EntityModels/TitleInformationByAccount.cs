using System.Text;

namespace NetSteps.Data.Entities.EntityModels
{
    public class TitleInformationByAccount
    {
        public int AccountId { get; set; }
        public int PeriodId { get; set; }
        public int TitleTypeID { get; set; }
        public string Name { get; set; }
        public int TitleID { get; set; }
        public string Titulo { get; set; }
     
    }
}
