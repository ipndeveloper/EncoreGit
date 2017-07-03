using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.Products
{
	public class EnrollmentKitsModel : SectionModel, IValidatableObject
	{
		#region Values
		public virtual int? SelectedProductID { get; set; }

		// Required - see comment in SelectedProductID
		public virtual IEnumerable<int> SelectedProductIds
		{
			get { return this.EnrollmentKits.Where(x => x.Selected).Select(x => x.ProductID); }
			set
			{
				foreach (var enrollments in this.EnrollmentKits)
					enrollments.Selected = value.Any(selectedId => enrollments.ProductID == selectedId);
			}
		}
		#endregion

		#region Resources
		public virtual IFormatProvider FormatProvider { get; set; }
		#endregion

		#region Models
		public virtual List<EnrollmentKitModel> EnrollmentKits { get; set; }
		#endregion

		#region Infrastructure
		public EnrollmentKitsModel()
		{
			this.EnrollmentKits = new List<EnrollmentKitModel>();
		}

		public virtual EnrollmentKitsModel LoadValues(
			int? selectedProductID)
		{
			this.SelectedProductID = selectedProductID;

			return this;
		}

        public virtual EnrollmentKitsModel LoadResources(
            IFormatProvider formatProvider,
            IEnumerable<Product> enrollmentKitProducts,
            IEnrollmentContext enrollmentContext)
        {
            this.FormatProvider = formatProvider;

            this.EnrollmentKits.Clear();
            foreach (var product in enrollmentKitProducts)
            {
                
                    var model = new EnrollmentKitModel();
                    model.LoadResources(product, enrollmentContext);
                    this.EnrollmentKits.Add(model);
            }

            return this;
        }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!HasSelectedProducts())
			{
				yield return new ValidationResult(NetSteps.Common.Globalization.Translation.GetTerm("ErrorEnrollmentKitRequired", "Please select an enrollment kit."));
			}
		}

		private bool HasSelectedProducts()
		{
			return SelectedProductID.HasValue || SelectedProductIds.Any();
		}
		#endregion
	}
}