<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.EditAccountViewModel>" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
<%@ Import Namespace="NetSteps.Data.Entities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%
		    string cultureInfo = (String) ViewData["countryID"];

            Country usCountry = ViewBag.CountryUS;
            IUser currentUser = ViewBag.CurrentUser;
            NetSteps.Data.Entities.Account accountToEdit = ViewBag.EditAccount;

            Address addressOfRecord = accountToEdit.Addresses.GetDefaultByTypeID(Constants.AddressType.Main)
                ?? new Address { AddressTypeID = (short)Constants.AddressType.Main, CountryID = (int)Constants.Country.UnitedStates };
                
            if (cultureInfo.Equals("USA"))
            {
           
                //GR-4749 Se agrega para el manejo de caracteres especiales
                if (addressOfRecord.County.IndexOf("'") > 0) addressOfRecord.County = addressOfRecord.County.Replace("'", "");
            }

            if (cultureInfo.Equals("BRA"))
            {
                dynamic additional = ViewBag.EditAccountAdditional;
                //GR-4602 Se agrega para el manejo de caracteres especiales
                addressOfRecord.County = Regex.Replace(addressOfRecord.County, @"\'?[^\w\.@-]", " ");
            }
            var accountPropertyType = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.AccountPropertyTypes;
            var accountPropertyExemptReason = accountPropertyType.Where(x => x.TermName == "ExemptReason").FirstOrDefault();
            var ExemptReasonAccPropTypeID = accountPropertyExemptReason != null ? accountPropertyExemptReason.AccountPropertyTypeID : 0;
            
       
        
	%>
    <style type="text/css">
        .mailLocked
        {
            border-style: solid;
            border-color: #0000ff;
            box-shadow: 0 0 5px 1px #969696;
        }
    </style>
<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/functions.js") %>"></script>
<script type="text/javascript">
        
    	$(function () {

            
			<%if(cultureInfo.Equals("BRA")){ %>
				$.post('<%=ResolveUrl("~/Accounts/Edit/CanEditAccountType") %>', function (response) {
                    if (response.result == 1) $( "#sAccountType" ).prop( "disabled", false ); 
                    else $( "#sAccountType" ).prop( "disabled", true );
                });

		         $('#txtRG').numeric();
		         $('#valor3').numeric();
			<%}%>
            
			<%if(cultureInfo.Equals("USA")){ %>
			    function initPhones() {
					    $('#phones .phoneContainer').each(function () {
						    var phone = $('.phone', this), number = phone.text(), guid = newGuid();
						    if (phone.data('phone')) {
							    number = phone.phone('getPhone');
							    phone.phone('destroy');
						    } else if ($('.phoneInput', phone).length) {
							    number = $('.phoneInput', phone).val().trim();
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
			<%}%>
			var generatedPassword = false,
			isComplete = true,
			currentPhone = 0,
			checkPasswords = function () {
				var confirm = $('#txtConfirmPassword');
				if (confirm.val() != $('#txtPassword').val()) {
					confirm.showError('<%= Html.JavascriptTerm("PasswordsMustMatch", "The passwords must match") %>');
					isComplete = false;
				} else {
					confirm.clearError();
				}
			}, checkSSN = function () {
				if (!/\d{9}/.test($('#ssn').inputsByFormat('getValue'))) {
					if (/\*{5}\d{4}/.test($('#ssn').inputsByFormat('getValue')) && '<%= accountToEdit.TaxNumber %>'.length) {
							$('#txtSSNPart1,#txtSSNPart2,#txtSSNPart3').clearError();
					} else {
							$('#txtSSNPart1,#txtSSNPart2').showError('');
							$('#txtSSNPart3').showError('<%= Html.JavascriptTerm("InvalidSSN", "Invalid SSN, SSN must be 9 digits.") %>');
							isComplete = false;
					}
				}
			};
			
			<%if(cultureInfo.Equals("USA")){ %>
				$('#dob').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] }).inputsByFormat('setValue', '<%= (accountToEdit.Birthday == null) ? "" : accountToEdit.Birthday.ToDateTime().ToString("MM/dd/yyyy").Replace("/", "") %>');
			<%}%>
			
			$('#txtDOBMonth').watermark('mm');
			$('#txtDOBDay').watermark('dd');
			$('#txtDOBYear').watermark('yyyy');
            
            <%if(cultureInfo.Equals("USA")){ %>
			    initPhones();
			<%}%>

			<%if(cultureInfo.Equals("BRA")){ %>
				 $('#txtIssueMonth').watermark('mm');
           		$('#txtIssueDay').watermark('dd');
           		$('#txtIssueYear').watermark('yyyy');
			<%}%>
            
			$('#phones .DeletePhone').live('click', function () {
					$(this).parent().fadeOut('fast', function () {
						$(this).remove();
					});
			});
            
			$('#sponsorModal').jqm({ modal: true,
					trigger: '#btnChangeSponsor',
					onShow: function (h) {
						h.w.css({
							top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
							left: Math.floor(parseInt($(window).width() / 2)) + 'px'
						}).fadeIn();
					},
					onHide: function (hash) {
						hash.w.fadeOut('fast', function () {
							hash.o.remove();
							$('#txtAdminUsername,#txtAdminPassword').val('').clearError();
						});
					}
			});

			$('#enrollerModal').jqm({ modal: true,
				trigger: '#btnChangeEnroller',
				onShow: function (h) {
					h.w.css({
						top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
						left: Math.floor(parseInt($(window).width() / 2)) + 'px'
					}).fadeIn();
				},
				onHide: function (hash) {
					hash.w.fadeOut('fast', function () {
						hash.o.remove();
						$('#txtAdminUsername,#txtAdminPassword').val('').clearError();
					});
				}
			});

			// For some reason, some inputs randomly get trampled on, so this is to prevent that.
			// $('#txtDOB').val('<%= accountToEdit.Birthday.ToShortDateString() %>');
			$('#txtAddress2').val('<%: addressOfRecord.Address2 %>');
			$('#txtAddress3').val('<%: addressOfRecord.Address3 %>');

			$('#isTaxExempt').click(function () {
					if ($('#isTaxExempt').prop('checked')) {
						$("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").show('fast');
					}
					else {
						$("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").hide('fast');
						$('#PropertyType<%= ExemptReasonAccPropTypeID %>').val('');
					}
			});

			if ($('#isTaxExempt').prop('checked')) {
					$("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").show('fast');
			}
			else {
					$("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").hide('fast');
			}

			$('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').change(function () {
					if ($('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').val() != '') {
						$('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').clearError();
					}
			});

			$('#btnGeneratePassword').click(function () {
					var newPassword = '';
					for (var i = 0; i < 8; i++) {
						newPassword += String.fromCharCode((Math.random() * 75) + 48);
					}
					$('#txtPassword,#txtConfirmPassword').val(newPassword);
					generatedPassword = true;

					$('#encrypedPassword').hide();
					$('#newPassword,#newPasswordConfirm').show();
			});

			$('#txtPassword').keyup(function () {
					generatedPassword = false;
			});

			$('#txtConfirmPassword').blur(checkPasswords);
			
			<%if(cultureInfo.Equals("BRA")){ %>
				$('.phone').numeric();
			<%}%>
			
			$('#btnAddPhone').click(function () {
					var phoneControl = $('<span class="phone"></span>'),
						container = $('<p class="phoneContainer"></p>')
							.append(phoneControl)
							.append('<select class="phoneType"><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>')
							.append('<a href="javascript:void(0);" class="DeletePhone DTL Remove"></a>');
					$('#phones').append(container);
			
					<%if(cultureInfo.Equals("BRA")){ %>
						var HTMLInputs = '<input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;"> - ' + 
									 '<input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" > ';

						phoneControl.append(HTMLInputs);
					<%}%>
					<%if(cultureInfo.Equals("USA")){ %>
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
					 <%}%>
					 <%if(cultureInfo.Equals("BRA")){ %>
						$('.phone').numeric();
					<%}%>
			});

			function checkForSSN() {
				if (!/\d{9}/.test($('#ssn').inputsByFormat('getValue'))) {
					if (/\*{5}\d{4}/.test($('#ssn').inputsByFormat('getValue')) && '<%= accountToEdit.TaxNumber %>'.length) {
						$('#txtSSNPart1,#txtSSNPart2,#txtSSNPart3').clearError();
						$('#txtEINPart1,#txtEINPart2').clearError();
					} else {

						$('#txtSSNPart1,#txtSSNPart2').showError('');
						$('#txtSSNPart3').showError('<%= Html.JavascriptTerm("InvalidSSN", "Invalid SSN, SSN must be 9 digits.") %>');
						$('#txtSSNPart1,#txtSSNPart2,#txtSSNPart3').keyup(function () {
							if (!/\d{9}/.test($('#ssn').inputsByFormat('getValue'))) {
								if (/\*{5}\d{4}/.test($('#ssn').inputsByFormat('getValue')) && '<%= accountToEdit.TaxNumber %>'.length) {
									$('#txtSSNPart1,#txtSSNPart2,#txtSSNPart3').clearError();
								} else {
									$('#txtSSNPart1,#txtSSNPart2').showError('');
									$('#txtSSNPart3').showError('<%= Html.JavascriptTerm("InvalidSSN", "Invalid SSN, SSN must be 9 digits.") %>');
								}
							} else {
								$('#txtSSNPart1,#txtSSNPart2,#txtSSNPart3').clearError();
							}
						});

						$('#txtEINPart1').showError('');
						$('#txtEINPart2').showError('<%= Html.JavascriptTerm("InvalidSSN", "Invalid SSN, SSN must be 9 digits.") %>'); //--- May be different Error message for non-US people ??
						$('#txtEINPart1,#txtEINPart2').keyup(function () {
							if (!/\d{9}/.test($('#ssn').inputsByFormat('getValue'))) {
								if (/\*{5}\d{4}/.test($('#ssn').inputsByFormat('getValue')) && '<%= accountToEdit.TaxNumber %>'.length) {
									$('#txtEINPart1,#txtEINPart2').clearError();
								} else {
									$('#txtEINPart1').showError('');
									$('#txtEINPart2').showError('<%= Html.JavascriptTerm("InvalidSSN", "Invalid SSN, SSN must be 9 digits.") %>'); //--- May be different Error message for non-US people ??
								}
							} else {
								$('#txtEINPart1,#txtEINPart2').clearError();
							}
						});

						isComplete = false;
					}
				}
			};
            
			$('#btnSaveAccount').click(function () {
                    
					var p = $(this).parent();
					isComplete = $('table.DataGrid').checkRequiredFields() && $('.FormContainer').checkRequiredFields() && checkAddressRequiredFields();
					checkPasswords();

					// If account type Distributor then SSN is required 
					var accountType = $('#sAccountType').val();
					if (accountType === '<%= (int)Constants.AccountType.Distributor  %>' && $('#mainAddressCountry').val() === '<%= (int)Constants.Country.UnitedStates  %>') {
						checkForSSN();
					}

					<%if(cultureInfo.Equals("USA")){ %>
						if (!$('#sponsorAccountNumber').val() && ($('#sAccountType').val() == 1 || $('#sAccountType').val() == 2)) {
							$('#txtSponsor').showError('<%= Html.JavascriptTerm("ChooseASponsor", "You must choose a sponsor, please type in 3 letters to start searching for a sponsor.") %>');
							$('#btnChangeSponsor').showError('<%= Html.JavascriptTerm("ChooseASponsor", "You must choose a sponsor, please type in 3 letters to start searching for a sponsor.") %>');
							isComplete = false;
						}
						else {
							$('#btnChangeSponsor').clearError();
						}

						if (!$('#enrollerAccountNumber').val() && ($('#sAccountType').val() == 1 || $('#sAccountType').val() == 2))
						{
							<%if (currentUser.HasFunction("Accounts-Change Enroller")){%>
								$('#txtEnroller').showError('<%= Html.JavascriptTerm("ChooseAnEnroller", "You must choose an enroller, please type in 3 letters to start searching for an enroller.") %>');
								$('#btnChangeEnroller').showError('<%= Html.JavascriptTerm("ChooseAnEnroller", "You must choose an enroller, please type in 3 letters to start searching for an enroller.") %>');
							<%}
							else{%>
								$('#enrollerLabel').parent().showError('<%=Html.Term("ChooseAnEnrollerNotAuthrized", "You do not have the required permissions to choose an enroller for this account.")%>');
							<%}%>
							isComplete = false;
						}
						else {
							$('#btnChangeEnroller').clearError();
						}
					<%}%>

					if (($('#txtEmail').attr('class').contains('required') || $('#txtEmail').val() != '')
						&& !/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/.test($('#txtEmail').val()))
					{
						$('#txtEmail').showError('<%= Html.JavascriptTerm("InvalidEmail", "Please enter a valid email address.") %>');
						isComplete = false;
					}
					else {
						$('#txtEmail').clearError();
					}

					var isCompleteForTaxExempt = true;
					if ($('#isTaxExempt').prop('checked')) {
						if ($('#PropertyType<%= ExemptReasonAccPropTypeID %>').val() && !$('#PropertyType<%= ExemptReasonAccPropTypeID %>').val() > 0) {
							$('#PropertyType<%= ExemptReasonAccPropTypeID %>').showError('<%= Html.JavascriptTerm("TaxExemptReason", "Please select any exempt reason")%>');
							isCompleteForTaxExempt = false;
						}
						else {
							$('#PropertyType<%= ExemptReasonAccPropTypeID %>').clearError();
							isCompleteForTaxExempt = true;
						}
					}
					
					<%if(cultureInfo.Equals("BRA")){ %>
						$('#txtIssueDay').clearError();
						$('#txtIssueMonth').clearError();
						$('#txtIssueYear').clearError();
                        
						if ($("#txtIssueDay").val() != '' || $("#txtIssueMonth").val() != '' || $("#txtIssueYear").val() != ''){
							var isDateValid_Issue = CheckValidDate($("#txtIssueDay").val(), $("#txtIssueMonth").val(),  $("#txtIssueYear").val());
                            
							if (!isDateValid_Issue) {
								$('#txtIssueDay').showError("");
								$('#txtIssueMonth').showError("");                                           
								$('#txtIssueYear').showError('<%= Html.JavascriptTerm("InvalidIssue","Please enter a valid Issue") %>');
								isComplete = false;
							}
							else{
								var isAgeValidIssue = CheckValidDateCurrent($("#txtIssueDay").val(), $("#txtIssueMonth").val(), $("#txtIssueYear").val());
								if (!isAgeValidIssue) 
								{
									$('#txtIssueDay').showError("");
									$('#txtIssueMonth').showError("");
									$('#txtIssueYear').showError('<%= Html.JavascriptTerm("TheDateShouldnotExceedTheCurrentDate","The Date should not exceed the current date.") %>');
									//$('#txtIssueMonth').focus();
									isComplete=false;//  return false;
								}
								else
								{
									$('#txtIssueYear').clearError();
								}
							}
                            
						}
						else{
							$('#txtIssueDay').clearError();
							$('#txtIssueMonth').clearError();
							$('#txtIssueYear').clearError();
						}
                        
                        $('#txtCPFPart1').clearError();
                        $('#txtCPFPart2').clearError();
                        
                        if ($('#txtCPFPart1').val().length != 9 || $('#txtCPFPart2').val().length != 2)
                        {
                            $('#txtCPFPart1').showError("");                                           
							$('#txtCPFPart2').showError('<%= Html.JavascriptTerm("InvalidCPF","Please enter a valid CPF") %>');
							isComplete = false;
                        }
                        
                        $('#txtPISPart1').clearError();
                        $('#txtPISPart2').clearError();
                        if ($('#txtPISPart1').val()!=''){
                             if ($('#txtPISPart1').val().length != 9 || $('#txtPISPart2').val().length != 2)
                             {
                                $('#txtPISPart1').showError("");                                           
							    $('#txtPISPart2').showError('<%= Html.JavascriptTerm("InvalidPIS","Please enter a valid PIS") %>');
							    isComplete = false;
                             }
                         }

                         $('#txtRG').clearError();

                         if ($('#txtRG').val().trim() == '')
                         {                                     
							$('#txtRG').showError('<%= Html.JavascriptTerm("InvalidRG","Please enter a valid RG") %>');
							isComplete = false;
                         }

					<%}%>

		            
					$('#txtDOBYear').clearError();
					$('#txtDOBDay').clearError();
					$('#txtDOBMonth').clearError();
            	
					var isDateValid = CheckValidDate($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
					if (!isDateValid) {
						// Retail Customers and Prospects can sign up without a date of birth.
						// Allow saving without a date of birth for them if they have none entered yet.
						if ($('#txtDOBDay').val() != ''
							|| $('#txtDOBMonth').val() != ''
							|| $('#txtDOBYear').val() != ''
							|| ('<%=accountToEdit.AccountTypeID%>' !== '<%= (int)Constants.AccountType.RetailCustomer%>'
								&& '<%=accountToEdit.AccountTypeID%>' !== '<%= (int)Constants.AccountType.Prospect%>')
							|| <%= accountToEdit.Birthday != null ? "true" : "false" %>
						)
						{

							$('#txtDOBDay').showError("");
							$('#txtDOBMonth').showError("");
							$('#txtDOBYear').showError('<%= Html.JavascriptTerm("InvalidDOB","Please enter a valid DOB") %>');
							isComplete = false;
						}
					}
					else {
						var isAgeValid = CheckValidAge($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
						if (!isAgeValid) {
							$('#txtDOBDay').showError("");
							$('#txtDOBMonth').showError("");
							$('#txtDOBYear').showError('<%= Html.JavascriptTerm("DOBInValidYear","Age should be greater than 18") %>');
							isComplete =  false;
						}
					}
			
					<%if(cultureInfo.Equals("BRA")){ %>
						var mainPhoneOk = false;
						var cellPhoneOk = false;
						var alternativeOK = true;

						$('#phones .phoneContainer').each(function (i) {
							var phoneType = $('.phoneType', this).val();
							var field1 = $('.phoneInput1', this).val();
							var field2 = $('.phoneInput2', this).val();
							var validate = false;

							//Si los campos están vacíos y el tipo de teléfono no es principal, para obviar validación
							//o
							//Si Los campos cumplen con las validaciones
							if ((field1.length == 0 && field2.length == 0 && phoneType != "1") || (field1.length == 3 && (field2.length >= 8 && field2.length <= 9)))
							{
								validate = true;
							}
							else if (field1.length == 3 && (field2.length >= 8 && field2.length <= 9)){
								validate = true;
							}

							switch(phoneType){
								case "1": mainPhoneOk = validate; break;
								case "2": cellPhoneOk = validate; break;
								default: alternativeOK = validate; break
							}

							if (!validate)
								return validate;
						});
        
						$('#phones').clearError();
						if (!mainPhoneOk) {
							$('#phones').showError('<%= Html.JavascriptTerm("ValidMainPhoneIsRequired", "A Valid Main Phone Is Required") %>');
							isComplete=false;//  return false;
						}
						else{ 
							$('#phones').clearError();
						}

						if (!cellPhoneOk) {
							$('#phones').showError('<%= Html.JavascriptTerm("ValidCellPhoneIsRequired", "A Valid Cell Phone Is Required") %>');
						  isComplete=false;//
						  //  return false;
						}else if(isComplete)
						{
							 $('#phones').clearError();
						}

						if (!alternativeOK) {
							$('#phones').showError('<%= Html.JavascriptTerm("ValidAltPhoneIsRequired", "A Valid Phone Is Required") %>');
						  isComplete=false;//
						  //  return false;
						}else if(isComplete)
						{
							 $('#phones').clearError();
						}

					<%}%>
                    
					if (isComplete && isCompleteForTaxExempt) {
						var CountyVar = $('#mainAddressCounty').val();
						CountyVar= CountyVar .replace("'"," ");
						var data = {
							accountId: $('#accountId').val(),
							accountType: $('#sAccountType').val(),
							defaultLanguageId: $('#defaultLanguageId').val(),
							<%if(cultureInfo.Equals("USA")){ %>
							sponsorAccountNumber: $('#sponsorAccountNumber').val(),
							enrollerAccountNumber: $('#enrollerAccountNumber').val(),
							<%}%>
							applicationOnFile: $('#chkApplicationOnFile').prop('checked'),
							isEntity: $('#chkIsEntity').prop('checked'),
							entityName: $('#entityName').val(),
							username: $('#txtUsername').val(),
							password: $('#txtPassword').val(),
							confirmPassword: $('#txtConfirmPassword').val(),
							userChangingPassword: $('#newPassword').is(':visible'),
							generatedPassword: generatedPassword,
							attention: $('#mainAddressAttention').val(),
							address1: $('#mainAddressAddress1').val(),
							address2: $('#mainAddressAddress2').val(),
							address3: $('#mainAddressAddress3').val(),
							zip: $('#mainAddressControl .PostalCode').fullVal(),
							city: $('#mainAddressCity').val(),
							<%if(cultureInfo.Equals("USA")){ %>
								county: CountyVar,
							<%}%>
							<%if(cultureInfo.Equals("BRA")){ %>
								county: $('#mainAddressCounty').val(),
							<%}%>
							state: $('#mainAddressState').val(),
							<%if(cultureInfo.Equals("BRA")){ %>
								street: $('#mainAddressStreet').val(),
							<%}%>
							countryId: $('#mainAddressCountry').val(),
							phone: $('#mainAddressPhone').data('phone') ? $('#mainAddressPhone').phone('getPhone'): $('#mainAddressPhone').val(),
							firstName: $('#txtFirstName').val(),
							<%if(cultureInfo.Equals("BRA")){ %>
								middleName: $('#txtMiddleName').val(),
							<%}%>
							lastName: $('#txtLastName').val(),
							email: $('#txtEmail').val(),
							hostedEmail: $('#txtHostedMailAccount').val(),
							isTaxExempt: $('#isTaxExempt').prop('checked'),
							isTaxExemptVerified:$('#isTaxExemptVerified').val(),
                            <%if(cultureInfo.Equals("USA")){ %>
							    ssn: $('#sAccountType').val() == '<%= (int)Constants.AccountType.Distributor  %>' ? $('#ssn').inputsByFormat('getValue') : '',
                            <%}%>
                            <%if(cultureInfo.Equals("BRA")){ %>
                                ssn: '',
                            <%}%>
							
							<%if(cultureInfo.Equals("USA")){ %>
                                dob: $('#dob').inputsByFormat('getValue', '{2}/{0}/{1}'),
								gender: $('#gender').val(),
							<%}%>
							<%if(cultureInfo.Equals("BRA")){ %>
                                dob: $('#dob').inputsByFormat('getValue', '{0}/{1}/{2}'),
								gender: $('#gender').val() == '0' ? null : $('#gender').val(),
							<%}%>
							userstatus: $('#statusId').val(),
							accountstatus: $('#editAccountStatusId').val(),
							propertyValuedropdown: $('#PropertyType<%= ExemptReasonAccPropTypeID %>').val(),
							coApplicant: $('#txtCoApplicantName').val(),
						};
						
						<%if(cultureInfo.Equals("USA")){ %>
							if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
							    $('#phones .phoneContainer').filter(function () {
									    return !!$('.phoneInput', this).val();
							    }).each(function (i) {
									    data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
									    data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
									    data['phones[' + i + '].PhoneNumber'] = $('.phoneInput', this).val();
							    });
						    } else {
							    $('#phones .phoneContainer').filter(function () {
									    return !!$('.phone', this).phone('getPhone');
							    }).each(function (i) {
									    data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
									    data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
									    data['phones[' + i + '].PhoneNumber'] = $('.phone', this).phone('getPhone');
							    });
						    }
						<%}%>
						<%if(cultureInfo.Equals("BRA")){ %>
							$('#phones .phoneContainer').filter(function () {
									return !!$('.phone', this).phone('getPhone');
							}).each(function (i) {
                                data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
								data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
								data['phones[' + i + '].PhoneNumber'] = $('.phoneInput1', this).val() + '' + $('.phoneInput2', this).val();
								data['phones[' + i + '].IsDefault'] = $('.phoneType', this).val() == 1 ? true : false;
							});


							//AccountProperties**
							for (var i = 0; i < 11; i++) {                                        
								data['AccountProperties[' + i + '].AccountPropertyID'] =$('#AccountPropertyID' + i).val();  
								data['AccountProperties[' + i + '].AccountPropertyTypeID'] = $('#TypeID' + i).val();
							 
								if (i == 5 || i == 6 || i == 7){                     
									data['AccountProperties[' + i + '].PropertyValue'] = $('#valor' + i).prop('checked');
									}
								else {                
									if (i == 2 || i == 3 || i == 9) 
										data['AccountProperties[' + i + '].PropertyValue'] = $('#valor' + i).val();   
									else
									   data['AccountProperties[' + i + '].AccountPropertyValueID'] = $('#valor' + i).val();              
								   }
							}
                  
							//AccountSuppliedIDs**
							var cpf = $('#txtCPFPart1').val() + $('#txtCPFPart2').val() ;
							var pis = $('#txtPISPart1').val() + $('#txtPISPart2').val() ;
							var dateOfIssue=  $('#dateOfIssue').inputsByFormat('getValue', '{0}/{1}/{2}') == '//' ? null : $('#dateOfIssue').inputsByFormat('getValue', '{0}/{1}/{2}');
						 
							data['AccountSuppliedIDs[0].AccountSuppliedID'] = $('#hdnAccountSuppliedID0').val(); 
							data['AccountSuppliedIDs[0].AccountSuppliedIDValue'] = cpf; 
							data['AccountSuppliedIDs[0].IsPrimaryID'] = false;
							data['AccountSuppliedIDs[0].IDTypeID'] = 8;
							
					  
							data['AccountSuppliedIDs[1].AccountSuppliedID'] = $('#hdnAccountSuppliedID1').val(); 
							data['AccountSuppliedIDs[1].AccountSuppliedIDValue'] = pis; 
							data['AccountSuppliedIDs[1].IsPrimaryID'] = false;
							data['AccountSuppliedIDs[1].IDTypeID'] = 9;

							data['AccountSuppliedIDs[2].AccountSuppliedID'] = $('#hdnAccountSuppliedID2').val(); 
							data['AccountSuppliedIDs[2].AccountSuppliedIDValue'] = $('#txtRG').val(); 
							data['AccountSuppliedIDs[2].IDExpeditionIDate'] = dateOfIssue; 
							data['AccountSuppliedIDs[2].IsPrimaryID'] = true;                   
							data['AccountSuppliedIDs[2].IDTypeID'] = 4;
							data['AccountSuppliedIDs[2].ExpeditionEntity'] = $('#txtOrgExp').val();
                   

							//AccountReferences**
							var phonenumber = '0'; 
							if(typeof $('#phone') != 'undefined'){            
								phonenumber = $('#txtphoneNumberMain01').val() + $('#txtphoneNumberMain02').val() + $('#txtphoneNumberMain03').val();
								if( isNaN(phonenumber))
									phonenumber = '0'; 
							}
                            
							//  var phonenumber = $('#txtphoneNumberMain01').val() + $('#txtphoneNumberMain02').val() + $('#txtphoneNumberMain03').val();            
							data.ReferenceID = (typeof $('#AccountReferencesID') != 'undefined')?-1: $('#AccountReferencesID').val();//$('#AccountReferencesID').val();
							data.ReferenceName = (typeof $('#txtReferenceName') != 'undefined')?'': $('#txtReferenceName').val();//$('#txtReferenceName').val();
							data.PhoneNumberMain = phonenumber;
							data.RelationShip =(typeof $('#RelationShip') != 'undefined')?-1: $('#RelationShip').val();
					
						
							for (var i = 0; i < 6; i++) {
								data['AccountSocialNetworks[' + i + '].AccountSocialNetworkID'] = $('#AccountSocialNetworkID_' + i).val()
								data['AccountSocialNetworks[' + i + '].SocialNetworkID'] = $('#SocialNetworkID_' + i).val();
								data['AccountSocialNetworks[' + i + '].Value'] = $('#valor_' + i).val();
							}
						<%}%>

						showLoading(p);
						<%if(cultureInfo.Equals("USA")){ %>
							$.post('<%= ResolveUrl("~/Accounts/Edit/SaveUSA") %>', data, function (response) {
								hideLoading(p);
								if (response.result) {
										showMessage('<%= Html.Term("AccountSaved", "Account saved successfully") %>', false);
										$('#accountDetailsSponsor').val($('#sponsor').val());
										var name = $('#txtFirstName').val() + " " + $('#txtLastName').val()
										$('#fullNameYellowWidget').html('<h1>' + name + '</h1>');
								} else {
										showMessage(response.message, true);
								}
							}, 'json');
						<%}%>
                        
						<%if(cultureInfo.Equals("BRA")){ %>
                            $.each(data, function(i, item) {
                                console.log('['+i+']:'+item);
                            });

							$.post('<%= ResolveUrl("~/Accounts/Edit/SaveBRA") %>', data, function (response) {
								hideLoading(p);
								if (response.result) {
									showMessage('<%= Html.Term("AccountSaved", "Account saved successfully") %>', false);
									$('#accountDetailsSponsor').val($('#sponsor').val());
									var name = $('#txtFirstName').val() + " " + $('#txtLastName').val();
									$('#fullNameYellowWidget').html('<h1>' + name + '</h1>');

                                    //------
                                     $('#AccountReferencesID').val(response.referenceID);

                                    for (var i = 0; i < response.accountProperties.length; i++) {
                                        $('#AccountPropertyID' + i).val(response.accountProperties[i].accountPropertyID);
                                    }                                   
                                   for (var i = 0; i < response.accountSuppliedIDs.length; i++) {
                                        $('#hdnAccountSuppliedID' + i).val(response.accountSuppliedIDs[i].accountSuppliedID);
                                    }      
                                    for (var i = 0; i < response.accountSocialNetworks.length; i++) {
                                        $('#AccountSocialNetworkID_' + i).val(response.accountSocialNetworks[i].accountSocialNetworkID);
                                    }    
                                    //------
								} else {

									if(response.campo == 'CPF')  { 
										$('#txtCPFPart1').showError(''); 
										$('#txtCPFPart2').showError('<%= Html.JavascriptTerm("InvalidCPF","Please enter a valid CPF") %>');                   
									}
                               
									if(response.campo == 'PIS') { 
									   $('#txtPISPart1').showError('');
									   $('#txtPISPart2').showError('<%= Html.JavascriptTerm("InvalidPIS","Please enter a valid PIS") %>');
									
									}
                               
									if(response.campo == 'RG') {  
										$('#txtRG').showError('<%= Html.JavascriptTerm("InvalidRG","Please enter a valid RG") %>');
									}

									showMessage(response.message, true);
								}
							}, 'json');
						<%}%>
					} else {
						showMessage('<%= Html.Term("ErrorsBelow", "There are some errors below, please correct them before continuing.") %>', true);
					}

                    
			});

			$('#btnChangePassword,#btnCancelChangePassword').click(function () {
					$('#txtPassword,#txtConfirmPassword').val('')
						.toggleClass('required').clearError();

					generatedPassword = false;
					$('#encrypedPassword,#newPassword,#newPasswordConfirm').toggle();
			});

			$('#chkIsEntity').click(function () {
					$('#uxEntityName').toggle();
					if ($('#sAccountType').val() === '<%= (int)Constants.AccountType.Distributor  %>') {
						addSSNInputs();
					}
			});

			if (parseBool('<%= accountToEdit.IsEntity %>')) {
					$('#uxEntityName').show();
			}

			function addSSNInputs() {
					if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString()  %>') {
						$('#ssn').inputsByFormat('destroy');
						$('#ssn').inputsByFormat({ format: '{0}', validateNumbers: false, attributes: [{ id: 'txtSSNPart1', length: 16, size: 16}] });
					}
					else {
						$('#ssn').inputsByFormat('destroy');
						if ($('#chkIsEntity').prop('checked')) {
							$('#ssnLabel').text('<%= Html.Term("EIN") %>');
							$('#ssn').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtEINPart1', length: 2, size: 2 }, { id: 'txtEINPart2', length: 7, size: 7}] });
						} else {
							$('#ssnLabel').text('<%= Html.Term("SSN") %>');
							$('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] });
						}
					}
			}
			
			if ($('#sAccountType').val() === '<%= (int)Constants.AccountType.Distributor  %>') {
					addSSNInputs();
					$('#ssn').inputsByFormat('setValue', '<%= accountToEdit.DecryptedTaxNumber.MaskString(4) %>');
			}
			
			$('#mainAddressCountry').change(function () {
				if ($('#sAccountType').val() === '<%= (int)Constants.AccountType.Distributor  %>') {
					addSSNInputs();
					$('#ssn').inputsByFormat('setValue', '<%= accountToEdit.DecryptedTaxNumber.MaskString(4) %>');
				}
                <%if(cultureInfo.Equals("USA")){ %>
				    initPhones();
                <%} %>
			});

			$('#mainAddressCountry').triggerHandler('change');

			$('#editAccountStatusId').change(function () {
					var val = $(this).val();
					var siteStatus = $('#statusId');

					if (val === '<%= (short)Constants.AccountStatus.Terminated %>') {
						siteStatus.val('<%= (short)Constants.UserStatus.Inactive %>');
						siteStatus.attr('disabled', 'disabled');
					} else {
						siteStatus.val('<%= accountToEdit.AccountStatusID %>');
						siteStatus.removeAttr('disabled');
					}
			});

			disableSiteDropdown();

	});



	function disableSiteDropdown() {
		if ($('#editAccountStatusId').val() === '<%= (short)Constants.AccountStatus.Terminated %>') {
			$('#statusId').attr('disabled', 'disabled');
		}
	
		<%if(cultureInfo.Equals("BRA")){ %>
			/*CS.11JUL2016.Inicio.Solo Lectura en Edicion*/
			var cuentaID = $('#accountId').val();
			if(cuentaID > 0){
				$('#editAccountStatusId').attr('disabled', 'disabled');
			}
			/*CS.11JUL2016.Fin.Solo Lectura en Edicion*/
		<%}%>
	}

	function checkAddressRequiredFields()
	{
		var accountType = $('#sAccountType').val();

		if (accountType === '<%= (int)Constants.AccountType.Distributor %>' || accountType === '<%= (int)Constants.AccountType.PreferredCustomer %>')
		{
			return $('#addresses').checkRequiredFields();
		}
		return true;
	}   

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts")%></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
            <%= CoreContext.CurrentAccount.FullName %></a>
    <%= Html.Term("EditAccount", "Edit Account")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        string cultureInfo = (String) ViewData["countryID"];
   
            var currentUser = CoreContext.CurrentUser;
            NetSteps.Data.Entities.Account accountToEdit = CoreContext.CurrentAccount ?? new NetSteps.Data.Entities.Account() { User = new User() };
            dynamic additional = ViewBag.EditAccountAdditional;
            int IDNationality = additional.Nationality;
            int IDMaritalStatus = additional.MaritalStatus;
            int IDRelationShip = additional.RelationShip;
            int IDSchoolingLevel = additional.SchoolingLevel;
            int IDOccupation = additional.Occupation;
            bool IDHasComputer = additional.HasComputer;
            int IDInternetUseFrecuency = additional.InternetUseFrecuency;
            string isCheckedNetwork = additional.AcceptShareWithNetwork ? "checked=checked" : string.Empty;
            string isCheckedEmailFrom = additional.AcceptEmailFrom ? "checked=checked" : string.Empty;
            string isCheckedLocator = additional.AcceptShareForLocator ? "checked=checked" : string.Empty;

            if (accountToEdit.User == null)
                accountToEdit.User = new User();
            Address addressOfRecord = accountToEdit.Addresses.GetDefaultByTypeID(Constants.AddressType.Main)
               ?? new Address { AddressTypeID = (short)Constants.AddressType.Main, CountryID = (int)Constants.Country.UnitedStates };

            if (cultureInfo.Equals("USA"))
            {
                //GR-4749 Se agrega para el manejo de caracteres especiales
                if (addressOfRecord.County.IndexOf("'") > 0) addressOfRecord.County = addressOfRecord.County.Replace("'", "");
            }
            else if (cultureInfo.Equals("BRA"))
            {
                //GR-4602 Se agrega para el manejo de caracteres especiales
                addressOfRecord.County = Regex.Replace(addressOfRecord.County, @"\'?[^\w\.@-]", " ");
            }
            
            MailAccount mailAccount = MailAccount.LoadByAccountID(accountToEdit.AccountID);
            string mailAccountEmailAddress = string.Empty;
            if (mailAccount != null && !mailAccount.EmailAddress.IsNullOrEmpty())
                mailAccountEmailAddress = (mailAccount.EmailAddress.Contains('@')) ? mailAccount.EmailAddress.Split('@')[0] : string.Empty;

            string domain = MailDomain.LoadDefaultForInternal().DomainName.ToLower();

            List<string> campos = (List<string>)ViewData["listField"];
       %>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("EditAccount", "Edit Account")%></h2>
    </div>
    <input type="hidden" id="accountId" value="<%= accountToEdit.AccountID %>" />
    <h3>
        <%= Html.Term("AccountDetails", "Account Details") %></h3>
    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "AccountType"))
              { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="sAccountType">
                        <%= Html.Term("AccountType", "Account Type")%>:</label>
                </td>
                <td>
                    <%: Html.DropDownAccountType(selectedTypeID: accountToEdit.AccountTypeID, htmlAttributes: new { id = "sAccountType", disabled = "disabled" })%>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "DefaultLanguage"))
              { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="defaultLanguageId">
                        <%= Html.Term("DefaultLanguage", "Default Language")%>:</label>
                </td>
                <td>
                    <%: Html.DropDownLanguages(selectedLanguageID: accountToEdit.DefaultLanguageID, htmlAttributes: new { id = "defaultLanguageId" })%>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "Sponsor"))
              { %>
            <tr>
            <%--The term we will use that corresponds to the SponsorId in the database is Placement, so when you are changing the sponsorid, you are changing placement--%>
                <td>
                    <label for="sponsor">
                        <%= Html.Term("Placement")%>:
                    </label>
                </td>
                <td>
                    <span id="sponsor">
                        <% if (accountToEdit.SponsorInfo != null)
                           { %>
                        <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") + accountToEdit.SponsorInfo.AccountNumber %>">
                            <%: accountToEdit.SponsorInfo.AccountNumber + " - " + accountToEdit.SponsorInfo.FullName%>
                        </a>
                        <% } %>
                    </span>
                    
                    <input type="hidden" name="sponsorAccountId" id="sponsorAccountNumber" value="<%= (accountToEdit.SponsorInfo != null) ? accountToEdit.SponsorInfo.AccountNumber.ToString() : "" %>" />

                    <%--<% if (currentUser.HasFunction("Accounts-Change Sponsor"))
                       { %>
                        - <a href="javascript:void(0);" id="btnChangeSponsor">
                        <%= Html.Term("ClickHereToChangePlacement", "Click here to change placement")%></a>
                        <div id="sponsorModal" class="LModal jqmWindow">
                            <div class="mContent">
                                <% Html.RenderPartial("ChangeSponsor"); %>
                            </div>
                        </div>
                    <% } %>--%>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "OriginalSponsor"))
              { %>
            <tr>            
                <td>
                    <label for="enroller" id="enrollerLabel">
                        <%= Html.Term("Enroller")%>:
                    </label>
                </td>
                <td>
                    <span id="enroller">
                        <% if (accountToEdit.EnrollerInfo != null)
                           { %>
                            <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") + accountToEdit.EnrollerInfo.AccountNumber %>">
                                <%: accountToEdit.EnrollerInfo.AccountNumber + " - " + accountToEdit.EnrollerInfo.FullName%>
                            </a>
                        <% } %>
                    </span>
                    <input type="hidden" name="enrollerAccountId" id="enrollerAccountNumber" value="<%= (accountToEdit.EnrollerInfo != null) ? accountToEdit.EnrollerInfo.AccountNumber.ToString() : "" %>" />
                    <input type="hidden" name="hasEnrollerFunction" id="hasEnrollerFunctionId" value="<%= (currentUser.HasFunction("Accounts-Change Enroller")) %>" />
                    <%--<% if (currentUser.HasFunction("Accounts-Change Enroller"))
                       { %>
                        - <a href="javascript:void(0);" id="btnChangeEnroller">
                        <%= Html.Term("ClickHereToChangeEnroller", "Click here to change enroller")%></a>
                        <div id="enrollerModal" class="LModal jqmWindow">
                            <div class="mContent">                                
                                <% Html.RenderPartial("ChangeEnroller"); %>
                            </div>
                        </div>
                    <% } %>--%>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "ApplicationOnFile"))
              { %>
            <tr>
                <td>
                    <label for="chkApplicationOnFile">
                        <%= Html.Term("ApplicationOnFile", "Application On File")%>:</label>
                </td>
                <td>
                    <input type="checkbox" id="chkApplicationOnFile" <%= accountToEdit.ReceivedApplication ? "checked=\"checked\"" : "" %> />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "IsEntity"))
              { %>
            <tr>
                <td>
                    <label for="chkIsEntity">
                        <%= Html.Term("IsEntity", "Is Entity")%>:</label>
                </td>
                <td>
                    <input type="checkbox" id="chkIsEntity" <%= accountToEdit.IsEntity ? "checked=\"checked\"" : "" %> />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "EntityName"))
              { %>
            <tr id="uxEntityName" style="display: none">
                <td>
                    <label for="entityName">
                        <%= Html.Term("EntityName", "Entity Name")%>:</label>
                </td>
                <td>
                    <input type="text" id="entityName" value="<%= accountToEdit.EntityName %>" />
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
    <h3>
        <%= Html.Term("AccountAccess", "Account Access") %></h3>
    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
         <%if (NetSteps.Data.Entities.Account.checkField(campos, "Username"))
           { %>
		    <% if (Model.DisplayUsernameField)
         { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtUsername">
                        <%= Html.Term("Username")%>:</label>
                </td>
                <td>
                    <input type="text" id="txtUsername" value="<%= accountToEdit.User.Username %>" name="<%= Html.Term("UsernameRequired", "Username is required") %>" />
                </td>
            </tr>
	        <%} %>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "Password"))
          { %>
            <tr id="encrypedPassword">
                <td>
                    <%= Html.Term("Password")%>:
                </td>
                <td>
                    (<%= Html.Term("Encrypted")%>) <a id="btnChangePassword" href="javascript:void(0);">
                        <%= Html.Term("ChangePassword", "Change Password")%></a>
                </td>
            </tr>
            <tr id="newPassword" style="display: none">
                <td>
                    <label for="password">
                        <%= Html.Term("Password", "Password")%>:</label>
                </td>
                <td>
                    <input type="hidden" name="userChangingPassword" id="userChangingPassword" value="false" />
                    <input type="password" id="txtPassword" value="<%--<%= accountToEdit.User.Password %>--%>"
                        name="<%= Html.Term("PasswordRequired", "Password is required") %>" />
                    <a id="btnCancelChangePassword" href="javascript:void(0);">
                        <%= Html.Term("CancelChangePassword", "Cancel Change Password")%></a>
                </td>
            </tr>
            <tr id="newPasswordConfirm" style="display: none">
                <td>
                    <label for="txtConfirmPassword">
                        <%= Html.Term("ConfirmPassword", "Confirm Password")%>:</label>
                </td>
                <td>
                    <input type="password" id="txtConfirmPassword" value="<%--<%= accountToEdit.User.Password %>--%>"
                        name="<%= Html.Term("PasswordRequired", "Password is required") %>" />
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "AccountStatus"))
          { %>
            <tr>
                <td style="width: 13.636em;">
                     <label for="editAccountStatusId">
                        <%= Html.Term("AccountStatus", "Account Status")%>:</label>
                </td>
                <td>
                    <%: Html.DropDownList("editAccountStatusId", SmallCollectionCache.Instance.AccountStatuses.ToSelectListItems(accountToEdit.AccountStatusID), htmlAttributes: new { disabled = "disabled" })%>
                </td>
            </tr>
        <%} %>
         <%if (NetSteps.Data.Entities.Account.checkField(campos, "SiteAccess"))
           { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="statusId">
                        <%= Html.Term("SiteAccess", "Site Access")%>:</label>
                </td>
                <td>
                    <%: Html.DropDownList("statusId", SmallCollectionCache.Instance.UserStatuses.ToSelectListItems(accountToEdit.User.UserStatusID))%>
                </td>
            </tr>
        <%} %>
        </tbody>
    </table>
     <%if (NetSteps.Data.Entities.Account.checkField(campos, "Generaterandompassword"))
     { %>
        <div id="passwordGeneration">
            <a href="javascript:void(0);" id="btnGeneratePassword">
                <%= Html.Term("GeneratePassword", "Generate random password")%></a>
        </div>
    <%} %>
    
    <h3>
        <%= Html.Term("HostedMailAccount", "Hosted Mail Account") %></h3>
    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "Email1"))
              { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= Html.Term("Email", "Email")%>:</label>
                </td>
                <td>
                    <input type="text" id="txtHostedMailAccount" value="<%= mailAccountEmailAddress %>" 
                        style="width: 8.333em;" />@<%= domain%>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>


    <h3>
        <%= Html.Term("PersonalInfo", "Personal Info") %></h3>
    <form>
    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "Name"))
              { %>
            <tr>
                <td style="width: 13.636em;">
                    <%= Html.Term("Name")%>:
                </td>
                <td>
                    <input type="text" id="txtFirstName" value="<%= accountToEdit.FirstName %>" class="required"
                        name="<%= Html.Term("FirstNameRequired", "First Name is required") %>" style="margin-right: .909em;" />
                    <%if (NetSteps.Data.Entities.Account.checkField(campos, "MiddleName"))
                      { %>
                        <input type="text" id="txtMiddleName" value="<%= accountToEdit.MiddleName %>" style="margin-right: .909em;" />
                    <%} %>
                    <input type="text" id="txtLastName" value="<%= accountToEdit.LastName %>" class="required"
                        name="<%= Html.Term("LastNameRequired", "Last Name is required") %>" />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "Email2"))
              { %>
            <tr>
                <td>
                    <label for="txtEmail">
                        <%= Html.Term("Email", "Email")%>:</label>
                </td>
                <td>
                	<%
                  // Retail Customers and Prospects can be imported without an email address.
                  // Allow saving without an email addressfor them if they have none entered yet.
                  var emailRequiredClass = (accountToEdit.AccountTypeID == (short)Constants.AccountType.RetailCustomer
                                              && string.IsNullOrEmpty(accountToEdit.EmailAddress))
                                            || accountToEdit.AccountTypeID == (short)Constants.AccountType.Prospect

                                              ? ""
                                              : "required"; %>
                    <input type="text" id="txtEmail" value="<%= accountToEdit.EmailAddress %>" class="<%= emailRequiredClass %>"
                        name="<%= Html.Term("EmailRequired", "Email is required") %>" style="width: 20.833em;" />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "CPF"))
              {%>
            <tr>
                <td class="FLabel">
                    <span class="requiredMarker">*</span>
                    <%= Html.Term("CPF")%>:
                    <input type="hidden" id="hdnAccountSuppliedID0" value="<%= additional.AccountSuppliedID_CPF %>" />
                </td>
                <td id="cpf">
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "PIS"))
              { %>
            <tr>
                <td class="FLabel">
                    <span class="requiredMarker"></span>
                    <%= Html.Term("PIS")%>:
                    <input type="hidden" id="hdnAccountSuppliedID1" value="<%= additional.AccountSuppliedID_PIS %>" />
                </td>
                <td id="pis">
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "RG"))
              { %>
            <tr>
                <td class="FLabel">
                    <span class="requiredMarker">*</span>
                    <%= Html.Term("RG")%>:
                    <input type="hidden" id="hdnAccountSuppliedID2" value="<%= additional.AccountSuppliedID_RG %>" />
                </td>
                <td>
                    <input type="text" id="txtRG" value="<%= additional.RG %>" />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "OrgExp"))
              { %>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("OrgExp", "Órgao Exp")%>:
                </td>
                <td>
                    <input type="text" id="txtOrgExp" value="<%= additional.ExpeditionEntity %>" />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "IssueDate"))
              { %>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("IssueDate", "Issue Date")%>:
                </td>
                <td id="dateOfIssue">
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "TaxExempt"))
              { %>
            <tr>
            <td>
                    <label for="isTaxExempt">
                        <%= Html.Term("TaxExempt", "Tax Exempt")%>:</label>
                </td>
                <td>
                <input type="checkbox" id="isTaxExempt"  <%= accountToEdit.IsTaxExempt.ToBool() ? "checked=\"checked\"" : "" %> />
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "AccountPropertiesData"))
              { %>
            <tr>
                <td colspan="2">
                    <%
                  if (ViewData["AccountPropertiesData"] != null)
                  {
                      AccountPropertiesModel accountPropertiesData = (AccountPropertiesModel)ViewData["AccountPropertiesData"];
                      Html.RenderPartial("AccountPropertiesForm", accountPropertiesData);
                  }
                    %>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "IsTaxExemptVerified"))
              { %>
            <tr>
                <td>
                    <label for="isTaxExemptVerified">
                        <%= Html.Term("IsTaxExemptVerified", "Is TaxExempt Verified")%>:</label>
                </td>
                <td>
                    <select id="isTaxExemptVerified" <% if(!NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser.HasFunction("Accounts-Edit IsTaxExemptVerified")) { %>
                        disabled="disabled" <% } %>>
                        <option value="true" <%= accountToEdit.IsTaxExemptVerified ? "selected=\"selected\"" : "" %>>
                            <%= Html.Term("Yes") %></option>
                        <option value="false" <%= !accountToEdit.IsTaxExemptVerified ? "selected=\"selected\"" : "" %>>
                            <%= Html.Term("No") %></option>
                    </select>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "SSN"))
              {%>
                <%if (accountToEdit.AccountTypeID == (int)Constants.AccountType.Distributor)
                  {%>
                <tr>
                    <td id="ssnLabel">
                        <%=Html.Term("TaxNumber", "SSN")%>:
                    </td>
                    <td id="ssn">
                    </td>
                </tr>
                <%
                  }%>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "DOB"))
              {%>
            <tr>
                <td>
                    <%= Html.Term("DateOfBirth", "DOB")%>:
                </td>
                <td id="dob"></td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "GenderUSA"))
              {%>
            <tr>
                <td>
                    <label for="gender">
                        <%= Html.Term("Gender", "Gender")%>:</label>
                </td>
                <td>
                    <select id="gender">
                        <option value="" <%= accountToEdit.GenderID == null ? "selected=\"selected\"" : "" %>>
                            <%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.NotSet).GetTerm()%></option>
                        <option value="<%= (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Male %>"
                            <%= accountToEdit.GenderID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Male ? "selected=\"selected\"" : "" %>>
                            <%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.Male).GetTerm()%></option>
                        <option value="<%= (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Female %>"
                            <%= accountToEdit.GenderID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Female ? "selected=\"selected\"" : "" %>>
                            <%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.Female).GetTerm()%></option>
                    </select>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "GenderBRA"))
              {%>
            <tr>
                <td>
                    <label for="gender">
                        <%= Html.Term("Gender", "Gender")%>:</label>
                </td>
                <td>
                    <select id="gender">
                        <option value="" <%= accountToEdit.GenderID == null ? "selected=\"selected\"" : "" %>>
                            No Set<%--<%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.NotSet).GetTerm() %>--%></option>
                        <option value="<%= (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Male %>"
                            <%= accountToEdit.GenderID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Male ? "selected=\"selected\"" : "" %>>
                            Male<%--<%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.Male).GetTerm() %>--%></option>
                        <option value="<%= (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Female %>"
                            <%= accountToEdit.GenderID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.Gender.Female ? "selected=\"selected\"" : "" %>>
                            Female<%--<%= SmallCollectionCache.Instance.Genders.GetById((int)Constants.Gender.Female).GetTerm() %>--%></option>
                    </select>
                </td>
            </tr>
            <%} %>
            <%if (NetSteps.Data.Entities.Account.checkField(campos, "PhoneNumbersUSA"))
              {
                  int ind = 0;
                  %>
            <tr>
                <td>
                    <%= Html.Term("PhoneNumbers", "Phone Numbers")%>:
                </td>
                <td>
                    <div id="phones">
                        <% foreach (AccountPhone phone in accountToEdit.AccountPhones)
                           { %>
                            <p class="phoneContainer" data-id="<%= phone.AccountPhoneID %>">
                                <input type="hidden" class="phoneId" value="<%= phone.AccountPhoneID %>" />
                                <span class="phone">
                                    <%= phone.PhoneNumber%>
                                </span>
                                <select class="phoneType">
                                    <%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes)
                                      { %>
                                        <option value="<%= phoneType.PhoneTypeID %>" <%= phoneType.PhoneTypeID == phone.PhoneTypeID ? "selected=\"selected\"" : "" %>>
                                            <%= phoneType.GetTerm()%>
                                        </option>
                                    <%} %>
                                </select>
                                <%if (ind > 1)
                                  { %>
                                    <a class="DeletePhone DTL Remove" href="javascript:void(0);"></a>
                                <%}
                                  else
                                  {
                                      ind++;
                                  } %>
                             </p>
                        <%} %>
                    </div>
                    <a id="btnAddPhone" href="javascript:void(0);" class="DTL Add">
                        <%= Html.Term("AddaPhoneNumber", "Add a phone number")%></a>
                </td>
            </tr>
          <%} %>
          <%if (NetSteps.Data.Entities.Account.checkField(campos, "PhoneNumbersBRA"))
              { %>
            <tr>
                <td>
                    <%= Html.Term("PhoneNumbers", "Phone Numbers") %>:
                </td>
                <td>
                    <div id="phones" style="width: 400px;">
                        <%
                        
                            int AccountPhoneID_Main = 0; string PhoneNumber_Main = null;
                            if (accountToEdit.AccountPhones.Select(a => a.PhoneTypeID).Contains(Constants.PhoneType.Main.ToInt()))
                            {
                                AccountPhoneID_Main = accountToEdit.AccountPhones.Where(x => x.PhoneTypeID == Constants.PhoneType.Main.ToInt()).ToList().Select(a => a.AccountPhoneID).First();
                                PhoneNumber_Main = accountToEdit.AccountPhones.Where(x => x.PhoneTypeID == Constants.PhoneType.Main.ToInt()).ToList().Select(a => a.PhoneNumber).First();
                            }
                            int AccountPhoneID_Cell = 0; string PhoneNumber_Cell = null;
                            if (accountToEdit.AccountPhones.Select(a => a.PhoneTypeID).Contains(Constants.PhoneType.Cell.ToInt()))
                            {
                                AccountPhoneID_Cell = accountToEdit.AccountPhones.Where(x => x.PhoneTypeID == Constants.PhoneType.Cell.ToInt()).ToList().Select(a => a.AccountPhoneID).First();
                                PhoneNumber_Cell = accountToEdit.AccountPhones.Where(x => x.PhoneTypeID == Constants.PhoneType.Cell.ToInt()).ToList().Select(a => a.PhoneNumber).First();

                            }
                       
                        
                        %>
                        <p class="phoneContainer" data-id="<%= AccountPhoneID_Main %>">
                            <input type="hidden" class="phoneId" value="<%= AccountPhoneID_Main==null?0:AccountPhoneID_Main %>" />
                            <span class="phone">
                                <input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;" value="<%= (PhoneNumber_Main !=null ? PhoneNumber_Main.Substring(0, 3) : String.Empty) %>"> - 
                                <input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" value="<%= (PhoneNumber_Main !=null ? PhoneNumber_Main.Substring(3, PhoneNumber_Main.Length - 3) : String.Empty) %>">
                            </span>
                            <select class="phoneType">
                                <%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes.Where(x => x.PhoneTypeID == Constants.PhoneType.Main.ToInt()).ToList())
                                  { %>
                                <option value="<%= phoneType.PhoneTypeID %>">
                                    <%= phoneType.GetTerm() %>
                                </option>
                                <%} %>
                            </select>
                        </p>
                        <p class="phoneContainer" data-id="<%= AccountPhoneID_Cell %>">
                            <input type="hidden" class="phoneId" value="<%= AccountPhoneID_Cell==null?0:AccountPhoneID_Cell %>" />
                            <span class="phone">
                                <input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;" value="<%= PhoneNumber_Cell == null ? String.Empty : PhoneNumber_Cell.Substring(0, 3) %>"> - 
                                <input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" value="<%= PhoneNumber_Cell == null ? String.Empty : PhoneNumber_Cell.Substring(3, PhoneNumber_Cell.Length - 3) %>">
                            </span>
                            <select class="phoneType">
                                <%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes.Where(x => x.PhoneTypeID == Constants.PhoneType.Cell.ToInt()).ToList())
                                  { %>
                                <option value="<%= phoneType.PhoneTypeID %>">
                                    <%= phoneType.GetTerm() %>
                                </option>
                                <%} %>
                            </select>
                        </p>
                        <% foreach (AccountPhone phone in accountToEdit.AccountPhones.Where(x => x.PhoneTypeID != 1 && x.PhoneTypeID != 2).ToList())
                           { %>
                        <p class="phoneContainer" data-id="<%= phone.AccountPhoneID %>">
                            <input type="hidden" class="phoneId" value="<%= phone.AccountPhoneID %>" />
                            <span class="phone">
                                <input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;" value="<%= phone.PhoneNumber.Substring(0, 3) %>"> - 
                                <input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" value="<%= phone.PhoneNumber.Substring(3, phone.PhoneNumber.Length - 3) %>">
                            </span>
                            <select class="phoneType">
                                <%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes.Where(x => x.PhoneTypeID != 1 && x.PhoneTypeID != 2).ToList())
                                  { %>
                                <option value="<%= phoneType.PhoneTypeID %>" <%= phoneType.PhoneTypeID == phone.PhoneTypeID ? "selected=\"selected\"" : "" %>>
                                    <%= phoneType.GetTerm() %>
                                </option>
                                <%} %>
                            </select>
                            <a class="DeletePhone DTL Remove" href="javascript:void(0);"></a>
                        </p>
                        <%} %>
                    </div>
                    <a id="btnAddPhone" href="javascript:void(0);" class="DTL Add">
                        <%= Html.Term("AddaPhoneNumber", "Add a phone number") %></a>
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "Nationality"))
          { %>
             <tr>
                <td class="FLabel">
                    <input type="hidden" id="AccountPropertyID0" value="<%= additional.AccountPropertyID_Nationality %>" />
                    <input type="hidden" id="TypeID0" value="<%= additional.AccountPropertyTypeID_Nationality %>" />
                    <%= Html.Term("Nationality", "Nationality")%>:
                </td>
                <td>
                    <%= @Html.DropDownNationality(selectedNationalityID: IDNationality, htmlAttributes: new { id = "valor0" })%>
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "MaritalStatus"))
          { %>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("MaritalStatus", "Marital Status")%>:
                    <input type="hidden" id="AccountPropertyID1" value="<%= additional.AccountPropertyID_MaritalStatus %>" />
                    <input type="hidden" id="TypeID1" value="<%= additional.AccountPropertyTypeID_MaritalStatus %>" />
                </td>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="width: 20%">
                                <%= Html.DropDownMaritalStatus(selectedMaritalStatusID: IDMaritalStatus, htmlAttributes: new { id = "valor1" })%>
                            </td>
                            <td style="width: 20%; text-align: right">
                                <%= Html.Term("SpouseName", "Spouse Name")%>:
                                <input type="hidden" id="AccountPropertyID2" value="<%= additional.AccountPropertyID_SpouseFirstName %>" />
                                <input type="hidden" id="TypeID2" value="<%= additional.AccountPropertyTypeID_SpouseFirstName %>" />
                            </td>
                            <td style="width: 20%; text-align: left">
                                <input type="text" id="valor2" value="<%= additional.SpouseAllName %>" />
                            </td>
                            <td style="width: 20%; text-align: right">
                                <%= Html.Term("Sons", "Sons")%>:
                                <input type="hidden" id="AccountPropertyID3" value="<%= additional.AccountPropertyID_SonsNumber %>" />
                                <input type="hidden" id="TypeID3" value="<%= additional.AccountPropertyTypeID_SonsNumber %>" />
                            </td>
                            <td style="width: 20%; text-align: left">
                                <input type="text" id="valor3" maxlength="2" value="<%= additional.SonsNumber %>" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "Occupation"))
          {%>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Occupation", "Occupation")%>:
                    <input type="hidden" id="AccountPropertyID4" value="<%= additional.AccountPropertyID_Occupation %>" />
                    <input type="hidden" id="TypeID4" value="<%= additional.AccountPropertyTypeID_Occupation %>" />
                </td>
                <td>
                    <%= Html.DropDownOccupation(selectedOccupationID: IDOccupation, htmlAttributes: new { id = "valor4" })%>
                </td>
            </tr>
        <%} %>
         <%if (NetSteps.Data.Entities.Account.checkField(campos, "Authorizationsharedatawithnetwork"))
              { %>
            <tr>
                <td colspan="2">
                    <input type="checkbox" class="toggleCustomUrl01" id="valor5" <%= isCheckedNetwork %> /><%= Html.Term("AutorizationNetWork", "Autorización compartir datos con la red")%>
                    <input type="hidden" id="AccountPropertyID5" value="<%= additional.AccountPropertyID_AcceptShareWithNetwork %>" />
                    <input type="hidden" id="TypeID5" value="<%= additional.AccountPropertyTypeID_AcceptShareWithNetwork %>" />
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "Authorizationsende_mails"))
          { %>
            <tr>
                <td colspan="2">
                    <input type="checkbox" class="toggleCustomUrl01" id="valor6" <%= isCheckedEmailFrom %> /><%= Html.Term("AutorizationEmail", "Autorización enviar e-mails")%>
                    <input type="hidden" id="AccountPropertyID6" value="<%= additional.AccountPropertyID_AcceptEmailFrom %>" />
                    <input type="hidden" id="TypeID6" value="<%= additional.AccountPropertyTypeID_AcceptEmailFrom %>" />
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "Authorizationsharedatawithlocator"))
              { %>
            <tr>
                <td colspan="2">
                    <input type="checkbox" class="toggleCustomUrl03" id="valor7" <%= isCheckedLocator %> /><%= Html.Term("AutorizationLocalizator", "Autorización compartir datos con localizador")%>
                    <input type="hidden" id="AccountPropertyID7" value="<%= additional.AccountPropertyID_AcceptShareForLocator %>" />
                    <input type="hidden" id="TypeID7" value="<%= additional.AccountPropertyTypeID_AcceptShareForLocator %>" />
                </td>
            </tr>
        <%} %>
        </tbody>
    </table>

    <%if (NetSteps.Data.Entities.Account.checkField(campos, "AddressUSA"))
      { %>
    <div id='addresses'>
        <h3><%= Html.Term("MainAddress", "Main Address")%></h3>
        <%NetSteps.Web.Mvc.Controls.Models.AddressModel model = new NetSteps.Web.Mvc.Controls.Models.AddressModel()
        {
            Address = addressOfRecord,
            LanguageID = CoreContext.CurrentLanguageID,
            ShowCountrySelect = true,
            ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
            ExcludeFields = new List<string>() { "ProfileName" },
            Prefix = "mainAddress",
            // Retail Customers and Prospects can sign up without an address.
            // Allow saving without an address for them if they have no address yet.
            HonorRequired = (accountToEdit.AccountTypeID != (short)Constants.AccountType.RetailCustomer
                && accountToEdit.AccountTypeID != (short)Constants.AccountType.Prospect)
                || !string.IsNullOrEmpty(addressOfRecord.Address1)
                || !string.IsNullOrEmpty(addressOfRecord.PostalCode)
                || !string.IsNullOrEmpty(addressOfRecord.City)
        };
          Html.RenderPartial("Address", model); %>
    </div>
    <%} %>
    <%if (NetSteps.Data.Entities.Account.checkField(campos, "AddressBRA"))
      { %>
    <div id='Div1'>
        <h3>
            <%= Html.Term("MainAddress", "Main Address")%></h3>
        <%NetSteps.Web.Mvc.Controls.Models.AddressModel model = new NetSteps.Web.Mvc.Controls.Models.AddressModel()
        {
            Address = addressOfRecord,
            LanguageID = CoreContext.CurrentLanguageID,
            ShowCountrySelect = true,
            ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
            ExcludeFields = new List<string>() { "ProfileName" },
            Prefix = "mainAddress",
            // Retail Customers and Prospects can sign up without an address.
            // Allow saving without an address for them if they have no address yet.
            HonorRequired = (accountToEdit.AccountTypeID != (short)Constants.AccountType.RetailCustomer
                && accountToEdit.AccountTypeID != (short)Constants.AccountType.Prospect)
                || !string.IsNullOrEmpty(addressOfRecord.Address1)
                || !string.IsNullOrEmpty(addressOfRecord.PostalCode)
                || !string.IsNullOrEmpty(addressOfRecord.City)
        };
          if (model.Street == null) model.Street = addressOfRecord.Street;
          Html.RenderPartial("Address", model); %>
    </div>
    <%} %>
    <%if (NetSteps.Data.Entities.Account.checkField(campos, "AdditionalInfo"))
      { %>
    <% Html.RenderPartial("AdditionalInfo", accountToEdit); %>
    <%} %>
    <br />
    <%if (NetSteps.Data.Entities.Account.checkField(campos, "ComplementaryInfo"))
      { %>
    <h3>
        <%= Html.Term("ComplementaryInfo", "Complementary Info")%></h3>
    <%} %>
    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "VRF"))
          { %>
         <% string valRC = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "VRF");
            if (valRC == "S")
            {%>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= Html.Term("ReferenceToCredit", "Reference To Credit")%>:</label>
                    <input type="hidden" id="AccountReferencesID" value="<%= additional.AccountReferencID %>" />
                </td>
                <td>
                    <input type="text" id="txtReferenceName" value="<%= additional.ReferenceName %>"
                        style="width: 8.333em;" />
                </td>
            </tr>
            <%}%>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "SchoolingLevel"))
          { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= Html.Term("SchoolingLevel", "Scolarship")%>:</label>
                    <input type="hidden" id="AccountPropertyID8" value="<%= additional.AccountPropertyID_SchoolingLevel %>" />
                    <input type="hidden" id="TypeID8" value="<%= additional.AccountPropertyTypeID_SchoolingLevel %>" />
                </td>
                <td>
                    <%= @Html.DropDownSchoolineLevel(selectedSchoolineLevelID: IDSchoolingLevel, htmlAttributes: new { id = "valor8" })%>
                </td>
            </tr>
        <%} %>
            <%--// 01--%>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkBlong"))
          { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link Blong")%>:</label>
                    <input type="hidden" id="SocialNetworkID_0" value="<%= additional.SocialNetworkID_Blog %>" />
                    <input type="hidden" id="AccountSocialNetworkID_0" value="<%= additional.AccountSocialNetworkID_Blog %>" />
                </td>
                <td>
                    <input type="text" id="valor_0" value="<%= additional.LinkBlog %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
            <%--// 02--%>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkFacebook"))
          { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link Facebook")%>:</label>
                    <input type="hidden" id="SocialNetworkID_1" value="<%= additional.SocialNetworkID_Facebook %>" />
                    <input type="hidden" id="AccountSocialNetworkID_1" value="<%= additional.AccountSocialNetworkID_Facebook %>" />
                </td>
                <td>
                    <input type="text" id="valor_1" value="<%= additional.LinkFacebook %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkMSN"))
          { %>
            <%--// 03--%>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link MSN")%>:</label>
                    <input type="hidden" id="SocialNetworkID_2" value="<%= additional.SocialNetworkID_MSN %>" />
                    <input type="hidden" id="AccountSocialNetworkID_2" value="<%= additional.AccountSocialNetworkID_MSN %>" />
                </td>
                <td>
                    <input type="text" id="valor_2" value="<%= additional.LinkEmail %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkOrkut"))
          { %>
            <%--// 04--%>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link Orkut")%>:</label>
                    <input type="hidden" id="SocialNetworkID_3" value="<%= additional.SocialNetworkID_Orkut %>" />
                    <input type="hidden" id="AccountSocialNetworkID_3" value="<%= additional.AccountSocialNetworkID_Orkut %>" />
                </td>
                <td>
                    <input type="text" id="valor_3" value="<%= additional.LinkOrkut %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
            <%--// 05--%>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkTwitter"))
              { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link Twitter")%>:</label>
                    <input type="hidden" id="SocialNetworkID_4" value="<%= additional.SocialNetworkID_Twitter %>" />
                    <input type="hidden" id="AccountSocialNetworkID_4" value="<%= additional.AccountSocialNetworkID_Twitter %>" />
                </td>
                <td>
                    <input type="text" id="valor_4" value="<%= additional.LinkTwitter %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
            <%--// 06--%>
        <%if (NetSteps.Data.Entities.Account.checkField(campos, "LinkLinkedID"))
          { %>
            <tr>
                <td style="width: 13.636em;">
                    <label for="txtHostedMailAccount">
                        <%= @Html.Term("Link LinkedID")%>:</label>
                    <input type="hidden" id="SocialNetworkID_5" value="<%= additional.SocialNetworkID_LinkedIN %>" />
                    <input type="hidden" id="AccountSocialNetworkID_5" value="<%= additional.AccountSocialNetworkID_LinkedIN %>" />
                </td>
                <td>
                    <input type="text" id="valor_5" value="<%= additional.LinkedIN %>" style="width: 25em;" />
                </td>
            </tr>
        <%} %>
         <%if (NetSteps.Data.Entities.Account.checkField(campos, "VCI"))
           { %>
            <tr>
                <td colspan="2">
                    <% string val = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "VCI");
                       if (val == "S")
                       {%>
                    <table>
                        <tr>
                            <td style="width: 13.636em;">
                                <label for="txtHostedMailAccount">
                                    <%= Html.Term("Computer", "Computer")%>:</label>
                                <input type="hidden" id="AccountPropertyID9" value="<%= additional.AccountPropertyID_HasComputer %>" />
                                <input type="hidden" id="TypeID9" value="<%= additional.AccountPropertyTypeID_HasComputer %>" />
                            </td>
                            <td>
                                <select id="valor9">
                                    <option value="1" <%= additional.HasComputer ? "selected=\"selected\"" : "" %>>
                                        <%= Html.Term("Yes")%></option>
                                    <option value="0" <%= !additional.HasComputer ? "selected=\"selected\"" : "" %>>
                                        <%= Html.Term("No")%></option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13.636em;">
                                <label for="txtHostedMailAccount">
                                    <%= Html.Term("InternetUse", "Internet Use")%>:</label>
                                <input type="hidden" id="AccountPropertyID10" value="<%= additional.AccountPropertyID_InternetUseFrecuency %>" />
                                <input type="hidden" id="TypeID10" value="<%= additional.AccountPropertyTypeID_InternetUseFrecuency %>" />
                            </td>
                            <td>
                                <%= @Html.DropDownInternetUse(selectedInternetUseID: IDInternetUseFrecuency, htmlAttributes: new { id = "valor10" })%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13.636em;">
                                <label for="txtHostedMailAccount">
                                    <%= Html.Term("RelationShip", "Relation Ship")%>:</label>
                            </td>
                            <td>
                                <%= @Html.DropDownRelationShip(selectedRelationShipID: IDRelationShip, htmlAttributes: new { id = "RelationShip" })%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13.636em;">
                                <label for="txtHostedMailAccount">
                                    <%= Html.Term("PhoneNumber", "Phone Number")%>:</label>
                            </td>
                            <td>
                                <div class="data" id="phone">
                                </div>
                            </td>
                        </tr>
                    </table>
                    
                    <%}%>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
    <br />
    <table class="FormTable">
        <tr>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                <p>
                    <a id="btnSaveAccount" href="javascript:void(0);" class="Button BigBlue">
                        <%= Html.Term("Save Information") %></a> <a id="btnCancel" href="<%= ResolveUrl("~/Accounts/Overview") %>"
                            class="Button"><%= Html.Term("Cancel") %></a></p>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        <%if(cultureInfo.Equals("BRA")){ %>
            $(function ()
            {
                $('#dateOfIssue').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtIssueDay', length: 2, size: 2 }, { id: 'txtIssueMonth', length: 2, size: 2 }, { id: 'txtIssueYear', length: 4, size: 4}] }).inputsByFormat('setValue', '<%= (additional.IssueDate == null) ? "" : additional.IssueDate.ToString("dd/MM/yyyy").Replace("/", "") %>');
                $('#dob').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] }).inputsByFormat('setValue', '<%= (accountToEdit.Birthday == null) ? "" : accountToEdit.Birthday.ToDateTime().ToString("dd/MM/yyyy").Replace("/", "") %>');
            
                $('#txtDOBMonth').watermark('mm');
                $('#txtDOBDay').watermark('dd');
                $('#txtDOBYear').watermark('yyyy');

                $('#txtIssueMonth').watermark('mm');
                $('#txtIssueDay').watermark('dd');
                $('#txtIssueYear').watermark('yyyy');

                addCpfInputs();
                addPisInputs();

                function addCpfInputs()
                {
                    $('#cpf').inputsByFormat('destroy');
                    $('#cpf').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtCPFPart1', length: 9, size: 9 }, { id: 'txtCPFPart2', length: 2, size: 2}] }).inputsByFormat('setValue', '<%= additional.CPF %>');
                }

                function addPisInputs()
                {
                    $('#pis').inputsByFormat('destroy');
                    $('#pis').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtPISPart1', length: 9, size: 9 }, { id: 'txtPISPart2', length: 2, size: 2}] }).inputsByFormat('setValue', '<%= additional.PIS %>');
                }
            });
        <%} %>
    </script>
</asp:Content>
