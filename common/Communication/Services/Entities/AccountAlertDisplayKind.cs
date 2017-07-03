using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Communication.Services.Entities
{
    [Table("AccountAlertDisplayKinds", Schema = "Communication")]
    public class AccountAlertDisplayKind
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountAlertDisplayKindId { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string TermName { get; set; }
        public bool Active { get; set; }
    }
}
