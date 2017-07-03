<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        $(function () {


            //            $('#ddlInvoiceNumbers').change(function () {
            //                LoadPDFReport(this.value);
            //            });

            $('#addMovementModel_1').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            $('#addMovementModel_2').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            $('#addMovementModel_3').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });



            $('.ShowPopup').live('click', function () {
                var key = $(this).attr('id');
                var splitResult = key.split('_');
                var type = splitResult[0];
                switch (true) {

                    case (type == "Invoice"):
                        $('#addMovementModel_1').jqmShow();
                        break;
                    case (type == "PartiallyPaid"):
                        $('#addMovementModel_2').jqmShow();
                        break;
                    case (type == "Shipped"):
                        $('#addMovementModel_3').jqmShow();
                        break;
                }

                //alert('this is a live function');
            });
        });

        function LoadPDFReport() {

            //                var result = '@Html.ActionLink("Download Temp PDF", "LoadPDFReport", new { InvoiceNumber = '+ invoiceNumber +' })';

            var dropdown = $('#ddlInvoiceNumbers');

            // get the value
            var invoiceNumber = dropdown.val();

            if (invoiceNumber && invoiceNumber != '') {

                var url = '<%= ResolveUrl("~/Orders/Details/LoadPDFReport") %>';

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify({ InvoiceNumber: invoiceNumber }),
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function (data) {
                        if (data.flag) {
                            $("#frmPDF").attr("src", data.url);
                        }
                        else {
                            alert("Error");
                        }
                    }
                });

            }
            else {
                alert("Select Invoice Number Please");
            }

            
        }

    </script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Invoices", "Invoices")%></h2>
		    <%= ViewData["Links"] %>
	</div>
    
    <br />

    

<table>
    <tr>
        <td style="padding: 10px;">
           <h4> <%= Html.Term("Select Invoice Number:", "Select Invoice Number:")%> </h4>
        </td>
        <td style="padding: 10px;">
            <%: Html.DropDownList("ddlInvoiceNumbers", TempData["InvoiceNumbers"] as IEnumerable<SelectListItem>, "Select One")%>
        </td>
    </tr>
    <tr>
        <td style="padding: 10px;">
            <input onclick="LoadPDFReport()" id="btnProcesar" class="Button BigBlue" type="button"
                    value="<%=Html.Term("Generate", "Generate") %>" />
        </td>
    </tr>
    <tr>
        <td>
            <div>
               <iframe name="frmPDF" id="frmPDF" style="display: none" src=""></iframe>
            </div>
        </td>
        <td>
        </td>
    </tr>
</table>

</asp:Content>



