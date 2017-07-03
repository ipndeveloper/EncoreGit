using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_AlertEdit_Page : GMP_Comunication_Base_Page
    {
        private SelectList _priority;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _priority = Document.SelectList("alertPriority");
        }

         public override bool IsPageRendered()
        {
            return _priority.Exists;
        }
    }
}
