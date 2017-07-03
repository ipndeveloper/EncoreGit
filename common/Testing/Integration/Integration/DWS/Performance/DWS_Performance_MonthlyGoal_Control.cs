using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    /// <summary>
    /// Monthly Goal
    /// Not implimented by all clients
    /// </summary>
    public class DWS_Performance_MonthlyGoal_Control : Control<Div>
    {
        public bool IsValid
        {
            get { return Element.GetElement<Link>(new Param("Button updateGoal editGoal", AttributeName.ID.ClassName)).Exists; }
        }
    }
}
