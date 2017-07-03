<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" 
Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {

    //FSV - GYS
//    var templateSelected = false;
//    var templateId = $('<input type="hidden" id="nameTemplateIdFilter" class="Filter" />');
//    $('#nameTemplateInputFilter').removeClass('Filter').css('width', '275px').watermark('<%=Html.JavascriptTerm("nameTemplateSearch","Look up Template by ID or name") %>').jsonSuggest('<%=ResolveUrl("~/NewPrintOrder/Search") %>',
//                        { onSelect: function (item) {
//                            templateId.val(item.id);
//                            templateSelected = false;
//                        }, minCharacter: 3,
//                            source: $('#nameTemplateInputFilter'),
//                            ajaxResults: true,
//                            maxResult: 50,
//                            showMore: true
//                        }).blur(function () {
//                            if (!$(this).val() || !$(this).val() == $(this).data('watermark')) {
//                                templateId.val('');
//                            } else if (!templateSelected) {
//                                templateId.val('-1');
//                            }
//                            templateSelected = false;
//                        }).after(templateId);
//        });


 </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
        <h2>
            <%= Html.Term("NewsPrintOrder", "News Print Order") %></h2>
        <a href="<%= ResolveUrl("~/Communication/NewPrintOrder/Edit") %>">
            <%= Html.Term("AddNewTemplate", "Add a new template") %>
        </a>
    </div>
     <% 
         Html.PaginatedGrid<NetSteps.Data.Entities.Business.NewPrintOrderSearchData>("~/Communication/NewPrintOrder/Get")
           .AutoGenerateColumns()
           .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "0", Html.Term("All") } }.AddRange(TempData["Status"] as Dictionary<string, string>))
           .AddSelectFilter(Html.Term("Section"), "section", new Dictionary<short, string>() { { 0, Html.Term("All") } }.AddRange(NewPrintOrder.ListGeneralSection()))
           .AddInputFilter(Html.Term("NameTemplate"), "nameTemplate")
           .AddSelectFilter(Html.Term("Period"), "period", new Dictionary<int, string>() { { 0, Html.Term("SelectPeriod", "Select Period...") } }.AddRange(NewPrintOrder.ListPeriod()))
           .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate",Html.Term("StartDate", "Start Date"), isDateTime: true)
           .AddInputFilter(Html.Term("To", "To"), "endDate", Html.Term("EndDate", "End Date"), isDateTime: true)
           //.AddInputFilter("To", "endDate", startingValue: Model.SearchParameters.EndDate, isDateTime: true)
           .Render(); %> 


            

</asp:Content>