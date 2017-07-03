using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace nsDistributor.Areas.Enroll.Models.Agreements
{
    public class AgreementModel
    {
        #region Values
        [NSRequired]
        public virtual int? PolicyID { get; set; }
        [NSRequireChecked(TermName = "PolicyAcceptanceRequired", ErrorMessage = "You must agree to continue")]
        public virtual bool Accepted { get; set; }
        #endregion

        #region Resources
        public virtual string Name { get; set; }
        public virtual string CssClass { get; set; }
        public virtual string Url { get; set; }
        public virtual MvcHtmlString Content { get; set; }
        #endregion Resources

        #region Infrastructure
        public virtual AgreementModel LoadValues(int policyID, bool accepted)
        {
            this.PolicyID = policyID;
            this.Accepted = accepted;

            return this;
        }

        public virtual AgreementModel LoadResources(
            string name,
            string cssClass,
            string url,
            MvcHtmlString content)
        {
            Name = name;
            CssClass = cssClass;
            Url = url;
            Content = content;

            return this;
        }
        #endregion
    }
}
