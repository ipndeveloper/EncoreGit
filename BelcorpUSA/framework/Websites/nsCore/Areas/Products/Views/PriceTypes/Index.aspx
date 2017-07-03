<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.ProductPriceType>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .tableHeader
    {
        font-weight: bold; 
        font-size:medium;
    }
     
    .itemMandatory
    {
        text-align:center;
        }   
    </style>
	<script type="text/javascript">
	    $(function () {
	        $('#btnAdd').click(function () {
	            $('#priceTypes').append('<tr><td colspan="2"><input type="text" name="value0" class="priceType" maxlength="50"/><span style="text-align:right;margin-left: 10px;"><input type="checkbox" class="activeMandatory" checked="true"></span></td><td class="itemMandatory"><a href="javascript:void(0);" class="delete listValue"><span class="UI-icon icon-x" title="Delete"></span></a></td></tr>');
	        });
	        $('.delete').live('click', function () {
	            if ($(this).parent().parent().find('input').attr('name').replace(/\D/g, '') == 0 || confirm('Are you sure you want to delete this price type?  This will delete all prices and associations to this price type.')) {
	                var priceType = $(this).parent().parent().find('input'), priceTypeId = priceType.attr('name').replace(/\D/g, '');
	                if (priceTypeId > 0) {
	                    $.post('<%= ResolveUrl("~/Products/PriceTypes/Delete") %>', { priceTypeId: priceTypeId });
	                }
	                priceType.parent().parent().fadeOut('normal', function () {
	                    $(this).remove();
	                });
	            }
	        });
	        $('#btnSave').click(function () {
	            var t = $("#frmCharging");
	            showLoading(t);

	            var data = {};

	            $('#priceTypes .priceType').each(function (i) {
	                data['priceTypes[' + i + '].ProductPriceTypeID'] = $(this).attr('name').replace(/\D/g, '');
	                data['priceTypes[' + i + '].Name'] = $(this).val();
	            });
	            //KLC - CSTI
	            $('#priceTypes .activeMandatory').each(function (aCounter) {
	                data['priceTypes[' + aCounter + '].Mandatory'] = $(this).prop('checked');
	            });

	            $.post('<%= ResolveUrl("~/Products/PriceTypes/Save") %>', data, function (response) {
	                if (response.result) {
	                    //window.location = '<%= ResolveUrl("~/Admin/ListTypes") %>';
	                    showMessage('Price Types saved!', false);
	                    $('#priceTypes').html(response.priceTypes);
	                }
	                else
	                    showMessage(response.message, true);
	            })
                .always(function () {
                    hideLoading(t);
                });
	        });
	    });
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("PriceTypes", "Price Types") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("PriceTypes", "Price Types") %>
		</h2>
		<a href="javascript:void(0);" id="btnAdd">
			<%= Html.Term("AddNewPriceType", "Add new price type") %></a>
	</div>

    <table id="priceTypes" class="FormTable Section">
        <thead >
            <tr style="margin-bottom: 10px;">
                <th></th>
                <th><span class="tableHeader"><%=Html.Term("PriceName", "Price Name") %></span></th>
                <th><span class="tableHeader"><%=Html.Term("Mandatory", "Mandatory") %></span></th>
            </tr>
        </thead>
        
	<%--<ul id="priceTypes" class="listValues">--%>
    
		<%foreach (ProductPriceType priceType in Model.OrderBy(r => r.Editable))
	{ %>
		<%--<li>--%>
        <tr>
		    <%
                bool activeMan = NetSteps.Data.Entities.Business.Logic.ProductPriceTypeExtensionBusinessLogic.Instance.GetMandatory(priceType.ProductPriceTypeID);   
        
               if (priceType.Editable)
	            { %>
			       <td colspan="2"> <input type="text" name="value<%= priceType.ProductPriceTypeID %>" value="<%= priceType.GetTerm() %>" class="priceType" maxlength="50" />
                   <span style="text-align:right;margin-left: 10px;"> <input type="checkbox" value="<%= priceType.ProductPriceTypeID %>" <%=activeMan == true ? "checked=\"checked\"" : ""%>  class="activeMandatory"  /></span></td>
                   <td class="itemMandatory"> <a href="javascript:void(0);" class="delete listValue">
                        <span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>"></span>
                        </a>
                    </td>
		        <%}
	            else
	            { %>
			    <td> <span class="UI-icon icon-lock" title="locked"></span> </td>
                <td> <input type="text" value="<%= priceType.GetTerm() %>" disabled="disabled" /> </td>
                <td class="itemMandatory"> <input type="checkbox" value="<%= priceType.ProductPriceTypeID %>" <%=activeMan == true ? "checked=\"checked\"" : ""%>  disabled="disabled"  /> </td>
		    <%} %>
            </tr>
            <%--</li>--%>
		 <%} %>
	<%--</ul>--%>
     </table>
	<span class="ClearAll"></span>
    <div id="frmCharging">
	    <p>
		    <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			    <%= Html.Term("Save", "Save") %></a>
	    </p>
    </div>
</asp:Content>
