<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Address>" %>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/address-validation.js") %>"></script>
<script type="text/javascript">
    $(function () {
        $('#shippingAddressPhone').phone().phone('setPhone', '<%= Model.PhoneNumber %>');
        $('#btnSaveAddress').click(function () {
            if ($('#shippingAddressControl').checkRequiredFields()) {
                $('#shippingAddressControl input').each(function () {
                    if ($(this).val() == $(this).attr('title')) {
                        $(this).val('');
                    }
                });
                var p = $(this).parent();
                showLoading(p);
                // address validation
                var validation = abstractAddressValidation({
                    address1: $('#shippingAddressAddress1').val(),
                    address2: $('#shippingAddressAddress2').val(),
                    address3: $('#shippingAddressAddress3').val(),
                    //city: $("#shippingAddressCity :selected").text() == "" ? $("#shippingAddressCity").val() : $("#shippingAddressCity :selected").text(), // in case Country is not US, it is a simple text input
                    city: $("#shippingAddressCity").val() == "" ? $("#shippingAddressCity").val() : $("#shippingAddressCity").val(), // in case Country is not US, it is a simple text input
                    //state: $('#shippingAddressState :selected').val(),
                    state: $('#shippingAddressState').val(),
                    postalCode: $('#shippingAddressControl .PostalCode').fullVal(),
                    //country: $('#shippingAddressCountry :selected').data("countrycode")
                    country: $('#shippingAddressCountry').data("countrycode")
                });

                validation.init();


                $(document).bind("validAddressFound", function (event, address) {
                    var p = $(this).parent();
                    showLoading(p);
                    $('#editAddressModal').jqmHide();
                    var postBaseUrl = '<%= ResolveUrl("~/Accounts/BillingShippingProfiles") %>';
                    if (($('#currentArea').val() == 'Orders' && $('#currentController').val() == 'OrderEntry') || ($('#currentArea').val() == 'Accounts' && $('#currentController').val() == 'Autoships'))
                        postBaseUrl = '<%= ResolveUrl("~/") %>' + $('#currentArea').val() + '/' + $('#currentController').val();

                    //alert("SaveAddress Accounts-Billing and Shipping Profiles");
                    $.post(postBaseUrl + '/SaveAddress', {
                        addressId: '<%= Model.AddressID %>'+"",
                        profileName: $('#shippingAddressProfileName').val() + "",
                        attention: $('#shippingAddressAttention').val() + "",
                        Address1: address.address1 + "",
                        address2: address.address2 + "",
                        address3: address.address3 + "",
                        postalCode: address.postalCode + "",
                        city: address.city + "",
                        county: $('#shippingAddressCounty').val() + "",
                        //state: address.state,
                        state: $('#shippingAddressState').val() + "",
                        street: $('#shippingAddressStreet').val() + "",
                        countryId: $('#shippingAddressCountry').val() + "",
                        phone: $('#shippingAddressPhone').length ? $('#shippingAddressPhone').phone('getPhone') + "" : '',
                        email: $('#shippingProfileEmail').length ? $('#shippingProfileEmail').val() + "" : ''
                    }, function (response) {
                        if (response.result) {
                            hideLoading(p);
                            getAddresses();
                            if (window['OnAddressSavedSuccessfully'])
                                window['OnAddressSavedSuccessfully'](response.addressID);
                        }
                        else {
                            hideLoading(p);
                            showMessage(response.message, true);
                        }
                    });
                });
            }
        });
    });
</script>
<h2>
    <%= Model.AddressID == 0 ? Html.Term("AddaNewShippingAddress", "Add a new shipping address") : Html.Term("Edit") + " " + Model.Name %></h2>
<%
    Html.RenderPartial("Address", new AddressModel()
                                      {
                                          Address = Model,
                                          LanguageID = CoreContext.CurrentLanguageID,
                                          ShowCountrySelect = true,
                                          ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                                          Prefix = "shippingAddress",
                                      });
    if (Model.AddressTypeID == 2 && NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting("ShowShipToEmail", false))
    {
        var prop = Model.AddressProperties.Where<NetSteps.Data.Entities.AddressProperty>((x) => x.AddressPropertyTypeID == Convert.ToInt32(NetSteps.Data.Entities.Constants.AddressPropertyType.EmailAddress)).FirstOrDefault(); 
%>
<div class="FRow">
    <div class="FLabel">
        <%= Html.Label(Html.Term("Notification Email Address")) %>
    </div>
    <div class="FInput">
        <%=Html.TextBox("shippingProfileEmail", prop == null ? "" : prop.PropertyValue) %>
    </div>
</div>
<%
   }
%>
<%--<%= AddressControl.RenderAddress(Model, CoreContext.CurrentLanguageID, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", prefix: "shippingAddress")%>--%>
<br />
<p>
    <a id="btnSaveAddress" href="javascript:void(0);" class="Button BigBlue">
        <%= Html.Term("SaveAddress", "Save Address")%></a> <a href="javascript:void(0);"
            class="Button jqmClose">
            <%= Html.Term("Cancel")%></a>
</p>
<span class="ClearAll"></span>