<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
    var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext;
    var usCountry = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.CountryCode.ToUpper() == "US"); 
	bool displayUserNameField = (bool)ViewData["DisplayUserNameField"];
%>
<script type="text/javascript">

    $(function () {
        window.onbeforeunloadmessage = '<%= Html.Term("TheEnrollmentProcessWillBeTerminatedIfYouNavigateAway", "The enrollment process will be terminated if you navigate away.") %>';
        $('#sponsor').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
		.jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributors") %>', { onSelect: function (item) {
		    $('#sponsorId').val(item.id);
		}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
		}).keyup(function () {
		    $(this).clearError();
		});
        $('#firstName').watermark('<%= Html.JavascriptTerm("FirstName", "First Name") %>');
        $('#middleName').watermark('<%= Html.JavascriptTerm("MiddleName", "Middle Name") %>');
        $('#lastName').watermark('<%= Html.JavascriptTerm("LastName", "Last Name") %>');
        $('#mainAddressAddress1').focusout(function () {
            if ($('#mainAddressAddress1').val())
                $('#mainAddressAddress1').clearError();
        });
        $('#phone').phone();
        $('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] });
        
        $('#taxExempt').click(function () {
            $(this).prop('checked') && $('#ssnContainer').fadeIn('fast') || $('#ssnContainer').fadeOut('fast');
        });

        function addSSNInputs() {
            if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString()  %>') {
                $('#ssn').inputsByFormat('destroy');
                $('#ssn').inputsByFormat({ format: '{0}', validateNumbers: false, attributes: [{ id: 'txtSSNPart1', length: 16, size: 16}] });
            }
            else {
                $('#ssn').inputsByFormat('destroy');
                $('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] });
            }
        }

        addSSNInputs();
        $('#mainAddressCountry').change(function () {
            addSSNInputs();
        });

        $('#password').watermark('<%= Html.JavascriptTerm("Password") %>');
        $('#passwordConfirm').watermark('<%= Html.JavascriptTerm("Confirmpassword", "Confirm password") %>');

        $('#generateUsername').click(function () {
            $(this).prop('checked') && $('#manualUsername').fadeOut('fast') || $('#manualUsername').fadeIn('fast');
        });

        $('#generatePassword').click(function () {
            $(this).prop('checked') && $('#manualPassword').fadeOut('fast') || $('#manualPassword').fadeIn('fast');
        });

        $('#btnNext').click(function () {
            var data = getData();
            if (data === false)
                return false;
            ValidateAddress();
        });
    });

    function getData() {
        var errorCount = 0;
        if (!$('#retailCustomerInfo').checkRequiredFields()) {
            errorCount++;
        }
        if (!$('#sponsorId').val()) {
            $('#sponsor').showError('<%= Html.JavascriptTerm("PleaseSelectaSponsor", "Please select a sponsor") %>');
            errorCount++;
        }

        if (!$('#generatePassword').prop('checked') && $('#password').val() != $('#passwordConfirm').val()) {
            $('#password,#passwordConfirm').showError('<%= Html.JavascriptTerm("PasswordsDoNotMatch", "Passwords do not match") %>').keyup(function () {
                if ($('#password').val() == $('#passwordConfirm').val())
                    $('#password,#passwordConfirm').clearError();
            });
            $('#password').focus();
            return false;
        }

        if (!$('#mainAddressAddress1').val() && $('#mainAddressAddress1').is(':visible')) {
            $('#mainAddressAddress1').showError('<%= Html.JavascriptTerm("AddressLine1Required", "Address Line 1 is required.") %>');
            errorCount++;
        }

        var postalCode = $('#mainAddressControl .PostalCode');
        if (!postalCode.val() && postalCode.is(':visible')) {
            postalCode.showError('<%= Html.JavascriptTerm("PostalCodeRequired", "Postal Code is required.") %>');
            errorCount++;
        }

        if (!$('#mainAddressCity').val() && $('#mainAddressCity').is(':visible')) {
            $('#mainAddressCity').showError('<%= Html.JavascriptTerm("CityRequired", "City is required.") %>');
            errorCount++;
        }


        if (!$('#mainAddressState').val() && $('#mainAddressState').is(':visible')) {
            $('#mainAddressState').showError('<%= Html.JavascriptTerm("StateRequired", "State is required.") %>');
            errorCount++;
        }

        if ($('#taxExempt').prop('checked') && !$('#ssn').inputsByFormat('getValue')) {
            $('#ssn input').showError('').filter(':last').clearError().showError('<%= Html.JavascriptTerm("SSNIsRequired", "SSN is required") %>').end()
		.keyup(function () {
		    if ($('#ssn').inputsByFormat('getValue'))
		        $('#ssn input').clearError();
		    else
		        $('#ssn input').showError('').filter(':last').clearError().showError('<%= Html.JavascriptTerm("SSNIsRequired", "SSN is required") %>')
		});
            errorCount++;
        }
        if (errorCount > 0) {
            return false;
        }
        else {
            return true;
        }
    }

    function ValidateAddress() {
        // do address validation
        var validation = abstractAddressValidation({
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

        validation.init();

        $(document).bind("validAddressFound", function (event, address) {
            var p = $(this).parent();
            showLoading(p);
            var data = {
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
                'mainAddress.Attention': $('#mainAddressAttention').val(),
                'mainAddress.Address1': address.address1,
                'mainAddress.Address2': address.address2,
                'mainAddress.Address3': address.address3,
                'mainAddress.PostalCode': address.postalCode,
                'mainAddress.City': address.city,
                'mainAddress.County': $('#mainAddressCounty').val(),
                'mainAddress.State': address.state,
                'mainAddress.CountryID': $('#mainAddressCountry').val(),
                'mainAddress.PhoneNumber': $('#mainAddressPhone').data('phone') ? $('#mainAddressPhone').phone('getPhone') : $('#mainAddressPhone').val()
            };

            window.letUnload = false;
            enrollmentMaster.postStepAction({
                step: "AccountInfo",
                stepAction: "SubmitStep",
                data: data,
                showLoadingElement: $('#btnNext'),
                load: true
            });
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
    <table id="retailCustomerInfo" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 150px;">
                <span class="requiredMarker">*</span>
                <label for="sponsor">
                    <%= Html.Term("Sponsor") %>:</label>
            </td>
            <td>
                <input type="text" id="sponsor" style="width: 300px;" />
                <input type="hidden" id="sponsorId" />
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                <span class="requiredMarker">*</span>
                <%= Html.Term("Name") %>:
            </td>
            <td>
                <input type="text" id="firstName" name="<%= Html.Term("FirstNameRequired", "First Name is required.") %>"
                    class="required" />
                <input type="text" id="middleName" />
                <input type="text" id="lastName" name="<%= Html.Term("LastNameRequired", "Last Name is required.") %>"
                    class="required" />
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                <span class="requiredMarker">*</span>
                <label for="email">
                    <%= Html.Term("Email") %>:</label>
            </td>
            <td>
                <input type="text" id="email" name="<%= Html.Term("EmailRequired", "Email is required.") %>"
                    class="required" style="width: 250px;" />
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                <span class="requiredMarker">*</span>
                <label for="languageId">
                    <%= Html.Term("Language") %>:</label>
            </td>
            <td>
                <%= Html.DropDownLanguages(htmlAttributes: new { id = "languageId" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>
            </td>
        </tr>
        <tr>
            <td style="width: 150px;">
                <label for="taxExempt">
                    <%= Html.Term("TaxExempt", "Tax Exempt") %>:</label>
            </td>
            <td>
                <input type="checkbox" id="taxExempt" />
            </td>
        </tr>
        <tr id="ssnContainer" style="display: none;">
            <td style="width: 150px;">
                <span class="requiredMarker">*</span>
                <%= Html.Term("SSN") %>:
            </td>
            <td id="ssn">
               <%--<input type="text" id="ssn" name="<%= Html.Term("SSNRequired", "SSN is required.") %>" class="required" /> --%> 
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
			<td style="<%= !displayUserNameField ? "display: none" : "width: 13.636em" %>" >
				<span class="requiredMarker">*</span>
				<label for="username">
					<%= Html.Term("Username")%>:</label>
			</td>
			<td style="<%= !displayUserNameField ? "display: none" : "" %>" >
				<input type="checkbox" id="generateUsername" checked="checked" /><%= Html.Term("GenerateUsername", "Generate Username (will be Account Number)")%>
				<div id="manualUsername" style="display: none;">
					<input type="text" id="username" name="<%= Html.Term("UsernameRequired", "Username is required.") %>" class="required" autocomplete="off" />
				</div>
			</td>
		</tr>
		<tr>
			<td style="width: 150px;">
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
        <tr style="visibility:hidden">
            <td>		                        
                <% var items = TempData["getEBanks"]; %>
		     </td>
        </tr>
    </table>
    <div id="addresses">
        <h4>
            <%= Html.Term("MainAddress", "Main Address") %></h4>
        <div class="FauxTable">
            <% 
                Html.RenderPartial("Address", new AddressModel()
                {
                    Address = null,
                    LanguageID = CoreContext.CurrentLanguageID,
                    ShowCountrySelect = true,
                    ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                    ExcludeFields = new List<string>() { "ProfileName" },
                    Prefix = "mainAddress"
                }); 
            %>
            <%--<%= AddressControl.RenderAddress(null, enrollmentContext.LanguageID.Value, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName" })%>--%>
        </div>
    </div>
    <span class="requiredMarker">*</span> =
    <%= Html.Term("Required") %>
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
sdsds
    <a id="btnNext" href="javascript:void(0);" class="Button BigBlue">
        <%= Html.Term("Next") %>&gt;&gt;</a>
</p>
<% Html.RenderPartial("AddressValidation"); %>
