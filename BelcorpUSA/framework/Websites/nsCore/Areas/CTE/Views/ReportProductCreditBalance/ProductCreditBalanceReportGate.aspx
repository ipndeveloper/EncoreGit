<%@ Page Title="" Language="C#"  MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseProductCreditBalanceReport", "Product Credit Balance Report")%>
        </h2>
	</div>
<script type="text/javascript">
    $(function () {
        $('#exportToExcel').click(function () {
            window.location = '<%= ResolveUrl("~/CTE/ReportProductCreditBalance/ProductCreditBalanceExport") %>';
        });

        var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />');
        $('#accountNumberOrNameInputFilter').change(function () {
            accountId.val('');
        });
        var url = '<%= ResolveUrl("~/Accounts/Search") %>';
     

            $('#accountNumberOrNameInputFilter').removeClass('Filter').after(accountId).css('width', '275px')

            .watermark('<%=Html.JavascriptTerm("AccountSearch", "Look up account by ID or name")%>')
            .jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {

            accountId.val(item.id);
            }, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
            });

    $('#credit_BalanceIniInputFilter,#credit_BalanceFinInputFilter').keyup(function (event) {

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

//        $.datepicker.regional['es'] = {
//            closeText: 'Cerrar',
//            prevText: '<Ant',
//            nextText: 'Sig>',
//            currentText: 'Hoy',
//            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
//            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
//            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
//            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
//            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
//            weekHeader: 'Sm',
//            dateFormat: 'dd/mm/yy',
//            firstDay: 1,
//            isRTL: false,
//            showMonthAfterYear: false,
//            yearSuffix: ''
//        };

//        $.datepicker.setDefaults($.datepicker.regional['es']);


    });
</script>



<% Html.PaginatedGrid<ProductCreditBalanceSearchData>("~/CTE/ReportProductCreditBalance/GetProductCreditBalanceReport")
        .AutoGenerateColumns()
        //.AddColumn(Html.Term("Description"), "EntryDescription", false)     
        //.HideClientSpecificColumns_()
          .AddSelectFilter(Html.Term("State", "State"), "State",
           new Dictionary<string, string>() {
                                                { " ", Html.Term("All", "All") },
                                                { "=", Html.Term("Balance Zero", "Balance Zero") },                                  
                                                { ">", Html.Term("BalancePositive", "Balance Positive")}  , 
                                                { "<", Html.Term("BalanceAgainst", "Balance Against")}                                                         
                                           })   
        .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountNumberOrName")
        .AddInputFilter(Html.Term("Credit_BalanceIni1", "Credit Balance --> Min"), "credit_BalanceIni")
        .AddInputFilter(Html.Term("Credit_BalanceFin1", "--> Max"), "credit_BalanceFin")
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", null, true, false)
        .AddInputFilter(Html.Term("To", "To"), "endDate", null, true, false)
           
                        
         
        
                
        .ClickEntireRow()
        .Render();
%>

</asp:Content>

