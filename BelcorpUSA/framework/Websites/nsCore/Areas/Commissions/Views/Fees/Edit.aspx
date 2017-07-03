
<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.SearchData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
    
        FeeSearchData editDisburmentFee = ViewBag.EditDisburmentFee;
        int disbursementFeeID = 0;
        string disbursementFeeType = "", disbursementType = "", currencyName = "", amount = "";
        if (editDisburmentFee != null)
        {
            disbursementFeeID = editDisburmentFee.DisbursementFeeID;
            disbursementFeeType = editDisburmentFee.FeeType;
            disbursementType = editDisburmentFee.DisbursementType;
            currencyName = editDisburmentFee.CurrencyName;
            amount = editDisburmentFee.Amount.ToString();
        }
        
    %>
    <div class="SectionHeader">
        <h2>
            <% if (disbursementFeeID == 0)
               { %>
            <%= Html.Term("AddNewFee", "Add new fee")%>
            <%}
               else
               { %>
               <%= Html.Term("EditFee", "Edit fee")%>
            <%} %>
        </h2>
    </div>
    <input type="hidden" id="DisbursementFeeID" value="<%= disbursementFeeID %>" />
    <table id="feeProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("DisbursementFeeType", "Disbursement Fee Type")%>:
            </td>
            <td>
                <select id="DisbursementFeeTypes">
                    <option value="0">
                        <%= Html.Term("SelectDisbursementFeeType", "Select Disbursement Fee Type") %></option>
                    <% if (ViewBag.DisbursementFeeTypes != null)
                       {
                           foreach (MinimumSearchData item in ViewBag.DisbursementFeeTypes)
                           {
                               if (item.Name == disbursementFeeType)
                               {
                    %>
                    <option value="<%= item.ID %>" selected="selected">
                        <%= item.Name%></option>
                    <%}
                           else
                           { %>
                    <option value="<%= item.ID %>">
                        <%= item.Name%></option>
                    <%} %>
                    <%}
                       } %>
                </select>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("DisbursementType", "Disbursement Type")%>:
            </td>
            <td>
                <select id="DisbursementTypes">
                    <option value="0">
                        <%= Html.Term("SelectDisbursementType", "Select Disbursement Type") %></option>
                    <% if (ViewBag.DisbursementTypes != null)
                       {
                           foreach (MinimumSearchData item in ViewBag.DisbursementTypes)
                           {
                               if (item.Name == disbursementType)
                               {
                    %>
                    <option value="<%= item.ID %>" selected="selected">
                        <%= item.Name%></option>
                    <%}
                           else
                           { %>
                    <option value="<%= item.ID %>">
                        <%= item.Name%></option>
                    <%} %>
                    <%}
                       } %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Currency")%>:
            </td>
            <td>
                <select id="Currencies">
                    <option value="0">
                        <%= Html.Term("SelectCurrencies", "Select Currency") %></option>
                    <% if (ViewBag.Currencies != null)
                       {
                           foreach (MinimumSearchData item in ViewBag.Currencies)
                           {
                               if (item.Name == currencyName)
                               {
                    %>
                    <option value="<%= item.ID %>" selected="selected">
                        <%= item.Name%></option>
                    <%}
                           else
                           { %>
                    <option value="<%= item.ID %>">
                        <%= item.Name%></option>
                    <%} %>
                    <%}
                       } %>
                </select>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Amount")%>:
            </td>
            <td>
                <input type="text" id="Amount" value="<%= amount %>"  monedaidioma='CultureIPN'/>
            </td>
        </tr>
    </table>
    <p>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
            <%= Html.Term("Save") %></a> <a href="<%= ResolveUrl("~/Commissions/Fees/Index") %>" class="Button">
                <span>
                    <%= Html.Term("Cancel") %></span></a>
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(document).ready(function () {

        $("#Amount").numeric({ allowNegative: false });

        $('input[monedaidioma=CultureIPN]').keyup(function (event) {

            var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
            // var value = parseFloat($(this).val());


            var formatDecimal = '$1.$2'; // valores por defaul 
            var formatMiles = ",";  // valores por defaul

            if (cultureInfo === 'en-US') {
                 formatDecimal = '$1.$2';
                 formatMiles = ",";
            }
            else if (cultureInfo === 'es-US') {
                 formatDecimal = '$1,$2';
                 formatMiles = ".";
            }
            else if (cultureInfo === 'pt-BR') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }


            //            if (!isNaN(value)) {
            if (event.which >= 37 && event.which <= 40) {
                event.preventDefault();
            }

            $(this).val(function (index, value) {


                return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
            });

            //            }

        });



        $("#btnSave").on("click", function () {

            if ($("#DisbursementFeeTypes").val() == '0') {
                showMessage('<%= Html.Term("SelectDisbursementFeeType", "Select Disbursement Fee Type") %>', true);
                return false;
            } else if ($("#DisbursementTypes").val() == '0') {
                showMessage('<%= Html.Term("SelectDisbursementType", "Select Disbursement Type") %>', true);
                return false;
            } else if ($("#Currencies").val() == '0') {
                showMessage('<%= Html.Term("SelectCurrencies", "Select Currency") %>', true);
                return false;
            } else if ($("#Amount").val() == '') {
                showMessage('<%= Html.Term("PleaseEnterAnAmount", "Please enter an amount") %>', true);
                return false;
            } else if ($("#Amount").val() == '0') {
                showMessage('<%= Html.Term("AmountMustBeGreaterThanZero", "The amount must be greater than zero") %>', true);
                return false;
            }

            var data = {
                DisbursementFeeID: $("#DisbursementFeeID").val(),
                DisbursementFeeTypeID: $("#DisbursementFeeTypes").val(),
                DisbursementTypeID: $("#DisbursementTypes").val(),
                CurrencyID: $("#Currencies").val(),
                Amount: $("#Amount").val().replace('.', ',')
            };

            var t = $(this);
            showLoading(t);
            $.post('<%= ResolveUrl("~/Commissions/Fees/Save") %>', data, function (response) {
                showMessage(response.message || '<%= Html.Term("FeeSaved", "Fee saved successfully!") %>', !response.result);
                if (response.result) {
                    $("#DisbursementFeeID").val(response.disbursementFeeID);
                }
            })
			.fail(function () {
			    showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
			})
			.always(function () {
			    hideLoading(t);
			});

        });
    });
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Commissions") %>">
        <%= Html.Term("MLM") %></a> > <a href="<%= ResolveUrl("~/Commissions/Fees/Index") %>">
            <%= Html.Term("Fees") %></a> >
    <%= Html.Term("AddNewFee", "Add new Fee") %>
</asp:Content>
