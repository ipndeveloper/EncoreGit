<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" 
Inherits="System.Web.Mvc.ViewPage<OrderPaymentTemplatesSearchParameters>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	
	<script type="text/javascript">

	    var currentPage = 0;

	    $(function () {

	        $('#btnSave').click(function () {

	            if ($('table.DataGrid').checkRequiredFields()) {
	                var data = {
                        id : $('#hdnID').val(),
                        description: $('#txtDescription').val(),
	                    days: $('#txtDays').val(),
	                    minimalAmount: $('#txtMinimalAmount').val()
	                };


	                $.post('<%= ResolveUrl("~/Communication/OrderPaymentTemplates/Save") %>', data, function (response) {
	                    if (response.result) {
	                        showMessage('Order Payment Templates saved!', false);
	                        window.location = '<%= ResolveUrl("~/Communication/OrderPaymentTemplates") %>';
	                    } else {
	                        showMessage(response.message, true);
	                    }
	                });
	            }
	        });

	   
	        $('#btnRemoveAction').click(function () {
	            var data = {};
	            $('#taskItems > tbody input[type="checkbox"]:checked').each(function (i) {
	                data['MarketIDs[' + i + ']'] = $(this).val();
	            });

	            $.post('<%= ResolveUrl("~/Support/Motive/RemoveTaskItems") %>', data, function (repsonse) {
	                getTaskItems();
	            });
	        });
	      
	       
	    });
	  
	</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Communication/OrderPaymentTemplates") %>">
		<%= Html.Term("Order Payment")%></a> 
	  
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
            <% if (Model.OrderPaymentTemplateId.ToString()=="0"){ %>
			<%= Html.Term("AddNew", "Create")%>
             <% } else {%>
            <%= Html.Term("AddUpdate", "Modify")%>
            <%} %>
             <input type="hidden" id="hdnID" value="<%= Model.OrderPaymentTemplateId.ToString() %>" />
        </h2>       
	</div>

    <table id="DataGrid" class="FormTable" width="100%">        
   
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Description", "Description")%>:
            </td>
            <td>
                <input id="txtDescription" type="text" value="<%= Model.Description %>" 
                    class="required" name="<%= Html.Term("DescriptionIsRequired", "Description is required") %>"  style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Days", "Days")%>:
            </td>
            <td>
                <input id="txtDays" type="text" value="<%= Model.Days %>" 
                    class="required" name="<%= Html.Term("DaysIsRequired", "Days is required") %>" 
                     />
            </td>
        </tr>
         <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("MinimalAmount", "Minimal Amount")%>:
            </td>
            <td>
                <input id="txtMinimalAmount" type="text" value="<%= Model.MinimalAmount %>" 
                    class="required" name="<%= Html.Term("MinimalAmountIsRequired", "Minimal Amount is required") %>" 
                     />
            </td>
        </tr>
        
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("Save", "Save") %></a>

                        <a  href="<%= ResolveUrl("~/Communication/OrderPaymentTemplates") %>" id="btnCancel" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("Cancel", "Cancel") %></a>
                </p>
            </td>
        </tr>

        
      
    </table>

</asp:Content>
