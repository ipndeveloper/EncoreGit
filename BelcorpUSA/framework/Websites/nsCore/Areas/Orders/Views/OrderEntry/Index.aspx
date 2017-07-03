<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/OrdersAddEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Web.Mvc.Controls.Models.OrderEntryModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders") %></a> >
    <%= Html.Term("OrderEntry", "Order Entry")%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
    <div class="SectionNav">
        <ul class="SectionLinks">            
            <li><a id="lnkNewOrder" href="javascript:void(0);" title="Start a New Order"><span>
                <%=Html.Term("StartOrder", "New Order")%></span> </a></li>
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
    <%--csti(mescoobar)-EB-486-Inicio--%>
    <style type="text/css">
        .fadeInbox
        {
            padding: 20px;
            margin: 16px;
            border: 3px solid #39399C;
            width: 500px;
            height: auto;
            background-color: #D9D1DE;
            color: Black;
            position: relative;
            z-index: 999;
            left: 400px;
            top: 700px;
            font-size: 1.2em;
            border-top-left-radius: 3em;
            border-top-right-radius: 3em;
            border-bottom-right-radius: 3em;
            border-bottom-left-radius: 3em;
        }
        .fadeInbox a
        {
            color: Blue;
            text-decoration: underline;
            font-style: italic;
        }
    </style>
    <%--csti(mescoobar)-EB-486-Fin--%>
    <script type="text/javascript">
		$(function () { 
                $.post('<%=ResolveUrl("~/OrderEntry/GetStatusMarkOrderDate") %>', function (response) {
                    if (response.StatusMarkOrderDate == 1) 
                    {
                        $('#dtpOrderDate').attr('disabled',true);
                    }
                    else 
                    {
                        $('#dtpOrderDate').removeAttr('disabled');
                    }
                });

            $('#lnkNewOrder').click(function () {
                $.ajax({
                    type: 'POST',
                    url: '<%=ResolveUrl("~/OrderEntry/ValidPreOrder") %>',
                    data: ({ Code: this.id }),
                    asyn: false,
                    success: function (data) {
                        if (data.result == true) {
                            $('#addMessage').jqmShow();
                        } else {
                            location = '/Orders/OrderEntry/NewOrder';
                        }
                    }
                });
            });
             $('#loaderApplyPayment').hide();
             $('#btnSubmitOrder').addClass("ButtonOff"); 
	         $('#btnSubmitOrder').attr('disabled', true);
              // csti(mescoobar)-EB-486-Inicio
	        $('.fadeInbox').jqm({
	            modal: true,
	            trigger: true,
	            onShow: function (h) {
	                h.w.css({
	                    position: 'relative',
	                    top: '-450px',
	                    left: '230px'
	                }).fadeIn();
	            },
	            overlay: 50,
	            overlayClass: 'HModalOverlay'
	        });

	        $('.closeFadeInbox').on("click", function () {
	            $('.fadeInbox').jqmHide();
	        });
	        // csti(mescoobar)-EB-486-Fin

			$('#orderWait').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' });
			//Modificacion submitOrder para el manejo de la pasarela de pago con PayPal (jmorales-csti)

			$('#btnSubmitOrder').click(function () {
                var payment = $('#sPaymentMethod').val();
                //Al presionar el boton desactivarlo ...
                  $('#btnSubmitOrder').addClass("ButtonOff");
                $('#btnSubmitOrder').attr('disabled', true);

                var Parameters = {
                                    paymentConfigurationID : payment, 
                                    paymentGatewayID: 5
                                  };
                $.post('<%= ResolveUrl("~/Orders/EntryPayPal/validaPaymentGatewayID") %>', Parameters, function (data) {
                 // csti(mescoobar)-EB-486-Inicio
                      if (data.validrule) {  
                      
                                            updateTotals(data);
                                            BalanceCredit(data);
                                            HabBotonesSubmitOrderSaveOrder(data);                  
                                            var strReplaceAll = data.message;
                                            var intIndexOfMatch = strReplaceAll.indexOf("|n");
                                            while (intIndexOfMatch != -1) {                                          
                                                strReplaceAll = strReplaceAll.replace("|n", String.fromCharCode(10))
                                                intIndexOfMatch = strReplaceAll.indexOf("|n");
                                            }
                                            $("#PopUpGenericPreViewMessage").html(strReplaceAll)
                                            $('.fadeInbox').jqmShow();
                                             $('#orderWait').jqmHide();
                                        return false;
                          }else{

                            if (data.result)
                            {
                                makePay();

                            }else{
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
				                $('#orderWait').jqmShow();

				                var data = {
					                invoiceNotes: $('#txtInvoiceNotes').val(),
					                email: $('#email').val(),
                                    PeriodID: $( "#ddlPeriod option:selected" ).text()
				                };

					            $.post('<%= ResolveUrl("~/Orders/OrderEntry/SubmitOrder") %>', data, function (response) {
					                if (response.result) {
						                window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
					                }
					                else {
                                            showMessage(response.message, true);
                                            //PayPal_001
                                            //Se agrega esto para que vuelva elegir medio de pago
                                            //Author: JMorales 
                                            //Date: 01/09/2016
                                    
                                            var sid = $("#hdid").val();
                                            var sindice = $("#hdindice").val();
                                            removePayment(sid, sindice);
                                            //End PayPal_001

                                            $('#paymentsGrid').html(response.paymentsGrid);
//                                        }
                                        $('#orderWait').jqmHide();
                                        return false;
                                        // csti(mescoobar)-EB-486-Fin
					                }
				                });
                            }
                        }  //  cierre de la validacion
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
            	var t = $(this);
				if (!t.hasClass('ButtonOff')) {

				    $('#orderWait').jqmShow();

				    var data = {
					    invoiceNotes: $('#txtInvoiceNotes').val(),
					    email: $('#email').val(),
                        PeriodID: $( "#ddlPeriod option:selected" ).text()
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
                }
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
					// update the html with the changes
					$('#products tbody:first').empty();
					// enable the page
					$(".OverrideDisable").removeAttr('disabled');
					$('.QuickAdd').show();
					// refresh the totals
					updateCartAndTotals(results);
					// remove payments (the payments were already removed from the object, now we need to update the html)
					$('#payments .paymentItem').remove();
				}
				else {
					// show a message explaining why the cancel did not work
					$('#overrideErrors').messageCenter('addMessage', results.message);
				}
			}, 'json');
		}

		function getOverrides() {
			// get the data to display in the modal
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
                        <%= Html.Term("OrderDate", "Order Date")%>:</span>
                    <input id="dtpOrderDate" type="text" value="<%= DateTime.Today.ToShortDateString() %>"
                        style="width: 9.091em;" class="DatePicker OverrideDisable" />                    
                </p>
                <p>
                    <span>
                        <%=Html.Term("Period","Period")%>: </span><span>                            
                        </span>
                    <select id="ddlPeriod">
                        <%
                            foreach (KeyValuePair<int, bool> kvp in ViewBag.Period)
                            {
                                if (kvp.Value)
                                {                            
                        %>
                        <option value="<%= kvp.Key %>" selected="selected">
                            <%= kvp.Key%></option>
                        <%
                                }
                                else
                                { 
                        %>
                        <option value="<%= kvp.Key %>">
                            <%= kvp.Key%></option>
                        <%
                                }
                            } 
                        %>
                    </select>
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
                    <a href="javascript:void(0);" id="btnPerformOverrides" class="Button<%= Model.Order.OrderCustomers[0].OrderItems.Count > 0 ? "" : " ButtonOff" %>">
                        <span>
                            <%= Html.Term("PerformOverrides", "Perform Overrides")%></span></a> -
                    <%= Html.Term("or")%>
                    - <input type="button" id="btnSaveOrder" class="Button ButtonOff" value="<%= Html.Term("Save Order", "Save Order")%>" /> 
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
    <%--csti(mescoobar)-EB-486-Inicio--%>
    <div id="PopUpGenericPreView" class="fadeInbox" style="display: none">
        <div style="text-align: left">
            <span id="PopUpGenericPreViewMessage"></span><a class="closeFadeInbox" href="javascript:void(0)">
                <%= Html.Term("GoBackAndAddProducts", "Click here to go back and add more products")  %></a>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnSubmitOrder').click(function (event) {
                event.stopPropagation();
            }); 
            var valor = "<%=  Model.Order.Balance %>";
            var valorm = valor.replace(",", ".");

            var num = parseFloat(valorm).toFixed(2);

            var numPayments = "<%= Model.Order.OrderPayments.Count() %>"
            var numOrderItems = "<%= Model.Order.OrderCustomers[0].OrderItems.Count() %>"

            if (numOrderItems > 0 && numPayments > 0 && num >= 0) { 
                $('#btnSubmitOrder').removeClass("ButtonOff");
                $('#btnSubmitOrder').attr('disabled', true);
            }

            if (num < 0) {
                num = num * (-1);
                $('#txtPaymentAmount').val(num);
                $('#sPaymentMethod').attr('disabled', false);
                $('#sShippingAddress').attr('disabled', false);
                $('#AddNewShippingAddress').show();

            } else {
                $('#sPaymentMethod').attr('disabled', true);
                $('#sShippingAddress').attr('disabled', true);
                $('#AddNewShippingAddress').hide();
            }

            if (numOrderItems > 0 && num != 0) { 
                $('#btnSaveOrder').removeClass("ButtonOff");
                $('#btnSaveOrder').attr('disabled', false);
            }


        });
    </script>
    <%--csti(mescoobar)-EB-486-Fin--%>
</asp:Content>
