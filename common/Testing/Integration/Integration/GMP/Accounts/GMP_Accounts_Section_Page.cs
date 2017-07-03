using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    /// <summary>
    /// Abstract class for pages that have Section Navigation
    /// </summary>
    public abstract class GMP_Accounts_Section_Page : GMP_Accounts_Base_Page
    {
        
        public GMP_Accounts_SectionNav_Control SectionNav
        {
            get { return Control.CreateControl<GMP_Accounts_SectionNav_Control>(_secNav); }
        }

        public string GetID()
        {
            Para status = _content.GetElement<Para>(new Param("DistributorStatus", AttributeName.ID.ClassName));
            return status.CustomGetText().Substring(1, status.Text.IndexOf(',') - 1);
        }
    }
}
