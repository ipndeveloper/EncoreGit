
namespace NetSteps.Common.Validation.NetTiers
{
    public class ValidationResult
    {
        public bool IsValid
        {
            get
            {
                return _brokenRulesList == null || _brokenRulesList.Count == 0;
            }
        }
        private BrokenRulesList _brokenRulesList = new BrokenRulesList();
        public BrokenRulesList BrokenRulesList
        {
            get { return _brokenRulesList; }
            set { _brokenRulesList = value; }
        }
    }
}
