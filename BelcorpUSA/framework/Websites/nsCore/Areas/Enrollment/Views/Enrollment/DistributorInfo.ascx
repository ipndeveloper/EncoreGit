<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% 
    
    var EnvironmentCountry = Convert.ToInt32(ConfigurationManager.AppSettings["EnvironmentCountry"]);// codigo del pais dobde esta desplegado
    var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext;
    bool isEntity = (bool)ViewData["IsEntity"];
    bool displayUserNameField = (bool)ViewData["DisplayUsernameField"];
    var usCountry = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.CountryCode.ToUpper() == "US");
    var accountPropertyType = NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.AccountPropertyTypes;
    var accountPropertyExemptReason = accountPropertyType.FirstOrDefault(x => x.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME);
    var ExemptReasonAccPropTypeID = accountPropertyExemptReason != null ? accountPropertyExemptReason.AccountPropertyTypeID : 0;
%>
<style>
    #text1, #text2
    {
        color: #00f;
    }
</style>
<script type="text/javascript">


    /**/

    /**/
    //Ajax Validations: Global variables

    //Enroller and Placement
    var sponsorName = '';
    var validationSponsor = true;
    var placementName = '';
    var validationPlacement = true;

    //User Name
    var validationUserName = false;
    var userName = '';

    //Documents
    var validationReqCPF = '<%= Html.JavascriptTerm("CPFinvalid", "CPF is Invalid.") %>';
    var validationCPF = validationReqCPF;
    var validationCPFCoApplicant = validationReqCPF;

    var validationReqPIS = ''; '<%= Html.JavascriptTerm("PISinvalid", "PIS is Invalid.") %>';
    var validationPIS = '';
    var validationPISCoApplicant = '';

    var validationReqRG = '<%= Html.JavascriptTerm("RGInvalid", "RG is Invalid.") %>';
    var validationRG = validationReqRG;
    var validationRGCoApplicant = validationReqRG;
    
    //Email
    var validationEmail = false;
    var emailText = '';
    $(function () {
    


    var ViewBagDPA ='<%= ViewBag.DPA %>' ==null ? '' : '<%= ViewBag.DPA %>';
      if (ViewBagDPA!='') {
        $('#divAccount2').hide();
        $('#percentToDepositAccount1').val('100').closest('tr').hide();
    }

    // COMENTADO POR HUNDRED 26042017 PARA GENERALIZAR FUNCIONALIDAD
//    var DPAshow = JSON.parse('<%= ViewBag.DPA %>');

//    if (!DPAshow) {
//        $('#divAccount2').hide();
//        $('#percentToDepositAccount1').val('100').closest('tr').hide();
//    }
// FIN 26042017
    $('.BankInput').numeric().keyup(function () {
        var select = $(this).closest('td').find('select');
        var selectVal = select.find('option.' + $(this).val()).val();
        select.val(selectVal);
    });

    $('#ddlBankNameAccount1, #ddlBankNameAccount2').change(function () {
        $(this).closest('td').find('.BankInput').val($( '#' + this.id + ' option:selected').attr('class'));
    });

    $('#txtAgencyAccount1, #txtAgencyAccount2').attr('maxlength','4');
    $('#txtAgencyAccount1, #txtAgencyAccount2, #txtAccountNumberAccount1, #txtAccountNumberAccount2').keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
             // Allow: Ctrl+A, Command+A
            (e.keyCode == 65 && ( e.ctrlKey === true || e.metaKey === true ) ) || 
             // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
                 // let it happen, don't do anything
                 return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $('#firstName, #middleName, #lastName').keyup(function (e) {
        $('#txtNameAccount1').val($('#firstName').val() + ' ' + $('#middleName').val() + ' ' + $('#lastName').val());
    });

                $('#RG').bind('keypress', function (e) {
                       return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) ? false : true;
                });
                   $('#RGCoApplicant').bind('keypress', function (e) {
                       return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) ? false : true;
                });
                   $('#txtSons').bind('keypress', function (e) {
                       return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) ? false : true;
                });
                
                /*
                $("#sponsor").blur(function () {
                    var sponsorId = $("#sponsorId").val();
                    if (!sponsorId ||$("#sponsor").val() ) {
                        $("#sponsor").showError('<%= Html.JavascriptTerm("EnrrollerInvalid", "Enrroller is Required") %>');
                    } else {
                        $("#sponsor").clearError();
                    }
                });
 
                $("#placement").blur(function () {
                    var placementId = $("#placementId").val();
                    if (!placementId || $("#placement").val()) {
                        $("#placement").showError('<%= Html.JavascriptTerm("placementInvalid", "Placement is Required") %>');
                    } else {
                        $("#placement").clearError();
                    }
                 });

                 */
         

        //@AINI
        $("#directDeposit, #paymentOrder").change(function () {
            if ($("#paymentOrder").is(":checked")) {
                $("#divDirectDeposit").hide();
                 $("#chkEnabledAccount1").attr("checked",false);
                $("#chkEnabledAccount1").change()
                 $("#chkEnabledAccount2").attr("checked",false);
                $("#chkEnabledAccount2").change()
            }
            if ($("#directDeposit").is(":checked")) {
                $("#divDirectDeposit").show();
                $("#chkEnabledAccount1").attr("checked",true);
                $("#chkEnabledAccount1").change()

                $("#chkEnabledAccount2").attr("checked",false);
                $("#chkEnabledAccount2").change()
                

            }
            $("#divDirectDeposit").clearError();
        });

        /* 
           KLC - CSTI
           ISSUE: EB-158
        */
        $('#ddlBankNameAccount2').on('change', function() {
          var bankID=this.value;
          var BankName = $("#ddlBankNameAccount2 option:selected").html();
          
          if(bankID==0){
             $('#txtBankNameAccount2').val("");           
             $('#txtBankNameAccount2Id').val("");
          }else{
             $('#txtBankNameAccount2').val(BankName);           
             $('#txtBankNameAccount2Id').val(bankID);
          }         
        });

        $('#ddlBankNameAccount1').on('change', function() {
          var bankID=this.value;
          var BankName = $("#ddlBankNameAccount1 option:selected").html();
          
          if(bankID==0){
             $('#txtBankNameAccount1').val("");           
             $('#txtBankNameAccount1Id').val("");
          }else{
             $('#txtBankNameAccount1').val(BankName);           
             $('#txtBankNameAccount1Id').val(bankID);
          }         
        });
        /// FIN

        $("#chkEnabledAccount1, #chkEnabledAccount2").change(function () {
            var enableAccount1 = $("#chkEnabledAccount1").is(':checked');
            var enableAccount2 = $("#chkEnabledAccount2").is(':checked');

            if (!enableAccount1)
                clearRequieredAccounts(1);

            if (!enableAccount2)
                clearRequieredAccounts(2);
            
            if (enableAccount1 && enableAccount2){
                $("#percentToDepositAccount1").val('');
                $("#percentToDepositAccount2").val('');
            }
            else if (enableAccount1 && !enableAccount2){
                $("#percentToDepositAccount1").val(100);
                $("#percentToDepositAccount2").val('');
            }
            else if (!enableAccount1 && enableAccount2){
                $("#percentToDepositAccount1").val('');
                $("#percentToDepositAccount2").val(100);
            }
               
            $.each($("#tableAccount1")[0].children[0].children, function (index, value) {
                if (index > 0) {
                    value.children[1].children[0].disabled = !enableAccount1;
                }
            });

            $.each($("#tableAccount2")[0].children[0].children, function (index, value) {
                if (index > 0) {
                    value.children[1].children[0].disabled = !enableAccount2;
                }
            });
        });

        //AFIN

        window.onbeforeunloadmessage = '<%= Html.Term("TheEnrollmentProcessWillBeTerminatedIfYouNavigateAway", "The enrollment process will be terminated if you navigate away.") %>';
        var currentPhone = 0;
        $(window).load(function () {
            $('#btnAddPhone').click();

            //se agregue de manera automatica el telefono de tipo cell[2]
            $(".phoneType").val(2);//cell
            $("#btnAddPhone").click()
            $("#phones .phoneContainer:eq(0) .phoneType").val(1)
            $("#phones .phoneContainer:eq(0) .phoneType").change();
            $("#phones .phoneContainer:eq(1) .phoneType").val(2)
            $("#phones .phoneContainer:eq(1) .phoneType").change();


            $('#btnAddPhoneCoApplicant').click();
            $("#phonesCoApplicant .phoneContainer:eq(0) .phoneType").change();

            $('#coApplicant :input').not('#chkCoApplicant').prop('disabled', true);
            $('#coApplicant a').hide();

            $('#phones .phoneContainer, #phonesCoApplicant .phoneContainer').filter(function () {
            
                return !!$('.phone', this).phone('getPhone');
            }).each(function (i) {
				$('.phoneInput1').prop('maxLength', 3);
                $('.phoneInput2').prop('maxLength', 9);
            });
        });

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
            $('#phonesCoApplicant .phoneContainer').each(function () {
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
                    phone.phone({ areaCodeId: 'txtAreaCodeCoApplicant' + guid, firstThreeId: 'txtFirstThreeCoApplicant' + guid, lastFourId: 'txtLastFourCoApplicant' + guid }).phone('setPhone', number);
                }
            });
            $('#phonesComplementary .phoneContainer').each(function () {
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
                    phone.phone({ areaCodeId: 'txtAreaCodeComplementary' + guid, firstThreeId: 'txtFirstThreeComplementary' + guid, lastFourId: 'txtLastFourComplementary' + guid }).phone('setPhone', number);
                }
            });
        }




        $('#expMonth,#expYear').change(function () {
            var today = new Date();
            var lastDayInMonth = new Date(today.getFullYear(), today.getMonth(), 0);
            if (new Date().setFullYear($('#expYear').val(), $('#expMonth').val() - 1, lastDayInMonth.getDate()) > today) {
                $('#expMonth,#expYear').clearError();
            }
        });

        $('#taxExempt').click(function () {
            if ($('#taxExempt').prop('checked')) {
                $("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").show('fast');
            }
            else {
                $("#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>").hide('fast');
                $('#PropertyType<%= ExemptReasonAccPropTypeID %>').val('');
            }
        });

        if ($('#taxExempt').prop('checked')) {
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

        function addSsnInputs() {
        
            if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString()  %>') {
                $('#ssn').inputsByFormat('destroy');
                $('#ssn').inputsByFormat({ format: '{0}', validateNumbers: false, attributes: [{ id: 'txtSSNPart1', length: 16, size: 16}] });
            }
            else {
                $('#ssn').inputsByFormat('destroy');
                if (parseBool('<%= isEntity %>')) {
                    $('#ssn').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtEINPart1', length: 2, size: 2 }, { id: 'txtEINPart2', length: 7, size: 7}] });
                } else {
                    $('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] });
                }
            }
        }

//        $("#sponsor").keyup(function () {
//            if (sponsorName != '' && $(this).val() != sponsorName) {
//                validationSponsor = false;
//                sponsorName = '';
//                $(this).val('');
//                $('#sponsorId').val('');
//            }
//        });

        $("#sponsor").focus(function(){
            validationSponsor = true;
            sponsorName = '';
            $(this).val('');
            $('#sponsorId').val('');
        });

        $('#sponsor').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
		.jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributors") %>', { onSelect: function (item) {
            $('#sponsor').clearError();
		    sponsorName = item.text;
            $('#sponsorId').val(item.id);
            validationSponsor = true;

		    if ($('#placementId').val() == '') {
                PlacementValidation(item.id, item.text, false)
            }
            else{
                validationPlacement = true;
            }
		    
		}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
		});



         // Bank name 
         /* $('#txtBankNameAccount1').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
		.jsonSuggest('<%= ResolveUrl("~/BillingShippingProfiles/BanksSearch") %>', { onSelect: function (item) {
		    $('#sponsor').clearError();
		        $('#txtBankNameAccount1Id').val(item.id);
		        $('#txtBankNameAccount1').val(item.text);

		    $('#txtBankNameAccountId').val(item.id);
		}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
		});*/
        //
        // Bank name 
       /* $('#txtBankNameAccount2').watermark('<%= Html.JavascriptTerm("BankSearch", "Look up Bank by ID or name") %>')
		.jsonSuggest('<%= ResolveUrl("~/BillingShippingProfiles/BanksSearch") %>', { onSelect: function (item) {
		    $('#sponsor').clearError();
		        $('#txtBankNameAccount2').val(item.text);
		    $('#txtBankNameAccount2Id').val(item.id);
		}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
		});*/
        //

        $("#placement").focus(function(){
            validationPlacement = true;
            placementName = '';
            $(this).val('');
            $('#placementId').val('');
             //$(this).clearError();
        });
        
        $('#placement').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')

        .jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributorsCEP") %>', { onSelect: function (item) {
           

            PlacementValidation(item.id, item.text, true);

        }, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: 300
        });


        $('#firstName').watermark('<%= Html.JavascriptTerm("FirstName", "First Name") %>');
        $('#middleName').watermark('<%= Html.JavascriptTerm("MiddleName", "Middle Name") %>');
        $('#lastName').watermark('<%= Html.JavascriptTerm("LastName", "Last Name") %>');

        $('#password').watermark('<%= Html.JavascriptTerm("Password") %>');
        $('#passwordConfirm').watermark('<%= Html.JavascriptTerm("ConfirmPassword", "Confirm password") %>');
        $('#manualPassword').hide();

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

        $('#generateUsername').click(function () {
            $(this).attr('checked') && $('#manualUsername').fadeOut('fast') || $('#manualUsername').fadeIn('fast');
            validationUserName = false;
            if (!$(this).attr('checked'))
                $('#username').val('').clearError().focus();
        });

        $('#generatePassword').click(function () {
            $(this).attr('checked') && $('#manualPassword').fadeOut('fast') || $('#manualPassword').fadeIn('fast');
            if (!$(this).attr('checked'))
                $('#password').val('').clearError().focus();
                $('#passwordConfirm').val('').clearError();
        });

        $('#btnAddPhone').click(function () {
            
            var HTMLselect = '';
            var HTMLdelete = '<a href="javascript:void(0);" class="DeletePhone DTL Remove"></a>';
            
            if ($('#phones .phoneContainer').length < 2){
                HTMLselect = '<select onchange="ValidarNumerosCaracteres(this)" class="phoneType" disabled="disabled"><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>';
                HTMLdelete = ''
            }
            else{
                HTMLselect = '<select onchange="ValidarNumerosCaracteres(this)" class="phoneType" ><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes.Where(x => x.PhoneTypeID != 1 && x.PhoneTypeID != 2)) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>';
            }

            
            var phoneControl = $('<span class="phone"></span>'),
					container = $('<p class="phoneContainer"></p>')
									.append(phoneControl)
									.append(HTMLselect)
									.append(HTMLdelete);
            $('#phones').append(container);

            var HTMLInputs = '<input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;"> - ' + 
                             '<input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" >';

            if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
                phoneControl.append(HTMLInputs);
            } else {
                phoneControl.phone({
                    areaCodeId: 'phone' + currentPhone + 'AreaCode',
                    firstThreeId: 'phone' + currentPhone + 'FirstThree',
                    lastFourId: 'phone' + currentPhone + 'LastFour'
                });
                ++currentPhone;
            }

            
        });
        //CoApplicant
        $('#btnAddPhoneCoApplicant').click(function () {
            var phoneControl = $('<span class="phone"></span>'),
					container = $('<p class="phoneContainer"></p>')
									.append(phoneControl)
									.append('<select onchange="ValidarNumerosCaracteres(this)" class="phoneType"><%foreach (PhoneType phoneType in SmallCollectionCache.Instance.PhoneTypes) { %><option value="<%= phoneType.PhoneTypeID %>"><%= phoneType.GetTerm() %></option><%} %></select>')
									.append('<a href="javascript:void(0);" class="DeletePhone DTL Remove"></a>');
            $('#phonesCoApplicant').append(container);
            
            var HTMLInputs = '<input maxlength="3" onkeypress="return isNumber(this,event)" type="text" class="phoneInput1" style="width: 50px;"> - ' + 
                             '<input maxlength="9" onkeypress="return isNumber(this,event)" type="text" class="phoneInput2" >';

            if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
                phoneControl.append(HTMLInputs);
            } else {
                phoneControl.phone({
                    areaCodeId: 'phone' + currentPhone + 'AreaCode',
                    firstThreeId: 'phone' + currentPhone + 'FirstThree',
                    lastFourId: 'phone' + currentPhone + 'LastFour'
                });
                ++currentPhone;
            }
        });

        //Delete CoApplicant Complementary
        $('#phonesComplementary .DeletePhone').live('click', function () {
            $(this).parent().fadeOut('fast', function () {
                $(this).remove();
            });
        });
        $('#phonesCoApplicant .DeletePhone').live('click', function () {
            $(this).parent().fadeOut('fast', function () {
                $(this).remove();
            });
        });
        $('#phones .DeletePhone').live('click', function () {
            $(this).parent().fadeOut('fast', function () {
                $(this).remove();
            });
        });

        $('#shippingAddressControl,#billingAddressControl').each(function () { $(this).parent().hide(); });

        $('#chkUseMainForShipping,#chkUseMainForBilling').click(function () {
            $(this).attr('checked') && $(this).closest('.FRow').next().fadeOut('fast') || $(this).closest('.FRow').next().fadeIn('fast');
        });

        //D02
        //document.getElementById('directDeposit').onchange = showFieldsAccounts;
        //document.getElementById('paymentOrder').onchange = disablefieldsAccounts;

        function showFieldsAccounts() {

            document.getElementById('chkEnabledAccount1').disabled = false;
            //document.getElementById('txtNameAccount1').disabled = false;
            document.getElementById('txtAccountNumberAccount1').disabled = false;
            document.getElementById('txtBankNameAccount1').disabled = false;
            document.getElementById('accountTypeAccount1').disabled = false;
            document.getElementById('percentToDepositAccount1').disabled = false;
            document.getElementById('chkEnabledAccount2').disabled = false;
            document.getElementById('txtNameAccount2').disabled = false;
            document.getElementById('txtAccountNumberAccount2').disabled = false;
            document.getElementById('txtBankNameAccount2').disabled = false;
            document.getElementById('accountTypeAccount2').disabled = false;
            document.getElementById('percentToDepositAccount2').disabled = false;
        };
        function disablefieldsAccounts() {
            document.getElementById('chkEnabledAccount1').checked = false;
            document.getElementById('chkEnabledAccount2').checked = false;
            document.getElementById('chkEnabledAccount1').disabled = true;
            //document.getElementById('txtNameAccount1').disabled = true;
            document.getElementById('txtAccountNumberAccount1').disabled = true;
            document.getElementById('txtBankNameAccount1').disabled = true;
            document.getElementById('accountTypeAccount1').disabled = true;
            document.getElementById('percentToDepositAccount1').disabled = true;
            document.getElementById('chkEnabledAccount2').disabled = true;
            document.getElementById('txtNameAccount2').disabled = true;
            document.getElementById('txtAccountNumberAccount2').disabled = true;
            document.getElementById('txtBankNameAccount2').disabled = true;
            document.getElementById('accountTypeAccount2').disabled = true;
            document.getElementById('percentToDepositAccount2').disabled = true;            
        };



        

              var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';

            var format ='';
                if (cultureInfo === 'en-US')
                {
                        $('#issueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
                        [
                            { id: 'txtIssueDateMonth', length: 2, size: 2 },
                            { id: 'txtIssueDateDay', length: 2, size: 2 },
                            { id: 'txtIssueDateYear', length: 4, size: 4}
                        ] 
                        });

                        $('#dateOfBirth').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
                        [
                            { id: 'txtDOBMonth', length: 2, size: 2 },
                            { id: 'txtDOBDay', length: 2, size: 2 },
                            { id: 'txtDOBYear', length: 4, size: 4}
                        ]
                        });


                        $('#dateOfBirthCoApplicant').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
                        [
                            { id: 'txtDOBMonthCoApplicant', length: 2, size: 2 },
                            { id: 'txtDOBDayCoApplicant', length: 2, size: 2 },
                            { id: 'txtDOBYearCoApplicant', length: 4, size: 4}
                        ]
                        });


                        $('#issueDateCoApplicant').inputsByFormat(
                        { format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
                        [
                            { id: 'txtIssueDateMonthCoApplicant', length: 2, size: 2 },
                            { id: 'txtIssueDateDayCoApplicant', length: 2, size: 2 }, 
                            { id: 'txtIssueDateYearCoApplicant', length: 4, size: 4}
                        ]
                        });


                        $('#txtDOBMonth').watermark('mm'); //.attr('placeholder', 'mm');
                        $('#txtDOBDay').watermark('dd'); //.attr('placeholder', 'dd');
                        $('#txtDOBYear').watermark('yyyy'); //.attr('placeholder', 'yyyy');


                    $('#txtDOBMonthCoApplicant').watermark('mm'); //.attr('placeholder', 'mm');
                    $('#txtDOBDayCoApplicant').watermark('dd'); //.attr('placeholder', 'dd');
                    $('#txtDOBYearCoApplicant').watermark('yyyy'); //.attr('placeholder', 'yyyy');

                    //cambiar de posicion las cajas de fechas  dd/mm/yyyy
                    var txtDOBMonthCoApplicant = $("#txtDOBMonthCoApplicant").clone(true);
                    var txtDOBDayCoApplicant = $("#txtDOBDayCoApplicant").clone(true);
                    var txtDOBYearCoApplicant = $("#txtDOBYearCoApplicant").clone(true);

                    $("#dateOfBirthCoApplicant").text("")
                    $("#dateOfBirthCoApplicant").append(txtDOBMonthCoApplicant);
                    $("#dateOfBirthCoApplicant").append(txtDOBDayCoApplicant);
                    $("#dateOfBirthCoApplicant").append(txtDOBYearCoApplicant);

                    $("<span>/</span>").insertAfter($("#txtDOBDayCoApplicant"));
                    $("<span>/</span>").insertAfter($("#txtDOBMonthCoApplicant"));



                $("#txtIssueDateMonth").watermark('mm');
                $("#txtIssueDateDay").watermark('dd');
                $("#txtIssueDateYear").watermark('yyyy');

                var txtIssueDateMonth = $("#txtIssueDateMonth").clone(true);
                var txtIssueDateDay = $("#txtIssueDateDay").clone(true);
                var txtIssueDateYear = $("#txtIssueDateYear").clone(true);
                $("#issueDate").text("")

                $("#issueDate").append(txtIssueDateMonth);
                $("#issueDate").append(txtIssueDateDay);
                $("#issueDate").append(txtIssueDateYear);

                $("<span>/</span>").insertAfter($("#txtIssueDateDay"));
                $("<span>/</span>").insertAfter($("#txtIssueDateMonth"));



                }
                else if ((cultureInfo === 'es-US') || (cultureInfo === 'pt-BR'))
                {
                    $('#issueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
                    [
                        { id: 'txtIssueDateDay', length: 2, size: 2 },
                        { id: 'txtIssueDateMonth', length: 2, size: 2 },
                        { id: 'txtIssueDateYear', length: 4, size: 4}
                    ] 
                    });


                    $('#dateOfBirth').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
                    [
                        { id: 'txtDOBDay', length: 2, size: 2 },
                        { id: 'txtDOBMonth', length: 2, size: 2 },
                        { id: 'txtDOBYear', length: 4, size: 4}
                    ]
                    });

                        $('#dateOfBirthCoApplicant').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
                        [
                            { id: 'txtDOBDayCoApplicant', length: 2, size: 2 },
                            { id: 'txtDOBMonthCoApplicant', length: 2, size: 2 },
                            { id: 'txtDOBYearCoApplicant', length: 4, size: 4}
                        ]
                        });

                   $('#issueDateCoApplicant').inputsByFormat(
                    { format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
                    [
                       
                        { id: 'txtIssueDateDayCoApplicant', length: 2, size: 2 }, 
                        { id: 'txtIssueDateMonthCoApplicant', length: 2, size: 2 },
                        { id: 'txtIssueDateYearCoApplicant', length: 4, size: 4}
                    ]
                    });
                    
                        $('#txtDOBMonth').watermark('mm'); //.attr('placeholder', 'mm');
                        $('#txtDOBDay').watermark('dd'); //.attr('placeholder', 'dd');
                        $('#txtDOBYear').watermark('yyyy'); //.attr('placeholder', 'yyyy');


                    $('#txtDOBMonthCoApplicant').watermark('mm'); //.attr('placeholder', 'mm');
                    $('#txtDOBDayCoApplicant').watermark('dd'); //.attr('placeholder', 'dd');
                    $('#txtDOBYearCoApplicant').watermark('yyyy'); //.attr('placeholder', 'yyyy');

                    //cambiar de posicion las cajas de fechas  dd/mm/yyyy
                    var txtDOBMonthCoApplicant = $("#txtDOBMonthCoApplicant").clone(true);
                    var txtDOBDayCoApplicant = $("#txtDOBDayCoApplicant").clone(true);
                    var txtDOBYearCoApplicant = $("#txtDOBYearCoApplicant").clone(true);

                    $("#dateOfBirthCoApplicant").text("")
                    $("#dateOfBirthCoApplicant").append(txtDOBDayCoApplicant);
                    $("#dateOfBirthCoApplicant").append(txtDOBMonthCoApplicant);
                    $("#dateOfBirthCoApplicant").append(txtDOBYearCoApplicant);

                    $("<span>/</span>").insertAfter($("#txtDOBDayCoApplicant"));
                    $("<span>/</span>").insertAfter($("#txtDOBMonthCoApplicant"));



                                        $("#txtIssueDateMonth").watermark('mm');
                                        $("#txtIssueDateDay").watermark('dd');
                                        $("#txtIssueDateYear").watermark('yyyy');

                                        var txtIssueDateMonth = $("#txtIssueDateMonth").clone(true);
                                        var txtIssueDateDay = $("#txtIssueDateDay").clone(true);
                                        var txtIssueDateYear = $("#txtIssueDateYear").clone(true);
                                        $("#issueDate").text("")
                                        $("#issueDate").append(txtIssueDateDay);
                                        $("#issueDate").append(txtIssueDateMonth);
                                        $("#issueDate").append(txtIssueDateYear);

                                        $("<span>/</span>").insertAfter($("#txtIssueDateDay"));
                                        $("<span>/</span>").insertAfter($("#txtIssueDateMonth"));
                }
           







        //$('#dateOfBirth').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' });

    //  inicio  06042017 IPN
//        $('#dateOfBirth').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
//            [
//                { id: 'txtDOBDay', length: 2, size: 2 },
//                { id: 'txtDOBMonth', length: 2, size: 2 },
//                { id: 'txtDOBYear', length: 4, size: 4}
//            ]
//         });
//  FIN 06042017


   //  inicio  06042017 IPN
//        $('#txtDOBMonth').watermark('mm'); //.attr('placeholder', 'mm');
//        $('#txtDOBDay').watermark('dd'); //.attr('placeholder', 'dd');
//        $('#txtDOBYear').watermark('yyyy'); //.attr('placeholder', 'yyyy');
//  FIN 06042017





        /*
         //cambiar de posicion las cajas de fechas  dd/mm/yyyy
        var txtDOBMonth = $("#txtDOBMonth").clone(true);
        var txtDOBDay = $("#txtDOBDay").clone(true);
        var txtDOBYear = $("#txtDOBYear").clone(true);

        $("#dateOfBirth").text("")
        $("#dateOfBirth").append(txtDOBDay);
        $("#dateOfBirth").append(txtDOBMonth);
        $("#dateOfBirth").append(txtDOBYear);

        $("<span>/</span>").insertAfter($("#txtDOBDay"));
        $("<span>/</span>").insertAfter($("#txtDOBMonth"));

        */
//  INICIO 06042017  POR IPN
//        $('#dateOfBirthCoApplicant').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
//         [
//            { id: 'txtDOBDayCoApplicant', length: 2, size: 2 },
//            { id: 'txtDOBMonthCoApplicant', length: 2, size: 2 },
//            { id: 'txtDOBYearCoApplicant', length: 4, size: 4}
//           ]
//          });
// FIN 06042017 



//  INICIO 06042017 POR IPN
//        $('#txtDOBMonthCoApplicant').watermark('mm'); //.attr('placeholder', 'mm');
//        $('#txtDOBDayCoApplicant').watermark('dd'); //.attr('placeholder', 'dd');
//        $('#txtDOBYearCoApplicant').watermark('yyyy'); //.attr('placeholder', 'yyyy');

//        //cambiar de posicion las cajas de fechas  dd/mm/yyyy
//        var txtDOBMonthCoApplicant = $("#txtDOBMonthCoApplicant").clone(true);
//        var txtDOBDayCoApplicant = $("#txtDOBDayCoApplicant").clone(true);
//        var txtDOBYearCoApplicant = $("#txtDOBYearCoApplicant").clone(true);

//        $("#dateOfBirthCoApplicant").text("")
//        $("#dateOfBirthCoApplicant").append(txtDOBDayCoApplicant);
//        $("#dateOfBirthCoApplicant").append(txtDOBMonthCoApplicant);
//        $("#dateOfBirthCoApplicant").append(txtDOBYearCoApplicant);

//        $("<span>/</span>").insertAfter($("#txtDOBDayCoApplicant"));
//        $("<span>/</span>").insertAfter($("#txtDOBMonthCoApplicant"));


// FIN 06042014



    
        






        //@01 new fields

        $('#CPF').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtCPFPart1', length: 9, size: 9 }, { id: 'txtCPFPart2', length: 2, size: 2}] });
        $('#PIS').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtPISPart1', length: 9, size: 9 }, { id: 'txtPISPart2', length: 2, size: 2}] });

            //    inicio 06042017  por IPN
            //        $('#issueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes:
            //         [
            //                { id: 'txtIssueDateDay', length: 2, size: 2 },
            //                { id: 'txtIssueDateMonth', length: 2, size: 2 },
            //                { id: 'txtIssueDateYear', length: 4, size: 4}
            //           ] 
            //         });
            //  FIN 06042017
        
        //cambiar de posicion las cajas de fechas  dd/mm/yyyy

        // INICIO 06042017 IPN
            //        $("#txtIssueDateMonth").watermark('mm');
            //        $("#txtIssueDateDay").watermark('dd');
            //        $("#txtIssueDateYear").watermark('yyyy');

            //        var txtIssueDateMonth = $("#txtIssueDateMonth").clone(true);
            //        var txtIssueDateDay = $("#txtIssueDateDay").clone(true);
            //        var txtIssueDateYear = $("#txtIssueDateYear").clone(true);
            //        $("#issueDate").text("")

            //        $("#issueDate").append(txtIssueDateMonth);
            //        $("#issueDate").append(txtIssueDateDay);
            //        $("#issueDate").append(txtIssueDateYear);

            //        $("<span>/</span>").insertAfter($("#txtIssueDateDay"));
            //        $("<span>/</span>").insertAfter($("#txtIssueDateMonth"));
        // FIN 06042017   

        $('#CPFCoApplicant').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtCPFCoApplicantPart1', length: 9, size: 9 }, { id: 'txtCPFCoApplicantPart2', length: 2, size: 2}] });
        $('#PISCoApplicant').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtPISCoApplicantPart1', length: 9, size: 9 }, { id: 'txtPISCoApplicantPart2', length: 2, size: 2}] });
//  INICIO 06042017  POR IPN
//        $('#issueDateCoApplicant').inputsByFormat(
//         { format: '{0} / {1} / {2}', validateNumbers: true, attributes: 
//                [
//                         { id: 'txtIssueDateDayCoApplicant', length: 2, size: 2 }, 
//                         { id: 'txtIssueDateMonthCoApplicant', length: 2, size: 2 },
//                         { id: 'txtIssueDateYearCoApplicant', length: 4, size: 4}
//                ]
//          });
//  FIN 06042017

        $('#txtIssueDateMonthCoApplicant').watermark('mm'); //.attr('placeholder', 'mm');
        $('#txtIssueDateDayCoApplicant').watermark('dd'); //.attr('placeholder', 'dd');
        $('#txtIssueDateYearCoApplicant').watermark('yyyy'); //.attr('placeholder', 'yyyy');
       

       /*
        //cambiar de posicion las cajas de fechas  dd/mm/yyyy
         var txtIssueDateMonthCoApplicant = $("#txtIssueDateMonthCoApplicant").clone(true);
         var txtIssueDateDayCoApplicant = $("#txtIssueDateDayCoApplicant").clone(true);
         var txtIssueDateYearCoApplicant = $("#txtIssueDateYearCoApplicant").clone(true);
  
        $("#issueDateCoApplicant").text("")
        $("#issueDateCoApplicant").append(txtIssueDateDayCoApplicant);
        $("#issueDateCoApplicant").append(txtIssueDateMonthCoApplicant);
        $("#issueDateCoApplicant").append(txtIssueDateYearCoApplicant);

        $("<span>/</span>").insertAfter($("#txtIssueDateDayCoApplicant"));
        $("<span>/</span>").insertAfter($("#txtIssueDateMonthCoApplicant"));

        */
     
       
        addSsnInputs();
        $('#mainAddressCountry').change(function () {
            addSsnInputs();
            initPhones();
        });
//        document.getElementById('txtCPFPart2').disabled = true;
//        $("#txtCPFPart1").bind('change keydown keyup', function () {
//            if ($(this).val().length == 9) {
//                var value = parseInt($(this).val());
//                var cociente;
//                var newValue = value;
//                var resto;
//                var sumdigit1 = 0;
//                var sumdigit2 = 0;
//                var digit1, digit2, digitFinal;
//                for (var i = 2; i < 11; i++) {
//                    sumdigit1 += (newValue % 10) * i;
//                    newValue = Math.floor(newValue / 10);
//                }
//                resto = sumdigit1 % 11;
//                if (resto < 2) digit1 = 0;
//                else digit1 = 11 - resto;
//                newValue = value;
//                sumdigit2 += digit1 * 2;
//                for (var i = 3; i <= 11; i++) {
//                    sumdigit2 += (newValue % 10) * i;
//                    newValue = Math.floor(newValue / 10);
//                }
//                resto = sumdigit2 % 11;
//                if (resto < 2) digit2 = 0;
//                else digit2 = 11 - resto;
//                digitFinal = "" + digit1 + "" + digit2 + "";
//                $("#txtCPFPart2").val(digitFinal);
//                return true;
//            }
//            else {
//                $("#txtCPFPart2").val("");
//            }
//        });


//        $("#txtPISPart2").bind('change keydown keyup', function () {
//            if ($(this).val().length == 1 && $("#txtPISPart1").val().length == 9) {
//                $(this).disabled = true;
//                var arrNumbers = [3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
//                var value = parseInt($(this).val());
//                var valuePart1 = parseInt($("#txtPISPart1").val());
//                var resto;
//                var sumdigit1 = 0;
//                var indexArray = 0;
//                var digit1, digit2, digitFinal;
//                for (var i = 0; i < 9; i++) {
//                    sumdigit1 += (valuePart1 % 10) * arrNumbers[8 - indexArray];
//                    valuePart1 = Math.floor(valuePart1 / 10);
//                    indexArray++;
//                }
//                sumdigit1 += value * arrNumbers[indexArray];
//                resto = sumdigit1 % 11;
//                if (resto < 2) digit1 = 0;
//                else digit1 = 11 - resto;
//                $("#txtPISPart2").val($("#txtPISPart2").val() + digit1);
//                return true;
//            }
//            else {
//                $("#txtPISPart2").val("");
//            }
//        });

        $('#chkCoApplicant').change(function () {
            if ($(this).prop('checked')) {
                $('#coApplicant :input').not(this).prop('disabled', false);
                $('#coApplicant a').show();
            }
            else{
                $('#coApplicant :input').not(this).prop('disabled', true);
                $('#coApplicant a').hide();
            }

            ClearCoApplicantInfo();
        });

        $('#btnNext').click(function () {   
            var data = getData();
            if (data === false)
                return false;
            ValidateAddress();
        });

    });

    $(document).ready(function () {
        //Ajax Validations: Events
        
        $('#mainAddressZip').attr('name', '');
        $('#mainAddressZipPlusFour').attr('class', 'PostalCode required').attr('name', 'Postal Code is required.') ;

        var chkUseMainForShipping = $('#chkUseMainForShipping');
        var divShippingAddress = chkUseMainForShipping.closest('.FauxTable').find('.FormContainer');

        divShippingAddress.show();
        divShippingAddress.find('input, select').not(chkUseMainForShipping).prop('disabled', true);

        chkUseMainForShipping.unbind().change(function () {
            if ($(this).prop('checked')) {
              
                //$('#shippingAddressCountry').val($('#mainAddressCountry').val()).trigger('change');
                $('#shippingAddressAttention').val($('#mainAddressAttention').val()).trigger('keyup');
                $('#shippingAddressZip').val($('#mainAddressZip').val()).trigger('keyup');
                $('#shippingAddressZipPlusFour').val($('#mainAddressZipPlusFour').val()).trigger('keyup');
//                setTimeout(function(){ 
//                    $('#shippingAddressState').val($('#mainAddressState').val()).trigger('change'); 
//                    $('#shippingAddressCity').val($('#mainAddressCity').val()).trigger('change'); 
//                    //$('#shippingAddressCounty').val($('#mainAddressCounty').val()).trigger('change'); 
//                    $('#shippingAddressStreet').val($('#mainAddressStreet').val()).trigger('change'); 
//                }, 10000);
                $('#shippingAddressAddress1').val($('#mainAddressAddress1').val()).trigger('keyup');
                $('#shippingAddressAddress2').val($('#mainAddressAddress2').val()).trigger('keyup');
                $('#shippingAddressAddress3').val($('#mainAddressAddress3').val()).trigger('keyup');

                divShippingAddress.find('input, select').not(this).prop('disabled', true);
            }
            else{
         

              $('#shippingAddressAttention').val('').trigger('keyup');
                $('#shippingAddressZip').val('').trigger('keyup');
                $('#shippingAddressZipPlusFour').val('').trigger('keyup');
                $('#shippingAddressAddress1').val('').trigger('keyup');
                $('#shippingAddressAddress2').val('').trigger('keyup');
                $('#shippingAddressAddress3').val('').trigger('keyup');


                divShippingAddress.find('input, select').not(this).prop('disabled', false);
            }
        });

//        $('#mainAddressCountry').change(function () {
//            if (chkUseMainForShipping.prop('checked')){
//                $('#shippingAddressCountry').val($(this).val()).trigger('change');
//            }
//        });

        $("#mainAddressAttention").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressAttention').val($(this).val());
            }
        });
        
        $("#mainAddressZip").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressZip').val($(this).val()).trigger('keyup');
            }
        });

        $("#mainAddressZipPlusFour").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressZipPlusFour').val($(this).val()).trigger('keyup');
            }
        });

//        $('#mainAddressState').change(function () {
//            if (chkUseMainForShipping.prop('checked')){
//                $('#shippingAddressState').val($(this).val()).trigger('change');
//            }
//        });

//        $('#mainAddressCity').change(function () {
//            if (chkUseMainForShipping.prop('checked')){
//                $('#shippingAddressCity').val($(this).val()).trigger('change');
//            }
//        });
//        
//        $('#mainAddressCounty').change(function () {
//            if (chkUseMainForShipping.prop('checked')){
//                $('#shippingAddressCounty').val($(this).val()).trigger('change');
//            }
//        });

//        $('#mainAddressStreet').change(function () {
//            if (chkUseMainForShipping.prop('checked')){
//                $('#shippingAddressStreet').val($(this).val()).trigger('change');
//            }
//        });

        $("#mainAddressAddress1").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressAddress1').val($(this).val()).trigger('keyup');
            }
        });

        $("#mainAddressAddress2").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressAddress2').val($(this).val()).trigger('keyup');
            }
        });

        $("#mainAddressAddress3").keyup(function () {
            if (chkUseMainForShipping.prop('checked')){
                $('#shippingAddressAddress3').val($(this).val()).trigger('keyup');
            }
        });

        $('#username').on('blur', function() {

            if ($.trim($(this).val()) == ''){
                validationUserName = false;
                $(this).showError('Username is Required');
            }
            else{
                UserNameValidation($(this).val());
            }            
        }).keyup(function () {
            if (userName != '' && $(this).val() != userName) {
                validationUserName = false;
            }
        });
        

        $('#RG, #RGCoApplicant').on('blur', function() {
            var control = $(this);
            var isCoApplicant = control.attr('id').indexOf("CoApplicant") >= 0 ? true : false;

            if (control.val() > 0)
                DocumentValidation(4, control.val(), control, null, isCoApplicant);
            else{
                if (!isCoApplicant)
                    validationRG = validationReqRG;
                else
                    validationRGCoApplicant = validationReqRG;

                control.showError(validationReqRG);
            }
        }).keydown(function(){
            $(this).clearError('');
        });

        $('#txtCPFPart1, #txtCPFPart2, ' +
          '#txtPISPart1, #txtPISPart2, ' +
          '#txtCPFCoApplicantPart1, #txtCPFCoApplicantPart2, ' +
          '#txtPISCoApplicantPart1, #txtPISCoApplicantPart2'
          ).keyup(function (e) {
            
            var controlID = $(this).attr('id');
            var validationMsg = '';
            var isCoApplicant = controlID.indexOf("CoApplicant") >= 0 ? true : false;

            if (controlID.indexOf("CPF") >= 0){
                var cpfBaseID = '#' + controlID.substr(0, controlID.length - 1);
                var cpfPart1 = $(cpfBaseID + '1');
                var cpfPart2 = $(cpfBaseID + '2');

                if (cpfPart1.val().length == cpfPart1.prop('maxLength') && 
                    cpfPart2.val().length == cpfPart2.prop('maxLength')){

                    DocumentValidation(8, cpfPart1.val() + cpfPart2.val(), cpfPart1, cpfPart2, isCoApplicant);
                }
                else{

                    if (!isCoApplicant)
                        validationCPF = validationReqCPF;
                    else
                        validationCPFCoApplicant = validationReqCPF;

                    cpfPart1.showError('');
                    cpfPart2.showError(validationReqCPF);
                }
            }
            else if (controlID.indexOf("PIS") >= 0){
                var pisBaseID = '#' + controlID.substr(0, controlID.length - 1);
                var pisPart1 = $(pisBaseID + '1');
                var pisPart2 = $(pisBaseID + '2');

                if (pisPart1.val().length > 0 || pisPart2.val().length > 0){
                    if (pisPart1.val().length == pisPart1.prop('maxLength') && 
                        pisPart2.val().length == pisPart2.prop('maxLength')){

                        DocumentValidation(9, pisPart1.val() + pisPart2.val(), pisPart1, pisPart2, isCoApplicant);
                    }
                    else{

                        if (!isCoApplicant)
                            validationPIS = validationReqPIS;
                        else
                            validationPISCoApplicant = validationReqPIS;

                        pisPart1.showError('');
                        pisPart2.showError(validationReqPIS);
                    }
                }
                else if (pisPart1.val().length == 0 && pisPart2.val().length == 0){
                    if (!isCoApplicant)
                        validationPIS = '';
                    else
                        validationPISCoApplicant = '';
                    pisPart1.clearError('');
                    pisPart2.clearError('');
                }
            }
        });

        $('#email').on('blur', function() {
            if ($.trim($(this).val()) == ''){
                validationEmail = false;
                emailText = '';
                $(this).showError('<%= Html.JavascriptTerm("EmailRequired","Email is required.") %>');
            }
            else if ($.trim($(this).val()).indexOf(" ") > -1){
                emailText = '';
                $(this).showError('<%= Html.JavascriptTerm("EmailRequired","A valid Email is required.") %>');
            }
            else{
                EmailValidation($(this).val());
            }            
        }).keyup(function () {
            if (emailText != '' && $(this).val() != emailText) {
                emailText = '';
                validationEmail = false;
            }
        });

        $('#emailConfirmation').on('blur', function() {
            if ($.trim($(this).val()) == ''){
                $(this).showError('<%= Html.JavascriptTerm("ConfirmationRequired","Confirmation Email is required") %>');
            }
            else if ($(this).val() != $('#email').val()){
                $(this).showError('<%= Html.JavascriptTerm("EmailConfirmationMatch","Email must match the Confirmation Email") %>');
            }
            else{
                $(this).clearError();
            }            
        });
    });

    function getData() {
    
        var isCompleteForTaxExempt = true;
         var isValid=true;

        if (!validationSponsor){
            isValid = false;
            $('#sponsor').clearError().showError('<%= Html.JavascriptTerm("PleaseSelectaSponsor","Please select a valid Sponsor.") %>');
            
        }

        if (!validationPlacement){
            isValid = false;
            $('#placement').clearError().showError('<%= Html.JavascriptTerm("PleaseSelectaValPlace","Please select a valid Placement.") %>');
        }

        var txtCPFPart1 = $('#txtCPFPart1');
        var txtCPFPart2 = $('#txtCPFPart2');

        if (validationCPF != ''){
            validate = false;
            txtCPFPart1.showError('');
            txtCPFPart2.showError(validationCPF);
        }
        else{
            txtCPFPart1.clearError();
            txtCPFPart2.clearError();
        }

        var RG = $('#RG');

        if (validationRG != ''){
            validate = false;
            RG.showError(validationRG);
        }
        else{
            RG.clearError();
        }

//        /**/
//         var fechaddMMyyyy = $("#txtIssueDateYear").val().toString() + $("#txtIssueDateMonth").val().toString() + $("#txtIssueDateDay").val();
//            var today = new Date();
//            var dd = today.getDate();
//            var mm = today.getMonth() + 1; //January is 0!
//            var yyyy = today.getFullYear();
//            if(dd < 10){    dd ='0'+dd } 
//            if(mm<10){      mm='0'+mm   } 
//            var today = yyyy + mm + dd;
//            
//            if(parseInt(fechaddMMyyyy) > parseInt(today))
//            {
//                $('#txtIssueDateYear').showError('<%= Html.JavascriptTerm("GenderRequired","Gender is required") %>');
//               isValid = false;
//            }
//            

//        /**/
        var txtPISPart1 = $('#txtPISPart1');
        var txtPISPart2 = $('#txtPISPart2');

        if (validationPIS != ''){
            validate = false;
            txtPISPart1.showError('');
            txtPISPart2.showError(validationPIS);
        }
        else{
            txtPISPart1.clearError();
            txtPISPart2.clearError();
        }
        
        var userNameControl = $('#username');

        if (!$('#generateUsername').prop('checked')){
            
            userNameControl.clearError();

            if ($.trim(userNameControl.val()) == ''){
                userNameControl.showError('<%= Html.JavascriptTerm("UserNameReq","User Name is required.") %>');
                isValid = false;
            }
            else if (!validationUserName){
                userNameControl.showError('<%= Html.JavascriptTerm("UserNameisNotAvailablePleaseEnteraDifferentUsername","User name is not available. Please enter a different Username.") %>');
            }
        }
        else{
            userNameControl.clearError();
        }

//        if ($('#gender').val() == 0){
//            $('#gender').showError('<%= Html.JavascriptTerm("GenderRequired","Gender is required") %>');
//            isValid = false;
//        }
//        else{
//             $('#gender').clearError();    
//        }

//        if ($('#ddlNationality').val() == 0){
//            $('#ddlNationality').showError('<%= Html.JavascriptTerm("NationalityRequired","Nationality is required") %>');
//            isValid = false;
//        }
//        else{
//             $('#ddlNationality').clearError();
//        }

//        if ($('#ddlMaritalStatus').val() == 0){
//            $('#ddlMaritalStatus').showError('<%= Html.JavascriptTerm("MaritalStatusRequired","Marital Status is required") %>');
//            isValid = false;
//        }
//        else{
//             $('#ddlMaritalStatus').clearError();
//        }

//        if ($('#ddlOccupation').val() == 0){
//            $('#ddlOccupation').showError('<%= Html.JavascriptTerm("OccupationRequired","Occupation is required") %>');
//            isValid = false;
//        }
//        else{
//             $('#ddlOccupation').clearError();
//        }

        var emailControl = $('#email');
        emailControl.clearError();

        if ($.trim(emailControl.val()) == ''){
            emailControl.showError('<%= Html.JavascriptTerm("EmailRequired","Email is required.") %>');
            isValid = false;
        }
        else if (!validationEmail){
            isValid = false;
            $('#email').showError('<%= Html.JavascriptTerm("EmailAccountAlreadyExists","An account with this e-mail already exists.") %>');
        }

        $('#emailConfirmation').clearError();

        if ($.trim($('#emailConfirmation').val()) == ''){
            $('#emailConfirmation').showError('<%= Html.JavascriptTerm("ConfirmationRequired","Confirmation Email is required") %>');
            isValid = false;
        }
        else if ($('#email').val() != $('#emailConfirmation').val()){
            $('#emailConfirmation').clearError();
            isValid = false;
            $('#emailConfirmation').showError('<%= Html.JavascriptTerm("EmailConfirmationMatch","Email must match the Confirmation Email") %>');
        }
    
        if (!validateDisbursementInformation())
            isValid = false;
        
        if (!validateCoApplicantInfo())
            isValid = false;

        if (parseBool('<%= NetSteps.Data.Entities.AvataxAPI.Util.IsAvataxEnabled() %>')) {
            if ($('#taxExempt').prop('checked')) {
                if ($('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').val() && !$('#PropertyType<%= ExemptReasonAccPropTypeID %>').val() > 0) {
                    $('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').showError('<%= Html.JavascriptTerm("TaxExemptReason", "Please select any exempt reason")%>');
                    isCompleteForTaxExempt = false;
                }
                else {
                    $('#PropertyTypeTR<%= ExemptReasonAccPropTypeID %>').clearError();
                    isCompleteForTaxExempt = true;
                }
            }
        }
        else {
            isCompleteForTaxExempt = true;
        }

        var mainPhoneOk = false;
        var cellPhoneOk = true;
        var alternativeOK = true;

        $('#phones .phoneContainer').each(function (i) {
      
            var phoneType = $('.phoneType', this).val();
 // comendatado HUNDRED inicio 27042017  funcionalidad solo es valida para brazil
//            var field1 = $('.phoneInput1', this).val();
//            var field2 = $('.phoneInput2', this).val();
            var validate = false;
//    fin 27042017

          // agregado por HUNDRED  inicio : 27042017 para validar inputs que se muestran solo para brazil
            var field1 = $('.phoneInput1', this).val() != null ? $('.phoneInput1', this).val() : '';
            var field2 = $('.phoneInput2', this).val() != null ? $('.phoneInput2', this).val() : '';

         // fin 27042017

           


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
            isValid=false;//  return false;
        }
        else{ 
            $('#phones').clearError();
        }

        if (!cellPhoneOk) {
            $('#phones').showError('<%= Html.JavascriptTerm("ValidCellPhoneIsRequired", "A Valid Cell Phone Is Required") %>');
          isValid=false;//
          //  return false;
        }else if(isValid)
        {
             $('#phones').clearError();
        }

        if (!alternativeOK) {
            $('#phones').showError('<%= Html.JavascriptTerm("ValidAltPhoneIsRequired", "A Valid Phone Is Required") %>');
          isValid=false;//
          //  return false;
        }else if(isValid)
        {
             $('#phones').clearError();
        }

        
        if (!$('#distributorInfo').checkRequiredFields() || !$('#addresses').checkRequiredFields() || !$('#accountProperties').checkRequiredFields() || !isCompleteForTaxExempt) {
             isValid=false;//
            //return false;
        }
//        
//        if (!$('#sponsorId').val()  ||  !$('#sponsor').val() ) {
//            $('#sponsor').showError('<%= Html.JavascriptTerm("PleaseSelectaSponsor", "Please select a sponsor") %>').focus();
//             isValid=false;//
//            //return false;
//        }else{ $('#sponsor').clearError()}
//        
//         if (!$('#placementId').val()  ||  !$('#placement').val()) {
//            $('#placement').showError('<%= Html.JavascriptTerm("PleaseSelectaPlacement", "Please select a Placement") %>').focus();
//             isValid=false;//
//            //return false;
//        }else{ $('#placement').clearError()}

        

        if (!$('#generatePassword').attr('checked') && $('#password').val() != $('#passwordConfirm').val()) {
            $('#password,#passwordConfirm').showError('<%= Html.JavascriptTerm("PasswordsDoNotMatch", "Passwords do not match") %>').keyup(function () {
                if ($('#password').val() == $('#passwordConfirm').val())
                    $('#password,#passwordConfirm').clearError();
            });
            $('#password').focus();
             isValid=false;//
            //return false;
        }
        
        if (!$('#ssn').inputsByFormat('getValue')) {
            $('#ssn input').showError('').filter(':last').clearError().showError('<%= isEntity ? "EIN" : "SSN" %> is required').end()
		.keyup(function () {
		    if ($('#ssn').inputsByFormat('getValue'))
		        $('#ssn input').clearError();
		    else
		        $('#ssn input').showError('').filter(':last').clearError().showError('<%= isEntity ? "EIN" : "SSN" %> is required')
		});
         isValid=false;//
            //return false;
        }
        // solo se aplica para USA 
         <%if(EnvironmentCountry!=73){ %>
        if (!CreditCard.validate($('#accountNumber').val()).isValid) {
            $('#accountNumber').showError('<%= Html.JavascriptTerm("CreditCardNumberIsInvalid", "Credit card number is invalid.") %>').focus().keyup(function () {
                if (CreditCard.validate($(this).val()).isValid)
                    $(this).clearError();
                else
                    $(this).showError('<%= Html.JavascriptTerm("CreditCardNumberIsInvalid", "Credit card number is invalid.") %>')
            });
            isValid=false;//// return false;
        }else
        { $('#accountNumber').clearError();}
        <%}%>


        var today = new Date();
        var lastDayInMonth = new Date(today.getFullYear(), today.getMonth(), 0);
        if (new Date().setFullYear($('#expYear').val(), $('#expMonth').val() - 1, lastDayInMonth.getDate()) < today) {
            $('#expMonth,#expYear').showError('<%= Html.JavascriptTerm("ThisExpirationDateIsInThePast", "This expiration date is in the past.") %>');
            $('#expMonth').focus();
          isValid=false;//  return false;
        }else
        {
            $('#expMonth,#expYear').clearError();
        }

        $('#txtDOBYear').clearError();
        $('#txtDOBDay').clearError();
        $('#txtDOBMonth').clearError();
        

        if ($("#txtIssueDateDay").val() != '' || $("#txtIssueDateMonth").val() != '' || $("#txtIssueDateYear").val() != ''){
            var isDateValidIssue = CheckValidDate($("#txtIssueDateDay").val(), $("#txtIssueDateMonth").val(), $("#txtIssueDateYear").val());
            if (!isDateValidIssue) {
                $('#txtIssueDateDay').showError("");
                $('#txtIssueDateMonth').showError("");
                $('#txtIssueDateYear').showError('<%= Html.JavascriptTerm("InvalidIssueDate","Please enter a valid Issue Date") %>');
                $('#txtIssueDateMonth').focus();
                isValid=false;//   return false;
            }
            else {
                    var isAgeValidIssue = CheckValidDateCurrent($("#txtIssueDateDay").val(), $("#txtIssueDateMonth").val(), $("#txtIssueDateYear").val());
                    if (!isAgeValidIssue) 
                    {
                        $('#txtIssueDateDay').showError("");
                        $('#txtIssueDateMonth').showError("");
                        $('#txtIssueDateYear').showError('<%= Html.JavascriptTerm("TheDateShouldnotExceedTheCurrentDate","The Date should not exceed the current date.") %>');
                        //$('#txtIssueDateMonth').focus();
                        isValid=false;//  return false;
                    }
                    else
                    {
                        $('#txtIssueDateDay').clearError();
                        $('#txtIssueDateMonth').clearError();
                        $('#txtIssueDateYear').clearError();
                    }
            }
        }
        else{
            $('#txtIssueDateDay').clearError();
            $('#txtIssueDateMonth').clearError();
            $('#txtIssueDateYear').clearError();
        }
        
        var isDateValid = CheckValidDate($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
        if (!isDateValid) {
            $('#txtDOBDay').showError("");
            $('#txtDOBMonth').showError("");
            $('#txtDOBYear').showError('<%= Html.JavascriptTerm("InvalidDOB","Please enter a valid DOB") %>');
            $('#txtDOBMonth').focus();
            isValid=false;// return false;
        }
        else 
        {
            var isAgeValid = CheckValidAge($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
            if (!isAgeValid) {
                $('#txtDOBDay').showError("");
                $('#txtDOBMonth').showError("");
                $('#txtDOBYear').showError('<%= Html.JavascriptTerm("DOBInValidYear","Age should be greater than 18") %>');
                $('#txtDOBMonth').focus();
                isValid=false;//return false;
            }
            else{
                $('#txtDOBDay, #txtDOBMonth, #txtDOBYear').clearError();
            }
        }

//        if (!isDateValid) {
//            if ($('#txtDOBDay').val() == '' || $('#txtDOBMonth').val() == '' || $('#txtDOBYear').val() == '') {
//                $('#txtDOBDay').showError("");
//                $('#txtDOBMonth').showError("");
//                $('#txtDOBYear').showError('<%= Html.JavascriptTerm("InvalidDOB","Please enter a valid DOB") %>');
//                $('#txtDOBMonth').focus();
//                isValid=false;//return false;
//            }else{$('#txtDOBYear').clearError()}
//        }
//        else {
//            var isAgeValid = CheckValidAge($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());
//            if (!isAgeValid) {
//                $('#txtDOBDay').showError("");
//                $('#txtDOBMonth').showError("");
//                $('#txtDOBYear').showError('<%= Html.JavascriptTerm("DOBInValidYear","Age should be greater than 18") %>');
//                $('#txtDOBMonth').focus();
//               isValid=false;// return false;
//            }else
//            {  $('#txtDOBYear').clearError()}
//        }
        
        var ddlRelationship = $('#ddlRelationship');
        var txtComplementaryPhone = $('#txtComplementaryPhone');

        if ($.trim($('#txtReferenceName').val()) != ''){
            
            if (ddlRelationship.val() == 0 || ddlRelationship.val() == ''){
                ddlRelationship.showError('<%= Html.JavascriptTerm("RelationshipReq","Relationship is required") %>');
                isValid = false;
            }
            else{
                ddlRelationship.clearError();
            }

            if (txtComplementaryPhone.val().length < txtComplementaryPhone.prop('maxLength')){
                txtComplementaryPhone.showError('<%= Html.JavascriptTerm("ComplementaryPhoneReq","Complementary Phone is required") %>');
                isValid = false;
            }
            else{
                txtComplementaryPhone.clearError();
            }
        }
        else{
            ddlRelationship.clearError();
            txtComplementaryPhone.clearError();
        }

        return isValid;
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
                state: $('#mainAddressState').val(),
                postalCode: $('#mainAddressControl .PostalCode').fullVal(),
                //country: $('#mainAddressCountry :selected').data("countrycode"),
                country: $('#mainAddressCountry').data("countrycode"),
                //street: $('#mainAddressStreet :selected').val()
                street: $('#mainAddressStreet').val()
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
                //country: $('#shippingAddressCountry :selected').data("countrycode"),
                country: $('#shippingAddressCountry').data("countrycode"),
                //strret: $('#shippingAddressStreet :selected').val()
                strret: $('#shippingAddressStreet').val()
            });

            //console.log('antes de validation.init()');
        validation.init();
        $(document).bind("validAddressFound", function (event, address) {
            var p = $(this).parent();
            console.log('validAddressFound');
            showLoading(p);
            var data;
            if ($('#chkUseMainForShipping').prop('checked'))
                data = {
                    sponsorId: $('#sponsorId').val() == '' ? null : $('#sponsorId').val(),
                    placementId: $('#placementId').val() == '' ? null : $('#placementId').val(),
                    firstName: $('#firstName').val(),
                    middleName: $('#middleName').val(),
                    lastName: $('#lastName').val(),
                    entityName: $('#entityName').val() || "",
                    email: $('#email').val(),
                    generateUsername: $('#generateUsername').prop('checked'),
                    username: $('#username').val(),
                    generatePassword: $('#generatePassword').prop('checked'),
                    password: $('#password').val(),
                    languageId: $('#languageId').val(), //  taxExempt: $('#taxExempt').prop('checked'), //   taxNumber: $('#ssn').inputsByFormat('getValue'),
                    applicationOnFile: $('#applicationOnFile').prop('checked'),
                    gender: $('#gender').val() == '0' ? null : $('#gender').val(),
                    dateOfBirth: $('#dateOfBirth').inputsByFormat('getValue', '{0}/{1}/{2}'),
                   
                    nameOnCard: $('#nameOnCard').val(),
                    accountNumber: $('#accountNumber').val(),
                    expirationDate: $('#expMonth').val() + '/1/' + $('#expYear').val(),
                    taxNumber: $('#CPF').inputsByFormat('getValue', '{0}{1}'),
                    //main address
                    'mainAddress.FirstName': $('#firstName').val(),
                    'mainAddress.LastName': $('#lastName').val(),
                    'mainAddress.Attention': $('#mainAddressAttention').val(),
                    'mainAddress.Address1': address.address1,
                    'mainAddress.Address2': address.address2,
                    'mainAddress.Address3': address.address3,
                    'mainAddress.PostalCode': address.postalCode,
                    'mainAddress.City': address.city,
                    'mainAddress.County': $('#mainAddressCounty').val(),
                    'mainAddress.State': address.state,
                    'mainAddress.CountryID': $('#mainAddressCountry').val(),
                    'mainAddress.PhoneNumber': $('#mainAddressPhone').length ? $('#mainAddressPhone').phone('getPhone') : '',
                    'mainAddress.Street': $('#mainAddressStreet').val()

                };
            else
                data = {
                    sponsorId: $('#sponsorId').val() == '' ? null : $('#sponsorId').val(),
                    placementId: $('#placementId').val() == '' ? null : $('#placementId').val(),
                    firstName: $('#firstName').val(),
                    middleName: $('#middleName').val(),
                    lastName: $('#lastName').val(),
                    entityName: $('#entityName').val() || "",
                    email: $('#email').val(),
                    generateUsername: $('#generateUsername').prop('checked'),
                    username: $('#username').val(),
                    generatePassword: $('#generatePassword').prop('checked'),
                    password: $('#password').val(),
                    languageId: $('#languageId').val(), // taxExempt: $('#taxExempt').prop('checked'), //       taxNumber: $('#ssn').inputsByFormat('getValue'),
                    applicationOnFile: $('#applicationOnFile').prop('checked'),
                    gender: $('#gender').val() == '0' ? null : $('#gender').val(),
                    dateOfBirth: $('#dateOfBirth').inputsByFormat('getValue', '{0}/{1}/{2}'),
                    nameOnCard: $('#nameOnCard').val(),
                    accountNumber: $('#accountNumber').val(),
                    expirationDate: $('#expMonth').val() + '/1/' + $('#expYear').val(),
                    taxNumber: $('#CPF').inputsByFormat('getValue', '{0}{1}'),
                    //main address
                    'mainAddress.FirstName': $('#firstName').val(),
                    'mainAddress.LastName': $('#lastName').val(),
                    'mainAddress.Attention': $('#mainAddressAttention').val(),
                    'mainAddress.Address1': $('#mainAddressAddress1').val(),
                    'mainAddress.Address2': $('#mainAddressAddress2').val(),
                    'mainAddress.Address3': $('#mainAddressAddress3').val(),
                    'mainAddress.PostalCode': $('#mainAddressControl .PostalCode').fullVal(),
                    //'mainAddress.City': $("#mainAddressCity :selected").text() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity :selected").text(), // in case Country is not US, it is a simple text input,
                    'mainAddress.City': $("#mainAddressCity").val() == "" ? $("#mainAddressCity").val() : $("#mainAddressCity").val(), // in case Country is not US, it is a simple text input,
                    'mainAddress.County': $('#mainAddressCounty').val(),
                    //'mainAddress.State': $('#mainAddressState :selected').val(),
                    'mainAddress.State': $('#mainAddressState').val(),
                    'mainAddress.CountryID': $('#mainAddressCountry').val(),
                    'mainAddress.PhoneNumber': $('#mainAddressPhone').length ? $('#mainAddressPhone').phone('getPhone') : '',
                    'mainAddress.Street': $('#mainAddressStreet').val()
                };

            $('#accountProperties .property').filter(function () { return !!$('.propertyValue', this).val(); }).each(function (i) {
                var value = $('.propertyValue', this);
                data['properties[' + i + '].AccountPropertyID'] = $('.accountPropertyId', this).val();
                data['properties[' + i + '].AccountPropertyTypeID'] = $('.accountPropertyTypeId', this).val();
                if (value.is('select')) {
                    data['properties[' + i + '].AccountPropertyValueID'] = value.val();
                } else if (value.prop('checkbox')) {
                    data['properties[' + i + '].PropertyValue'] = value.prop('checked') ? 1 : 0;
                }
                else {
                    data['properties[' + i + '].PropertyValue'] = value.val();
                }
            });

            if (!$('#chkUseMainForShipping').prop('checked')) {
                //shipping address
                data['shippingAddress.FirstName'] = $('#firstName').val();
                data['shippingAddress.LastName'] = $('#lastName').val();
                data['shippingAddress.Attention'] = $('#shippingAddressAttention').val();
                data['shippingAddress.Address1'] = address.address1;
                data['shippingAddress.Address2'] = address.address2;
                data['shippingAddress.Address3'] = address.address3;
                data['shippingAddress.PostalCode'] = address.postalCode;
                data['shippingAddress.City'] = address.city;
                data['shippingAddress.County'] = $('#shippingAddressCounty').val();
                data['shippingAddress.State'] = address.state;
                data['shippingAddress.CountryID'] = $('#shippingAddressCountry').val();
                data['shippingAddress.PhoneNumber'] = $('#shippingAddressPhone').length ? $('#shippingAddressPhone').phone('getPhone') : '';
                data['shippingAddress.Street'] = $('#shippingAddressStreet').val();
            }

            if (!$('#chkUseMainForBilling').prop('checked')) {
                //billing address
                data['billingAddress.FirstName'] = $('#firstName').val();
                data['billingAddress.LastName'] = $('#lastName').val();
                data['billingAddress.Attention'] = $('#billingAddressAttention').val();
                data['billingAddress.Address1'] = $('#billingAddressAddress1').val();
                data['billingAddress.Address2'] = $('#billingAddressAddress2').val();
                data['billingAddress.Address3'] = $('#billingAddressAddress3').val();
                data['billingAddress.PostalCode'] = $('#billingAddressControl .PostalCode').fullVal();
                data['billingAddress.City'] = $('#billingAddressCity').val();
                data['billingAddress.County'] = $('#billingAddressCounty').val();
                data['billingAddress.State'] = $('#billingAddressState').val();
                data['billingAddress.CountryID'] = $('#billingAddressCountry').val();
                data['billingAddress.PhoneNumber'] = $('#billingAddressPhone').length ? $('#billingAddressPhone').phone('getPhone') : '';
                data['billingAddress.Street'] = $('#billingAddressStreet').val();
            }

            data['AccountSocialNetworks[0].SocialNetworkID'] = 1;
            data['AccountSocialNetworks[0].Value'] = $('#linkBlog').val();
            data['AccountSocialNetworks[1].SocialNetworkID'] = 2;
            data['AccountSocialNetworks[1].Value'] = $('#linkFacebook').val();
            data['AccountSocialNetworks[2].SocialNetworkID'] = 3;
            data['AccountSocialNetworks[2].Value'] = $('#linkOrkut').val();
            data['AccountSocialNetworks[3].SocialNetworkID'] = 4;
            data['AccountSocialNetworks[3].Value'] = $('#linkTwitter').val();
            data['AccountSocialNetworks[4].SocialNetworkID'] = 5;
            data['AccountSocialNetworks[4].Value'] = $('#linkLinkedIn').val();
            data['AccountSocialNetworks[5].SocialNetworkID'] = 6;
            data['AccountSocialNetworks[5].Value'] = $('#EmailMsn').val();


            data['AccountSuppliedIDs[0].AccountSuppliedIDValue'] = $('#CPF').inputsByFormat('getValue', '{0}{1}');
            data['AccountSuppliedIDs[0].IsPrimaryID'] = true;
            data['AccountSuppliedIDs[0].IDTypeID'] = 8;
            data['AccountSuppliedIDs[1].AccountSuppliedIDValue'] = $('#PIS').inputsByFormat('getValue', '{0}{1}');
            data['AccountSuppliedIDs[1].IsPrimaryID'] = false;
            data['AccountSuppliedIDs[1].IDTypeID'] = 9;
            data['AccountSuppliedIDs[2].AccountSuppliedIDValue'] = $('#RG').val();
            data['AccountSuppliedIDs[2].ExpeditionEntity'] = $('#OrgExp').val();
            data['AccountSuppliedIDs[2].IDExpeditionIDate'] = $('#issueDate').inputsByFormat('getValue', '{0}/{1}/{2}') == '//' ? null : $('#issueDate').inputsByFormat('getValue', '{0}/{1}/{2}');
            data['AccountSuppliedIDs[2].IsPrimaryID'] = false;
            data['AccountSuppliedIDs[2].IDTypeID'] = 4;

            //phonesComplementary
            //var numberPhone = 0;
//            $('#phonesComplementary .phoneContainer').filter(function () {
//                return !!$('.phone', this).phone('getPhone');
//            }).each(function (i) {

//                if ($('.phoneType', this).val() == 1)
//                    numberPhone = $('.phone', this).phone('getPhone');
//            });

            data['AccountReferences.ReferenceName'] = $('#txtReferenceName').val();
            data['AccountReferences.PhoneNumberMain'] = $('#txtComplementaryPhone').val();
            data['AccountReferences.RelationShip'] = $('#ddlRelationship').val();

            data['AccountProperties[0].AccountPropertyTypeID'] = 1005;           
            data['AccountProperties[0].AccountPropertyValueID'] = $('#ddlSchoolinelevel').val();
            data['AccountProperties[1].AccountPropertyTypeID'] = 1012;
            data['AccountProperties[1].PropertyValue'] = $('#ddlHasComputer').val();
            data['AccountProperties[2].AccountPropertyTypeID'] = 1013;
            data['AccountProperties[2].AccountPropertyValueID'] = $('#ddlInternetUse').val();

            data['AccountProperties[3].AccountPropertyTypeID'] = 1010;  //Nationality
            data['AccountProperties[3].AccountPropertyValueID'] = $('#ddlNationality').val();

            data['AccountProperties[4].AccountPropertyTypeID'] = 1009;  //MaritalStatus
            data['AccountProperties[4].AccountPropertyValueID'] = $('#ddlMaritalStatus').val();

            data['AccountProperties[5].AccountPropertyTypeID'] = 1014;  //SpouseFirstName
            data['AccountProperties[5].PropertyValue'] = $('#txtSpouseName').val();

            data['AccountProperties[6].AccountPropertyTypeID'] = 1018;  //SonsNumber
            data['AccountProperties[6].PropertyValue'] = $('#txtSons').val();
            
            data['AccountProperties[7].AccountPropertyTypeID'] = 1011;  //Occupation
            data['AccountProperties[7].AccountPropertyValueID'] = $('#ddlOccupation').val();

            data['AccountProperties[8].AccountPropertyTypeID'] = 1006;  //Accept Share With Network
            data['AccountProperties[8].PropertyValue'] = $('#AuthNetworkData').prop('checked');

            data['AccountProperties[9].AccountPropertyTypeID'] = 1007;  //Accept Share With Network
            data['AccountProperties[9].PropertyValue'] = $('#AuthEmailSend').prop('checked');

            data['AccountProperties[10].AccountPropertyTypeID'] = 1008;  //Accept Share For Locator
            data['AccountProperties[10].PropertyValue'] = $('#AuthShareData').prop('checked');

            if ($('#chkCoApplicant').prop('checked')){
                data['CoApplicantObj.FirstName'] = $('#firstNameCoApplicant').val();
                data['CoApplicantObj.MiddleName'] = $('#middleNameCoApplicant').val();
                data['CoApplicantObj.LastName'] = $('#lastNameCoApplicant').val();
                data['CoApplicantObj.Relationship'] = $('#ddlRelationshipCoApplicant').val();
                data['CoApplicantObj.Gender'] = $('#genderCoApplicant').val() == 'Male' ? 1 : 2;
                data['CoApplicantObj.DateOfBirth'] = $('#dateOfBirthCoApplicant').inputsByFormat('getValue', '{0}/{1}/{2}');
                data['CoApplicantObj.CPF'] = $('#CPFCoApplicant').inputsByFormat('getValue', '{0}{1}');
                data['CoApplicantObj.PIS'] = $('#PISCoApplicant').inputsByFormat('getValue', '{0}{1}');
                data['CoApplicantObj.RG'] = $('#RGCoApplicant').val();
                data['CoApplicantObj.OrgExp'] = $('#OrgExpCoApplicant').val();
                data['CoApplicantObj.IssueDate'] = $('#issueDateCoApplicant').inputsByFormat('getValue', '{0}/{1}/{2}') == '//' ? null : $('#issueDateCoApplicant').inputsByFormat('getValue', '{0}/{1}/{2}');
            
                $('#phonesCoApplicant .phoneContainer').filter(function () {
                    return !!$('.phone', this).phone('getPhone');
                }).each(function (i) {
                
                    if ($.trim($('.phoneInput', this).val()) != ''){
                        data['CoApplicantObj.Phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
                        data['CoApplicantObj.Phones[' + i + '].PhoneNumber'] = $('.phoneInput1', this).val() + '' + $('.phoneInput2', this).val();//$('.phoneInput', this).val();
                        data['CoApplicantObj.Phones[' + i + '].IsDefault'] = $('.phoneType', this).val() == 1 ? true : false;
                    }
                });
            }
            else{
                data['CoApplicantObj'] = null;
            }
            
            if ($('input[name=disbursementMethod]:checked').val() == 1){
                var counter = 0;
                
                var DisbursementTypeID = '<%= NetSteps.Commissions.Common.Models.DisbursementMethodKind.EFT.ToInt() %>';

                if ($('#chkEnabledAccount1').prop('checked')){
                    
                    data['DisbursementInformation[' + counter + '].DisbursementTypeID'] = DisbursementTypeID;
                    data['DisbursementInformation[' + counter + '].Name'] = $('#txtNameAccount1').val();
                    data['DisbursementInformation[' + counter + '].AccountNumber'] = $('#txtAccountNumberAccount1').val();
                    data['DisbursementInformation[' + counter + '].BankID'] = $('#ddlBankNameAccount1').val();  //Missing
                    data['DisbursementInformation[' + counter + '].BankName'] = $("#ddlBankNameAccount1 option:selected").text();
                    data['DisbursementInformation[' + counter + '].BankAgency'] = $('#txtAgencyAccount1').val(); //Missing
                    data['DisbursementInformation[' + counter + '].AccountType'] = $('#accountTypeAccount1').val();
                    data['DisbursementInformation[' + counter + '].PercentToDeposit'] = $('#percentToDepositAccount1').val();
                    counter++;

                    if ($('#chkEnabledAccount2').prop('checked')){
                        data['DisbursementInformation[' + counter + '].DisbursementTypeID'] = DisbursementTypeID;
                        data['DisbursementInformation[' + counter + '].Name'] = $('#txtNameAccount2').val();
                        data['DisbursementInformation[' + counter + '].AccountNumber'] = $('#txtAccountNumberAccount2').val();
                        data['DisbursementInformation[' + counter + '].BankID'] = $('#ddlBankNameAccount2').val();  //Missing
                        data['DisbursementInformation[' + counter + '].BankName'] = $("#ddlBankNameAccount2 option:selected").text();
                        data['DisbursementInformation[' + counter + '].BankAgency'] = $('#txtAgencyAccount2').val(); //Missing
                        data['DisbursementInformation[' + counter + '].AccountType'] = $('#accountTypeAccount2').val();
                        data['DisbursementInformation[' + counter + '].PercentToDeposit'] = $('#percentToDepositAccount2').val();
                    }
                    else{
                        data['DisbursementInformation[' + counter + '].DisbursementTypeID'] = DisbursementTypeID;
                        data['DisbursementInformation[' + counter + '].Name'] = '';
                        data['DisbursementInformation[' + counter + '].AccountNumber'] = '';
                        data['DisbursementInformation[' + counter + '].BankID'] = 1;  //Missing
                        data['DisbursementInformation[' + counter + '].BankName'] = '';
                        data['DisbursementInformation[' + counter + '].BankAgency'] = ''; //Missing
                        data['DisbursementInformation[' + counter + '].AccountType'] = 1;
                        data['DisbursementInformation[' + counter + '].PercentToDeposit'] = 0;
                    }
                }

                
            }

            $('#phones .phoneContainer').filter(function () {
            
                    return !!$('.phone', this).phone('getPhone');
            }).each(function (i) {
                
                var number = $('.phoneInput1', this).val() + '' + $('.phoneInput2', this).val();

                if (number != ''){
                    data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
                    data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
                    data['phones[' + i + '].PhoneNumber'] = number;
                    data['phones[' + i + '].IsDefault'] = $('.phoneType', this).val() == 1 ? true : false;
                }
            });

            /*
            if ($('#mainAddressCountry').val() != '<%= usCountry.CountryID.ToString() %>') {
                $('#distributorInfo .phoneContainer').filter(function () {
                    return !!$('.phoneInput', this).val();
                }).each(function (i) {
                    data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
                    data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
                    data['phones[' + i + '].PhoneNumber'] = $('.phoneInput', this).val();
                });
            } else {
                $('#distributorInfo .phoneContainer').filter(function () {
                    return !!$('.phone', this).phone('getPhone');
                }).each(function (i) {
                    data['phones[' + i + '].AccountPhoneID'] = $('.phoneId', this).length ? $('.phoneId', this).val() : 0;
                    data['phones[' + i + '].PhoneTypeID'] = $('.phoneType', this).val();
                    data['phones[' + i + '].PhoneNumber'] = $('.phone', this).phone('getPhone');
                });
            }
            */
            window.letUnload = false;

            data.taxExempt = false;
            data.coApplicant = ""

            hideMessage();

            enrollmentMaster.postStepAction({
                step: "AccountInfo",
                stepAction: "SubmitStep",
                data: data,
                showLoadingElement: $('#btnNext'),
                load: true
            });
        });        
 }
 function ValidarNumerosCaracteres(ctr)
 {
 
     var PhoneType=ctr.value;
     if(PhoneType==2)//Teléfono Celular
     {
        $(ctr).prev("span").find("input:text").attr("maxlength","11");
     }else
     { 
        $(ctr).prev("span").find("input:text").attr("maxlength","10");
     }
 }
   function isNumber(ctr,evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    function validateDisbursementInformation(){
        
        var validate = true;

        if ($('#divDirectDeposit').css('display') != 'none') {
            
            var chkEnabledAccount1 =  $('#chkEnabledAccount1').prop('checked');
            var chkEnabledAccount2 =  $('#chkEnabledAccount2').prop('checked');

            if (!chkEnabledAccount1 && !chkEnabledAccount2){
                $('#divDirectDeposit').showError('<%= Html.JavascriptTerm("AccountReq","Account is required") %>');
                validate = false;
            }
            else{

                $('#divDirectDeposit').clearError();
                
                if (chkEnabledAccount1)
                    validate = validateDisbursementInformationDetail(1);

                if (chkEnabledAccount2)
                    validate = validateDisbursementInformationDetail(2);

                if (chkEnabledAccount1 && chkEnabledAccount2){
                    var percentToDepositAccount1 = $('#percentToDepositAccount1');
                    var percentToDepositAccount2 = $('#percentToDepositAccount2');

                    if ((parseInt(percentToDepositAccount1.val()) + parseInt(percentToDepositAccount2.val())) != 100){
                        percentToDepositAccount1.showError('<%= Html.JavascriptTerm("PercentSumReq","Sum between percentages must be 100%") %>');
                        percentToDepositAccount2.showError('<%= Html.JavascriptTerm("PercentSumReq","Sum between percentages must be 100%") %>');
                        validate = false;
                    }
                    else{
                        percentToDepositAccount1.clearError();
                        percentToDepositAccount2.clearError();
                    }
                }
                else if (chkEnabledAccount1){
                    var percentToDepositAccount1 = $('#percentToDepositAccount1');

                    if (parseInt(percentToDepositAccount1.val()) != 100){
                        percentToDepositAccount1.showError('<%= Html.JavascriptTerm("PercentReq","Percentage must be 100%") %>');
                        validate = false;
                    }
                    else{
                        percentToDepositAccount1.clearError();
                    }
                }
                else if (chkEnabledAccount2){
                    var percentToDepositAccount2 = $('#percentToDepositAccount2');

                    if (parseInt(percentToDepositAccount2.val()) != 100){
                        percentToDepositAccount2.showError('<%= Html.JavascriptTerm("PercentReq","Percentage must be 100%") %>');
                        validate = false;
                    }
                    else{
                        percentToDepositAccount2.clearError();
                    }
                }
            }
            
        }
        return validate;
    }

    function validateDisbursementInformationDetail(type){
        var validate = true;

        var txtNameAccount = $('#txtNameAccount' + type);

        if ($.trim(txtNameAccount.val()) == ''){
            txtNameAccount.showError('<%= Html.JavascriptTerm("NameAccountReq","Name on Account is required") %>');
            validate = false;
        }
        else{
            txtNameAccount.clearError();
        }

        var txtAccountNumberAccount = $('#txtAccountNumberAccount' + type);

        if ($.trim(txtAccountNumberAccount.val()) == ''){
            txtAccountNumberAccount.showError('<%= Html.JavascriptTerm("AccountNumberReq","Account # is required") %>');
            validate = false;
        }
        else{
            txtAccountNumberAccount.clearError();
        }

        var ddlBankNameAccount = $('#ddlBankNameAccount' + type);

        if (ddlBankNameAccount.val() == 0 || ddlBankNameAccount.val() == ''){
            ddlBankNameAccount.showError('<%= Html.JavascriptTerm("BankNameReq","Bank Name is required") %>');
            validate = false;
        }
        else{
            ddlBankNameAccount.clearError();
        }

        var txtAgencyAccount = $('#txtAgencyAccount' + type);

        if ($.trim(txtAgencyAccount.val()) == '' || txtAgencyAccount.attr('maxlength') < 4){
            txtAgencyAccount.showError('<%= Html.JavascriptTerm("AgencyReq","A 4 number Agency is required") %>');
            validate = false;
        }
        else{
            txtAgencyAccount.clearError();
        }

        var accountTypeAccount = $('#accountTypeAccount' + type);

        if (accountTypeAccount.val() == 0 || accountTypeAccount.val() == ''){
            accountTypeAccount.showError('<%= Html.JavascriptTerm("AccountTypeReq","Account Type is required") %>');
            validate = false;
        }
        else{
            accountTypeAccount.clearError();
        }

        var percentToDepositAccount = $('#percentToDepositAccount' + type);

        if ($.trim(percentToDepositAccount.val()) == ''){
            percentToDepositAccount.showError('<%= Html.JavascriptTerm("PercentToReq","% to Deposit is required") %>');
            validate = false;
        }
        else{
            percentToDepositAccount.clearError();
        }

        return validate;
    }

    function validateCoApplicantInfo(){
        var validate = true;

        if (!$('#chkCoApplicant').prop('checked'))
            return validate;

        var firstNameCoApplicant = $('#firstNameCoApplicant');

        if ($.trim(firstNameCoApplicant.val()) == ''){
            firstNameCoApplicant.showError('<%= Html.JavascriptTerm("FirstNameReq","First Name is required") %>');
            validate = false;
        }
        else{
            firstNameCoApplicant.clearError();
        }

        var lastNameCoApplicant = $('#lastNameCoApplicant');

        if ($.trim(lastNameCoApplicant.val()) == ''){
            lastNameCoApplicant.showError('<%= Html.JavascriptTerm("LastNameReq","Last Name is required") %>');
            validate = false;
        }
        else{
            lastNameCoApplicant.clearError();
        }

        var ddlRelationshipCoApplicant = $('#ddlRelationshipCoApplicant');

        if (ddlRelationshipCoApplicant.val() == 0 || ddlRelationshipCoApplicant.val() == ''){
            ddlRelationshipCoApplicant.showError('<%= Html.JavascriptTerm("RelationshipReq","Relationship is required") %>');
            validate = false;
        }
        else{
            ddlRelationshipCoApplicant.clearError();
        }

        var genderCoApplicant = $('#genderCoApplicant');

        if (genderCoApplicant.val() == 0 || genderCoApplicant.val() == ''){
            genderCoApplicant.showError('<%= Html.JavascriptTerm("GenderRequired","Gender is required") %>');
            validate = false;
        }
        else{
            genderCoApplicant.clearError();
        }

        var txtDOBDayCoApplicant = $("#txtDOBDayCoApplicant");
        var txtDOBMonthCoApplicant = $("#txtDOBMonthCoApplicant");
        var txtDOBYearCoApplicant = $("#txtDOBYearCoApplicant");
        
        var isDateValid = CheckValidDate(txtDOBDayCoApplicant.val(), txtDOBMonthCoApplicant.val(), txtDOBYearCoApplicant.val());
        
        if (!isDateValid) {
            txtDOBDayCoApplicant.showError("");
            txtDOBMonthCoApplicant.showError("");
            txtDOBYearCoApplicant.showError('<%= Html.JavascriptTerm("InvalidDate","Please enter a valid Date") %>');
            validate = false;
        }
        else {

            var isAgeValidCo = CheckValidAge(txtDOBDayCoApplicant.val(), txtDOBMonthCoApplicant.val(), txtDOBYearCoApplicant.val());

            if (!isAgeValidCo) {
                txtDOBDayCoApplicant.showError("");
                txtDOBMonthCoApplicant.showError("");
                txtDOBYearCoApplicant.showError('<%= Html.JavascriptTerm("DOBInValidYear","Age should be greater than 18") %>');
                validate = false;
            }
            else{
                txtDOBDayCoApplicant.clearError();
                txtDOBMonthCoApplicant.clearError();
                txtDOBYearCoApplicant.clearError();
            }
        }

        var txtCPFCoApplicantPart1 = $('#txtCPFCoApplicantPart1');
        var txtCPFCoApplicantPart2 = $('#txtCPFCoApplicantPart2');

        if (validationCPFCoApplicant != ''){
            validate = false;
            txtCPFCoApplicantPart1.showError('');
            txtCPFCoApplicantPart2.showError(validationCPFCoApplicant);
        }
        else{
            if ($('#CPFCoApplicant').inputsByFormat('getValue', '{0}{1}') == $('#CPF').inputsByFormat('getValue', '{0}{1}')){
                validate = false;
                txtCPFCoApplicantPart1.showError('');
                txtCPFCoApplicantPart2.showError("Can't use the same CPF for Applicant and CoApplicant.");
            }
            else{
                txtCPFCoApplicantPart1.clearError();
                txtCPFCoApplicantPart2.clearError();
            }
        }

        var RGCoApplicant = $('#RGCoApplicant');

        if (validationRGCoApplicant != ''){
            validate = false;
            RGCoApplicant.showError(validationRGCoApplicant);
        }
        else{
            if ($('#RGCoApplicant').val() == $('#RG').val()){
                validate = false;
                RGCoApplicant.showError("Can't use the same CPF for Applicant and CoApplicant.");
            }
            else{
                RGCoApplicant.clearError();
            }
        }

        var txtPISCoApplicantPart1 = $('#txtPISCoApplicantPart1');
        var txtPISCoApplicantPart2 = $('#txtPISCoApplicantPart2');

        if (validationPISCoApplicant != ''){
            validate = false;
            txtPISCoApplicantPart1.showError('');
            txtPISCoApplicantPart2.showError(validationPISCoApplicant);
        }
        else{
            var PISApplicantVal = $('#PIS').inputsByFormat('getValue', '{0}{1}');
            var PISCoApplicantVal = $('#PISCoApplicant').inputsByFormat('getValue', '{0}{1}');
            if (PISApplicantVal != '' && PISCoApplicantVal != '' && PISApplicantVal == PISCoApplicantVal){
                validate = false;
                txtPISCoApplicantPart1.showError('');
                txtPISCoApplicantPart2.showError("Can't use the same PIS for Applicant and CoApplicant.");
            }
            else{
                txtPISCoApplicantPart1.clearError();
                txtPISCoApplicantPart2.clearError();
            }
        }
        
//        var OrgExpCoApplicant = $('#OrgExpCoApplicant');

//        if ($.trim(OrgExpCoApplicant.val()) == ''){
//            OrgExpCoApplicant.showError('<%= Html.JavascriptTerm("OrgExpReq","Org. Exp. is required") %>');
//            validate = false;
//        }
//        else{
//            OrgExpCoApplicant.clearError();
//        }

        var txtIssueDateDayCoApplicant = $("#txtIssueDateDayCoApplicant");
        var txtIssueDateMonthCoApplicant = $("#txtIssueDateMonthCoApplicant");
        var txtIssueDateYearCoApplicant = $("#txtIssueDateYearCoApplicant");
        
        if (txtIssueDateDayCoApplicant.val() != '' || txtIssueDateMonthCoApplicant.val() != '' || txtIssueDateYearCoApplicant.val() != ''){
            var isDateValid = CheckValidDate(txtIssueDateDayCoApplicant.val(), txtIssueDateMonthCoApplicant.val(), txtIssueDateYearCoApplicant.val());
        
            if (!isDateValid) {
                txtIssueDateDayCoApplicant.showError("");
                txtIssueDateMonthCoApplicant.showError("");
                txtIssueDateYearCoApplicant.showError('<%= Html.JavascriptTerm("InvalidDate","Please enter a valid Date") %>');
                validate = false;
            }
            else {
                txtIssueDateDayCoApplicant.clearError();
                txtIssueDateMonthCoApplicant.clearError();
                txtIssueDateYearCoApplicant.clearError();
            }
        }
        else{
            txtIssueDateDayCoApplicant.clearError();
            txtIssueDateMonthCoApplicant.clearError();
            txtIssueDateYearCoApplicant.clearError();
        }

        return validate;
    }
    
    function ClearCoApplicantInfo(){
        $('#firstNameCoApplicant').val('').clearError();
        $('#middleNameCoApplicant').val('').clearError();
        $('#lastNameCoApplicant').val('').clearError();
        $('#genderCoApplicant').val('0').clearError();
        $('#txtDOBDayCoApplicant, #txtDOBMonthCoApplicant, #txtDOBYearCoApplicant').val('').clearError();
        $('#txtCPFCoApplicantPart1, #txtCPFCoApplicantPart2').val('').clearError();
        $('#txtPISCoApplicantPart1, #txtPISCoApplicantPart2').val('').clearError();
        $('#RGCoApplicant').val('').clearError();
        $('#OrgExpCoApplicant').val('').clearError();
        $('#txtIssueDateDayCoApplicant, #txtIssueDateMonthCoApplicant, #txtIssueDateYearCoApplicant').val('').clearError();

        $('#phonesCoApplicant .phoneContainer').filter(function () {
            return !!$('.phone', this).phone('getPhone');
        }).each(function (i) {
            if (i == 0){
                $('.phoneType', this).val(1);
                $('.phoneInput1, .phoneInput2', this).val('');
            }
            else{
                $(this).remove();
            }
        });
    }

    function clearRequieredAccounts(type){
        //$('#txtNameAccount' + type).clearError();
        $('#txtAccountNumberAccount' + type).clearError();
        $('#ddlBankNameAccount' + type).clearError();
        $('#txtAgencyAccount' + type).clearError();
        $('#accountTypeAccount' + type).clearError();
        $('#percentToDepositAccount' + type).clearError();
    }

    //Ajax Validations: Methods

    function PlacementValidation(PlacementID, PlacementText, showError){
        
        //validationPlacement = false;
        showError = showError == undefined ? false : showError;

        var placementControl = $('#placement');
        placementControl.clearError();
        placementControl.prop('disabled', true);

        var strURL = '<%= ResolveUrl("~/Enrollment/PlacementValidation") %>';
        var Parameters = {
            PlacementID: PlacementID
        };

        $.ajax({
            type: 'POST',
            url: strURL,
            data: JSON.stringify(Parameters),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.result) {
                    $('#placementId').val(PlacementID);
		            placementControl.val(PlacementText);
                    placementName = PlacementText; 
                    validationPlacement = true;
                }
                else {
                    $('#placementId').val('')
                    placementControl.val('');

                    if (showError)
                        placementControl.showError(data.message);
                }
                placementControl.prop('disabled', false);
            }
        });
    }

    function UserNameValidation(UserName){
        
        validationUserName = false;
        var userNameControl = $('#username');
        userNameControl.clearError();
        userNameControl.prop('disabled', true);

        var strURL = '<%= ResolveUrl("~/Enrollment/UserNameValidation") %>';
        var Parameters = {
            UserName: UserName
        };

        $.ajax({
            type: 'POST',
            url: strURL,
            data: JSON.stringify(Parameters),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {

                validationUserName = data.result;

                if (validationUserName) {
                    userNameControl.clearError();
                }
                else {
                    userNameControl.showError(data.message);
                }
                userNameControl.prop('disabled', false);
            }
        });
    }

    function DocumentValidation(DocumentType, DocumentValue, Control1, Control2, isCoApplicant){
        
        Control1.clearError();

        if (Control2 != null)
            Control2.clearError();

        var strURL = '<%= ResolveUrl("~/Enrollment/DocumentValidation") %>';
        var Parameters = {
            DocumentType: DocumentType,
            DocumentValue: DocumentValue
        };

        $.ajax({
            type: 'POST',
            url: strURL,
            data: JSON.stringify(Parameters),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                              
                switch (DocumentType)
                {
                    case 8: //CPF
                        if (!isCoApplicant)
                            validationCPF = data.message;
                        else{
                            if (DocumentValue == $('#CPF').inputsByFormat('getValue', '{0}{1}')){
                                data.result = false;
                                data.message = "Can't use the same CPF for Applicant and CoApplicant.";
                            }

                            validationCPFCoApplicant = data.message;
                        }
                        break;
                    case 9: //PIS
                        if (!isCoApplicant)
                            validationPIS = data.message;
                        else{
                            if (DocumentValue == $('#PIS').inputsByFormat('getValue', '{0}{1}')){
                                data.result = false;
                                data.message = "Can't use the same PIS for Applicant and CoApplicant.";
                            }
                            validationPISCoApplicant = data.message;
                        }
                        break;
                    case 4: //RGs
                        if (!isCoApplicant)
                            validationRG = data.message;
                        else{
                            if (DocumentValue == $('#RG').val()){
                                data.result = false;
                                data.message = "Can't use the same RG for Applicant and CoApplicant.";
                            }
                            validationRGCoApplicant = data.message;
                        }
                        break;
                }

                if (!data.result && Control2 != null){
                    Control1.showError('');
                    Control2.showError(data.message);
                }
                else if (!data.result && Control2 == null){
                    Control1.showError(data.message);
                }
            }
        });
    }

    function EmailValidation(Email){
        
        validationUserName = false;
        var emailControl = $('#email');
        emailControl.clearError();
        emailControl.prop('disabled', true);

        var emailConfirmationControl = $('#emailConfirmation');
        emailConfirmationControl.prop('disabled', true);

        var strURL = '<%= ResolveUrl("~/Enrollment/EmailValidation") %>';
        var Parameters = {
            Email: Email
        };

        $.ajax({
            type: 'POST',
            url: strURL,
            data: JSON.stringify(Parameters),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {

                validationEmail = data.result;

                if (validationEmail) {
                    emailText = Email;
                    emailControl.clearError();
                }
                else {
                    emailText = '';
                    emailControl.showError(data.message);
                }
                emailControl.prop('disabled', false);
                emailConfirmationControl.prop('disabled', false);
            }
        });
    }
</script>
<script>

    function AsignarTipoBusquedaPorCodigoPostal(EsBusquedaCep) {
        var data = JSON.stringify({
            EsBusquedaCep: EsBusquedaCep
        });
        var url = '<%= ResolveUrl("~/Accounts/Browse/AsignarTipoBusqueda") %>';
        $.ajax({
            async: false,
            data: data,
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            success: function (response) {

            },
            error: function (error) {

            }
        })
    }

    function On_Change_TipoBusqueda(ctrCheckBox) {

        var estaMarcado = ctrCheckBox.checked;

        if (estaMarcado) {
            $("#placement").attr("placeholder", '<%= Html.JavascriptTerm("AccountSearchCEP", "Look  By CEP") %>');
        } else {
            $("#placement").attr("placeholder", '<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')

        }
        AsignarTipoBusquedaPorCodigoPostal(estaMarcado);

    }
</script>
<div class="StepGutter">
    <h3>
        <b>
            <%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"]) %></b>
        <%= Html.Term("EnterInTheAccountInformation", "Enter in the account information") %></h3>
</div>
<div class="StepBody">
    <h4>
        <span class="requiredMarker">* </span>
        <%= Html.Term("MainInformation", "Main Information")%></h4>
    <table id="distributorInfo" width="100%" cellspacing="0" class="FormTable">
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="sponsor">
                    <%= Html.Term("Enroller") %>:</label>
            </td>
            <td>
                <% AccountSlimSearchData sponsor = ViewData["Sponsor"] as AccountSlimSearchData; %>
                <input type="text" id="sponsor" style="width: 300px;" value="<%= sponsor == null ? "" : sponsor.FullName + " (#" + sponsor.AccountNumber + ")" %>" />
                <input type="hidden" id="sponsorId" value="<%= sponsor == null ? "" : sponsor.AccountID.ToString() %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <label for="placement">
                    <%= Html.Term("Placement", "Placement") %>:</label>
            </td>
            <td>
                <input type="text" id="placement" style="width: 300px;" value="<%= sponsor == null ? "" : sponsor.FullName + " (#" + sponsor.AccountNumber + ")" %>" />
                <input type="hidden" id="placementId" value="<%= sponsor == null ? "" : sponsor.AccountID.ToString() %>" />
            </td>
            <td>
                <label for="tipoBusqueda">
                    <%= Html.Term("SearchByCEP", "Search By CEP")%>.</label>
                <input type="checkbox" onchange="On_Change_TipoBusqueda(this)" id="tipoBusqueda" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <%= isEntity ? Html.Term("PrimaryApplicantName", "Primary Applicant Name") : Html.Term("Name")%>:
            </td>
            <td>
                <input maxlength="100" type="text" id="firstName" name="<%= Html.Term("FirstNameRequired", "First Name is required.") %>"
                    class="required" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                <input maxlength="100" type="text" id="middleName" />
                <input maxlength="100" type="text" id="lastName" name="<%= Html.Term("LastNameRequired", "Last Name is required.") %>"
                    class="required" />
            </td>
        </tr>
        <% if (isEntity)
           {%>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="entityName">
                    <%= Html.Term("EntityName", "Entity Name") %>:</label>
            </td>
            <td>
                <input type="text" id="entityName" name="<%= Html.Term("EntityNameRequired", "Entity Name is required.") %>"
                    class="required" />
            </td>
        </tr>
        <%} %>
        <tr>
            <td class="FLabel" style="<%= !displayUserNameField ? "display: none": "" %>">
                <span class="requiredMarker">*</span>
                <label for="username">
                    <%= Html.Term("Username")%>:</label>
            </td>
            <td style="<%= !displayUserNameField ? "display: none": "" %>">
                <input type="checkbox" id="generateUsername" checked="checked" /><%= Html.Term("GenerateUsername", "Generate Username (will be Account Number)")%>
                <div id="manualUsername" style="display: none;">
                    <input type="text" id="username" name="<%= Html.Term("UsernameRequired", "Username is required.") %>"
                        class="required" autocomplete="off" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="password">
                    <%= Html.Term("Password") %>:</label>
            </td>
            <td>
                <input type="checkbox" id="generatePassword" checked="checked" /><%= Html.Term("GenerateaRandomPassword", "Generate a random password") %>
                <div id="manualPassword">
                    <input type="password" id="password" name="<%= Html.Term("PasswordRequired", "Password is required.") %>"
                        class="required" autocomplete="off" /><br />
                    <input type="password" id="passwordConfirm" name="<%= Html.Term("PasswordConfirmationRequired", "Password Confirmation is required.") %>"
                        class="required" autocomplete="off" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="languageId">
                    <%= Html.Term("Language") %>:</label>
            </td>
            <td>
                <%= Html.DropDownLanguages(htmlAttributes: new { id = "languageId" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <%= Html.Term("CPF","CPF") %>:
            </td>
            <td id="CPF">
                <%--<input type="text" id="ssn" name="<%= Html.Term("SSNRequired", "SSN is required.") %>" class="required" />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%-- <span class="requiredMarker">*</span>--%>
                <%= Html.Term("PIS","PIS") %>:
            </td>
            <td id="PIS">
                <%--<input type="text" id="ssn" name="<%= Html.Term("SSNRequired", "SSN is required.") %>" class="required" />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <%= Html.Term("RG","RG") %>:
            </td>
            <td>
                <input type="text" maxlength="100" id="RG" style="width: 250px;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("OrgExp","Org. Exp") %>:
            </td>
            <td>
                <input type="text" id="OrgExp" style="width: 250px;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("IssueDate","Issue Date") %>:1
            </td>
            <td id="issueDate">
                <%--<input type="text" id="ssn" name="<%= Html.Term("SSNRequired", "SSN is required.") %>" class="required" />--%>
            </td>
        </tr>
        <%if (!isEntity)
          { %>
        <tr>
            <td class="FLabel">
                <label for="gender">
                    <%= Html.Term("Gender") %>:</label>
            </td>
            <td>
                <select id="gender">
                    <option value="0">
                        <%= Html.Term("Select", "Select") %></option>
                    <option value="Male">
                        <%= Html.Term("Male") %></option>
                    <option value="Female">
                        <%= Html.Term("Female") %></option>
                </select>
            </td>
        </tr>
        <%} %>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <%= isEntity ? Html.Term("PrimaryApplicantDateOfBirth", "Primary Applicant Date of Birth") : Html.Term("DateOfBirth", "Date of Birth")%>:
            </td>
            <td id="dateOfBirth">
                <%--<input id="dateOfBirth" class="DatePicker TextInput" value="<%= DateTime.Today.AddYears(-40).ToShortDateString() %>" />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Nationality","Nationality") %>:
            </td>
            <td>
                <!--davy-->
                <%= Html.DropDownNationality(htmlAttributes: new { id = "ddlNationality" })%>
            </td>
         </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("MaritalStatus","Marital Status") %>:
                </td>
                <td>
                    <%= Html.DropDownMaritalStatus(htmlAttributes: new { id = "ddlMaritalStatus" })%>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("SpouseName", "Spouse Name")%>:
                </td>
                <td>
                    <input type="text" id="txtSpouseName" value="" />
                    <%= Html.Term("Sons", "Sons")%>:
                    <input type="text" maxlength="2" id="txtSons" value="" />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Occupation","Occupation") %>:
                </td>
                <td id="Td6">
                    <%= Html.DropDownOccupation(htmlAttributes: new { id = "ddlOccupation" })%>
                </td>
            </tr>
            <%--@DINI--%>
            <%--<tr>
            <td class="FLabel">
                <label for="applicationOnFile">
                    <%= Html.Term("ApplicationOnFile", "Application On File") %>:</label>
            </td>
            <td>
                <input type="checkbox" id="applicationOnFile" />
            </td>
        </tr>--%>
            <%--@DFIN--%>
    </table>
    <h4>
        <span class="requiredMarker">* </span>
        <%= Html.Term("ContactInfo", "Contact Info")%></h4>
    <table id="contactInfo" width="100%" cellspacing="0" class="FormTable">
        <tr>
            <td class="FLabel">
                <%= Html.Term("PhoneNumbers", "Phone Numbers") %>:
                <p class="InputTools">
                    <a id="btnAddPhone" href="javascript:void(0);" class="AddPhone">
                        <%= Html.Term("AddaPhoneNumber", "Add a phone number") %></a>
                </p>
            </td>
            <td>
                <div id="phones">
                </div>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="email">
                    <%= Html.Term("Email") %>:</label>
            </td>
            <td>
                <input type="text" id="email" style="width: 250px;" />
            </td>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                <label for="emailConfirmation">
                    <%= Html.Term("EmailConfirmation","Confirm Email") %>:</label>
            </td>
            <td>
                <input type="text" id="emailConfirmation" style="width: 250px;" />
            </td>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <%--<tr>
            <td>
                <input type="checkbox" id="AuthNetworkData" />
            </td>
            <td class="FLabel">
                <label for="AuthNetworkData">
                    <%= Html.Term("AuthNetworkData", "Autorizo compartilhar meus dados de contato (e-mail, telefone) con minha rede.") %></label>
            </td>
            
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="AuthEmailSend" />
            </td>
            <td class="FLabel">
                <label for="AuthEmailSend">
                    <%= Html.Term("AuthEmailSend", "Aceito receber e-mails da Belcorp.") %></label>
            </td>
            
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="AuthShareData" />
            </td>
            <td class="FLabel">
                <label for="AuthShareData">
                    <%= Html.Term("AuthShareData", "Autorizo a divulgaçao dos meus dados (nome, telefone e e-mail) no localizador de consultores Belcorp.") %></label>
            </td>
            
        </tr>--%>
    </table>
    <table width="100%" cellspacing="0" class="FormTable">
        <tr>
            <td>
                <input type="checkbox" id="AuthNetworkData" checked="checked" />
                <label for="AuthNetworkData">
                    <%= Html.Term("AuthNetworkData", "Autorizo compartilhar meus dados de contato (e-mail, telefone) con minha rede.") %></label>
            </td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="AuthEmailSend" checked="checked" />
                <label for="AuthEmailSend">
                    <%= Html.Term("AuthEmailSend", "Aceito receber e-mails da Belcorp.") %></label>
            </td>
        </tr>
        <tr>
            <td>
                <input type="checkbox" id="AuthShareData" checked="checked" />
                <label for="AuthShareData">
                    <%= Html.Term("AuthShareData", "Autorizo a divulgaçao dos meus dados (nome, telefone e e-mail) no localizador de consultores Belcorp.") %></label>
            </td>
        </tr>
    </table>
    <div id="addresses">
        <h4>
            <span class="requiredMarker">*</span>
            <%= Html.Term("MainAddress", "Main Address") %></h4>
        <% Html.RenderPartial("Address", new AddressModel()
               {
                   Address = null,
                   LanguageID = CoreContext.CurrentLanguageID,
                   ShowCountrySelect = true,
                   ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                   ExcludeFields = new List<string>() { "ProfileName" },
                   Prefix = "mainAddress"
               }); %>
        <h4>
            <%= Html.Term("ShippingAddress", "Shipping Address") %></h4>
        <div class="FauxTable">
            <div class="FRow">
                <span class="FLabel">
                    <%= Html.Term("UseMainAddress", "Use Main Address") %></span> <span class="FInput">
                        <input type="checkbox" id="chkUseMainForShipping" checked="checked" /></span>
            </div>
            <% Html.RenderPartial("Address", new AddressModel()
                   {
                       Address = null,
                       LanguageID = CoreContext.CurrentLanguageID,
                       ShowCountrySelect = true,
                       ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                       ExcludeFields = new List<string>() { "ProfileName" },
                       Prefix = "shippingAddress"
                   }); %>
            <%--<%= AddressControl.RenderAddress(null, enrollmentContext.LanguageID.Value, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", new List<string>() { "ProfileName" }, "shippingAddress")%>--%>
        </div>
        <%  bool enabled = false; %>
        <div id="disbursementInformation">
            <h4>
                <%= Html.Term("DisbursementInformation", "Disbursement Information")%></h4>
            <input type="radio" name="disbursementMethod" id="paymentOrder" value="0" checked="checked" />
            <label for="paymentOrder">
                Payment Order</label>
            <br />
            <input type="radio" name="disbursementMethod" id="directDeposit" value="1" />
            <label for="directDeposit">
                Direct Deposit</label>
            <div id="divDirectDeposit" style="display: none;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <h4 class="ModTitle">
                                <%= Html.Term("Account1", "Account 1")%></h4>
                            <table id="tableAccount1" cellspacing="0" class="DataGrid EFTAccount" border="1">
                                <tr>
                                    <td style="width: 130px;">
                                        <label for="chkEnabledAccount1">
                                            <%= Html.Term("Enabled", "Enabled")%>:</label>
                                    </td>
                                    <td>
                                        <input id="chkEnabledAccount1" type="checkbox" checked="checked" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtNameAccount1">
                                            <%= Html.Term("Name on Account", "Name on Account")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtNameAccount1" type="text" disabled="disabled" name="<%= Html.Term("NameRequired", "Name is required") %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtBankNameAccount1">
                                            <%= Html.Term("BankName", "Bank Name")%>:</label>
                                    </td>
                                    <td>
                                        <input style="width: 80px;" class="BankInput"/>
                                        <select id="ddlBankNameAccount1" class="required">
                                            <option value="0">
                                                <%= Html.Term("termSelectbank", " Select Bank ")%></option>
                                            <% foreach (var items in TempData["getEBanks"] as List<GLDropdownlistUtilSearchData>)
                                               { %>
                                            <option value="<%=items.id %>" class="<%=items.Value %>">
                                                <%=items.Name%></option>
                                            <% } %>
                                        </select>
                                        <input id="txtBankNameAccount1" type="hidden" disabled="disabled" />
                                        <input type="hidden" id="txtBankNameAccount1Id" value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="accountTypeAccount1">
                                            <%= Html.Term("AccountType", "Account Type")%>:</label>
                                    </td>
                                    <td>
                                        <select id="accountTypeAccount1" disabled="disabled">
                                            <option value="<%= (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Checking  %>">
                                                Checking</option>
                                            <option value="<%= (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Savings  %>">
                                                Savings</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtAgencyAccount1">
                                            <%= Html.Term("Agency", "Agency")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtAgencyAccount1" type="text" disabled="disabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtAccountNumberAccount1">
                                            <%= Html.Term("Account#", "Account #")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtAccountNumberAccount1" type="text" disabled="disabled" name="<%= Html.Term("AccountNumberRequired", "Account Number is required") %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="percentToDepositAccount1">
                                            <%= Html.Term("%toDeposit", "% to Deposit")%>:</label>
                                    </td>
                                    <td>
                                        <input id="percentToDepositAccount1" type="text" class="percentToDeposit" maxlength="3"
                                            disabled="disabled" onkeypress="return isNumber(this,event)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding: 25px;">
                        </td>
                        <td>
                            <div id="divAccount2">
                            <h4 class="ModTitle">
                                <%= Html.Term("Account2", "Account 2")%></h4>
                            <table id="tableAccount2" cellspacing="0" class="DataGrid EFTAccount">
                                <tr>
                                    <td style="width: 130px;">
                                        <label for="chkEnabledAccount2">
                                            <%= Html.Term("Enabled", "Enabled")%>:</label>
                                    </td>
                                    <td>
                                        <input id="chkEnabledAccount2" type="checkbox" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtNameAccount2">
                                            <%= Html.Term("Name on Account", "Name on Account")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtNameAccount2" type="text" disabled="disabled" name="<%= Html.Term("NameRequired", "Name is required") %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtAccountNumberAccount2">
                                            <%= Html.Term("Account#", "Account #")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtAccountNumberAccount2" type="text" disabled="disabled" name="<%= Html.Term("AccountNumberRequired", "Account Number is required") %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtBankNameAccount2">
                                            <%= Html.Term("BankName", "Bank Name")%>:</label>
                                    </td>
                                    <td
                                        <input style="width: 80px;" class="BankInput"/>
                                        <select id="ddlBankNameAccount2">
                                            <option value="0">
                                                <%= Html.Term("termSelectbank", " Select Bank ")%></option>
                                            <% foreach (var items in TempData["getEBanks"] as List<GLDropdownlistUtilSearchData>)
                                               {%>
                                            <option value="<%=items.id %>" class="<%=items.Value %>">
                                                <%=items.Name%></option>
                                            <% }%>
                                        </select>
                                        <input id="txtBankNameAccount2" type="hidden" disabled="disabled" />
                                        <input type="hidden" id="txtBankNameAccount2Id" value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="txtAgencyAccount2">
                                            <%= Html.Term("Agency", "Agency")%>:</label>
                                    </td>
                                    <td>
                                        <input id="txtAgencyAccount2" type="text" disabled="disabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="accountTypeAccount2">
                                            <%= Html.Term("AccountType", "Account Type")%>:</label>
                                    </td>
                                    <td>
                                        <select id="accountTypeAccount2" disabled="disabled">
                                            <option value="<%= (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Checking  %>">
                                                Checking</option>
                                            <option value="<%= (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Savings  %>">
                                                Savings</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="requiredMarker">*</span>
                                        <label for="percentToDepositAccount2">
                                            <%= Html.Term("%toDeposit", "% to Deposit")%>:</label>
                                    </td>
                                    <td>
                                        <input id="percentToDepositAccount2" type="text" class="percentToDeposit" maxlength="3"
                                            disabled="disabled" onkeypress="return isNumber(this,event)" />
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="coApplicant" disabled="disabled">
            <h4>
                <span class="requiredMarker">* </span>
                <%= Html.Term("CoApplicant", "CoApplicant")%></h4>
            <div class="FauxTable">
                <div class="FRow">
                    <span class="FLabel">Include CoApplicant </span><span class="FInput">
                        <input id="chkCoApplicant" type="checkbox" />
                    </span>
                </div>
            </div>
            <table width="100%" cellspacing="0" class="FormTable">
                <tr>
                    <td class="FLabel">
                        <span class="requiredMarker">*</span>
                        <%= isEntity ? Html.Term("PrimaryApplicantName", "Primary Applicant Name") : Html.Term("CoApplicant","CoApplicant")%>:
                    </td>
                    <td>
                        <input type="text" id="firstNameCoApplicant" name="<%= Html.Term("FirstNameRequired", "First Name is required.") %>"
                            placeholder="First Name" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" id="middleNameCoApplicant" placeholder="Middle Name" />
                        <input type="text" id="lastNameCoApplicant" name="<%= Html.Term("LastNameRequired", "Last Name is required.") %>"
                            placeholder="Last Name" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 13.636em;">
                        <span class="requiredMarker">*</span>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("RelationShip", "RelationShip")%>:</label>
                    </td>
                    <td>
                        <%= @Html.DropDownRelationShip(htmlAttributes: new { id = "ddlRelationshipCoApplicant" })%>
                    </td>
                </tr>
                <%if (!isEntity)
                  { %>
                <tr>
                    <td class="FLabel">
                        <label for="gender">
                            <%= Html.Term("Gender") %>:</label>
                    </td>
                    <td>
                        <select id="genderCoApplicant">
                            <option value="0">
                                <%= Html.Term("Select", "Select") %></option>
                            <option value="Male">
                                <%= Html.Term("Male") %></option>
                            <option value="Female">
                                <%= Html.Term("Female") %></option>
                        </select>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td class="FLabel">
                        <span class="requiredMarker">*</span>
                        <%= isEntity ? Html.Term("PrimaryApplicantDateOfBirth", "Primary Applicant Date of Birth") : Html.Term("DateOfBirth", "Date of Birth")%>:
                    </td>
                    <td id="dateOfBirthCoApplicant">
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("PhoneNumbers", "Phone Numbers") %>:
                        <p class="InputTools">
                            <a id="btnAddPhoneCoApplicant" href="javascript:void(0);" class="AddPhone">
                                <%= Html.Term("AddaPhoneNumber", "Add a phone number") %></a>
                        </p>
                    </td>
                    <td>
                        <div id="phonesCoApplicant">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <span class="requiredMarker">*</span>
                        <%= Html.Term("CPF","CPF") %>:
                    </td>
                    <td id="CPFCoApplicant">
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("PIS","PIS") %>:
                    </td>
                    <td id="PISCoApplicant">
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <span class="requiredMarker">*</span>
                        <%= Html.Term("RG","RG") %>:
                    </td>
                    <td>
                        <input type="text" maxlength="100" id="RGCoApplicant" style="width: 250px;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("OrgExp","Org. Exp") %>:
                    </td>
                    <td>
                        <input type="text" id="OrgExpCoApplicant" style="width: 250px;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("IssueDate","Issue Date") %>:
                    </td>
                    <td id="issueDateCoApplicant">
                    </td>
                </tr>
            </table>
        </div>
        <%if (EnvironmentCountry != 73)
          { %>
        <h4>
            <%= Html.Term("BillingInformation", "Billing Information") %></h4>
        <div class="FauxTable">
            <div class="FRow">
                <span class="FLabel"><span class="requiredMarker">*</span>
                    <label for="nameOnCard">
                        <%= Html.Term("NameOnCard", "Name on Card") %>:</label></span> <span class="FInput">
                            <input type="text" maxlength="50" id="nameOnCard" name="<%= Html.Term("NameOnCardIsRequired", "Name On Card is required.") %>"
                                class="required" /></span>
            </div>
            <div class="FRow">
                <span class="FLabel"><span class="requiredMarker">*</span>
                    <label for="accountNumber">
                        <%= Html.Term("CreditCardNumber", "Credit Card #") %>:</label></span> <span class="FInput">
                            <input type="text" maxlength="16" id="accountNumber" name="<%= Html.Term("CardNumberIsRequired", "Card Number is required.") %>"
                                class="required" /></span>
            </div>
            <div class="FRow">
                <span class="FLabel"><span class="requiredMarker">*</span>
                    <%= Html.Term("Expiration") %>:</span> <span class="FInput">
                        <select id="expMonth" name="<%= Html.Term("ExpMonthIsRequired", "Exp Month is required.") %>"
                            title="0" class="required">
                            <% for (int i = 1; i <= 12; i++)
                               { %>
                            <option value="<%= i %>" <%= i == DateTime.Today.Month + 1 ? "selected=\"selected\"" : "" %>>
                                <%= i + " - " + Html.Term(Enum.ToObject(typeof(Constants.Month), i).ToString()) %></option>
                            <% } %>
                        </select>
                        /
                        <select id="expYear" name="<%= Html.Term("ExpYearIsRequired", "Exp Year is required.") %>"
                            title="" class="required">
                            <% for (int i = DateTime.Today.Year; i <= DateTime.Today.Year + 15; i++)
                               {%>
                            <option value="<%= i %>">
                                <%= i.ToString() %></option>
                            <% } %>
                        </select>
                    </span>
            </div>
            <div class="FRow">
                <span class="FLabel">
                    <%= Html.Term("UseMainAddress", "Use Main Address") %></span> <span class="FInput">
                        <input type="checkbox" id="chkUseMainForBilling" checked="checked" /></span>
            </div>
            <%}%>
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
          <% string valRC = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "VRF");
            if (valRC == "S")
                           {%>
        <div id="divReferenceToCredit">
            <h4>
                <%= Html.Term("ReferenceToCredit", "Reference to Credit")%></h4>
            <table width="100%" class="DataGrid" cellspacing="0">
                <tr>
                    <td style="width: 13.636em;">
                        <label for="txtReferenceName">
                            <%= Html.Term("ReferenceToCredit", "Reference To Credit")%>:</label>
                        <%--     <input type="hidden" id="AccountReferencesID" />--%>
                    </td>
                    <td>
                        <input type="text" id="txtReferenceName" style="width: 8.333em;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 13.636em;">
                        <label for="ddlRelationship">
                            <%= Html.Term("RelationShip", "RelationShip")%>:</label>
                    </td>
                    <td>
                        <%= @Html.DropDownRelationShip(htmlAttributes: new { id = "ddlRelationship" })%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 13.636em;">
                        <label for="txtComplementaryPhone">
                            <%= Html.Term("PhoneNumber", "Phone Number")%>:</label>
                    </td>
                    <td>
                        <input id="txtComplementaryPhone" maxlength="10" onkeypress="return isNumber(this,event)"
                            type="text" class="phoneInput" />
                    </td>
                </tr>
            </table>
        </div>
         <%}%>

        <div id="complementaryInfo">
            <h4>
                <%= Html.Term("ComplementaryInfo", "Complementary Info")%></h4>
            <table width="100%" class="DataGrid" cellspacing="0">
                <tr>
                    <td style="width: 13.636em;">
                        <label for="ddlSchoolinelevel">
                            <%= Html.Term("SchoolingLevel", "Scolarship")%>:</label>
                    </td>
                    <td>
                        <%= @Html.DropDownSchoolineLevel(htmlAttributes: new { id = "ddlSchoolinelevel" })%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <% string val = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "VCI");
                           if (val == "S")
                           {%>
                        <table>
                            <tr>
                                <td style="width: 13.636em;">
                                    <label for="ddlHasComputer">
                                        <%= Html.Term("Computer", "Computer")%>
                                        :</label>
                                </td>
                                <td>
                                    <select id="ddlHasComputer">
                                        <option value="1">
                                            <%= Html.Term("Yes") %></option>
                                        <option value="0">
                                            <%= Html.Term("No") %></option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13.636em;">
                                    <label for="txtHostedMailAccount">
                                        <%= Html.Term("InternetUse", "Internet Use")%>:</label>
                                </td>
                                <td>
                                    <%= @Html.DropDownInternetUse(htmlAttributes: new { id = "ddlInternetUse" })%>
                                </td>
                            </tr>
                        </table>
                        <%}%>
                    </td>
                </tr>
            </table>
            <%--@AINI--%>
            <hr />
            <table width="100%" class="DataGrid" cellspacing="0">
                <tr>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("LinkBlog", "Link Blog")%>:</label>
                        <br />
                        <input type="text" id="linkBlog" style="width: 250px;" />
                    </td>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("LinkOrkut", "Link Orkut")%>:</label>
                        <br />
                        <input type="text" id="linkOrkut" style="width: 250px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("LinkFacebook", "Link Faceboook")%>:</label>
                        <br />
                        <input type="text" id="linkFacebook" style="width: 250px;" />
                    </td>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("LinkTwitter", "Link Twitter")%>:</label>
                        <br />
                        <input type="text" id="linkTwitter" style="width: 250px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("EmailMsn", "E-mail MSN")%>:</label>
                        <br />
                        <input type="text" id="EmailMsn" style="width: 250px;" />
                    </td>
                    <td>
                        <label for="txtHostedMailAccount">
                            <%= Html.Term("LinkLinkedIn", "Link LinkedIn")%>:</label>
                        <br />
                        <input type="text" id="linkLinkedIn" style="width: 250px;" />
                    </td>
                </tr>
            </table>
            <%--@AFIN--%>
        </div>
    </div>
    <!-- /end step body -->
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
    <a id="btnNext" href="javascript:void(0);" class="Button BigBlue">
        <%= Html.Term("Next") %>&gt;&gt;</a>
</p>
<% Html.RenderPartial("AddressValidation"); %>