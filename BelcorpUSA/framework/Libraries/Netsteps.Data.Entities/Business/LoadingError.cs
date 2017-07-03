using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    [Table("LoadingErrors")]
    public class LoadingError
    {
        [Column("AccountNumber")]
        [Required(ErrorMessage = "Required")]
        public string AccountNumber { get; set; }
    }
}
