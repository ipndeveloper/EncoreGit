<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Commissions.Models.ManualBonusEntryModel>" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
<%@ Import Namespace="NetSteps.Data.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">


<%
    string srcOK = Url.Content("~/Content/Images/Icon_Success.png");
    string altOK = Html.Term("Valid", "Valid");
    string srcError = Url.Content("~/Content/Images/Icon_Error.png");
    string altError = Html.Term("Invalid", "Invalid");
    string srcDuplicated = Url.Content("~/Content/Images/exclamation.png");
    string altDuplicated = Html.Term("Duplicated", "Duplicated");
        
    bool showErrors = false;
        
    if (Model.Errors != null && Model.Errors.Count > 0)
        showErrors = true;
            
    %>

<style>
    .icon
    {
        width: 20px;
        height: 20px;
    }

    table
    {
        margin-left: 20px;
        margin-bottom: 20px;
    }
    
    .DataGrid
    {
        width: 660px;
        border: 1px solid white;
    }
    
    .DataGrid tbody,
    .DataGrid thead { display: block; }

    .DataGrid thead tr th { 
        height: 30px;
        line-height: 30px;
    }

    .DataGrid tbody {
        height: 500px;
        overflow-y: auto;
        overflow-x: hidden;
    }

    tbody td:last-child, thead th:last-child {
        border-right: none;
    }
        
    .DataGrid thead tr td
    {
        width: 100px;
        background: #5A5A5A;
        color: #FFFFFF;
        vertical-align: middle;
        text-align: center;
    }
        
    .DataGrid tbody tr:nth-child(even) 
    {
        background: #FFFFFF;
    }
        
    .DataGrid tbody tr:nth-child(odd) 
    {
        background: #EFEFEF;
    }
        
    .DataGrid tbody tr td
    {
        width: 100px;
        vertical-align: middle;
        text-align: center;
        font-weight:bold;
    }
                
</style>

<script type="text/javascript">

    var _validFileExtensions = [".xls", ".xlsx"];

    $(function () {

        $(window).load(function () {
            var IsValid = '<%= Model.IsValid %>';

            if (IsValid == 'True' && confirm('<%= Html.JavascriptTerm("ConfirmManualBonus", "Are you sure you want to load the bonuses?") %>')) {
                Load();
            }
            else {
                var message = '<%= string.IsNullOrEmpty(Model.Message) ? string.Empty : Model.Message.Replace(System.Environment.NewLine, " ") %>';

                if (message != '')
                    showMessage(message, true);
            }
        });

        $('#upload_link').on('click', function (e) {
            e.preventDefault();
            $('#upload:hidden').trigger('click');
        });

        $('#upload').on('change', function () {
            hideMessage();
            $('#uploadFile').val(this.value);
        });

        $('#template').on('click', function (e) {
            window.open('<%=ResolveUrl("~/Commissions/ManualBonusEntry/DownloadTemplate")%>', '_blank');
        });

        $('#proccess').on('click', function (e) {
            e.preventDefault();

            if ($('#upload').val() == '') {
                showMessage('<%= Html.JavascriptTerm("SelectFile", "Select a File.") %>', true);
            }
            else if (ValidateExtension(document.getElementById('ManualBonusForm'))) {
                $('#ManualBonusForm').submit();
            }

        });
    });

    function ValidateExtension(oForm) {
        hideMessage();

        var arrInputs = oForm.getElementsByTagName("input");
        for (var i = 0; i < arrInputs.length; i++) {
            var oInput = arrInputs[i];
            if (oInput.type == "file") {
                var sFileName = oInput.value;
                if (sFileName.length > 0) {
                    var blnValid = false;
                    for (var j = 0; j < _validFileExtensions.length; j++) {
                        var sCurExtension = _validFileExtensions[j];
                        if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                            blnValid = true;
                            break;
                        }
                    }

                    if (!blnValid) {
                        var message = sFileName + ' ' + '<%= Html.JavascriptTerm("InvalidFile", "is an invalid file") %>' + '. ' + '<%= Html.JavascriptTerm("AllowedExtensions", "Allowed extensions are: ") %>' + ' ' + _validFileExtensions.join(", ") + '.';
                        showMessage(message, true);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    function Load() {
        var strURL = '<%= ResolveUrl("~/ManualBonusEntry/Load") %>';

        $.ajax({
            type: 'GET',
            url: strURL,
            dataType: 'json',
            success: function (response) {
                showMessage(response.message, !response.result);
            }
        });
    }

</script>

<form id="ManualBonusForm" method="post" action="<%= Url.Action("Proccess", "ManualBonusEntry") %>" enctype="multipart/form-data">
    
    <div class="SectionHeader">
        <h2><%= Html.Term("BonusPerPeriod", "Manual Bonus")%></h2>
    </div>

    <table>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Period", "Period") %>:
            </td>
            <td>
                <span style="font-weight: bold;"><%= Model.OpenPeriod %></span>
            </td>
            <td rowspan="2" style="vertical-align:middle; text-align:center; padding-left: 20px;">
                <a id="template" href="" class="Button BigBlue"><%= Html.Term("DownloadTemplate", "Download Template") %></a>​
            </td>
            <td rowspan="2" style="vertical-align:middle; text-align:center; padding-left: 5px;">
                <a id="proccess" href="" class="Button BigBlue"><%= Html.Term("Proccess") %></a>​
            </td>
        </tr>
        <tr> 
            <td class="FLabel">
                <%= Html.Term("FilePath", "File Path") %>
            </td>
            <td>
                <input id="uploadFile" placeholder="<%= Html.Term("ChooseFile", "Choose a File") %>" disabled="disabled" style="width:300px;" />
                <input id="upload" type="file" name="file" style="display: none;"/>
                <a href="" id="upload_link" class="Button"><%= Html.Term("File") %></a>​
            </td>
        </tr>
    </table>

    <table id="Errors" style='display: <%= showErrors ? "block": "none" %>' class="DataGrid" >
        <thead>
            <tr>
                <td>
                    <%= Html.Term("RowNumber", "Row Number") %>     
                </td>
                <td>
                    <%= Html.Term("Period", "Period") %>     
                </td>
                <td>
                    <%= Html.Term("BonusType", "Bonus Type") %>     
                </td>
                <td>
                    <%= Html.Term("Account", "Account") %>     
                </td>
                <td>
                    <%= Html.Term("BonusAmount", "Bonus Amount") %>     
                </td>
                <td>
                    <%= Html.Term("Duplicated", "Duplicated") %>     
                </td>
            </tr>
        </thead>
        <tbody>
            <% if (showErrors)
               {
                   foreach (var error in Model.Errors)
                   {     
            %>
                <tr>
                    <td>
                        <%=error.RowNumber%>     
                    </td>
                    <td>
                        <img src="<%= error.Period ? srcOK : srcError %>" alt="<%= error.Period ? altOK : altError %>" title="<%= error.Period ? altOK : altError %>" class="icon" />
                    </td>
                    <td>
                        <img src="<%= error.BonusType ? srcOK : srcError %>" alt="<%= error.BonusType ? altOK : altError %>" title="<%= error.BonusType ? altOK : altError %>" class="icon" />       
                    </td>
                    <td>
                        <img src="<%= error.Account ? srcOK : srcError %>" alt="<%= error.Account ? altOK : altError %>" title="<%= error.Account ? altOK : altError %>" class="icon" />   
                    </td>
                    <td>
                        <img src="<%= error.BonusAmount ? srcOK : srcError %>" alt="<%= error.BonusAmount ? altOK : altError %>" title="<%= error.BonusAmount ? altOK : altError %>" class="icon" />     
                    </td>
                    <td>
                        <% if (error.Duplicated){ %>
                            <img src="<%=srcDuplicated%>" alt="<%=altDuplicated%>" title="<%=altDuplicated%>" class="icon" />     
                        <%} %>
                    </td>
                </tr>
            <%      }
               } 
            %>
        </tbody>
    </table>

</form>  

</asp:Content>
