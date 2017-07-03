using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class AutoshipBundleModel
    {
        #region Resources
        public virtual int ProductID { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Name { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string LongDescription { get; set; }
        public virtual bool Selected { get; set; }
        #endregion

        #region Infrastructure
        public virtual AutoshipBundleModel LoadResources(
            Product product,
            IEnrollmentContext enrollmentContext)
        {
            this.ProductID = product.ProductID;
            this.Price = product.GetPrice(enrollmentContext.AccountTypeID, enrollmentContext.CurrencyID, null);
            this.Name = product.Name;
            this.ShortDescription = (product.Translations != null && product.Translations.Count > 0
                                         ? product.Translations[0].ShortDescription
                                         : string.Empty);
            this.LongDescription = (product.Translations != null && product.Translations.Count > 0
                                         ? product.Translations[0].LongDescription
                                         : string.Empty);
            return this;
        }

        public virtual bool Matches(AutoshipBundleModel model)
        {
            return model.ProductID == this.ProductID;
        }
        #endregion
    }
}