using System.Web.UI.WebControls;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: GridView Extensions
    /// Created: 05-18-2009
    /// </summary>
    public static class GridViewExtensions
    {
        public static void AddNewBoundField(this GridView value, string dataField, string headerText)
        {
            BoundField boundField = new BoundField();
            boundField.DataField = dataField;
            boundField.HeaderText = headerText;
            value.Columns.Add(boundField);
        }
    }
}
