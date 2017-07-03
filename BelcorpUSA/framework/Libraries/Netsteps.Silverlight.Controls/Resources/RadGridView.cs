using Telerik.Windows.Controls;

namespace NetSteps.Silverlight.Controls.Resources
{
    public class RadGridView : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                //---------------------- RadGridView Group Panel text:
                case "GridViewGroupPanelText":
                    return Translation.GetTerm("GridViewGroupPanelText", "Drag a column header and drop here to group by that column");

                //---------------------- RadGridView Filter Dropdown items texts:
                case "GridViewClearFilter":
                    return Translation.GetTerm("GridViewClearFilter", "Clear filter");

                case "GridViewFilterSelectAll":
                    return Translation.GetTerm("GridViewFilterSelectAll", "Select all");

                case "GridViewFilterShowRowsWithValueThat":
                    return Translation.GetTerm("GridViewFilterShowRowsWithValueThat", "Show rows with value that");

                case "GridViewFilterContains":
                    return Translation.GetTerm("GridViewFilterContains", "Contains");

                case "GridViewFilterEndsWith":
                    return Translation.GetTerm("GridViewFilterEndsWith", "Ends with");

                case "GridViewFilterIsContainedIn":
                    return Translation.GetTerm("GridViewFilterIsContainedIn", "Is contained in");

                case "GridViewFilterIsEqualTo":
                    return Translation.GetTerm("GridViewFilterIsEqualTo", "Is equal to");

                case "GridViewFilterIsGreaterThan":
                    return Translation.GetTerm("GridViewFilterIsGreaterThan", "Is greater than");

                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return Translation.GetTerm("GridViewFilterIsGreaterThanOrEqualTo", "Is greater than or equal to");

                case "GridViewFilterIsLessThan":
                    return Translation.GetTerm("GridViewFilterIsLessThan", "Is less than");

                case "GridViewFilterIsLessThanOrEqualTo":
                    return Translation.GetTerm("GridViewFilterIsLessThanOrEqualTo", "Is less than or equal to");

                case "GridViewFilterIsNotEqualTo":
                    return Translation.GetTerm("GridViewFilterIsNotEqualTo", "Is not equal to");

                case "GridViewFilterStartsWith":
                    return Translation.GetTerm("GridViewFilterStartsWith", "Starts with");

                case "GridViewFilterAnd":
                    return Translation.GetTerm("GridViewFilterAnd", "And");

                case "GridViewFilter":
                    return Translation.GetTerm("GridViewFilter", "Filter");
            }
            return base.GetStringOverride(key);
        }
    }
}
