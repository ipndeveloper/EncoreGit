<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.getDispatchByOrder>>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.SearchData" %>

<%
        List<OrderItem> listnonPromotionalItems = (List<OrderItem>)Session["nonPromotionalItems"];
        var listDispatchProcess = (List<getDispatchByOrder>)Session["loadDispatchProcessOrder"];
        var listG = listDispatchProcess.GroupBy(x => new { x.DispatchID, x.Ddescripcion }).Select(y => new GrupoDispatchByOrder()
          {
              DispatchID = y.Key.DispatchID,
              Ddescripcion = y.Key.Ddescripcion
          }).ToList();
        foreach (var itemG in listG)
        {
            var listaCarga = listDispatchProcess.Where(donde => donde.DispatchID == itemG.DispatchID);
             %>
             <tr class = "UI-lightBg specialPromotionItem">
                <td>
                </td>
                <td>
                </td>
                <td colspan = "5">
                <%=itemG.Ddescripcion%>
                </td>
            </tr>
            <% 
            foreach (var item in listaCarga)
            {
                foreach (var objE in PreOrderExtension.GetProductDispatch(item.OrderItemID))
                {
                    %>
                     <tr>
                        <td>
                        </td>
                        <td data-label="@Html.Term("SKU", "SKU")">
                        <%: Html.Link(objE.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + objE.SKU)%>
                        </td>
                        <td data-label="@Html.Term("Product", "Product")">
                        <%=objE.ProductName%>
                        </td>
                        <td>
                        </td>
                        <td>
                           <%=Html.Term("Cart_FreeItemPrice", "FREE")%>
                        </td>
                        <td colspan = "4">
                        </td>
                    </tr> 
                <% 
                }                
            } 
    }%>
