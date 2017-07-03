<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.OrderLogisticProviderSearchData>"%>
<div id="newsletterForm" style="background-color: White; padding: 10px;">  

    <table id="campaignAction" class="FormTable" width="100%">
    <tr id="valuePanel">  
         <td class="FLabel" style="width: 18.182em;">
               <%= Html.Term("LogisticProvider", "Logistic Provider:")%>
          </td>
          <td>
            <input type="text" id="logisticsProviderInput"  />
            <input type="hidden" id="hdnOrderShipmentID" value="<%= Model.OrderShipmentID %>" />
            <input type="hidden" id="LogisticsProviderID" class="Filter" />
          </td>
              
        </tr>
          <tr>
              <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("Accept", "Accept")%></a>
                </p>
            </td>
              <td>
                <p>
                    <a href="javascript:void(0);" id="btnCancel" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("Cancel", "Cancel")%></a>
                </p>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function () {

            var LogisticsProviderID = $('#LogisticsProviderID').val('');
            $('#logisticsProviderInput').change(function () {
                LogisticsProviderID.val('');
            });
            $('#logisticsProviderInput').removeClass('Filter').after(LogisticsProviderID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("logisticsProviderSearch", "Look up Prov. by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Logistics/searchProv") %>', { onSelect: function (item) {
				    //				    LogisticsProviderID.val(item.id);
				    $('#LogisticsProviderID').val(item.id)
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});



            $('#btnSave').click(function () {

                if ($('#logisticsProviderInput').val().length == 0) {
                    return false;
                }


                var data = {
                    orderShipmentID: $('#hdnOrderShipmentID').val(),
                    logisticsProviderID: $('#LogisticsProviderID').val()
                };

                $.post('<%= ResolveUrl("~/Logistics/Logistics/UpdateChangeLogisticProvider") %>', data, function (response) {
                    if (response.result) {
                        showMessage('<%=@Html.Term("LogisticUpdatedSuccessfully", "Logistic Provider Updated Successfully")%>', false);

                        //  window.location.replace('<%= ResolveUrl("~/Logistics/Logistics/GetOrdersAllocate/LogisticsProviderID") %>' + "/" + $('#LogisticsProviderID').val());
                        window.location.replace('<%= ResolveUrl("~/Logistics/Logistics/BrowseOrdersAllocate") %>');

                    } else {
                        showMessage(response.message, true);
                    }
                });

            });

            $('#btnCancel').click(function () {
                window.location.replace('<%= ResolveUrl("~/Logistics/Logistics/BrowseOrdersAllocate") %>');
               });
        });
        </script>
</div>
