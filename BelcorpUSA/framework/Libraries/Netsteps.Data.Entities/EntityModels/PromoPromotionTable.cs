namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using System;

    [Table("Promotions", Schema="Promo")]
    public class PromoPromotionTable
    {
        [Column("PromotionID"),Key]
        public int PromotionID { get; set; }

        [Column("StartDate")]
        public DateTime? StartDate { get; set; }

        [Column("EndDate")]
        public DateTime? EndDate { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("PromotionStatusTypeID")]
        public int PromotionStatusTypeID { get; set; }

        [Column("SuccessorPromotionID")]
        public int? SuccessorPromotionID { get; set; }

        [Column("PromotionKind")]
        public string PromotionKind { get; set; }
    }
}
