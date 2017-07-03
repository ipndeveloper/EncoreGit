<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/OrdersAddEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Web.Mvc.Controls.Models.OrderEntryModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders") %></a> >
    <%= Html.Term("OrderEntry", "Order Entry")%>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>--%>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
    <div class="SectionNav">
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Orders/OrderEntry?accountId=" + Model.Order.ParentOrder.OrderCustomers[0].AccountID, "New Order", LinkSelectionType.ActualPage, CoreContext.CurrentUser, "") %>
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
            <%= Html.Term("ReplacementOrder", "Replacement Order")%></h2>
    </div>
    <script type="text/javascript">
        $(function ()
        {
            $('#orderWait').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' });
            $('#btnSubmitOrder').click(function ()
            {
                if ($(this).hasClass('ButtonOff'))
                {
                    return false;
                }

                var balanceDue = parseFloat($('.balanceDue').text().replace(/[^\d\.]/g, ''));

                if (balanceDue !== 0 && !isNaN(balanceDue))
                {
                    showMessage('<%= Html.Term("TheOrderCouldNotBeSubmitted:ThereisStillAnUnpaidBalance", "The order could not be submitted: There is still an unpaid balance.")%>', true);
                    $('#orderWait').jqmHide();
                    return false;
                }
                $('#orderWait').jqmShow();

                var data = {
                    invoiceNotes: $('#txtInvoiceNotes').val(),
                    email: $('#email').val()
                };

                $.post('<%= ResolveUrl("~/Orders/OrderEntry/SubmitOrder") %>', data, function (response)
                {
                    if (response.result)
                    {
                        window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
                    }
                    else
                    {
                        showMessage('<%= Html.Term("TheOrderCouldNotBeSubmitted", "The order could not be submitted:")%> ' + response.message, true);
                        $('#orderWait').jqmHide();
                        $('#paymentsGrid').html(response.paymentsGrid);
                        return false;
                    }
                });
            });
            $('#btnPerformOverrides').click(function ()
            {
                var t = $(this);
                if (!t.hasClass('ButtonOff'))
                {
                    if (t.hasClass('cancelOverrides'))
                    {
                        t.removeClass('cancelOverrides').html('<span><%= Html.Term("PerformOverrides", "Perform Overrides")%></span>');
                        cancelOverrides();
                    }
                    else
                    {
                        t.addClass('cancelOverrides').attr('disabled', 'disabled').html('<span><%= Html.Term("CancelOverrides", "Cancel Overrides")%></span>');
                        getOverrides();
                        t.removeAttr('disabled');
                    }
                }
            });
            $('#btnSaveOrder').click(function ()
            {
                $('#orderWait').jqmShow();

                var data = {
                    invoiceNotes: $('#txtInvoiceNotes').val(),
                    email: $('#email').val()
                };

                $.post('<%= ResolveUrl("~/Orders/OrderEntry/SaveOrder") %>', data, function (response)
                {
                    if (response.result)
                    {
                        window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
                    }
                    else
                    {
                        showMessage('<%= Html.Term("TheOrderCouldNotBeSaved", "The order could not be saved:")%> ' + response.message, true);
                        $('#orderWait').jqmHide();
                        return false;
                    }
                });
            });
            $('#cancelOrder').click(function ()
            {
                $.post('<%= ResolveUrl("~/Orders/OrderEntry/CancelOrder") %>', function (results)
                {
                    if (results.result)
                        location = '/Orders/OrderEntry/NewOrder';
                    else
                        showMessage(results.message, true);
                });
            });
        });

        function cancelOverrides()
        {
            $('#overrideErrors').messageCenter('clearAllMessages');
            $.post('<%= ResolveUrl("~/Orders/OrderEntry/CancelOverrides") %>', function (results)
            {
                if (results.result)
                {
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
                else
                {
                    // show a message explaining why the cancel did not work
                    $('#overrideErrors').messageCenter('addMessage', results.message);
                }
            }, 'json');
        }

        function getOverrides()
        {
            //			if (!overrideErrors)
            //				overrideErrors = $('#overrideErrors').messageCenter(null, null, '<%= ResolveUrl("~/Content/Images/exclamation.png") %>', '1000');
            // get the data to display in the modal
            $('#overridesLoading').show();
            $('#btnSaveOverride').hide();
            $('#overrideErrors').messageCenter();

            //"~/Orders/OrderEntry/GetOverrides"

            $.getJSON('<%= ResolveUrl(Model.GetOverridesLocation) %>', {}, function (results)
            {
                $('#overrideProducts tbody:first').empty().html(results.products);
                $('#overrideProducts .price,#gdOverrideProducts .quantity').numeric();
                $('#txtOverrideTax').val(results.totals['taxTotal'].replace(/[^\d\.]/g, ''));
                $('#txtOverrideShipping').val(results.totals['shippingTotal'].replace('$', '').replace(',', ''));
                $('#overrideProducts tbody:first tr').each(function (index, row)
                {
                    $('#overridePrices' + row.id).data('price', $('#overridePrices' + row.id).val());
                    $('#cvAmount' + row.id).data('amount', $('#cvAmount' + row.id).val());
                });
                $('#overridesLoading').hide();
                $('#btnSaveOverride').show();
            });
            $('#overridesModal').jqm({ modal: false, onShow: function (h)
            {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            }).jqmShow();
        }
	
    </script>
    <% string baseUrl = ResolveUrl("~/") + ViewContext.RouteData.DataTokens["area"].ToString() + "/" + ViewContext.RouteData.Values["controller"].ToString() + "/"; %>
    <% Html.RenderPartial("PartialOrderEntry", Model); %>
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
                    - <a href="javascript:void(0);" id="btnPerformOverrides" class="Button<%= Model.Order.OrderCustomers[0].OrderItems.Count > 0 ? "" : " ButtonOff" %>">
                        <span>
                            <%= Html.Term("PerformOverrides", "Perform Overrides")%></span></a> -
                    <%= Html.Term("or")%>
                    - <a href="javascript:void(0);" id="btnSaveOrder" class="Button"><span>
                        <%= Html.Term("Save Order", "Save Order")%>
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
