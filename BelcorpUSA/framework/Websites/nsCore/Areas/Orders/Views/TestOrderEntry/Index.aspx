<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/OrdersAddEdit.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Web.Mvc.Controls.Models.OrderEntryModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Orders") %>">
		<%= Html.Term("Orders") %></a> >
	  <%= Html.Term("StartNewTestOrder", "Start a New Test Order")%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
	<div class="SectionNav">
		<ul class="SectionLinks">
			<%= Html.SelectedLink("~/Orders/OrderEntry/NewOrder?accountId=" + Model.Order.OrderCustomers[0].AccountID, "New Order", LinkSelectionType.ActualPage, CoreContext.CurrentUser, "") %>
			<li><a id="cancelOrder" href="javascript:void(0);" title="CancelOrder">Cancel Order</a>
			</li>
		</ul>
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("ActionErrorMessage"); %>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("StandardOrder", "Standard Order")%></h2>
	</div>
	<script type="text/javascript">
		$(function () {
			$('#orderWait').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' });
			$('#btnSubmitOrder').click(function () {
				if ($(this).hasClass('ButtonOff')) {
					return false;
				}
				<%if (Model.Order.OrderTypeID == (short)Constants.OrderType.EnrollmentOrder)
				  { %>
				var enrolOrderWarning = '<%= HttpUtility.JavaScriptStringEncode(Html.Term("GMPEnrollmentOrderCompletionWarning", "Warning:  This enrollment was initialized on a distributor’s personal web site and completing it here will not necessarily duplicate all the required steps.")) %>';
				if (!confirm(enrolOrderWarning)) {
					return false;
				}
				<%} %>
				if (parseFloat($('.balanceDue').text().replace(/[^\d\.]/g, '')) !== 0) {
					showMessage('<%= Html.Term("TheOrderCouldNotBeSubmitted:ThereisStillAnUnpaidBalance", "The order could not be submitted: There is still an unpaid balance.")%>', true);
					$('#orderWait').jqmHide();
					return false;
				}
				$('#orderWait').jqmShow();

				var data = {
					invoiceNotes: $('#txtInvoiceNotes').val(),
					email: $('#email').val()
				};

				$.post('<%= ResolveUrl("~/Orders/OrderEntry/SubmitOrder") %>', data, function (response) {
					if (response.result) {
						window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
					}
					else {
						// response.message is already term translated
						showMessage(response.message, true);
						$('#orderWait').jqmHide();
						$('#paymentsGrid').html(response.paymentsGrid);
						return false;
					}
				});
			});
			$('#btnPerformOverrides').click(function () {
				var t = $(this);
				if (!t.hasClass('ButtonOff')) {
					if (t.hasClass('cancelOverrides')) {
						t.removeClass('cancelOverrides').html('<span><%= Html.Term("PerformOverrides", "Perform Overrides")%></span>');
						cancelOverrides();
					}
					else {
						t.addClass('cancelOverrides').attr('disabled', 'disabled').html('<span><%= Html.Term("CancelOverrides", "Cancel Overrides")%></span>');
						getOverrides();
						t.removeAttr('disabled');
					}
				}
			});

			<% if(Model.HasOverrides) { %>
			$('#btnPerformOverrides').addClass('cancelOverrides').html('<span><%= Html.Term("CancelOverrides", "Cancel Overrides")%></span>');
			<% } %>

			$('#btnSaveOrder').click(function () {
				$('#orderWait').jqmShow();

				var data = {
					invoiceNotes: $('#txtInvoiceNotes').val(),
					email: $('#email').val()
				};

				$.post('<%= ResolveUrl("~/Orders/OrderEntry/SaveOrder") %>', data, function (response) {
					if (response.result) {
						window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
					}
					else {
						showMessage('<%= Html.Term("TheOrderCouldNotBeSaved", "The order could not be saved:")%> ' + response.message, true);
						$('#orderWait').jqmHide();
						return false;
					}
				});
			});
			
			$('#cancelOrder').click(function () {
				$.post('<%= ResolveUrl("~/Orders/OrderEntry/CancelOrder") %>', function (results) {
					if (results.result)
						location = '/Orders/OrderEntry/NewOrder';
					else
						showMessage(results.message, true);
				});
			});

			$('#overridesModal').jqm({ modal: false, onShow: function (h) {
					h.w.css({
						top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
						left: Math.floor(parseInt($(window).width() / 2)) + 'px'
					}).fadeIn();
				}
			});
		});

		function cancelOverrides() {
			$('#overrideErrors').messageCenter('clearAllMessages');
			$.post('<%= ResolveUrl("~/Orders/OrderEntry/CancelOverrides") %>', function (results) {
				if (results.result) {
					$('#overridesModal').jqmHide();
					$('#products tbody:first').empty();
					$(".OverrideDisable").removeAttr('disabled');
					$('.QuickAdd').show(); 
					updateCartAndTotals(results); 
					$('#payments .paymentItem').remove();
				}
				else { 
					$('#overrideErrors').messageCenter('addMessage', results.message);
				}
			}, 'json');
		}

		function getOverrides() { 
			$('#overridesLoading').show();
			$('#btnSaveOverride').hide();
			$('#overrideErrors').messageCenter();
			$.getJSON('<%= ResolveUrl("~/Orders/OrderEntry/GetOverrides") %>', {}, function (results) {
				$('#overrideProducts tbody:first').empty().html(results.products);
				$('#overrideProducts .price,#gdOverrideProducts .quantity').numeric();
				$('#txtOverrideTax').val(results.totals['taxTotal'].replace(/[^\d\.]/g, ''));
				$('#txtOverrideShipping').val(results.totals['shippingTotal'].replace('$', '').replace(',', ''));
				$('#overrideProducts tbody:first tr').each(function (index, row) {
					$('#overridePrices' + row.id).data('price', $('#overridePrices' + row.id).val());
					$('#cvAmount' + row.id).data('amount', $('#cvAmount' + row.id).val());
				});
				$('#overridesLoading').hide();
				$('#btnSaveOverride').show();
			});
			$('#overridesModal').jqmShow();
		}
	
	</script>
	<% if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
	   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
		-moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
		border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
		margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
	</div>
	<% } %>
	<div id="orderWait" class="PModal WaitWin">
		<%= Html.Term("PleaseWaitWhileWeProcessYourOrder", "Please wait while we process your order...")%>
		<br />
		<img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="<%= Html.Term("Processing", "processing...")%>" />
	</div>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Customer")%>
			</td>
			<td>
				<b>
					<%= Model.Order.OrderCustomers[0].AccountInfo.FullName%>
					(#<%= Model.Order.OrderCustomers[0].AccountInfo.AccountNumber%>)</b>
				<p>
					<span>
						<%=Html.Term("OrderDate", "Order Date")%>:</span>
                     <span>
                     <%=ViewBag.DatePeriod%>
                        </span>
                    <%--<input type="text" value="<%=Vie Session["AccountNumber"] %>" style="width: 9.091em;"
						class="DatePicker OverrideDisable" />--%>
			    </p>
                <p>
                    <%--<span>
                    <%=Html.Term("Period","Period")%>: </span>
                    <span><%=ViewBag.Perio%></span> --%>
                </p>
			</td>
		</tr>
	</table>
	<% Html.RenderPartial("PartialOrderEntry"); %>
	<table class="FormTable" width="100%">
		<tr>
			<td class="FLabel">
				&nbsp;
			</td>
			<td>
				<span class="ClearAll"></span>
				<p class="NextSection">
					<a href="javascript:void(0);" id="btnSubmitOrder" class="Button BigBlue<%= Model.Order.OrderCustomers[0].OrderPayments.Count > 0 && Model.Order.Balance <= 0 ? "" : " ButtonOff" %> Submit">
						<span>
							<%= Html.Term("SubmitOrder", "Submit Order")%>
							>></span></a> -
					<%= Html.Term("or")%>
                    - <a href="javascript:void(0);" id="btnPerformOverrides" 
                        class="Button<%= Model.Order.OrderCustomers[0].OrderItems.Count > 0 ? "" : " ButtonOff" %>"><span><%= Html.Term("PerformOverrides", "Perform Overrides")%></span></a> -
					<%= Html.Term("or")%>
                    - <a href="javascript:void(0);" id="btnSaveOrder" class="Button"><span><%= Html.Term("Save Order", "Save Order")%>
						>></span></a>
				</p>
			</td>
		</tr>
	</table>
	<div id="overridesModal" class="jqmWindow LModal Overrides">
		<div class="mContent">
			<div id="overrideErrors">
			</div>
			<% ViewData["Function"] = "Orders-Override Order"; Html.RenderPartial("Authorize"); %>
		</div>
	</div>
	<% Html.RenderPartial("AddressValidation"); %>
</asp:Content>

