<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.CoApplicantSearchParameters>" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
<%@ Import Namespace="NetSteps.Data.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <% 
        var selectRelationships = NetSteps.Data.Entities.Business.Logic.AccountPropertiesBusinessLogic.GetRelationShip();
        string IssueDateStr = string.Empty;
        string DateOfBirthStr = string.Empty;
        
        if (Model != null)
        {
            if (Convert.ToDateTime(Model.IssueDate).Year > 1900)
            {
                IssueDateStr = Model.IssueDate.ToString("ddMMyyyy");
            }

            if (Convert.ToDateTime(Model.DateOfBirth).Year > 1900)
            {
                DateOfBirthStr = Model.DateOfBirth.ToString("ddMMyyyy");
            }
        }
    %>

    <style>
        
    #txtphoneNumberMobile1 {        
    max-width : 50px;
    }

    #txtphoneNumberMobile2 {
    max-width: 50px;
    }

    #txtphoneNumberMobile3 {
    max-width: 50px;
    }
        
    #txtCPFPart1 {
    max-width: 100px;
    }

    #txtCPFPart2 {
    max-width: 50px;
    }        
        
    #txtPISPart1 {
    max-width:100px;
    }

    #txtPISPart2 {
    max-width: 50px;
    }
       
    #txtDOBDay, #txtDOBMonth, #txtDOBYear, #txtIssueDateDay, #txtIssueDateMonth, #txtIssueDateYear {
    max-width: 80px;
    }        
        
    </style>

    <script type="text/javascript">

        var CoApplicant = JSON.parse('<%= Newtonsoft.Json.JsonConvert.SerializeObject(Model) %>'); ;

        //Ajax Validations: Global variables

        //Documents
        var validationReqCPF = 'CPF is Invalid.';
        var validationCPF = validationReqCPF;

        var validationReqPIS = 'PIS is Invalid.';
        var validationPIS = validationReqPIS;

        var validationReqRG = 'RG is Invalid.';
        var validationRG = validationReqRG;

        $(document).ready(function () {

            InitControls();
            
            var cultureInfo = '<%= CoreContext.CurrentCultureInfo%>';
            if (CoApplicant != null) {
                //ActivateReadOnly();
                LoadValues();
            }

            $('#txtRG').bind('keypress', function (e) {
                return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) ? false : true;
            }).on('blur', function () {
                var control = $(this);
                var isCoApplicant = false;

                if (control.val().length > 0) {
                    validationRG = '';
                    control.showError('');
                    //                    if (control.val() == CoApplicant.RG) {
                    //                        validationRG = '';
                    //                        control.showError('');
                    //                    }
                    //else
                    //DocumentValidation(4, control.val(), control, null, isCoApplicant);
                }
                else {
                    validationRG = validationReqRG;
                    control.showError(validationReqRG);
                }
            }).keydown(function () {
                $(this).clearError('');
            });

            $('#txtCPFPart1, #txtCPFPart2, #txtPISPart1, #txtPISPart2').keyup(function (e) {


                var controlID = $(this).attr('id');
                var validationMsg = '';
                var isCoApplicant = false;

                if (controlID.indexOf("CPF") >= 0) {
                    var cpfBaseID = '#' + controlID.substr(0, controlID.length - 1);
                    var cpfPart1 = $(cpfBaseID + '1');
                    var cpfPart2 = $(cpfBaseID + '2');

                    var valorComparativo1 = (cpfPart1.val().length == cpfPart1.prop('maxLength'));
                    var valorComparativo2 = cpfPart2.val().length == cpfPart2.prop('maxLength');

                    if (valorComparativo1 && valorComparativo2) {

                        var valorCPF = $('#CPF').inputsByFormat('getValue', '{0}{1}');

                        if (CoApplicant != null)
                            valorComparativo1 = (valorCPF == CoApplicant.CPF);
                        valorComparativo2 = (valorCPF.length == 11);

                        if (valorComparativo1 || valorComparativo2) {
                            validationCPF = '';
                            cpfPart1.clearError('');
                            cpfPart2.clearError('');

                        }
                        else
                            DocumentValidation(8, cpfPart1.val() + cpfPart2.val(), cpfPart1, cpfPart2, isCoApplicant);
                    }
                    else {

                        validationCPF = validationReqCPF;

                        cpfPart1.showError('');
                        cpfPart2.showError(validationReqCPF);
                    }
                }
                else if (controlID.indexOf("PIS") >= 0) {
                    var pisBaseID = '#' + controlID.substr(0, controlID.length - 1);
                    var pisPart1 = $(pisBaseID + '1');
                    var pisPart2 = $(pisBaseID + '2');

                    if (pisPart1.val().length > 0 || pisPart2.val().length > 0) {
                        if (pisPart1.val().length == pisPart1.prop('maxLength') &&
				            pisPart2.val().length == pisPart2.prop('maxLength')) {

                            if ($('#PIS').inputsByFormat('getValue', '{0}{1}') == CoApplicant.PIS) {
                                validationPIS = '';
                                pisPart1.clearError('');
                                pisPart2.clearError('');
                            }
                            else
                                DocumentValidation(9, pisPart1.val() + pisPart2.val(), pisPart1, pisPart2, isCoApplicant);
                        }
                        else {

                            validationPIS = validationReqPIS;

                            pisPart1.showError('');
                            pisPart2.showError(validationReqPIS);
                        }
                    }
                    else if (pisPart1.val().length == 0 && pisPart2.val().length == 0) {
                        validationPIS = '';
                        pisPart1.clearError('');
                        pisPart2.clearError('');
                    }
                }
            });

            $('#btnSaveCoApplicant').click(function () {

                if (ValidateFields()) {
                    SaveCoApplicant();
                }
            });

        });

        function InitControls(){
            $('#CPF').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtCPFPart1', length: 9, size: 9 }, { id: 'txtCPFPart2', length: 2, size: 2}] });
            $('#PIS').inputsByFormat({ format: '{0} - {1}', validateNumbers: true, attributes: [{ id: 'txtPISPart1', length: 9, size: 9 }, { id: 'txtPISPart2', length: 2, size: 2}] });

            
            if (cultureInfo === 'en-US') {
                $('#IssueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtIssueDateMonth', length: 2, size: 2 }, { id: 'txtIssueDateDay', length: 2, size: 2 }, { id: 'txtIssueDateYear', length: 4, size: 4}] });
                $('#DOB').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] });
            }
            else if ((cultureInfo === 'es-US') || (cultureInfo === 'pt-BR')) {
                $('#IssueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtIssueDateDay', length: 2, size: 2 }, { id: 'txtIssueDateMonth', length: 2, size: 2 }, { id: 'txtIssueDateYear', length: 4, size: 4}] });
                $('#DOB').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] });
            }
//            $('#IssueDate').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtIssueDateMonth', length: 2, size: 2 }, { id: 'txtIssueDateDay', length: 2, size: 2 }, { id: 'txtIssueDateYear', length: 4, size: 4}] });
            $('#txtIssueDateMonth').watermark('mm');
            $('#txtIssueDateDay').watermark('dd');
            $('#txtIssueDateYear').watermark('yyyy');

//            $('#DOB').inputsByFormat({ format: '{0} / {1} / {2}', validateNumbers: true, attributes: [{ id: 'txtDOBDay', length: 2, size: 2 }, { id: 'txtDOBMonth', length: 2, size: 2 }, { id: 'txtDOBYear', length: 4, size: 4}] });
            $('#txtDOBMonth').watermark('mm');
            $('#txtDOBDay').watermark('dd');
            $('#txtDOBYear').watermark('yyyy');

            $('#phoneNumberMobile').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtphoneNumberMobile1', length: 3, size: 3 }, { id: 'txtphoneNumberMobile2', length: 4, size: 4 }, { id: 'txtphoneNumberMobile3', length: 4, size: 4}] });
        }

        function LoadValues(){
            $('#txtFirstName').val(CoApplicant.FirstName);
            $('#txtLastName').val(CoApplicant.LastName);
            $('#ddlRelationship').val(CoApplicant.Relationship);
            $("input[name=rbGender][value=" + CoApplicant.Gender + "]").prop('checked', true);
            //$('#DOB').inputsByFormat('setValue', CoApplicant.DateOfBirth.split('/').join(''));
            $('#DOB').inputsByFormat('setValue', '<%=DateOfBirthStr%>');
            $('#CPF').inputsByFormat('setValue', CoApplicant.CPF);
            $('#PIS').inputsByFormat('setValue', CoApplicant.PIS);
            $('#txtRG').val(CoApplicant.RG);
            $('#txtOrgExp').val(CoApplicant.OrgExp);
            $('#IssueDate').inputsByFormat('setValue', '<%=IssueDateStr%>');

            if (CoApplicant.Phones != null) {
                for (var i = 0; i < CoApplicant.Phones.length; i++) {
                    if (CoApplicant.Phones[i].PhoneTypeID == 2) {
                        $('#phoneNumberMobile').inputsByFormat('setValue', CoApplicant.Phones[i].PhoneNumber);
                    }
                }
            }          
        }

        function ActivateReadOnly(){
            $('#btnSaveCoApplicant').hide();
            $('input, select').prop('disabled', true);
        }

        function ValidateFields(){
            var result = true;
            
            /*FirstName*/
            var txtFirstName = $('#txtFirstName');
            if ($.trim(txtFirstName.val()) == ''){
                txtFirstName.showError('<%= Html.Term("FirstNameReq", "First Name is Required") %>');
                reuslt = false;
            }
            else{
                txtFirstName.clearError();
            }

            /*LastName*/
            var txtLastName = $('#txtLastName');
            if ($.trim(txtLastName.val()) == ''){
                txtLastName.showError('<%= Html.Term("LastNameReq", "Last Name is Required") %>');
                reuslt = false;
            }
            else{
                txtLastName.clearError();
            }

            /*Relationship*/
            var ddlRelationship = $('#ddlRelationship');
            if (ddlRelationship.val() == '' || ddlRelationship.val() == 0){
                ddlRelationship.showError('<%= Html.Term("RelationshipReq", "Relationship is Required") %>');
                reuslt = false;
            }
            else{
                ddlRelationship.clearError();
            }

            /*Gender*/
            var divGender = $('#divGender');

            if ($('input:radio[name="rbGender"]:checked').val() == undefined){
                divGender.showError('<%= Html.Term("GenderReq", "Gender is Required") %>');
                result = false;
            }
            else{
                divGender.clearError();
            }

            /*DOB*/

            var txtDOBDay = $("#txtDOBDay");
            var txtDOBMonth = $("#txtDOBMonth");
            var txtDOBYear = $("#txtDOBYear");
            var isValidDateDOB = CheckValidDate(txtDOBDay.val(), txtDOBMonth.val(), txtDOBYear.val());

            if (!isValidDateDOB) {
                result = false;
                txtDOBDay.showError("");
                txtDOBMonth.showError("");
                txtDOBYear.showError('<%= Html.Term("InvalidDOB","Please enter a valid DOB") %>');
            }
            else {
                
                var isValidAge = CheckValidAge($("#txtDOBDay").val(), $("#txtDOBMonth").val(), $("#txtDOBYear").val());

                if (!isValidAge) {
                    result = false;
                    txtDOBDay.showError("");
                    txtDOBMonth.showError("");
                    txtDOBYear.showError('<%= Html.Term("DOBInValidYear","Age should be greater than 18") %>');
                }
                else{
                    txtDOBDay.clearError();
                    txtDOBMonth.clearError();
                    txtDOBYear.clearError();
                }
            }

            var validateValue = null;

            /*CPF*/

            validateValue = CoApplicant != null ? CoApplicant.CPF : null;

            var valorCPF = $('#CPF').inputsByFormat('getValue', '{0}{1}');
            if (valorCPF == validateValue || valorCPF.length == 11) {
                validationCPF = '';
            }
            
            var txtCPFPart1 = $('#txtCPFPart1');
            var txtCPFPart2 = $('#txtCPFPart2');

            if (validationCPF != ''){
                result = false;
                txtCPFPart1.showError('');
                txtCPFPart2.showError(validationCPF);
            }
            else{
                txtCPFPart1.clearError();
                txtCPFPart2.clearError();
            }

            /*PIS*/
            validateValue = CoApplicant != null ? CoApplicant.PIS : null;

            var valorPIS = $('#PIS').inputsByFormat('getValue', '{0}{1}');
            if (valorPIS == validateValue || valorPIS.length == 11)
                validationPIS = '';

            var txtPISPart1 = $('#txtPISPart1');
            var txtPISPart2 = $('#txtPISPart2');

            if (validationPIS != ''){
                result = false;
                txtPISPart1.showError('');
                txtPISPart2.showError(validationPIS);
            }
            else{
                txtPISPart1.clearError();
                txtPISPart2.clearError();
            }

            /*RG*/
            validateValue = CoApplicant != null ? CoApplicant.RG : null;
            var valorRG = $('#txtRG').val();
            //if (valorRG == validateValue || valorRG.length == 9) {
            if (valorRG.length > 0) {
                validationRG = '';
            }
            var txtRG = $('#txtRG');
            if (validationRG != ''){
                result = false;
                txtRG.showError(validationRG);
            }
            else{
                txtRG.clearError();
            }

            /*Org. Exp.*/
            var txtOrgExp = $('#txtOrgExp');
//            if ($.trim(txtOrgExp.val()) == ''){
//                txtOrgExp.showError('<%= Html.Term("OrgExpReq", "Org. Exp. is Required") %>');
//                reuslt = false;
//            }
//            else{
//                txtOrgExp.clearError();
//            }

            /*Issue Date*/

            var txtIssueDateDay = $("#txtIssueDateDay");
            var txtIssueDateMonth = $("#txtIssueDateMonth");
            var txtIssueDateYear = $("#txtIssueDateYear");
            var isValidDateIssueDate = CheckValidDate(txtIssueDateDay.val(), txtIssueDateMonth.val(), txtIssueDateYear.val());

            var longitudTotalIssue = txtIssueDateDay.val().length + txtIssueDateMonth.val().length + txtIssueDateYear.val().length;

            if (longitudTotalIssue > 0) {
                if (!isValidDateIssueDate) {
                    result = false;
                    txtIssueDateDay.showError("");
                    txtIssueDateMonth.showError("");
                    txtIssueDateYear.showError('<%= Html.Term("InvalidIssueDate","Please enter a valid Issue Date") %>');
                }
                else {

                    var today = new Date();
                    var issueDate = new Date(txtIssueDateYear.val(), txtIssueDateMonth.val() - 1, txtIssueDateDay.val());

                    if (issueDate > today) {
                        result = false;
                        txtIssueDateDay.showError("");
                        txtIssueDateMonth.showError("");
                        txtIssueDateYear.showError('<%= Html.Term("InvalidIssueDate2","Issue Date must not be greater than today.") %>');
                    }
                    else {
                        txtIssueDateDay.clearError();
                        txtIssueDateMonth.clearError();
                        txtIssueDateYear.clearError();
                    }
                }
            }
            

            /*Phone Number*/

//            var txtphoneNumberMobile1 = $("#txtphoneNumberMobile1");
//            var txtphoneNumberMobile2 = $("#txtphoneNumberMobile2");
//            var txtphoneNumberMobile3 = $("#txtphoneNumberMobile3");
//            var totalSize = txtphoneNumberMobile1.prop('maxLength') + txtphoneNumberMobile2.prop('maxLength') + txtphoneNumberMobile3.prop('maxLength');
//            var currentPhoneNumber = $('#phoneNumberMobile').inputsByFormat('getValue', '{0}{1}{2}');

//            if (currentPhoneNumber.length < totalSize){
//                result = false;
//                txtphoneNumberMobile1.showError("");
//                txtphoneNumberMobile2.showError("");
//                txtphoneNumberMobile3.showError('<%= Html.Term("InvalidPhoneNumber","Please enter a valid Phone Number") %>');
//            }
//            else{
//                txtphoneNumberMobile1.clearError();
//                txtphoneNumberMobile2.clearError();
//                txtphoneNumberMobile3.clearError();
//            }

            return result;
        }

        function SaveCoApplicant(){
            
            $('#btnSaveCoApplicant').hide();

            var MobilePhone = {
                PhoneTypeID: 2,
                PhoneNumber: $('#phoneNumberMobile').inputsByFormat('getValue', '{0}{1}{2}'),
                IsDefault: false   
            };

            var NewCoApplicant = {
                FirstName: $('#txtFirstName').val(),
                LastName: $('#txtLastName').val(),
                Relationship: $('#ddlRelationship').val(),
                Gender: $('input:radio[name="rbGender"]:checked').val(),
                DateOfBirth: $('#DOB').inputsByFormat('getValue', '{0}/{1}/{2}'),
                CPF: $('#CPF').inputsByFormat('getValue', '{0}{1}'),
                PIS: $('#PIS').inputsByFormat('getValue', '{0}{1}'),
                RG: $('#txtRG').val(),
                OrgExp: $('#txtOrgExp').val(),
                IssueDate: $('#IssueDate').inputsByFormat('getValue', '{0}/{1}/{2}'),
                Phones: []
            };

            NewCoApplicant.Phones.push(MobilePhone);

            var strURL = '<%= ResolveUrl("~/EditCoApplicant/SaveCoApplicant") %>';

            if (CoApplicant != null) {
                strURL = '<%= ResolveUrl("~/EditCoApplicant/UpdateCoApplicant") %>';
            }


            $.ajax({
                type: 'POST',
                url: strURL,
                data: JSON.stringify(NewCoApplicant),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    showMessage(response.message, !response.result);
                    CoApplicant = response.CoApplicant;
                    $('#btnSaveCoApplicant').show();
                }
            });
        }

        function CheckValidDate(dayfield, monthfield, yearfield) {
            var dayobj = new Date(yearfield, monthfield - 1, dayfield)
            if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
                return false;
            else
                return true;
        }

        function CheckValidAge(dayfield, monthfield, yearfield) {
            var age = 18;
            var isAgeValid = false;
            var mydate = new Date();
            var passedDate = new Date(yearfield, monthfield - 1, dayfield)
            var minRequiredCurrdate = new Date();
            minRequiredCurrdate.setFullYear(minRequiredCurrdate.getFullYear() - age);

            isAgeValid = passedDate <= minRequiredCurrdate;
            return isAgeValid;
        }

        function DocumentValidation(DocumentType, DocumentValue, Control1, Control2, isCoApplicant){
            Control1.clearError();
            if (Control2 != null)
                Control2.clearError();

            var strURL = '<%= ResolveUrl("~/EditCoApplicant/DocumentValidation") %>';
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
                            validationCPF = data.message;
                            break;
                        case 9: //PIS
                            validationPIS = data.message;
                            break;
                        case 4: //RGs
                            validationRG = data.message;
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
    </script>

    <div class="SectionHeader">
        <h2><%= Html.Term("EditCoApplicant", "Edit CoApplicant")%></h2>
    </div>

    <table width="100%" class="DataGrid" cellspacing="0">
        <tbody>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("First Name") %>:
                </td>
                <td>
                    <input type="text" id="txtFirstName" class="required" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Last Name") %>:
                </td>
                <td>
                    <input type="text" id="txtLastName" class="required" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Relationship") %>:
                </td>
                <td>
                    <%= Html.DropDownList("ddlRelationship", new SelectList(selectRelationships, "Key", "Value"))%>  
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Gender") %>:
                </td>
                <td>
                    <div id="divGender" class="data" style="width: 150px;">
                        <input type="radio" name="rbGender" value="1" />Male
                        <input type="radio" name="rbGender" value="2" />Female
                    </div>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("DateOfBirth", "DOB") %>:
                </td>
                <td>
                    <div class="data" id="DOB">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("CPF") %>:
                </td>
                <td>
                    <div id="CPF">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("PIS") %>:
                </td>
                <td>
                    <div id="PIS">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("RG") %>:
                </td>
                <td>
                    <input type="text" id="txtRG" style="width:100px"  />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Org. Exp") %>:
                </td>
                <td>
                    <input type="text" id="txtOrgExp"  class="required" style="width:100px" />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("RG issue date") %>:
                </td>
                <td>
                    <div id="IssueDate">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Phone Number (Mobile)") %>:
                </td>
                <td>
                    <div id="phoneNumberMobile">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>

    <div style="float: left;">
        <p>
            <a id="btnSaveCoApplicant" href="javascript:void(0);" class="Button BigBlue"> <%= Html.Term("Save") %></a> 
            <a id="btnCancel" href="<%= ResolveUrl("~/Accounts/Overview") %>" class="Button"><%= Html.Term("Cancel") %></a>
        </p>
    </div>
</asp:Content>
