﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
	var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext;
	var usCountry = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.CountryCode.ToUpper() == "US");
	bool displayUserNameField = (bool)ViewData["DisplayUserNameField"];
%>

<script type="text/javascript">
	$(function () {
		window.onbeforeunloadmessage = '<%= Html.Term("TheEnrollmentProcessWillBeTerminatedIfYouNavigateAway", "The enrollment process will be terminated if you navigate away.") %>';

		function initPhones() {
			$('#phones .phoneContainer').each(function () {
				var phone = $('.phone', this), number = phone.text(), guid = newGuid();
				if (phone.data('phone')) {
					number = phone.phone('getPhone');
					phone.phone('destroy');
				} else if ($('.phoneInput', phone).length) {
					number = $('.phoneInput', phone).val();
				}
				phone.empty();
				if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
					phone.html('<input type="text" value="' + number + '" class="phoneInput" />');
				}
				else {
					phone.phone({ areaCodeId: 'txtAreaCode' + guid, firstThreeId: 'txtFirstThree' + guid, lastFourId: 'txtLastFour' + guid }).phone('setPhone', number);
				}
			});
		}

		function addSsnInputs() {
			if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString()  %>') {
				$('#ssn').inputsByFormat('destroy');
				$('#ssn').inputsByFormat({ format: '{0}', validateNumbers: false, attributes: [{ id: 'txtSSNPart1', length: 16, size: 16}] });
			}
			else {
				$('#ssn').inputsByFormat('destroy');
				$('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] });
			}
		}
		
		$('#expMonth,#expYear').change(function () {
			var today = new Date();
			var lastDayInMonth = new Date(today.getFullYear(), today.getMonth(), 0);
			if (new Date().setFullYear($('#expYear').val(), $('#expMonth').val() - 1, lastDayInMonth.getDate()) > today) {
				$('#expMonth,#expYear').clearError();
			}
		});

		var currentPhone = 0;
		$('#sponsor').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by id or name") %>')
		.jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributors") %>', { onSelect: function (item) {
			$('#sponsor').clearError();
			$('#sponsorId').val(item.id);
		}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
		});
		$('#firstName').watermark('<%= Html.JavascriptTerm("FirstName", "First Name") %>');
		$('#middleName').watermark('<%= Html.JavascriptTerm("MiddleName", "Middle Name") %>');
		$('#lastName').watermark('<%= Html.JavascriptTerm("LastName", "Last Name") %>');

		$('#password').watermark('<%= Html.JavascriptTerm("Password") %>');
		$('#passwordConfirm').watermark('<%= Html.JavascriptTerm("Confirmpassword", "Confirm password") %>');

		$('#mainAddressPhone').phone({
			areaCodeId: 'mainAddressAreaCode',
			firstThreeId: 'mainAddressFirstThree',
			lastFourId: 'mainAddressLastFour'
		});
		$('#shippingAddressPhone').phone({
			areaCodeId: 'shippingAddressAreaCode',
			firstThreeId: 'shippingAddressFirstThree',
			lastFourId: 'shippingAddressLastFour'
		});
		$('#billingAddressPhone').phone({
			areaCodeId: 'billingAddressAreaCode',
			firstThreeId: 'billingAddressFirstThree',
			lastFourId: 'billingAddressLastFour'
		});

		$('#taxExempt').click(function () {
			$(this).prop('checked') && $('#ssnContainer').fadeIn('fast') || $('#ssnContainer').fadeOut('fast');
		});

		$('#generateUsername').click(function () {
			$(this).prop('checked') && $('#manualUsername').fadeOut('fast') || $('#manualUsername').fadeIn('fast');
		});

		$('#generatePassword').click(function () {
			$(this).prop('checked') && $('#manualPassword').fadeOut('fast') || $('#manualPassword').fadeIn('fast');
		});
        //Phone Default initial Charge 
        //
          
         


		$('#btnAddPhone').click(function () {
			var phoneControl = $('<span class="phone"></span>'),
					container = $('<p class="phoneContainer"></p>')
									.append(phoneControl)
									.append('<select class="phoneType"><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>')
									.append('<a href="javascript:void(0);" class="DeletePhone DTL Remove"></a>');
			$('#phones').append(container);
			if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
				phoneControl.append('<input type="text" class="phoneInput" />');
			} else {
				phoneControl.phone({
					areaCodeId: 'phone' + currentPhone + 'AreaCode',
					firstThreeId: 'phone' + currentPhone + 'FirstThree',
					lastFourId: 'phone' + currentPhone + 'LastFour'
				});
				++currentPhone;
			}
		});

		$('#preferredCustomerInfo .DeletePhone').live('click', function () {
			$(this).parent().fadeOut('fast', function () {
				$(this).remove();
			});
		});

		$('#shippingAddressControl,#billingAddressControl').each(function () { $(this).parent().hide(); });

		$('#chkUseMainForShipping').click(function () {
			$(this).prop('checked') && $('#shippingAddressControl').parent().fadeOut('fast') || $('#shippingAddressControl').parent().fadeIn('fast');
		});
		$('#chkUseMainForBilling').click(function () {
			$(this).prop('checked') && $('#billingAddressControl').parent().fadeOut('fast') || $('#billingAddressControl').parent().fadeIn('fast');
		});

		//$('#dateOfBirth').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' });
		$('#dateOfBirth').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] });
		$('#txtDOBMonth').watermark('mm');
		$('#txtDOBDay').watermark('dd');
		$('#txtDOBYear').watermark('yyyy');

		addSsnInputs();
		$('#mainAddressCountry').change(function () {
			addSsnInputs();
			initPhones();
		});

		$('#btnNext').click(function () {
			var data = getData();
			if (data === false)
				return false;
			ValidateAddress();
		});
	});

	function getData() {

		$('#txtDOBYear').clearError();
		$('#txtDOBDay').clearError();
		$('#txtDOBMonth').clearError();
		var isDateValid = CheckValidDate($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
		if (!isDateValid) 
		{
			$('#txtDOBDay').showError("");
			$('#txtDOBMonth').showError("");
			$('#txtDOBYear').showError('<%= Html.JavascriptTerm("InvalidDOB","Please enter the valid DOB") %>');
			return false;
		}
		else
		{
			var isAgeValid = CheckValidAge($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
			if (!isAgeValid) {
				$('#txtDOBDay').showError("");
				$('#txtDOBMonth').showError("");
				$('#txtDOBYear').showError('<%= Html.JavascriptTerm("DOBInValidYear","Age should be greater than 18") %>');
				return false;
			}
		}

		if (!$('#preferredCustomerInfo').checkRequiredFields() || !$('#addresses').checkRequiredFields()) {
			return false;
		}
		if (!$('#sponsorId').val()) {
			$('#sponsor').showError('<%= Html.JavascriptTerm("PleaseSelectaSponsor", "Please select a sponsor") %>').focus();
			return false;
		}
		if (!$('#generatePassword').prop('checked') && $('#password').val() != $('#passwordConfirm').val()) {
			$('#password,#passwordConfirm').showError('<%= Html.JavascriptTerm("PasswordsDoNotMatch", "Passwords do not match") %>').keyup(function () {
				if ($('#password').val() == $('#passwordConfirm').val())
					$('#password,#passwordConfirm').clearError();
			});
			$('#password').focus();
			return false;
		}

		if ($('#taxExempt').prop('checked') && !$('#ssn').inputsByFormat('getValue')) {
			$('#ssn input').showError('').filter(':last').clearError().showError('<%= Html.JavascriptTerm("SSNIsRequired", "SSN is required") %>').end()
		.keyup(function () {
			if ($('#ssn').inputsByFormat('getValue'))
				$('#ssn input').clearError();
			else
				$('#ssn input').showError('').filter(':last').clearError().showError('<%= Html.JavascriptTerm("SSNIsRequired", "SSN is required") %>')
		});
			return false;
		}

		if (!CreditCard.validate($('#accountNumber').val()).isValid) {
			$('#accountNumber').showError('<%= Html.JavascriptTerm("CreditCardNumberIsInvalid", "Credit card number is invalid.") %>').focus().keyup(function () {
				if (CreditCard.validate($(this).val()).isValid)
					$(this).clearError();
				else
					$(this).showError('<%= Html.JavascriptTerm("CreditCardNumberIsInvalid", "Credit card number is invalid.") %>')
			});
			return false;
		}

		var today = new Date();
		var lastDayInMonth = new Date(today.getFullYear(), today.getMonth(), 0);
		if (new Date().setFullYear($('#expYear').val(), $('#expMonth').val() - 1, lastDayInMonth.getDate()) < today) {
			$('#expMonth,#expYear').showError('<%= Html.JavascriptTerm("ThisExpirationDateIsInThePast", "This expiration date is in the past.") %>');
			$('#expMonth').focus();
			return false;
		}
	}

	function ValidateAddress() {
		// do address validation
		var validation;
		if ($('#chkUseMainForShipping').prop('checked'))
			validation = abstractAddressValidation({
				address1: $('#mainAddressAddress1').val(),
				address2: $('#mainAddressAddress2').val(),
				address3: $('#mainAddressAddress3').val(),
				//city: $("#mainAddressCity :selected").text() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity :selected").text(), // in case Country is not US, it is a simple text input
                city: $("#mainAddressCity").val() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity").val(), // in case Country is not US, it is a simple text input
				//state: $('#mainAddressState :selected').val(),
                state: $('#mainAddressState').val(),
				postalCode: $('#mainAddressControl .PostalCode').fullVal(),
				//country: $('#mainAddressCountry :selected').data("countrycode")
                country: $('#mainAddressCountry').data("countrycode")
			});
		else
			validation = abstractAddressValidation({
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
			var data;
			if ($('#chkUseMainForShipping').prop('checked'))
				data = {
					sponsorId: $('#sponsorId').val(),
					firstName: $('#firstName').val(),
					middleName: $('#middleName').val(),
					lastName: $('#lastName').val(),
					email: $('#email').val(),
					generateUsername: $('#generateUsername').prop('checked'),
					username: $('#username').val(),
					generatePassword: $('#generatePassword').prop('checked'),
					password: $('#password').val(),
					languageId: $('#languageId').val(),
					taxExempt: $('#taxExempt').prop('checked'),
					taxNumber: $('#taxExempt').prop('checked') ? $('#ssn').inputsByFormat('getValue') : '',
					gender: $('#gender').val(),
					dateOfBirth: $('#dateOfBirth').inputsByFormat('getValue', '{0}/{1}/{2}'),
					nameOnCard: $('#nameOnCard').val(),
					accountNumber: $('#accountNumber').val(),
					expirationDate: $('#expMonth').val() + '/1/' + $('#expYear').val(),
					//main address
					'mainAddress.Attention': $('#mainAddressAttention').val(),
					'mainAddress.Address1': address.address1,
					'mainAddress.Address2': address.address2,
					'mainAddress.Address3': address.address3,
					'mainAddress.PostalCode': address.postalCode,
					'mainAddress.City': address.city,
					'mainAddress.County': $('#mainAddressCounty').val(),
					'mainAddress.State': address.state,
					'mainAddress.CountryID': $('#mainAddressCountry').val()
				};
			else
				data = {
					sponsorId: $('#sponsorId').val(),
					firstName: $('#firstName').val(),
					middleName: $('#middleName').val(),
					lastName: $('#lastName').val(),
					email: $('#email').val(),
					generateUsername: $('#generateUsername').prop('checked'),
					username: $('#username').val(),
					generatePassword: $('#generatePassword').prop('checked'),
					password: $('#password').val(),
					languageId: $('#languageId').val(),
					taxExempt: $('#taxExempt').prop('checked'),
					taxNumber: $('#taxExempt').prop('checked') ? $('#ssn').inputsByFormat('getValue') : '',
					gender: $('#gender').val(),
					dateOfBirth: $('#dateOfBirth').inputsByFormat('getValue', '{0}/{1}/{2}'),
					nameOnCard: $('#nameOnCard').val(),
					accountNumber: $('#accountNumber').val(),
					expirationDate: $('#expMonth').val() + '/1/' + $('#expYear').val(),
					//main address
					'mainAddress.Attention': $('#mainAddressAttention').val(),
					'mainAddress.Address1': $('#mainAddressAddress1').val(),
					'mainAddress.Address2': $('#mainAddressAddress2').val(),
					'mainAddress.Address3': $('#mainAddressAddress3').val(),
					'mainAddress.PostalCode': $('#mainAddressControl .PostalCode').fullVal(),
					//'mainAddress.City': $("#mainAddressCity :selected").text() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity :selected").text(),
                    'mainAddress.City': $("#mainAddressCity").val() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity").val(),
					'mainAddress.County': $('#mainAddressCounty').val(),
					//'mainAddress.State': $('#mainAddressState :selected').val(),
                    'mainAddress.State': $('#mainAddressState').val(),
					'mainAddress.CountryID': $('#mainAddressCountry').val(),
				};


			if (!$('#chkUseMainForShipping').prop('checked')) {
				//shipping address
				data['shippingAddress.Attention'] = $('#shippingAddressAttention').val();
				data['shippingAddress.Address1'] = address.address1;
				data['shippingAddress.Address2'] = address.address2;
				data['shippingAddress.Address3'] = address.address3;
				data['shippingAddress.PostalCode'] = address.postalCode;
				data['shippingAddress.City'] = address.city;
				data['shippingAddress.County'] = $('#shippingAddressCounty').val();
				data['shippingAddress.State'] = address.state;
				data['shippingAddress.CountryID'] = $('#shippingAddressCountry').val();
			}

			if (!$('#chkUseMainForBilling').prop('checked')) {
				//billing address
				data['billingAddress.Attention'] = $('#billingAddressAttention').val();
				data['billingAddress.Address1'] = $('#billingAddressAddress1').val();
				data['billingAddress.Address2'] = $('#billingAddressAddress2').val();
				data['billingAddress.Address3'] = $('#billingAddressAddress3').val();
				data['billingAddress.PostalCode'] = $('#billingAddressControl .PostalCode').fullVal();
				data['billingAddress.City'] = $('#billingAddressCity').val();
				data['billingAddress.County'] = $('#billingAddressCounty').val();
				data['billingAddress.State'] = $('#billingAddressState').val();
				data['billingAddress.CountryID'] = $('#billingAddressCountry').val();
			}

			if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
				$('#preferredCustomerInfo .phoneContainer').filter(function () {
					return !!$('.phoneInput', this).val();
				}).each(function (i) {
					data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
					data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
					data['phones[' + i + '].PhoneNumber'] = $('.phoneInput', this).val();
				});
			} else {
				$('#preferredCustomerInfo .phoneContainer').filter(function () {
					return !!$('.phone', this).phone('getPhone');
				}).each(function (i) {
					data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
					data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
					data['phones[' + i + '].PhoneNumber'] = $('.phone', this).phone('getPhone');
				});
			}
			window.letUnload = false;
			enrollmentMaster.postStepAction({
				step: "AccountInfo",
				stepAction: "SubmitStep",
				data: data,
				showLoadingElement: $('#btnNext'),
				load: true
			});
		});

         $(document).ready(function () {
          var phoneControl = $('<span class="phone"></span>'),
					container = $('<p class="phoneContainer"></p>')
									.append(phoneControl)
									.append('<select class="phoneType"><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>')
									.append('<a href="javascript:void(0);" class="DeletePhone DTL Remove"></a>');
          $('#phones').append(container);
          if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
              phoneControl.append('<input type="text" class="phoneInput" />');
          } else {
              phoneControl.phone({
                  areaCodeId: 'phone' + currentPhone + 'AreaCode',
                  firstThreeId: 'phone' + currentPhone + 'FirstThree',
                  lastFourId: 'phone' + currentPhone + 'LastFour'
              });
              ++currentPhone;
          }
  });
	}

</script>

<div class="StepGutter">
	<h3>
		<b>
			<%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"])%></b>
		<%= Html.Term("EnterInTheAccountInformation", "Enter in the account information") %></h3>
</div>
<div class="StepBody">
	<table id="preferredCustomerInfo" width="100%" cellspacing="0" class="DataGrid">
		<tr>
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<label for="sponsor">
					<%= Html.Term("Sponsor") %>:</label>
			</td>
			<td>
				<% AccountSlimSearchData sponsor = ViewData["Sponsor"] as AccountSlimSearchData; %>
				<input type="text" id="sponsor" style="width: 25em;" value="<%= sponsor == null ? "" : sponsor.FullName + " (#" + sponsor.AccountNumber + ")" %>" />
				<input type="hidden" id="sponsorId" value="<%= sponsor == null ? "" : sponsor.AccountID.ToString() %>" />
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<%= Html.Term("Name") %>:
			</td>
			<td>
				<input type="text" id="firstName" name="<%= Html.Term("FirstNameRequired", "First Name is required.") %>" class="required" />
				<input type="text" id="middleName" />
				<input type="text" id="lastName" name="<%= Html.Term("LastNameRequired", "Last Name is required.") %>" class="required" />
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<label for="email">
					<%= Html.Term("Email") %>:</label>
			</td>
			<td>
				<input type="text" id="email" name="<%= Html.Term("EmailRequired", "Email is required.") %>" class="required" style="width: 20.833em;" />
			</td>
		</tr>
		<tr>
			<td style="<%= !displayUserNameField ? "display: none" : "width: 13.636em" %>" >
				<span class="requiredMarker" >*</span>
				<label for="username" ><%= Html.Term("Username")%>:</label>
			</td>
			<td style="<%= !displayUserNameField ? "display: none" : "" %>" >
				<input type="checkbox" id="generateUsername" checked="checked" /><%= Html.Term("GenerateUsername", "Generate Username (will be Account Number)")%>
				<div id="manualUsername" style="display: none;">
					<input type="text" id="username" name="<%= Html.Term("UsernameRequired", "Username is required.") %>" class="required" autocomplete="off" />
				</div>
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<label for="password">
					<%= Html.Term("Password") %>:</label>
			</td>
			<td>
				<input type="checkbox" id="generatePassword" checked="checked" /><%= Html.Term("GenerataRandomPassword", "Generate a random password") %>
				<div id="manualPassword" style="display: none;">
					<input type="password" id="password" name="<%= Html.Term("PasswordRequired", "Password is required.") %>" class="required" autocomplete="off" /><br />
					<input type="password" id="passwordConfirm" name="<%= Html.Term("PasswordConfirmationRequired", "Password Confirmation is required.") %>" class="required" />
				</div>
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<label for="languageId">
					<%= Html.Term("Language") %>:</label>
			</td>
			<td>
				<%= Html.DropDownLanguages(htmlAttributes: new { id = "languageId" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>        				
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<label for="taxExempt">
					<%= Html.Term("TaxExempt", "Tax Exempt") %>:</label>
			</td>
			<td>
				<input type="checkbox" id="taxExempt" />
			</td>
		</tr>
		<tr id="ssnContainer" style="display: none;">
			<td style="width: 13.636em;">
				<span class="requiredMarker">*</span>
				<%= Html.Term("SSN") %>:
			</td>
			<td id="ssn">
			    <%-- <input type="text" id="ssn" name="<%= Html.Term("SSNRequired", "SSN is required.") %>" class="required" />--%>
			</td>
		</tr>
		<tr>
			<td>
				<label for="gender">
					<%= Html.Term("Gender") %>:</label>
			</td>
			<td>
				<select id="gender">
					<option value="NotSet">
						<%= Html.Term("PreferNotToSay", "Prefer not to say") %></option>
					<option value="Male">
						<%= Html.Term("Male") %></option>
					<option value="Female">
						<%= Html.Term("Female") %></option>
				</select>
			</td>
		</tr>
		<tr>
			<td>
				<span class="requiredMarker">*</span>
				<%= Html.Term("DateOfBirth", "Date of Birth") %>:
			</td>
			<td id="dateOfBirth">
				<%--<input id="dateOfBirth" class="DatePicker TextInput" value="Date of birth" />--%>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("PhoneNumbers", "Phone Numbers") %>:
			</td>
			<td>
				<div id="phones">
				</div>
				<a id="btnAddPhone" href="javascript:void(0);" class="DTL Add">
					<%= Html.Term("AddaPhoneNumber", "Add a phone number") %></a>
			</td>
		</tr>
	</table>
	<div id="addresses">
		<h4>
			<span class="requiredMarker">*</span>
			<%= Html.Term("MainAddress", "Main Address") %></h4>
		<div class="FauxTable">
			<% Html.RenderPartial("Address", new AddressModel()
				   {
					   Address = null,
					   LanguageID = CoreContext.CurrentLanguageID,
					   ShowCountrySelect = true,
					   ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
					   ExcludeFields = new List<string>() { "ProfileName" },
					   Prefix = "mainAddress"
				   }); %>
			<%--<%= AddressControl.RenderAddress(null,enrollmentContext.LanguageID.Value, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName" }, "mainAddress")%>--%>
		</div>
		<h4>
			<%= Html.Term("ShippingAddress", "Shipping Address") %></h4>
		<div class="FauxTable enrollShippingInfo">
			<p class="FRow">
				<span class="FLabel">
					<%= Html.Term("UseMainAddress", "Use Main Address") %></span> <span class="FRow">
						<input type="checkbox" id="chkUseMainForShipping" checked="checked" /></span>
			</p>
			<% Html.RenderPartial("Address", new AddressModel()
				   {
					   Address = null,
					   LanguageID = CoreContext.CurrentLanguageID,
					   ShowCountrySelect = true,
					   ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
					   ExcludeFields = new List<string>() { "ProfileName" },
					   Prefix = "shippingAddress"
				   }); %>
			<%--<%= AddressControl.RenderAddress(null,enrollmentContext.LanguageID.Value, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName" }, "shippingAddress")%>--%>
		</div>
		<h4>
			<%= Html.Term("BillingInformation", "Billing Information") %></h4>
		<div class="FauxTable enrollBillingInfo">
			<div class="FRow">
				<span class="FLabel"><span class="requiredMarker">*</span>
					<%= Html.Term("NameOnCard", "Name on Card") %>:</span> <span class="FInput">
						<input type="text" maxlength="50" id="nameOnCard" name="<%= Html.Term("NameOnCardIsRequired", "Name On Card is required.") %>" class="required" /></span>
			</div>
			<div class="FRow">
				<span class="FLabel"><span class="requiredMarker">*</span>
					<label for="accountNumber">
						<%= Html.Term("CreditCardNumber", "Credit Card #") %>:</label></span> <span class="FInput">
							<input type="text" maxlength="16" id="accountNumber" name="<%= Html.Term("CardNumberIsRequired", "Card Number is required.") %>" class="required" /></span>
			</div>
			<div class="FRow">
				<span class="FLabel"><span class="requiredMarker">*</span>
					<%= Html.Term("Expiration") %>:</span> <span class="FInput">
						<select id="expMonth" name="Exp Month is required." title="0" class="required">
							<% for (int i = 1; i <= 12; i++)
		  { %>
							<option value="<%= i %>" <%= i == DateTime.Today.Month + 1 ? "selected=\"selected\"" : "" %>>
								<%= i + " - " + Html.Term(Enum.ToObject(typeof(Constants.Month), i).ToString()) %></option>
							<% } %>
						</select>
						/
						<select id="expYear" name="Exp Year is required." title="" class="required">
							<% for (int i = DateTime.Today.Year; i <= DateTime.Today.Year + 15; i++)
		  {%>
							<option value="<%= i %>">
								<%= i.ToString() %></option>
							<% } %>
						</select>
					</span>
			</div>
			<input type="checkbox" id="chkUseMainForBilling" checked="checked" />
				<label for="chkUseMainForBilling"><%= Html.Term("UseMainAddress", "Use Main Address") %></label><br />
			<% Html.RenderPartial("Address", new AddressModel()
				   {
					   Address = null,
					   LanguageID = CoreContext.CurrentLanguageID,
					   ShowCountrySelect = true,
					   ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
					   ExcludeFields = new List<string>() { "ProfileName" },
					   Prefix = "billingAddress"
				   }); %>
			<%--<%= AddressControl.RenderAddress(null, enrollmentContext.LanguageID.Value, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName" }, "billingAddress")%>--%>
		</div>
	</div>
	<!--/end step body -->
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
	<a id="btnNext" href="javascript:void(0);" class="Button BigBlue"><%= Html.Term("Next") %>&gt;&gt;</a>
</p>
<% Html.RenderPartial("AddressValidation"); %>