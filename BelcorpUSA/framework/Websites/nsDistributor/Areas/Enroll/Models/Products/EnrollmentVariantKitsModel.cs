using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.Products
{
	public class EnrollmentVariantKitsModel : EnrollmentKitsModel
	{
		public override EnrollmentKitsModel LoadResources(
			IFormatProvider formatProvider,
			IEnumerable<Product> enrollmentKitProducts,
			IEnrollmentContext enrollmentContext)
		{
			this.FormatProvider = formatProvider;

			this.EnrollmentKits.Clear();
			foreach (var product in enrollmentKitProducts)
			{
				var model = new EnrollmentVariantKitModel();
				model.LoadResources(product, enrollmentContext);
				this.EnrollmentKits.Add(model);
			}

			return this;
		}

		public virtual int? SelectedVariantProductID { get; set; }

		public EnrollmentVariantKitModel SelectedKit { get; set; }

		// Required - see comment in SelectedProductID
		public override IEnumerable<int> SelectedProductIds
		{
			get { return this.EnrollmentKits.Where(x => x.Selected).Select(x => x.ProductID); }
			set
			{
				foreach (var enrollments in this.EnrollmentKits)
					enrollments.Selected = value.Any(selectedId => enrollments.ProductID == selectedId);
			}
		}
	}
}
