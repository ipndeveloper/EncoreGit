using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public class DWS_MyAccount_ProductCredit_Page : DWS_MyAccount_Base_Page
    {
        private Div _ledger;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _ledger = Document.GetElement<Div>(new Param("ledgerContainer"));
        }

         public override bool IsPageRendered()
        {
            return _ledger.Exists || Document.GetElement<TableCell>(new Param("No ledger entries.", AttributeName.ID.InnerText)).Exists;
        }
    }
}
