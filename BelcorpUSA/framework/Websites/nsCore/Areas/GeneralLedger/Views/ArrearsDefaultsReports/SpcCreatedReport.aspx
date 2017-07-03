<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/ArrearsDefaultsReports.Master"
                                        Inherits="System.Web.Mvc.ViewPage<List<nsCore.Areas.GeneralLedger.Models.FileClass>>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/GeneralLedger") %>">
        <%= Html.Term("GMP-Nav-General-Ledger", "General Ledger")%></a> >
    <%= Html.Term("ArrearsDefaultReports", "Arrears and Defaults Reports")%> >
    <%= Html.Term("SpcCreatedReport", "SPC Created Report")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    
    <div class="SectionHeader">
        <h2><%= Html.Term("SpcCreatedFiles", "SPC Created Files")%></h2>
    </div>

    <div>
        <h3><%= Html.Term("SpcCreatedFiles", "SPC Created Files")%></h3>
        <hr />
        <br />
    </div>
    
    <input type="hidden" value="" id="fileId" class="Filter"/>

    <%
        Html.PaginatedGrid<nsCore.Areas.GeneralLedger.Models.FileClass>("~/GeneralLedger/ArrearsDefaultsReports/ListFilePaginate")
        .AutoGenerateColumns()
        .AddInputFilter(Html.Term("FileName", "File Name"), "fileId", startingValue: "")
        .AddSelectFilter("Creation Date", "creationDate", ViewBag.CreationDates as Dictionary<string, string>)
        .Render(); %>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#fileIdInputFilter').change(function () {
//                $('#fileId').val("");
            });

            $('#fileIdInputFilter').removeClass('Filter').after($('#fileId')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("fileSearch", "Look up file by name") %>')
				.jsonSuggest('<%= ResolveUrl("~/GeneralLedger/ArrearsDefaultsReports/GetFileNames") %>', { onSelect: function (item) {
				    $('#fileId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
			});

        });


        jQuery.fn.refreshTable = function () {
            $(this).find('tr:odd').addClass('Alt');
            $(this).find('tr:even').removeClass('Alt');
            return $(this);
        }
    
    </script>

</asp:Content>

