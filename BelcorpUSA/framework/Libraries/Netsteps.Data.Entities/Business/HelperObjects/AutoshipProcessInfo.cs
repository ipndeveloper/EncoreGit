
namespace NetSteps.Data.Entities.Business.HelperObjects
{
	public class AutoshipProcessInfo
	{
		public int AutoshipOrderID { get; set; }
        public int AutoshipScheduleID { get; set; }
        public int TemplateOrderID { get; set; }
		public string TemplateOrderNumber { get; set; }
		public int AccountID { get; set; }
        public string AccountNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
