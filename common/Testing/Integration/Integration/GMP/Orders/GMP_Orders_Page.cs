using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_Page : GMP_Orders_Base_Page
    {
        private TextField _search;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _search = Document.GetElement<TextField>(new Param("txtSearch"));
        }

         public override bool IsPageRendered()
        {
            return _search.Exists;
        }
    }
}
