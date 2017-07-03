using WatiN.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_Card_Control : Control<Div>
    {
        private List<string> _data = new List<string>();
        private Link _closeCard;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _closeCard = Element.GetElement<Link>(new Param("minimizeInfoCard"));
            _data.Add(_closeCard.CustomGetText());

            UnorderedList lst = Element.GetElement<UnorderedList>(new Param("lr dash flat info", AttributeName.ID.ClassName));

            for (int index = 0; index < Element.ListItems.Count; index++)
            {
                _data.Add(lst.ListItems[index].GetElement<Div>().CustomGetText());
            }
        }

        public string Name
        {
            get { return _data[0]; }
        }

        public string Level
        {
            get { return _data[1]; }
        }

        public string FlatDownlineCount
        {
            get { return _data[2]; }
        }

        public string AccountNumber
        {
            get { return _data[3]; }
        }

        public string EmailAddress
        {
            get { return _data[4]; }
        }

        public string Birthday
        {
            get { return _data[5]; }
        }

        public string PaidAsTitle
        {
            get { return _data[6]; }
        }

        public string CurrentTitle
        {
            get { return _data[7]; }
        }

        public string PersonalVolume
        {
            get { return _data[8]; }
        }

        public string TeamVolume
        {
            get { return _data[9]; }
        }

        public string AutoshipDate
        {
            get { return _data[10]; }
        }

        public string EnrollmentDate
        {
            get { return _data[11]; }
        }

        public string Location
        {
            get { return _data[12]; }
        }

        public string AccountType
        {
            get { return _data[13]; }
        }

        public string LastCommissionDate
        {
            get { return _data[14]; }
        }

        public string CommissionQualified
        {
            get { return _data[15]; }
        }

        public string TotalCommission
        {
            get { return _data[16]; }
        }

        public void CloseCard()
        {
            _closeCard.CustomClick(5);
        }
    }
}
