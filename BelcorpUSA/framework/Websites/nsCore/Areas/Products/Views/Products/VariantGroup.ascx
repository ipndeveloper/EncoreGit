<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.ProductPropertyType>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<!-- variant groups -->

            
    <!-- single variant group -->
    
        <div class="pad10 utilities">
            <div class="FL mr10">
            <input type="text" class="pad2 TextInput addVarLines" /> 
            <span class="clr lawyer"><%=Html.Term("example") %>: <i><%=Html.Term("CommaSeperatedColors", "red, blue, green, brown")%></i></span>
            </div>
            <div class="FL">
                <a href="javascript:void(0);" class="DTL Add"><span><%=Html.Term("Add")%></span></a>
            </div>
            <div class="FR">
                <input type="hidden" class="isMaster" value="<%=Model.IsMaster %>" />
                <input type="hidden" class="productPropertyTypeId" value="<%=Model.ProductPropertyTypeID %>" />
                <%=Html.Term("Type") %>: <select class="htmlInputTypeId">
                <option value="0"><%= Html.Term("PleaseSelect", "Please Select") %></option>
                <%foreach(var type in SmallCollectionCache.Instance.HtmlInputTypes.Where(hit=>hit.Active)) { %>
                    <option value="<%=type.HtmlInputTypeID %>" <%=type.HtmlInputTypeID.Equals(Model.HtmlInputTypeID) ? "selected='selected'" : "" %>><%=type.GetTerm() %></option>
                <%}%>
            </select>
            </div>
            <span class="clr"></span>
        </div>
        <ul class="flatList variantAttributes">
        <%foreach (var propertyValue in Model.ProductPropertyValues)
        {
            Html.RenderPartial("VariantGroupAttribute", propertyValue);
        }%>
        </ul>
        <div class="brdrNNYY pad10 variantAttributeSaving">
            <p>
                <input type="checkbox" class="showNameAndThumbnail" id="<%=Model.ProductPropertyTypeID %>" <%= Model.ShowNameAndThumbnail ? "checked=\"checked\"" : "" %> /> <label for="<%=Model.ProductPropertyTypeID %>"><%=Html.Term("DisplayNameAndThumbnail", "Display both name &amp; thumbnail on front-end.")%></label>
            </p>
            <p>
                <a href="javascript:void(0);" class="Button BigBlue Save"><span><%=Html.Term("SaveGroup", "Save Group")%></span></a>
                <a href="javascript:void(0);" class="Button BtnDelete" title="Remove this group for just this product"><span><%=Html.Term("Products_RemoveVariantGroup", "Remove Group")%></span></a>
            </p>
        </div>

    <!--/ end single variant group -->                

<!--/ end single individual group wrapper -->