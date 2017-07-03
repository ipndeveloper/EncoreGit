
namespace NetSteps.Data.Entities.Commissions
{
	public class CommissionJob
	{
		public int CommissionRunID { get; set; }
		public int PlanID { get; set; }
		public int CommissionRunTypeID { get; set; }
		public string CommissionRunType { get; set; }
		public string JobName { get; set; }
		public string JobDisplayName { get; set; }
	}
}
