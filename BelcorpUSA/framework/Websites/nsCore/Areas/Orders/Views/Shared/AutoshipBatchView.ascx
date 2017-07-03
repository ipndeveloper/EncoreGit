<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<System.Data.DataRow>>" %>
<table cellspacing="0" class="DataGrid" border="0" id="batch<%= ViewData["AutoshipBatchID"] %>data"
	width="100%">
	<thead>
		<tr class="GridColHead">
			<th>
				<a href="javascript:void(0);" class="sort currentSort Ascending">
					<%= Html.Term("AutoshipLogID", "Log ID") %></a>
			</th>
			<th>
				<a href="javascript:void(0);" class="sort">
					<%= Html.Term("TemplateID", "Template ID") %></a>
			</th>
			<th>
				<a href="javascript:void(0);" class="sort">
					<%= Html.Term("OrderID", "Order ID") %></a>
			</th>
			<th>
				<a href="javascript:void(0);" class="sort">
					<%= Html.Term("Succeeded", "Succeeded") %></a>
			</th>
			<th>
				<a href="javascript:void(0);" class="sort">
					<%= Html.Term("Results", "Results") %></a>
			</th>
			<th>
				<a href="javascript:void(0);" class="sort">
					<%= Html.Term("DateRan", "Date Ran") %></a>
			</th>
		</tr>
	</thead>
	<tbody>
	</tbody>
</table>
<a href="javascript:void(0);" id="batch<%= ViewData["AutoshipBatchID"]  %>data_prev_page">
	&lt;&lt;
	<%= Html.Term("Previous") %></a> <a href="javascript:void(0);" id="batch<%= ViewData["AutoshipBatchID"] %>data_next_page">
		<%= Html.Term("Next") %>
		&gt;&gt;</a>
<%= Html.Term("PageSize", "Page Size")%>:
<select id="batch<%= ViewData["AutoshipBatchID"] %>data_pageSize">
	<option value="10">10</option>
	<option selected="selected" value="20">20</option>
	<option value="50">50</option>
	<option value="100">100</option>
</select>
<script type="text/javascript">
	$(function () {
		window['currentPage<%= ViewData["AutoshipBatchID"] %>'] = 0;
		window['maxPage<%= ViewData["AutoshipBatchID"] %>'];
		getBatchData('<%= ViewData["AutoshipBatchID"] %>');
		$('#batch<%= ViewData["AutoshipBatchID"] %>data .sort').click(function () {
			currentPage = 0;
			if ($(this).attr('class').indexOf('currentSort') > 0) {
				if ($(this).attr('class').indexOf('Ascending') > 0) {
					$(this).removeClass('Ascending').addClass('Descending');
				} else {
					$(this).removeClass('Descending').addClass('Ascending');
				}
			} else {
				$('#batch<%= ViewData["AutoshipBatchID"] %>data .currentSort').attr('class', 'sort');
				$(this).addClass('currentSort Ascending');
			}
			getBatchData('<%= ViewData["AutoshipBatchID"] %>');
		});
		$('#batch<%= ViewData["AutoshipBatchID"] %>data_prev_page').click(function () {
			if (window['currentPage<%= ViewData["AutoshipBatchID"] %>'] > 0) {
				--window['currentPage<%= ViewData["AutoshipBatchID"] %>'];
				getBatchData('<%= ViewData["AutoshipBatchID"] %>');
			}
		});
		$('#batch<%= ViewData["AutoshipBatchID"] %>data_next_page').click(function () {
			if (window['currentPage<%= ViewData["AutoshipBatchID"] %>'] < window['maxPage<%= ViewData["AutoshipBatchID"] %>']) {
				++window['currentPage<%= ViewData["AutoshipBatchID"] %>'];
				getBatchData('<%= ViewData["AutoshipBatchID"] %>');
			}
		});
		$('#batch<%= ViewData["AutoshipBatchID"] %>data_pageSize').change(function () {
			getBatchData('<%= ViewData["AutoshipBatchID"] %>');
		});
	});
</script>
