<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Admin.Models.ListTypes.ListTypesViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>"> <%= Html.Term("Admin", "Admin") %></a> > <%= Html.Term("ListTypes", "List Types")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ListTypes", "List Types")%>
		</h2>
	</div>
	<ul>
	    <% foreach (var listType in Model.ListTypes) { %>
            <li><a href="<%= ResolveUrl("~/Admin/ListTypes/Values/") + listType %>">
            <% 
            String mensaje;

            mensaje = listType.ToString().Trim();
            
            switch (mensaje)
            {
                case "AccountStatusChangeReason": mensaje = Html.Term("AccountStatusChangeReason", "Account Status Change Reason");
                    break;
                case "ArchiveType": mensaje = Html.Term("ArchiveType", "Archive Type");
                    break;
                case "CommunicationPreference": mensaje = Html.Term("CommunicationPreference", "Communication Preference");
                    break;
                case "ContactCategory": mensaje = Html.Term("ContactCategory", "Contact Category");
                    break;
                case "ContactMethod": mensaje = Html.Term("ContactMethod", "Contac tMethod");
                    break;
                case "NewsType": mensaje = Html.Term("NewsType", "News Type");
                    break;
                case "ReturnReasons": mensaje = Html.Term("ReturnReasons", "ReturnReasons");
                    break;
                case "ReturnTypes": mensaje = Html.Term("ReturnTypes", "ReturnTypes");
                    break;
                case "SiteStatusChangeReason": mensaje = Html.Term("SiteStatusChangeReason", "Site Status Change Reason");
                    break;
                case "SupportTicketCategory": mensaje = Html.Term("SupportTicketCategory", "Support Ticket Category");
                    break;
                case "SupportTicketPriority": mensaje = Html.Term("SupportTicketPriority", "Support Ticket Priority");
                    break;
                case "ReplacementReason": mensaje = Html.Term("ReplacementReason", "Replacement Reason");
                    break;
                case "ContactType": mensaje = Html.Term("ContactType", "Contact Type");
                    break;
                case "ContactStatus": mensaje = Html.Term("ContactStatus", "Contact Status");
                    break;
                default:
                    mensaje = "No hay nada";
                    break;
            }
                %> <%= mensaje %> </a></li>
        <% } %>
	</ul>
</asp:Content>
