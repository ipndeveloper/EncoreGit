using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    /// <summary>
    /// Volume Compare
    /// Not implemented by all clients
    /// </summary>
    public class DWS_Performance_VolumeCompare_Control : Control<Div>
    {
        public bool IsValid
        {
            get { return true; }
        }
    }
}
