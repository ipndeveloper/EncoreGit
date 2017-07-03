<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("Commissions", "Commissions")%></a> > <%= Html.Term("Disbursements") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <script type="text/javascript">
     function ProcessDisbursementsClick() {
         var period = $('#periodID').val();
         if (!period) {
             alert('Please enter a valid Period ID');
             return;
         }
         $.ajax({
             type: "POST",
             url: '<%= ResolveUrl("~/Disbursements/ProcessDisbursements") %>?periodID=' + period,
             data: "{PeriodID:" + period + "}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (msg) {
                 alert(msg.PayoneerDisbursementStatus);
             },
             error: function () {
                 alert('failed to process disbursements');
             }
         });
     }
    </script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Disbursements")%>
		</h2>
        <p class="FormButtons" style="height: 1.818em;">
            Please Enter Period ID: <input type="text" id="periodID" />
            <a href="javascript:ProcessDisbursementsClick()" id="btnSave" class="Button BigBlue" style="position: absolute;">
                <%= Html.Term("ProcessDisbursements", "Process Disbursements")%></a>
        </p>
	</div>
	<ul>
	</ul>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
