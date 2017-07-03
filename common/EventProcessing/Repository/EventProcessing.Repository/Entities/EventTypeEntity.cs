using System.ComponentModel.DataAnnotations;

namespace NetSteps.EventProcessing.Repository.Entities
{
	[Table("EventTypes", Schema = "Event")]
	public class EventTypeEntity
	{
		[Key]
		public int EventTypeID { get; set; }
		public string Name { get; set; }
		public string TermName { get; set; }
		public string Description { get; set; }
		public string EventHandler { get; set; }
		public byte? MaxRetryCount { get; set; }
		public int RetryInterval { get; set; }
		public bool Enabled { get; set; }
	}
}
