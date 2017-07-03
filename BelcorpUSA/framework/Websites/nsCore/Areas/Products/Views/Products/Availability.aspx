<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        $(function () {
            var getCatalogs = function () {
                showLoading($('#availability'));
                $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetCatalogs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', {}, function (response) {
                    hideLoading($('#availability'));
                    if (response.result) {
                        $('#catalogList').html(response.catalogs);
                        $('#availability .DatePicker').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' });
                        $('#availability .TimePicker').timepickr({ convention: 12 });
                    } else {
                        showMessage(response.message, true);
                    }
                });
            }

            getCatalogs();

            $('#btnSave').click(function () {
                var t = $(this);
                showLoading(t);
                var data = {};

                var dateRegex = /\d+\/\d+\/\d+/i, timeRegex = /\d+\:\d+\s(am|pm)/i;
                $('.catalog:checked').each(function (i) {
                    var catalogId = $(this).val(),
						catalogContainer = $('#catalog' + catalogId),
						startDate = catalogContainer.find('.StartDate').val(),
						startTime = catalogContainer.find('.StartTime').val(),
						endDate = catalogContainer.find('.EndDate').val(),
						endTime = catalogContainer.find('.EndTime').val();
                    data['catalogs[' + i + '].CatalogId'] = catalogId;

                    var newStartDate = dateRegex.test(startDate) ? startDate : '';
                    var newStartTime = timeRegex.test(startTime) && newStartDate != '' ? ' ' + startTime : '';

                    var newEndDate = dateRegex.test(endDate) ? endDate : '';
                    var newEndTime = timeRegex.test(endTime) && newEndDate != '' ? ' ' + endTime : '';

                    data['catalogs[' + i + '].StartDate'] = newStartDate + newStartTime;
                    data['catalogs[' + i + '].EndDate'] = newEndDate + newEndTime;
                });

                $.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveAvailability/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', data, function (response) {
                    if (response.result) {
                        //window.location = '<%= ResolveUrl("~/Products/Overview/") %>' + response.productId;
                        showMessage('Availability saved!', false);
                    }
                    else {
                        showMessage(response.message, true);
                    }
                })
                .always(function() {
                    hideLoading(t);                    
                });
            });

        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>">
                <%= Model.Translations.Name() %></a> >
    <%= Html.Term("ProductAvailability", "Product Availability") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ProductAvailability", "Product Availability") %></h2>
    </div>
    <table id="availability" class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("Catalogs", "Catalogs") %>:
            </td>
            <td>
                <p id="catalogList">
                </p>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
                    <%= Html.Term("Save", "Save") %></a>
            </td>
        </tr>
    </table>
    <%Html.RenderPartial("MessageCenter"); %>
</asp:Content>
