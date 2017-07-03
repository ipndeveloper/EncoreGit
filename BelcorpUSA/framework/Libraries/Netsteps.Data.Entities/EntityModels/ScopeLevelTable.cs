using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.EntityModels
{
    [Table("ScopeLevels")]
    public class ScopeLevelTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ScopeLevelID"), Key]
        public int ScopeLevelID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }
    }
}