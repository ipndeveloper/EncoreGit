using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_ValidateConfig_Page : GMP_Admin_Base_Page
    {
        Link _ping;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _ping = Document.GetElement<Link>(new Param("btnPingAvatax"));
        }

         public override bool IsPageRendered()
        {
            return _ping.Exists;
        }
    }
}
