using System;
using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration
{
    public class Pagination_Control : Control<Div>
    {
        [Obsolete("Use 'SelectPageSize(int, int?)'")]
        public TPage SelectPageSize<TPage>(int index, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            SelectList pageSize = Element.GetElement<SelectList>(new Param("pageSize", AttributeName.ID.ClassName));
            pageSize.CustomSelectDropdownItem(index, timeout);
            Util.CustomRunScript("$('#paginatedGridPagination select.pageSize').change()");
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
        
        [Obsolete("Use 'ClickNextPage(int?)'")]
        public TPage ClickNextPage<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("nextPage", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public void SelectPageSize(int index, int? timeout = null)
        {
            SelectList pageSize = Element.GetElement<SelectList>(new Param("pageSize", AttributeName.ID.ClassName));
            pageSize.CustomSelectDropdownItem(index, timeout);
            Util.CustomRunScript("$('#paginatedGridPagination select.pageSize').change()");
        }

        public void ClickNextPage(int? timeout = null)
        {
            timeout = Element.GetElement<Link>(new Param("nextPage", AttributeName.ID.ClassName)).CustomClick(timeout);
        }
    }
}
