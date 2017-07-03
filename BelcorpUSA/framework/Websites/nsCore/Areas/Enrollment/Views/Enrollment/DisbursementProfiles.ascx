<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="NetSteps.Commissions.Common.Models" %>
<%@ Import Namespace="NetSteps.Encore.Core.IoC" %>
<% 
    var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext;
    Address addressOfRecord = enrollmentContext.EnrollingAccount.Addresses != null
                                ? (Address)enrollmentContext.EnrollingAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address()
                                : new Address();

    var formattedName = ((NetSteps.Data.Entities.Account)enrollmentContext.EnrollingAccount).FullName != null ? ((NetSteps.Data.Entities.Account)enrollmentContext.EnrollingAccount).FullName.Replace("\'", "\\\'") : "";
    var formattedAddress1 = addressOfRecord.Address1 != null ? addressOfRecord.Address1.Replace("\'", "\\\'") : "";
	var formattedAddress2 = addressOfRecord.Address2 != null ? addressOfRecord.Address2.Replace("\'", "\\\'") : "";
	var formattedAddress3 = addressOfRecord.Address3 != null ? addressOfRecord.Address3.Replace("\'", "\\\'") : "";
	var formattedCity = addressOfRecord.City != null ? addressOfRecord.City.Replace("\'", "\\\'") : "";
	var formattedState = addressOfRecord.State != null ? addressOfRecord.State.Replace("\'", "\\\'") : "";
	string formattedZip = string.Empty;
    string formattedPlusFour = string.Empty;
    if(addressOfRecord.PostalCode != null)
    {
        if (addressOfRecord.CountryID == (int)Constants.Country.UnitedStates)
        {
            if (addressOfRecord.PostalCode.Length == 5)
            {
                formattedZip = addressOfRecord.PostalCode;
            }
            else if (addressOfRecord.PostalCode.Length == 9)
            {
                formattedZip = addressOfRecord.PostalCode.Substring(0, 5);
                formattedPlusFour = addressOfRecord.PostalCode.Substring(5);
            }
        }
    }
    
 %>


<script type="text/javascript">
	$(function () {
		//Add the following 4 lines to the top of any addon - DES
		window.enrollmentAccountNumber = '<%= enrollmentContext.EnrollingAccount.AccountNumber %>';
		if (parseBool('<%= ViewData["IsSkippable"] %>')) {
			$('#btnDPSkip').show();
		}


		$(".Tabber li").click(function () {
			$(".Tabber .current").removeClass("current");
			$(this).addClass("current");
			$(".TabContent").css("display", "none");
			var content_show = $(this).attr("rel");
			$("#" + content_show).css("display", "block");
			return false;
		});

		var checkUseAddressOfRecord = function () {
			if ($('#chkUseAddressOfRecord').prop('checked')) {
				$('#check input:not(#chkUseAddressOfRecord),#check select').attr('disabled', 'disabled');
				$('#checkProfileAttention').val('<%= formattedName %>');
				$('#checkProfileAddress1').val('<%= formattedAddress1 %>');
				$('#checkProfileAddress2').val('<%= formattedAddress2 %>');
				$('#checkProfileAddress3').val('<%= formattedAddress3 %>');
				$('#checkProfileCity').val('<%= formattedCity %>');
				$('#checkProfileState').val('<%= formattedState %>');
				$('#checkProfileStreet').val('<%= formattedStreet %>');/*CS*/
				$('#checkProfileZip').val('<%= formattedZip %>');
                $('#checkProfileZipPlusFour').val('<%= formattedPlusFour %>');
			} else {
				$('#check input:not(#chkUseAddressOfRecord),#check select').removeAttr('disabled');
				$('#check input:text').val('');
			}
		};

		$('#chkUseAddressOfRecord').click(checkUseAddressOfRecord);
		checkUseAddressOfRecord();

		$('.percentToDeposit').keyup(function () {
			var total = 0;
			$('.percentToDeposit').each(function () {
				total += parseFloat($(this).val());
			});
			if (total > 100)
				$(this).val(parseFloat($(this).val()) - (total - 100));
		}).numeric();

		$('#btnDPNext').click(function () {
//			var t = this;
//			showLoading(t);

			var data = getData();
			if (data === false) {
//				hideLoading(t);
				return false;
			}

			window.letUnload = false;
			enrollmentMaster.postStepAction({
				step: "DisbursementProfiles",
				stepAction: "SubmitStep",
				data: data,
				showLoadingElement: $(this).parent(),
				load: true
			});
		});

		$('#btnDPSkip').click(function () {
			window.letUnload = false;
			enrollmentMaster.postStepAction({
				step: "DisbursementProfiles",
				stepAction: "SkipStep",
				load: true
			});
		});
	});

function getData() {
    alert("getData");
        var data, isComplete = true;
        if ($('#check').is(':visible')) {
            if ($('#chkUseAddressOfRecord').prop('checked')) {
                data = {
                    id: $('#CheckDisbursementProfileID').val(),
                    preference: 'Check',
                    useAddressOfRecord: true
                };
            } else {
                isComplete = $('#check').checkRequiredFields();
                if (isComplete) {
                    data = {
                        id: $('#CheckDisbursementProfileID').val(),
                        preference: 'Check',
                        useAddressOfRecord: false,
                        profileName: "checkProfileProfileName999", //$('#checkProfileProfileName').val(),
                        payableTo: "12",//$('#checkProfileAttention').val(),
                        address1: "12", //$('#checkProfileAddress1').val(),
                        address2: "12", //$('#checkProfileAddress2').val(),
                        address3: "12", //$('#checkProfileAddress3').val(),
                        city: "12", //$('#checkProfileCity').val(),
                        county: "12", //$('#checkProfileCounty').val(),
                        street: "12", //$('#checkProfileStreet').val(),
                        state: "12", //$('#checkProfileState').val(),
                        zip: "12", //($('#checkProfileCountry').val() == 1) ? $('#checkProfileZip').val() : $('#checkProfilePostalCode').val(),
                        country: "12"//$('#checkProfileCountry').val()
                    };
                }
            }
        } else {
            isComplete = $('#eft').checkRequiredFields();
            if (isComplete) {
                var total = 0;
                $('.percentToDeposit').numeric().each(function () {
                    if (!isNaN($(this).val()) && $(this).val()!="")
                        total += parseFloat($(this).val());
                });
                
                if (total == 100) {
                    $('#eft input').each(function () {
                        var t = $(this);
                        if (t.val() == t.data('watermark'))
                            t.val('');
                    });
                    data = {
                        id: $('#CheckDisbursementProfileID').val(),
                        preference: 'EFT',
                        agreementOnFile: $('#chkHardRelease').prop('checked')
                    };
                    var i = 1, i2;
                   
                    for (i; i <= $('.EFTAccount').length; i++) {
                        i2 = i - 1;
                        data['accounts[' + i2 + '].DisbursementProfileID'] = $('#EFTDisbursementProfileID' + i).val();
                        data['accounts[' + i2 + '].Enabled'] = $('#chkEnabledAccount' + i).prop('checked');
                        data['accounts[' + i2 + '].Name'] = $('#txtNameAccount' + i).val();
                        data['accounts[' + i2 + '].RoutingNumber'] = $('#txtRoutingNumberAccount' + i).val();
                        data['accounts[' + i2 + '].AccountNumber'] = $('#txtAccountNumberAccount' + i).val();
                        data['accounts[' + i2 + '].BankName'] = $('#txtBankNameAccount' + i).val();
                        data['accounts[' + i2 + '].BankPhone'] = $('#bankPhoneAccount' + i).phone('getPhone');
                        data['accounts[' + i2 + '].BankAddress1'] = $('#txtBankAddressLine1Account' + i).val();
                        data['accounts[' + i2 + '].BankAddress2'] = $('#txtBankAddressLine2Account' + i).val();
                        data['accounts[' + i2 + '].BankAddress3'] = $('#txtBankAddressLine3Account' + i).val();
                        data['accounts[' + i2 + '].BankCity'] = $('#txtBankCityAccount' + i).val();
                        data['accounts[' + i2 + '].BankState'] = $('#txtBankStateAccount' + i).val();
                        data['accounts[' + i2 + '].BankZip'] = $('#txtBankZipAccount' + i).val();
                        data['accounts[' + i2 + '].AccountType'] = $('#accountTypeAccount' + i).val();
                        data['accounts[' + i2 + '].PercentToDeposit'] = $('#percentToDepositAccount' + i).val();
                    }
                } else {
                    showMessage('Please make sure the percent to deposit is exactly 100%', true);
                    isComplete = false;
                }
            }
        }
        return isComplete ? data : false;
    }
</script>
<div class="StepGutter">
    <h3>
        <b>
            <%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"])%></b>
        <%= Html.Term("SetUpYourDisbursementProfile(s)", "Set up your disbursement profile(s)") %></h3>
</div>
<div class="StepBody">
    <ul class="Tabber" id="PaymentMethods">
        <%if ((bool)ViewData["EnableCheckProfile"])
          { %>
        <li class="current" rel="check"><a href="javascript:void(0);"><span>
            <%= Html.Term("Pay with Check") %></span></a></li>
        <%} if ((bool)ViewData["EnableEFTProfile"])
          { %>
        <li rel="eft"><a href="javascript:void(0);"><span>
            <%= Html.Term("Pay with EFT") %></span></a></li>
        <% } %>
    </ul>
    <span class="ClearAll"></span>
    <%if ((bool)ViewData["EnableCheckProfile"])
      { %>
    <div id="check" class="TabContent">
        <h3>
            <%= Html.Term("CheckProfile", "Check Profile") %>:
            <input id="chkUseAddressOfRecord" checked="checked" type="checkbox" /><label for="chkUseAddressOfRecord"><%= Html.Term("UseAddressOfRecord", "Use Address Of Record") %></label>
            <input type="hidden" id="CheckDisbursementProfileID" name="CheckDisbursementProfileID" value="<%= ViewBag.CheckProfileID %>"/>
        </h3>
        <% 
               
          Html.RenderPartial("Address", new AddressModel()
 {
     Address = addressOfRecord,
     LanguageID = CoreContext.CurrentLanguageID,
     ShowCountrySelect = true,
     ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
     ExcludeFields = new List<string>() { "ProfileName", "PhoneNumber" },
     Prefix = "checkProfile"
 }); %>
        <%--<%= NetSteps.Web.Mvc.Business.Controls.AddressControl.RenderAddress(addressOfRecord, CoreContext.CurrentLanguageID, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName", "PhoneNumber" }, "checkProfile")%>--%>
    </div>
    <%} if ((bool)ViewData["EnableEFTProfile"])
      { %>
    <div id="eft" class="TabContent" style="display: none;">
        <h3>
            <%= Html.Term("EFTProfile", "EFT Profile") %>:
            <input id="chkHardRelease" type="checkbox" /><label for="chkHardRelease"><%= Html.Term("HardRelease/AgreementOn-File", "Hard Release/Agreement On-File") %></label></h3>
        <% for (int i = 1; i < 3; i++)
           { %>
        <div class="FL" style="margin-right: 15px;">
            <% Html.RenderPartial("~/Areas/Accounts/Views/DisbursementProfiles/DisbursementProfileEFTAccountInfo.ascx"
                   , new DisbursementProfileEFTAccountInfoEditModel(Create.New<IEFTDisbursementProfile>()), new ViewDataDictionary()
		 {
			 {"AccountNumber", i}
		 }); %>
        </div>
        <% } %>
        <span class="ClearAll"></span>
    </div>
    <%}       
    %>
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
    <a id="btnDPNext" href="javascript:void(0);" class="Button BigBlue"><%= Html.Term("Next") %>&gt;&gt;</a>
    <a id="btnDPSkip" href="javascript:void(0);" class="Button" style="display: none;"><%= Html.Term("Skip") %>&gt;&gt;</a>
</p>
