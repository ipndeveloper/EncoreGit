using System.Text;
using System.Web;

namespace NetSteps.Web.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendCell(this StringBuilder builder, string value, string cssClass = "", string style = "", int? columnSpan = null)
        {
            return builder.Append(string.Format("<td{0}{1}{2}>{3}</td>",
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(style) ? " style=\"" + style + "\"" : style,
                (columnSpan != null) ? " colspan=\"" + columnSpan.ToString() + "\"" : string.Empty,
                value));
        }

        public static StringBuilder AppendLinkCell(this StringBuilder builder, string url, string linkText, string cssClass = "", string style = "", string target = "", string linkID = "", string linkCssClass = "")
        {
            if (string.IsNullOrWhiteSpace(url))
                return builder.AppendCell(linkText, cssClass: cssClass, style: style);

            return builder.Append(string.Format("<td{0}{1}><a href=\"{2}\"{3}{4}{5}>{6}</a></td>",
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(style) ? " style=\"" + style + "\"" : style,
                url.StartsWith("~") ? VirtualPathUtility.ToAbsolute(url) : url,
                !string.IsNullOrEmpty(target) ? " target=\"" + target + "\"" : target,
                !string.IsNullOrEmpty(linkID) ? " id=\"" + linkID + "\"" : linkID,
                !string.IsNullOrEmpty(linkCssClass) ? " class=\"" + linkCssClass + "\"" : linkCssClass,
                linkText));
        }

        public static StringBuilder AppendCheckBoxCell(this StringBuilder builder, string id = "", string value = "", string cssClass = "", string name = "", bool disabled = false)
        {
            return builder.Append(string.Format("<td><input type=\"checkbox\"{0}{1}{2}{3}{4} /></td>",
                !string.IsNullOrEmpty(id) ? " id=\"" + id + "\"" : id,
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(value) ? " value=\"" + value + "\"" : value,
                !string.IsNullOrEmpty(name) ? " name=\"" + name + "\"" : name,
                disabled ? " disabled " : ""));
        }

        public static StringBuilder AppendButtonCell(this StringBuilder builder, string id = "", string value = "", string action = "", string labelId = "", string labelValue = "", string cssClass = "", string name = "", bool disabled = false)
        {
            return builder.Append(string.Format("<td><input type=\"button\"{0}{1}{2}{3}{4}{5} /><label for={6}{7}>{8}</label></td>",
                !string.IsNullOrEmpty(id) ? " id=\"" + id + "\"" : id,
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(value) ? " value=\"" + value + "\"" : value,
                !string.IsNullOrEmpty(name) ? " name=\"" + name + "\"" : name,
                disabled ? " disabled " : "",
                !string.IsNullOrEmpty(action) ? " onclick=\"" + action + "\"" : action,
                !string.IsNullOrEmpty(id) ? id : id,
                !string.IsNullOrEmpty(labelId) ? " id=\"" + labelId + "\"" : labelId,
                 labelValue
                 ));
        }


        public static StringBuilder AppendInputImageCell(this StringBuilder builder, string id = "", string source = "", string action = "", string cssClass = "", string value = "", string name = "", bool disabled = false)
        {
            return builder.Append(string.Format("<td><input type=\"image\"{0}{1}{2}{3}{4}{5}{6}/></td>",
                !string.IsNullOrEmpty(id) ? " id=\"" + id + "\"" : id,
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(value) ? " value=\"" + value + "\"" : value,
                !string.IsNullOrEmpty(name) ? " name=\"" + name + "\"" : name,
                disabled ? " disabled " : "",
                !string.IsNullOrEmpty(source) ? " src= \"" + source + "\"" : source,
                !string.IsNullOrEmpty(action) ? " onclick=\"" + action + "\"" : action));
        }

        public static StringBuilder AppendIdCell(this StringBuilder builder, string id, string value, string cssClass = "", string style = "", int? columnSpan = null)
        {
            return builder.Append(string.Format("<td{0}{1}{2}{3}>{4}</td>",
                !string.IsNullOrEmpty(id) ? " id=\"" + id + "\"" : id,
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(style) ? " style=\"" + style + "\"" : style,
                (columnSpan != null) ? " colspan=\"" + columnSpan.ToString() + "\"" : string.Empty,
                value));
        }


        public string FormatFecha(string  value)
        {

        }

    }
}
