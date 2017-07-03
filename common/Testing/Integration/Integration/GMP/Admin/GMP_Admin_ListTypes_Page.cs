using WatiN.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_ListTypes_Page : GMP_Admin_Base_Page
    {
        List<string> _types;

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Admin/ListTypes/Values/AccountStatusChangeReason", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

        public List<string> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new List<string>();
                    foreach (Link lnk in Document.GetElement<TableCell>(new Param("CoreContent", AttributeName.ID.ClassName)).UnorderedList(Find.Any).Links)
                    {
                        _types.Add(lnk.CustomGetText());
                    }
                }
                return _types;
            }
        }

        public GMP_Admin_ListTypeValues_Page EditType(string type = null, int? timeout = null, bool pageRequired = true)
        {
            if (string.IsNullOrEmpty(type))
            {
                List<string> types = Types;
                type = types[Util.GetRandom(0, types.Count - 1)];
            }
            timeout = Document.GetElement<Link>(new Param(type, AttributeName.ID.InnerText, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_ListTypeValues_Page>(timeout, pageRequired);
        }
    }
}
