using System.ComponentModel.DataAnnotations;

namespace NetSteps.Events.EmailEventTemplate.Repository.Entities
{
	[Table("EmailEventEmailTemplates", Schema = "Event")]
	public class EmailEventTemplateEntity
	{
		[Key]
		public int EmailEventEmailTemplateID { get; set; }
		public int EventTypeID { get; set; }
		public int EmailTemplateID { get; set; }
	}
}
