using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    /// <summary>
    /// Class related to controls and ops of DWS Contacts page.
    /// </summary>
    public class DWS_Contacts_Page : DWS_Contacts_Base_Page
    {
        private Table _contacts;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _contacts = Document.GetElement<Table>(new Param("paginatedGrid"));
        }

        public override bool IsPageRendered()
        {
            return Util.Browser.Url.Contains("/Contacts");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeIndex">Index of the Type Field</param>
        /// <param name="index"></param>
        /// <param name="timeout"></param>
        /// <param name="delay"></param>
        /// <param name="pageRequired"></param>
        /// <returns></returns>
        public DWS_Contacts_Contact_Control GetContact(int typeIndex, int? index = null, int? timeout = null, int? delay = 2, bool pageRequired = true)
        {
            _contacts.CustomWaitForSpinner(timeout, delay);
            TableRowCollection rows = _contacts.TableBody(Find.Any).OwnTableRows;
            if (!index.HasValue)
                index = Util.GetRandom(0, rows.Count - 1);
            return rows[(int)index].As<DWS_Contacts_Contact_Control>().SetTypeIndex(typeIndex);
        }
    }
}