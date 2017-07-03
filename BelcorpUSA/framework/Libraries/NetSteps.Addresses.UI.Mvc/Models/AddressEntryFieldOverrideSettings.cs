
namespace NetSteps.Addresses.UI.Mvc.Models
{
    public class AddressEntryFieldOverrideSettings
    {
        public BoolOverrideSetting FieldEnabled { get; set; }
        public BoolOverrideSetting FieldIncluded { get; set; }
        /// <summary>
        /// Leave blank to not override
        /// </summary>
        public string LabelOverride { get; set; }
        public string FieldName { get; set; }
    }
}
