using WatiN.Core;
using NetSteps.Testing.Integration;

namespace NetSteps.Testing.Integration
{
    /// <summary>
    /// Class related to GMP unverified address page.
    /// </summary>
    public class UnverifiedAddress_Page : NS_Page
    {
        private Div _divUnverifiedAddress;
        private Link _lnkContinueWithThisAddress;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            this._divUnverifiedAddress = Document.GetElement<Div>(new Param("UnverifiedAddress"));
            this._lnkContinueWithThisAddress = this._divUnverifiedAddress.GetElement<Link>(new Param("FR mb10 Button btnContinue useTypedAddress", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return _divUnverifiedAddress.IsVisible();
        }

        /// <summary>
        /// Click continue with this address.
        /// </summary>
         public void ClickContinueWithThisAddress(int? timeout = null)
        {
            _divUnverifiedAddress.GetElement<Link>(new Param("FR mb10 Button btnContinue useTypedAddress", AttributeName.ID.ClassName)).CustomClick(timeout);
        }
    }
}
