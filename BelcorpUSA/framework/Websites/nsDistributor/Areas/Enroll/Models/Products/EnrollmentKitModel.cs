using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Enrollment.Common.Models.Context;
using System.Collections.Generic;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class EnrollmentKitModel
    {
        #region Resources
		public virtual int ProductID { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Name { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Selected { get; set; }

		#endregion

        #region Infrastructure
        public virtual EnrollmentKitModel LoadResources(
            Product product,
            IEnrollmentContext enrollmentContext)
        {
			this.ProductID = product.ProductID;
            this.Price = product.GetPrice(enrollmentContext.AccountTypeID, enrollmentContext.CurrencyID, enrollmentContext.InitialOrder.OrderTypeID);
            this.Name = product.Name;
            this.Description = product.Translations.GetByLanguageIdOrDefault(ApplicationContext.Instance.CurrentLanguageID).DetokenizeShortDescription();
            if (product.MainImage != null)
            {
                this.ImageUrl = product.MainImage.FilePath.ReplaceFileUploadPathToken();
            }

            return this;
        }
        #endregion
    }
}