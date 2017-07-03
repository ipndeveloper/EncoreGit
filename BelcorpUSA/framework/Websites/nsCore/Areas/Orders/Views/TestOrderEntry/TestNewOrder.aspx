<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
   
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders", "Orders")%></a> >
    <%= Html.Term("StartNewTestOrder", "Start a New Test Order")%>
</asp:Content>
<asp:Content  ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content> 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <script type="text/javascript">
     $(function () {
         $('#ddlPeriod').append('<option value="0" selected="selected"><%= Html.JavascriptTerm("SelectPeriod", "Select Period") %></option>');
         $('#ddlPeriod').change(function () {
             $('#periodId').val($('#ddlPeriod').val());
         });
         $("#btnXML").click(function () {  
            $.ajax({
                type: 'POST',
                url: '/TestOrderEntry/XMLPrueba', 
                asyn: false,
                success: function (data) { 
                }
            });
        });
     });
     $('form').submit(function () {
         if (!$('#ddlPeriod').val())
             return false;
     });
     
 </script>
    <form action="<%= ResolveUrl("~/Orders/TestOrderEntry") %>" method="get">
   
     <%=Html.Term("Periods", "Periods")%>: <span class="LawyerText"/>
     <%=Html.DropDownList("ddlPeriod", (TempData["GetPeriod"] as IEnumerable<SelectListItem>))%>
  
    <%--<input type="text" id="txtCustomerSuggest" style="width: 250px;" />--%>
    <input type="hidden" name="periodId" id="periodId" />
    <a href="javascript:void(0);" onclick="if($('#periodId').val()){
                                                $(this).closest('form').submit();}else{$('#ddlPeriod').showError('Select a Period');}" 
        class="Button BigBlue"> <%= Html.Term("TestOrders", "Test Orders")%></a>
    </form>
   <%-- <input type="button" id="btnXML" name="CliclXML" />--%>
</asp:Content>
