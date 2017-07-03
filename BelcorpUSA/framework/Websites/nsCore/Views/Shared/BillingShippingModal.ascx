<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%--
This control will handle the following address functions:
    editAddress
    setDefaultAddress
    deleteAddress
    getAddresses
    editPaymentMethod
    setDefaultPaymentMethod
    deletePaymentMethod
    getPaymentMethods
    
To implement this control add the following to your html:
    1. <% Html.RenderPartial("BillingShippingModal"); %> to add the contents of this file to your html
    2. when adding billing or shipping profiles create links similar to the following:
        <a title="Edit" style="cursor: pointer;" onclick="editAddress(<%= shippingAddress.AccountAddressID %>);">
        <a href="javascript:void(0);" onclick="setDefaultAddress(<%= shippingAddress.AccountAddressID %>);" <%= shippingAddress.IsDefault ? "style=\"display:none;\"" : "" %>>
        <a href="javascript:void(0);" onclick="deleteAddress(<%= shippingAddress.AccountAddressID %>);">
    3. To get a list of all shipping or billing profiles then make a javascript call to getPaymentMethods() or getAddresses()
--%>
<script type="text/javascript">
    $(function () {
        $.each(['Address', 'PaymentMethod'], function (i, item) {
            var plural = item == 'Address' ? 'Addresses' : 'PaymentMethods',
				camelCase = item.substr(0, 1).toLowerCase() + item.substr(1) + 'Id';
            window['edit' + item] = function (id) {
                $('#edit' + item + 'Modal').jqm({
                    modal: false,
                    ajax: '<%= ResolveUrl("~/Accounts/BillingShippingProfiles/") %>' + item + 'Modal?' + camelCase + '=' + id,
                    onShow: function (h) {
                        h.w.css({
                            top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt((h.w.height() + 400) / 2)) + 'px',
                            left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                        }).fadeIn();
                    }, onHide: function (h) {
                        h.w.fadeOut('slow', function () {
                            h.o.remove();
                            $('#edit' + item + 'Modal .mContent').empty();
                        });
                        var sShippingAddress = $('#sShippingAddress');
                        if (sShippingAddress)
                            sShippingAddress.change();
                    }
                }).jqmShow();
            };
            window['setDefault' + item] = function (id) {
                var data = {};
                data[camelCase] = id;
                $.post('<%= ResolveUrl("~/Accounts/BillingShippingProfiles/") %>SetDefault' + item, data, function (response) {
                    if (response.result) {
                        window['get' + plural]();
                    }
                    else
                        showMessage(response.message, true);
                });
            };
            window['delete' + item] = function (id) {
                var data = {};
                data[camelCase] = id;
                if (confirm(String.format('<%= Html.Term("DeleteMsg", "Are you sure you want to delete this {0}?") %>', item.toSpaced().toLowerCase()))) {
                    $.post('<%= ResolveUrl("~/Accounts/BillingShippingProfiles/") %>Delete' + item, data, function (response) {
                        if (response.result) {
                            window['get' + plural]();
                        }
                        else
                            showMessage(response.message, true);
                    });
                }
            };
            window['get' + plural] = function () {
                $.get('<%= ResolveUrl("~/") + ViewContext.RouteData.DataTokens["area"].ToString() + "/" + ViewContext.RouteData.Values["controller"].ToString() %>/Get' + plural, {}, function (response) {
                    if (response.result === undefined || response.result) {
                        if (window['display' + plural])
                            window['display' + plural](response);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            };
        });
    });
</script>
<input type="hidden" id="currentArea" value="<%= ViewContext.RouteData.DataTokens["area"].ToString() %>" />
<input type="hidden" id="currentController" value="<%= ViewContext.RouteData.Values["controller"].ToString() %>" />
<div id="editAddressModal" class="jqmWindow LModal ShipWin">
    <div class="mContent">
    </div>
</div>
<div id="editPaymentMethodModal" class="jqmWindow LModal PaymentWin">
    <div class="mContent">
    </div>
</div>
