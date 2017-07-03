using System;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class HeaderTotalsModel
    {
        #region Resources
        public virtual int Count { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual IFormatProvider FormatProvider { get; set; }
        #endregion

        #region Infrastructure
        public virtual HeaderTotalsModel LoadResources(
            int count,
            decimal amount,
            IFormatProvider formatProvider)
        {
            this.Count = count;
            this.Amount = amount;
            this.FormatProvider = formatProvider;

            return this;
        }
        #endregion
    }
}