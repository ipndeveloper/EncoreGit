using System.ComponentModel.DataAnnotations;

namespace NetSteps.Events.PartyArguments.Repository.Entities
{
	[Table("PartyEventArguments", Schema = "Event")]
	public class PartyEventArgumentEntity
	{
		[Key]
		public int ArgumentID { get; set; }
		public int EventID { get; set; }
		public int PartyID { get; set; }
	}
}
