using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: HtmlTable Extensions
    /// Created: 06-09-2009
    /// </summary>
    public static class HtmlTableExtensions
    {
        #region AddRow
        public static HtmlTableRow AddRow(this HtmlTable _tbl)
        {
            HtmlTableRow retval = new HtmlTableRow();
            _tbl.Rows.Add(retval);
            return retval;
        }

        public static HtmlTableRow AddRow(this HtmlTable _tbl, string _class)
        {
            HtmlTableRow retval = new HtmlTableRow();
            retval.Attributes.Add("class", _class);
            _tbl.Rows.Add(retval);
            return retval;
        }
        #endregion AddRow

        #region AddCell
        public static HtmlTableCell AddCell(this HtmlTableRow _row)
        {
            HtmlTableCell retval = new HtmlTableCell();
            _row.Cells.Add(retval);
            return retval;
        }

        public static HtmlTableCell AddCell(this HtmlTableRow _row, string _innerHtml)
        {
            HtmlTableCell retval = new HtmlTableCell();
            retval.InnerHtml = _innerHtml;
            _row.Cells.Add(retval);
            return retval;
        }

        public static HtmlTableCell AddCell(this HtmlTableRow _row, string _innerHtml, string _class)
        {
            HtmlTableCell retval = new HtmlTableCell();
            retval.Attributes.Add("class", _class);
            retval.InnerHtml = _innerHtml;
            _row.Cells.Add(retval);
            return retval;
        }

        public static HtmlTableCell AddCell(this HtmlTableRow _row, Control _control, string _class)
        {
            HtmlTableCell retval = new HtmlTableCell();
            retval.Attributes.Add("class", _class);
            retval.Controls.Add(_control);
            _row.Cells.Add(retval);
            return retval;
        }
        #endregion AddCell
    }
}
