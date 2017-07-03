<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Commissions.Models.RunnerModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 

    <div class="Content">
        <table width="100%">
            <tr>
                <td style="padding: 10px; width: 40%" id="RunSettings" rowspan="2">
                    <h2>
                        <%= Html.Term("RunnerSettings", "Runner Settings") %>
                    </h2>
                    <table class="DataGrid">
                        <tr>
                            <td>
                               <span id="PlanLabel"><%= Html.Term("Plan", "Plan") %>:</span>
                            </td>
                            <td>
                                <select id="SelectedPlanID">
                                    <% foreach (var plan in Model.CommissionRunPlans) { %>
                                    <option value="<%= plan.PlanID %>" <% if (plan.DefaultPlan) { %> selected="selected" <% } %>>
                                        <%: plan.PlanCode  %>
                                    </option>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <span id="PeriodLabel"><%= Html.Term("Period", "Period") %>:</span>
                            </td>
                            <td>
                                <select id="SelectedPeriodID">
                                    <% foreach (var period in Model.OpenPeriods) { %>
                                    <option value="<%= period.PeriodID %>">
                                        <%= period.EndDate.ToString("yyyy-MM") %>
                                    </option>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <p>   
                                    <a id="btnRun" href="javascript:void(0);" class="Button BigBlue allowDisable">
                                        <span><%= Html.Term("PrepCommissionsRun", "Prep Commissions Run") %></span>                                                                                  
                                    </a>
                                    <a id="btnClose" href="javascript:void(0);" class="Button BigBlue allowDisable">
                                        <span><%= Html.Term("CommissionRunClose", "Close Commission Run") %></span>
                                    </a>                                    
                                </p>                                
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 1px; background: #efefef;" rowspan="2">
                </td>
                <td style="padding: .909em; width: 60%" id="RunStatusColumn">
                    <h2 id="RunStatusHeading">
                        <%= Html.Term("RunStatus", "Run Status") %>
                        <span id="InProgress"><img src="/Content/Images/loading.gif" alt="" /></span>
                    </h2>
                    <p>
                        <span id="PercentCompleteMessage"></span>                        
                    </p>
                    <ul class="RunProgress" id="RunProgress">
                    </ul>
                </td>
            </tr>
        </table>        
    </div>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Scripts/Commissions/Run.js"></script>
    <script type="text/javascript">

         $(document).ready(function() {
            commissionRunner.App.init();            
            commissionRunner.App.systemEventApplicationID = '<%= Model.SystemEventApplicationID %>';            
            <% if(Model.CommissionRunInProgress) { %>                
                commissionRunner.App.disablePageControls();
                commissionRunner.App.setCommissionRunInProgressMessage('<h2><%= Html.Term("CommissionRunInProgress", "There is a commission run currently open and/or in progress.") %></h2>');
            <% } else { %>
                commissionRunner.App.enablePageControls();
            <% } %>
         });

    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("Commissions", "Commissions")%>
    </a> > 
    <%= Html.Term("Runner", "Runner") %>
</asp:Content>
