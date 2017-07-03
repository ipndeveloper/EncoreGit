<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.DynamicKitGroupRule>" %>       
<li>
<% 
	var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
%>
    <span class="FL check"><input type="checkbox" /><input type="hidden" class="ruleId" value="<%=Model.DynamicKitGroupRuleID %>" /></span>
    <span class="product">
        <%= Model.ProductTypeID.HasValue ? 
                SmallCollectionCache.Instance.ProductTypes.GetById(Model.ProductTypeID.Value).GetTerm() : 
                inventory.GetProduct(Model.ProductID.Value).Translations.Name()%> &nbsp;
                
    </span>
    <span>
        <input type="text" class="kitGroupRuleSortOrder numeric qty" name="<%= Model.DynamicKitGroupRuleID %>" value="<%=Model.SortOrder %>"/>
    </span>
    <a class=" IconLink IconOnly Delete block FR deleteRule" href="javascript:void(0);" title="<%= Html.Term("Remove") %>">
        <span><%= Html.Term("Remove") %></span></a>
    <span class="clr"></span>
</li>