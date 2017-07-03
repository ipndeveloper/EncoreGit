using System.ComponentModel.DataAnnotations;

namespace NetSteps.Communication.Services.Entities
{
    [Table("MessageAccountAlerts", Schema = "Communication")]
    public class MessageAccountAlert
    {
        [Key, ForeignKey("AccountAlert")]
        public int AccountAlertId { get; set; }
        [Required]
        public string Message { get; set; }

        public virtual AccountAlert AccountAlert { get; set; }
    }
}
