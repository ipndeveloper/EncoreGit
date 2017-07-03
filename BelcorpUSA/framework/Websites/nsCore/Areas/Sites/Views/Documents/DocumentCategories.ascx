<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DocumentCategoriesModel>" %>
<%@ Import Namespace="nsCore.Areas.Sites.Models" %>
<%
	// Model.ParentCategory is the top category.  I will need to call into this method with the two lists, current category and selectedCategories.  
	//The selected categories appear to be the collection of categories that this document is set up to use
	//The recursive partial that will be called needs to be take the child categories AND the full list of selected categories
	
	//Plan is to start at the current category and build/check everything as needed
	//Then loop through the child categories.
	//If there is a new child category, create a new DocumentCategoriesModel assigning the child category to the ParentCategory and then pass down SelectedCategories
%>

<div id="<%=String.Format("categoryContainer{0}", Model.ParentCategory.CategoryID) %>"> 
	<ul>
		<%foreach (Category childCategory in Model.ParentCategory.ChildCategories)
		{
			DocumentCategoriesModel childModel = new DocumentCategoriesModel();
			childModel.ParentCategory = childCategory;
			childModel.SelectedCategories = Model.SelectedCategories;
			%>
			<li class="leaf" style="padding-left:15px;">
				<%= childModel.ParentCategory.Translations.Name() %>
				<span class="AddCat"> <input type="checkbox" id="<%=String.Format("chkCategory{0}", childModel.ParentCategory.CategoryID)%>" class="category" value="<%=childModel.ParentCategory.CategoryID %>"
					<% if(childModel.SelectedCategories.Contains(childModel.ParentCategory.CategoryID)) 
					{%>
						checked="checked" 
					<% }%>	
					/>
				</span>
				<%if (childModel.ParentCategory.ChildCategories != null && childModel.ParentCategory.ChildCategories.Count > 0)
				{%>
					<% Html.RenderPartial("DocumentCategories", childModel);%>
				<%} %>
			</li>
		<%} %>
	</ul>
</div>