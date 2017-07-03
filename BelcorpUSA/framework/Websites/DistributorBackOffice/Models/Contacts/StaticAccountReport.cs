
namespace DistributorBackOffice.Models.Contacts
{
    public class StaticAccountReport
    {
        private string _termDefault;
        public string AccountType { get; set; }
        public string Term { get; set; }
        public string TermDefault
        {
            get
            {
                return string.IsNullOrEmpty(_termDefault) ? Term : _termDefault;
            }
            set
            {
                _termDefault = value;
            }
        }
    }
}