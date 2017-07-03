<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
		var currentPage = 0, maxPage;
		$(function () {
			getBatches();

			$('.sort').click(function () {
				currentPage = 0;
				if ($(this).attr('class').indexOf('currentSort') > 0) {
					if ($(this).attr('class').indexOf('Ascending') > 0) {
						$(this).removeClass('Ascending').addClass('Descending');
					} else {
						$(this).removeClass('Descending').addClass('Ascending');
					}
				} else {
					$('.currentSort').attr('class', 'sort');
					$(this).addClass('currentSort Ascending');
				}
				getBatches();
			});

			$('#grdBatches_prev_page').click(function () {
				if (currentPage > 0) {
					--currentPage;
					getBatches();
				}
			});

			$('#grdBatches_next_page').click(function () {
				if (currentPage < maxPage) {
					++currentPage;
					getBatches();
				}
			});

			$('#grdBatches_pageSize').change(getBatches);
		});

		function getBatches() {
			$('#grdBatches tbody').empty().append('<tr><td colspan="9"><img src="<%= Page.ResolveUrl("/Content/Images/icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');

			$.getJSON('<%= Page.ResolveUrl("/Orders/Autoships/GetAutoshipBatchLog") %>',
				{
					page: currentPage,
					pageSize: $('#grdBatches_pageSize').val(),
					orderBy: $('.currentSort').text().replace(/\s/g, ''),
					orderByDirection: $('.currentSort').attr('class').split(' ')[2]
				},
				function (results) {
					$('#grdBatches tbody').empty().append(results.data);
					maxPage = results.resultCount == 0 ? 0 : Math.ceil(results.resultCount / $('#grdBatches_pageSize').val()) - 1;
					$('#grdBatches_next_page,#grdBatches_prev_page').removeAttr('disabled').css({ color: '', cursor: '' });
					if (currentPage == maxPage)
						$('#grdBatches_next_page').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
					if (currentPage == 0)
						$('#grdBatches_prev_page').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
				});
		}

		function showBatchDetails(id) {
			var batchRow = $('#row' + id);
			$('#row' + id + 'data').remove();
			if (batchRow.hasClass('displayDetails')) {
			    $('#row' + id + 'img').removeClass('icon-arrowDown-circle').addClass('icon-arrowNext-circle');
			}
			else {
			    $('#row' + id + 'img').removeClass('icon-arrowNext-circle').addClass('icon-arrowDown-circle');
				$.get('<%= ResolveUrl("~/Orders/Autoships/AutoshipBatchView") %>', { autoshipBatchId: id }, function (results) {
					batchRow.after('<tr id="row' + id + 'data"><td></td><td colspan="7">' + results + '</td></tr>');
				});
			}
			batchRow.toggleClass('displayDetails');
		}

		function highlightRow(id) {
			$('.GridRowHover').removeClass('GridRowHover');
			$('#row' + id).addClass('GridRowHover');
		}

		function getBatchData(id) {
			$('#batch' + id + 'data tbody').empty().append('<tr><td colspan="9"><img src="<%= Page.ResolveUrl("/Content/Images/icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
			$.getJSON('<%= Page.ResolveUrl("/Orders/Autoships/GetAutoshipLog") %>',
				{
					page: window['currentPage' + id],
					pageSize: $('#batch' + id + 'data_pageSize').val(),
					orderBy: $('#batch' + id + 'data .currentSort').text().replace(/\s/g, ''),
					orderByDirection: $('#batch' + id + 'data .currentSort').attr('class').split(' ')[2],
					autoshipBatchID: id
				},
				function (results) {
					$('#batch' + id + 'data tbody').empty().append(results.data);
					window['maxPage' + id] = results.resultCount == 0 ? 0 : Math.ceil(results.resultCount / $('#batch' + id + 'data_pageSize').val()) - 1;
					$('#batch' + id + 'data_next_page,#batch' + id + 'data_prev_page').removeAttr('disabled').css({ color: '', cursor: '' });
					if (window['currentPage' + id] == window['maxPage' + id])
						$('#batch' + id + 'data_next_page').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
					if (window['currentPage' + id] == 0)
						$('#batch' + id + 'data_prev_page').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
				});
		}
	</script>

</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Orders") %>"><%= Html.Term("Orders") %></a> > 
	<%= Html.Term("AutoshipRunOverview", "Autoship Run Overview") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("AutoshipRunOverview", "Autoship Run Overview") %></h2>
	</div>
	<a target="_blank"></a>
	<table cellspacing="0" class="DataGrid" border="0" id="grdBatches" width="100%">
		<thead>
			<tr class="GridColHead">
				<th></th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("AutoshipBatchID", "ID") %></a> </th>
				<th><a href="javascript:void(0);" class="sort currentSort Descending">
					<%= Html.Term("StartDate", "Start Date") %></a> </th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("EndDate", "End Date") %></a> </th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("Succeeded", "Succeeded") %></a> </th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("Failure", "Failure") %></a> </th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("UserName", "User Name") %></a> </th>
				<th><a href="javascript:void(0);" class="sort">
					<%= Html.Term("Notes", "Notes") %></a> </th>
			</tr>
		</thead>
		<tbody>
		</tbody>
	</table>
	<p class="Pagination">
		<a href="javascript:void(0);" id="grdBatches_prev_page">&lt;&lt;
			<%= Html.Term("Previous") %></a> <a href="javascript:void(0);" id="grdBatches_next_page">
				<%= Html.Term("Next") %>
				&gt;&gt;</a>
		<%= Html.Term("PageSize", "Page Size") %>:
		<select id="grdBatches_pageSize">
			<option value="10">10</option>
			<option selected="selected" value="20">20</option>
			<option value="50">50</option>
			<option value="100">100</option>
		</select>
	</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
