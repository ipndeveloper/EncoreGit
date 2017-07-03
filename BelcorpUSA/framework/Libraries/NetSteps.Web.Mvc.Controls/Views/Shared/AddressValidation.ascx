<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script src="<%= Url.Content("~/Scripts/address-validation.js") %>"></script>
<div id="UnverifiedAddress" class="jqmWindow LModal left" style="z-index: 9000; width: 760px;">
    <div class="mContent">
        <h2>
            <%:Html.Term("AddressVerificationFailed_Heading", "We were unable to verify your address") %></h2>
        <div class="pad5 mb10 mt20">
            <div class="FR splitCol addressSuggestions">
                <h3 class="UI-darkBg pad5 brdrYYNN suggestedAddressesHeader">
                    <%: Html.Term("AddressVerificationFailed_SuggestionsHeading", "Here are some alternative suggestions")%></h3>
                <div class="brdr1 pad2 suggestedAddressList">
                    <%-- <div class="m5 pad5 brdrAll suggestedAddressContainer">
                        <div class="FL suggestedAddress">
                            <span class="street1">5555 S Example Address</span><br>
                            <span class="cityState">Salt Lake City, UT</span><br>
                            <span class="zip">84102</span>
                        </div>
                        <a class="FR UI-icon-container"><span class="icon-label">Select</span> <span class="FR UI-icon icon-arrowNext"></span></a><span class="clr"></span>
                    </div>
                    <div class="m5 pad5 brdrAll suggestedAddressContainer">
                        <div class="FL suggestedAddress">
                            <span class="street1">5555 S Example Address</span><br>
                            <span class="street2">Suite 2102</span><br>
                            <span class="cityState">Salt Lake City, UT</span><br>
                            <span class="zip">84102</span>
                        </div>
                        <a class="FR UI-icon-container"><span class="icon-label">Select</span> <span class="FR UI-icon icon-arrowNext"></span></a><span class="clr"></span>
                    </div>--%>
                </div>
            </div>
            <div class="FL pad5 UI-lightBg brdrAll splitCol useTypedAddress">
                <div class="mt10 typedAddress">
                    <div class="FormContainer pad5">
                        <div id="countryContainer" class="FRow">
                            <div class="FLabel">
                                <label for="country">
                                    Country:</label></div>
                            <div class="FInput">
                                <input type="text" id="unverified-country" readonly="readonly" class="country" value="UnitedStates" />
                            </div>
                            <br>
                        </div>
                        <div id="unverifiedAddressControl">
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="address1">
                                        <span class="req">*</span>Address Line 1:</label>
                                </div>
                                <div class="FInput">
                                    <input size="27" data-val="true" data-val-required="Address Line 1 is required."
                                        value="Original entered St 1234 West" name="Address1" maxlength="50" id="unverified-address1"
                                        class="Address1 required">
                                </div>
                                <br>
                            </div>
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="address2">
                                        Address Line 2:</label>
                                </div>
                                <div class="FInput">
                                    <input size="27" value="Suite 2101" name="Address2" maxlength="50" id="unverified-address2"
                                        class="Address2">
                                </div>
                                <br>
                            </div>
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="address3">
                                        Address Line 3:</label>
                                </div>
                                <div class="FInput">
                                    <input size="27" name="Address3" maxlength="50" id="unverified-address3" class="Address3">
                                </div>
                                <br>
                            </div>
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="zip">
                                        <span class="req">*</span>Postal Code:</label>
                                </div>
                                <div class="FInput">
                                    <input class="PostalCode required" id="unverified-postalcode" maxlength="5" name="Postal Code is required."
                                        value="84604" size="5">
                                    &nbsp;-&nbsp;
                                    <input size="4" name="" maxlength="4" id="zipPlusFour" class="PostalCode"><img style="height: 15px;
                                        display: none;" alt="" src="/Content/Images/Icons/loading-blue.gif" class="zipLoading"><br>
                                </div>
                                <br>
                            </div>
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="city">
                                        <span class="req">*</span>City:</label>
                                </div>
                                <div class="FInput">
                                    <input type="text" id="unverified-city" class="City required" value="some city" />
                                </div>
                                <br>
                            </div>
                            <%--   <div class="FRow">
                                <div class="FLabel">
                                    <label for="county">
                                        <span class="req">*</span>County:</label>
                                </div>
                                <div class="FInput">
                                    <input type="text" id="unverified-county" class="County required" />
                                    <%--<select class="County required" id="county" name="County is required.">
                                        <option value="Utah">Utah</option>
                                        <option value="Wasatch">Wasatch</option>
                                    </select>
                                </div>
                                <br>
                            </div>--%>
                            <div class="FRow">
                                <div class="FLabel">
                                    <label for="state">
                                        <span class="req">*</span>State:</label>
                                </div>
                                <div class="FInput">
                                    <input type="text" id="unverified-state" class="State required" value="state" />
                                </div>
                                <br>
                            </div>
                        </div>
                    </div>
                </div>
                <a class="FR mb10 Button btnContinue useTypedAddress" href="#"><span>
                    <%: Html.Term("AddressVerificationFailed_ContinueButton", "Continue with this address") %></span></a>
                <span class="clr"></span>
            </div>
            <div class="FL mt10 mb10 upsValidationNote">
                <span class="FR pad5 upsNotice">Address Validation<br />
                    is Powered by UPS</span>
                <img class="FL pad5 upsLogo" src="<%= Url.Content("~/Resource/Content/Images/UPSLogo.png") %>"
                    alt="" />
            </div>
            <span class="clr"></span>
        </div>
        <span class="clr"></span>
    </div>
</div>
