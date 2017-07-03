<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {

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
    </script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Tracking", "Tracking")%></h2>
		    <%= ViewData["Links"] %>
	</div>
    
    <br />

    <% Html.RenderPartial("PartialOrderInformation", new nsCore.Areas.Orders.Models.Details.PartialOrderInformationModel(Model, false)); %>

    <br />
   
        <h1>  
        <p>
        <strong>
        <%--
        <%= Html.Term("Order","Order") %>&nbsp; : &nbsp;<%=ViewData["OrderNumber"]%> <br />--%>
        <%= Html.Term("ExpectedDeliveryDate", "Expected Delivery Date")%>&nbsp; : &nbsp;<%=ViewData["DeliveryDateUTC"]%> 
       </strong>
        </p>
        </h1>
    <br />

     <% 
         Html.PaginatedGrid("~/Orders/Details/TrackingDetail/" + ViewData["OrderNumber"],"Grid_1")
        .AddColumn(Html.Term("Pointer", "Pointer"), "FinalTackingDateUTC", true, true, Constants.SortDirection.Ascending)
        .AddColumn(Html.Term("NStatus", "N° Status"), "OrderStatusID", true)
        .AddColumn(Html.Term("IconStatus", "Icon Status"), "FinalTackingDateUTC", true)
        .AddColumn(Html.Term("SituationOrderState", "Situation(Order State)"), "Name", true)
        .AddColumn(Html.Term("StartDateHour","Start Date/Hour"),"InitialTackingDateUTC",true)
        .AddColumn(Html.Term("EndDateHour", "End Date/Hour"), "FinalTackingDateUTC", true)
        .AddColumn(Html.Term("Comment", "Comment"), "Comment", true)
        .Render();
     %>

     
    <br />
    <div id="addMovementModel_1" class="jqmWindow LModal Overrides" style="width: 750px; height: 600px;">
        <div class="mContent" style="width: 730px; height: 580px;">          
            <% 
                Html.PaginatedGrid("~/Orders/Details/ListOrderStatusInvoice/" + ViewData["OrderNumber"], "Grid_4")
                .AddColumn(Html.Term("InvoiceNumber", "Invoice Number"), "InvoiceNumber", true, true, Constants.SortDirection.Ascending)
                .AddColumn(Html.Term("DateInvoice", "Date Invoice"), "DateInvoice", true)
                .Render();
             %>
            <br />
            <p>                
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
        </div>
    </div>
    <div id="addMovementModel_2" class="jqmWindow LModal Overrides" style="width: 750px; height: 600px;" >
        <div class="mContent" style="width: 730px; height: 580px;">          
           <%                
               Html.PaginatedGrid<OrderStatusPartiallyPaidSearchData>("~/Orders/Details/ListOrderStatusPartiallyPaid/" + ViewData["OrderNumber"], "Grid_3")
                .AddColumn(Html.Term("TicketNumber", "Ticket Number"), "TicketNumber", true, true, Constants.SortDirection.Ascending)
                .AddColumn(Html.Term("Status", "Status"), "StatusName")
                .Render(); 
           %>
            <br />
            <p>                
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
        </div>
    </div>
    <div id="addMovementModel_3" class="jqmWindow LModal Overrides" style="width: 750px; height: 600px;">
        <div class="mContent" style="width: 730px; height: 580px;">          
            <% 
                Html.PaginatedGrid("~/Orders/Details/ListOrderStatusShipped/" + ViewData["OrderNumber"], "Grid_2")
                .AddColumn(Html.Term("LogDateUTC", "LogDateUTC"), "LogDateUTC", true, true, Constants.SortDirection.Ascending)
                .AddColumn(Html.Term("Description", "Description"), "Description", true)
                .AddColumn(Html.Term("TrackingNumber", "Tracking Number"), "TrackingNumber", true)
                .AddColumn(Html.Term("StatusTrackingName", "Status"), "Name", true)
                .Render();
             %>
            <br />
            <p>                
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
        </div>
    </div>
</asp:Content>
