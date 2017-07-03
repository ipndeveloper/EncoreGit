using System.ComponentModel.DataAnnotations;

namespace NetSteps.Events.AccountArguments.Repository.Entities
{
	[Table("AccountEventArguments", Schema = "Event")]
	public class AccountEventArgumentEntity
	{
		[Key]
		public int ArgumentID { get; set; }
		public int EventID { get; set; }
		public int AccountID { get; set; }
	}
}
