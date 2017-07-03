using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_BarChart_Control : DWS_Performance_TitleProgress_Control
    {
        float _personalVolume;
        float _teamVolume;
        int _activeLegs;
        float _downlineVolume;
        int _teamLeadLegs;
        int _lastTeamLead;
        int _car;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _personalVolume = float.Parse(GetStringValue(0));
            _teamVolume = float.Parse(GetStringValue(1));
            _activeLegs = int.Parse(GetStringValue(2));
            _downlineVolume = float.Parse(GetStringValue(3));
            _teamLeadLegs = int.Parse(GetStringValue(4));
            _lastTeamLead = int.Parse(GetStringValue(5));
            _car = int.Parse(GetStringValue(6));
        }

        public float PersonalVolume
        {
            get { return _personalVolume; }
        }

        public float TeamVolume
        {
            get { return _teamVolume; }
        }

        public int ActiveLegs
        {
            get { return _activeLegs; }
        }

        public float DownlineVolume
        {
            get {return _downlineVolume; }
        }

        public int TeamLeadLegs
        {
            get { return _teamLeadLegs; }
        }

        public int LastTeamLead
        {
            get { return _lastTeamLead; }
        }

        public int Car
        {
            get { return _car; }
        }

        private string GetStringValue(int index)
        {
            string str = Element.ElementWithTag("tspan", Find.ByIndex(index)).CustomGetText();
            int start = str.IndexOf('(') + 1;
            return str.Substring(start, str.Length - start - 1);
        }

        public bool IsValid(DWS_Performance_BarChart_Control barChartControl)
        {
            return
                (
                _personalVolume == barChartControl.PersonalVolume
                && _teamLeadLegs == barChartControl.TeamLeadLegs
                && _activeLegs == barChartControl.ActiveLegs
                && _downlineVolume == barChartControl.DownlineVolume
                && _teamLeadLegs == barChartControl.TeamLeadLegs
                && _lastTeamLead == barChartControl.LastTeamLead
                && _car == barChartControl.Car
                );

        }

    }
}
