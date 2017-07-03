using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public class DWS_MyAccount_Autoship_Control :Control<Div>
    {
        private Link _button, _edit;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _button = Element.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName));
            _edit = Element.GetElement<Link>(new Param("EditMyAccount", AttributeName.ID.ClassName, RegexOptions.None));

        }
        public DWS_MyAccount_Autoship_Page ClickEnrollNow(int? timeout = null, bool pageRequired = true)
        {
            timeout = _button.CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_Autoship_Page>(timeout, pageRequired);
        }

        public DWS_MyAccount_Autoship_Page ClickEdit(int? timeout = null, bool pageRequired = true)
        {
            timeout = _edit.CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_Autoship_Page>(timeout, pageRequired);
        }

        public bool Button
        {
            get { return _button.Exists; }
        }

        public string Type
        {
            get { return Element.Elements[0].CustomGetText(); }
        }
    }
}
