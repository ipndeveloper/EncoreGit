<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.Overview.IndexModel>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts") %></a> > <%= CoreContext.CurrentAccount.FullName %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Overview", "Overview")%>
        </h2>
		<%= Html.Term("Overview", "Overview")%>
		| <a href="<%= ResolveUrl("~/Orders/OrderEntry/NewOrder?accountId=") + CoreContext.CurrentAccount.AccountID %>"><%= Html.Term("PlaceNewOrder", "Place New Order") %></a>
        | <a href="<%= ResolveUrl("~/Accounts/Overview/PoliciesChangeHistory") %>"><%= Html.Term("PoliciesChangeHistory", "Policies Change History") %></a>
        | <a href="<%= ResolveUrl("~/Accounts/Overview/AuditHistory") %>"><%= Html.Term("AuditHistory", "Audit History") %></a>
	</div>
	<% if (TempData["Error"] != null) { %>
	    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
		    -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
		    border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
		    margin-bottom: 10px; padding: 7px;">
		    <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
			    <%= TempData["Error"] %>
            </div>
	    </div>
	<% } %>
	<table width="100%" cellspacing="0">
		<tr>
			<td class="splitCol50 brdr-right pad5 ">
				<% Html.RenderPartial("OrderHistory"); %>
			</td>
			<td class="splitCol pad5">
                <% if (Model.ProxyLinks != null && Model.ProxyLinks.Any()) { %>
				    <div class="mb10">
					    <h3>
						    <%= Html.Term("AccountProxyLinks", "Account Proxy Links")%></h3>
					    <div class="DistributorProxies">
                            <% if (Model.IsUserActive) { %>
						        <ul class="flatList">
							        <%: Html.DisplayFor(m => m.ProxyLinks)%>
                                    <%--<% 
                                   var index = 0;
                                    foreach (var item in Model.ProxyLinks)
                                    {
                                        if (index == 1)
                                        {
                                            %>
                                           <li><a href="<%= item.Url.ToString().Replace(".staging.encore.belcorp.us", "") %>" target="_blank"><%= item.LocalizedName %></a><br /></li>
                                            <%
                                        }
                                        else { 
                                            %>
                                            <li><a href="<%= item.Url.ToString() %>" target="_blank"><%= item.LocalizedName %></a></li>
                                            <%
                                        }
                                        index += 1; 
                                    } 
                                    %>--%>
						        </ul>
                            <% } else { %>
                                <%: Html.Term("InActiveUserStatus", "User status is not active.") %>
                            <% } %>
					    </div>
				    </div>
                <% } %>
                <% if (Model.AutoshipScheduleOverviews != null && Model.AutoshipScheduleOverviews.Any()) { %>
				    <div class="mb10 autoships">
					    <h3>
						    <%= Html.Term("Autoships") %></h3>
					    <div>
                            <ul class="flatList">
                                <%: Html.DisplayFor(m => m.AutoshipScheduleOverviews)%>
                            </ul>
					    </div>
				    </div>
                <% } %>
                <%if (Model.SubscriptionScheduleOverviews != null && Model.SubscriptionScheduleOverviews.Any()) { %>
				    <div class="mb10 subscriptions">
					    <h3>
						    <%= Html.Term("Subscriptions") %></h3>
					    <div class="flatList">
                            <%: Html.DisplayFor(m => m.SubscriptionScheduleOverviews)%>
					    </div>
				    </div>
                <% } %>
			</td>
		</tr>
	</table>
</asp:Content>
