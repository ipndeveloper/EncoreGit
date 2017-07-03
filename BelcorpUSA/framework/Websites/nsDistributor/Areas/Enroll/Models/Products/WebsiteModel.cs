using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.Products
{
	public class WebsiteModel : SectionModel, IValidatableObject
	{
		#region Values

		[NSDisplayName("MyPersonalWebsite", "My Personal Website")]
		[NSRegularExpression(@"^[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*$")]
		public virtual string Subdomain { get; set; }

		public virtual bool Skippable { get; set; }

		public virtual bool IsRequirementMet
		{
			get
			{
				return CurrentOrderValue >= RequiredOrderValue;
			}
		}
		public virtual decimal RequiredOrderValue { get; set; }
		public virtual decimal CurrentOrderValue { get; set; }
		#endregion

		#region Resources

		public virtual string Domain { get; set; }

		#endregion

		#region Infrastructure

		public virtual string Url
		{
			get
			{
				string result = null;
				if (!String.IsNullOrWhiteSpace(Subdomain) && !String.IsNullOrWhiteSpace(Domain))
				{
					result = String.Format("http://{0}.{1}", Subdomain, Domain);
				}
				return result;
			}
		}

		public virtual WebsiteModel LoadValues(string subDomain, bool skippable, decimal requiredOrderValue, decimal currentOrderValue)
		{
			Subdomain = subDomain;
			RequiredOrderValue = requiredOrderValue;
			CurrentOrderValue = currentOrderValue;

			if (!IsRequirementMet)
			{
				Skippable = true;
			}
			else
			{
				Skippable = skippable;
			}

			return this;
		}

		public virtual WebsiteModel LoadResources(
			string domain)
		{
			this.Domain = domain;

			return this;
		}

		public virtual WebsiteModel ApplyTo(
			IEnrollmentContext enrollmentContext)
		{
			enrollmentContext.SiteSubscriptionUrl = this.Subdomain;

			return this;
		}

		public override SectionModel LoadBaseResources(bool active, string action, string title, string partialViewName, bool completed)
		{
			string pViewName = partialViewName;
			string a = action;
			if (!IsRequirementMet && !completed)
			{
				pViewName = "_WebsiteRequirementsUnmet";
				a = "WebsiteRequirementsUnmet";
			}
			return base.LoadBaseResources(active, a, title, pViewName, completed);
		}

		#endregion

		public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!Skippable && string.IsNullOrWhiteSpace(Url))
				yield return new ValidationResult(Translation.GetTerm("PWS_Enroll_Website_MyPersonalWebsiteIsRequired", "My Personal Website is required."));
		}
	}
}