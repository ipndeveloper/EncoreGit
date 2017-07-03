<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Promotions/Promotions.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.Promotions.CartRewardsPromotionModel>" %>

<%@ Import Namespace="nsCore.Areas.Products.Models.Promotions.Interfaces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <% Html.RenderPartial("_EditScript"); %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Promotions") %>">
            <%= Html.Term("Promotions") %></a> > <a href="<%= ResolveUrl("~/Products/Promotions") %>">
                <%= Html.Term("ProductPromotion", "Product Promotion")%></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Promotions_CreateCartRewardsPromotionHeading", "Create a Cart Rewards Promotion") %></h2>
    </div>
    <div id="NameType" class="mb10">
        <table width="100%" class="FormTable">
            <tbody>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("Promotions_PromotionNameLabel", "Promotion Name") %>:
                    </td>
                    <td>
                        <input type="hidden" value="<%= Model.PromotionID %>" id="promotionId" />
                        <input type="text" value="<%= Model.Name %>" name='<%= Html.Term("NameIsRequired", "Name is required") %>'
                            class="required pad5 fullWidth" id="txtName" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <% Html.RenderPartial("PromotionsPlugins/_OptionSingleMarket"); %>
    <div id="PromotionForm" class="splitCol mb10">
        <h3 class="UI-lightBg pad10">
            <%= Html.Term("Promotion_OptionsHeading", "Options:")%>
            <a style="" id="reviewOptions" class="FR viewHideOptions" href="#">
                <%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
            <span class="clr"></span>
        </h3>
        <div id="PromoOptions">
            <% Html.RenderPartial("PromotionsPlugins/_OptionCouponCode"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionOneTime"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionContinuity"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionActiveImmediately"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictAccounts"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictNewBAs"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictActivityStatuses"); %>
            <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictOrderTypes"); %>
            <% Html.RenderPartial("PromotionsPlugins/_AccountIDLoad"); %>
            <div class="UI-lightBg pad2 bold overflow">
                <a style="" id="reviewOptionsBottom" class="FR mr10 viewHideOptions" href="#">
                    <%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
            </div>
        </div>
    </div>
    <% Html.RenderPartial("PromotionsPlugins/_CartRewardsPanel"); %>
    <div class="mt10" id="SaveRewards">
        <a class="Button BigBlue" id="btnSave" href="javascript:void(0);">
            <%= Html.Term("Promotions_SavePromotion", "Save Promotion")%></a>
    </div>
    <% Html.RenderPartial("MessageCenter"); %>
</asp:Content>
