using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
    [Table("Holiday")]
    public class Holiday
    {
        [Column("HolidayID"), Key]
        public int HolidayID { get; set; }

        [Column("CountryID")]
        public int CountryID { get; set; }

        [Column("StateID")]
        public int? StateID { get; set; }

        [Column("DateHoliday")]        
        public DateTime DateHoliday { get; set; }

        [Column("IsIterative")]
        public bool IsIterative { get; set; }

        [Column("Reason")]
        [Required(ErrorMessage="Required")]
        public string Reason { get; set; }

        [Column("Active")]
        public bool Active { get; set; }

        [Column("LastUpdatedUTC")]
        public DateTime? LastUpdatedUTC { get; set; }

        #region no mapped

        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string StateProvinceName { get; set; }

        #endregion
    }
}
