<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Campaign>" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        $(function () {
            $('#btnSaveNewsletter').click(function () {
                if ($('#market').val() == 'Select') {
                    showMessage('<%: Html.Term("SelectMarket", "Must Select Market") %>', true);
                }
                else if (!$('#NewsletterTable').checkRequiredFields()) {
                    showMessage('<%: Html.Term("AllFieldsRequired", "All fields are required.") %>', true);
                }
                else {
                    $.ajax({
                        type: 'post',
                        data: { campaignID: $('#campaignID').val(),
                            name: $('#name').val(),
                            active: $('#active').prop('checked'),
                            marketID: $('#market').val()
                        },
                        url: '/Communication/NewsletterCampaigns/Save',
                        success: function (data, textStatus, xmlHttpRequest) {
                            showMessage(data.message, data.result);

                            if (data.success === true) {
                                window.location.href = '/Communication/NewsletterCampaigns';
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            showMessage('<%: Html.Term("Anerroroccuredsavingthenewsletter", "An error occurred saving the newsletter.") %>', false);
                        }
                    });
                }
            });

            $('#newsletterModal').jqm({
                modal: false
            });

            $('.btnEditCampaignAction').live('click', function () {
                var obj=$(this).closest('tr'), campaignActionID = obj.attr('data-id'), type = obj.attr('data-type');
                $.get('<%= ResolveUrl("~/Communication/NewsletterCampaigns/EditCampaignActionModal")%>', {
                	campaignActionID: campaignActionID,
					campaignActionType : type
                },
				function (result) {
				    $('#newsletterModal').html(result);
				    $('#newsletterModal').jqmShow();
				});
            });

            $('.btnViewStats').live('click', function () {
                var campaignActionID = $(this).closest('tr').attr('data-id');
                $.get('<%= ResolveUrl("~/Communication/MailStats/Header")%>', {
                    campaignActionID: campaignActionID
                },
				function (result) {
				    $('#newsletterModal').html(result);
				    $('#newsletterModal').jqmShow();
				});
            });

            $('#addCampaignActionLink').click(function () {
                $.get('<%= ResolveUrl("~/Communication/NewsletterCampaigns/EditCampaignActionModal")%>', {
                    newsletterCampaignID: $('#campaignID').val()
                },
				function (result) {
				    $('#newsletterModal').html(result);
				    $('#newsletterModal').jqmShow();
				});
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%: Model.CampaignID > 0 ? "Update Newsletter Campaign" : "Create a Newsletter Campaign"%></h2>
    </div>
    <div class="FLcolWrapper">
        <div class="FL splitCol40">
            <h3>
                Campaign Info</h3>
            <table id="NewsletterTable" class="FormTable" cellspacing="0" width="100%">
                <tr class="">
                    <td class="FLabel">
                        <%= Html.Term("Market")%>
                    </td>
                    <td>
                        <div style="float: left; padding-right: 10px;">
                            <%= Html.DropDownMarkets(htmlAttributes: new { id = "market" }, selectedMarketID: Model.MarketID, selectTextTermName: "Select")%>
                        </div>
                    </td>
                </tr>
                <tr class="">
                    <td class="FLabel">
                        <%= Html.Term("NewsletterName", "Newsletter Name")%>
                    </td>
                    <td>
                        <input id="name" type="text" class="required fullWidth pad5 newsLetterName" value="<%: Model.Name %>" />
                    </td>
                </tr>
                <tr class="">
                    <td class="FLabel">
                        <label for="active">
                            <%= Html.Term("Active")%></label>
                    </td>
                    <td>
                        <input id="active" type="checkbox" <%: Model.Active ? "checked": "" %> value="<%: Model.Active %>" />
                    </td>
                </tr>
            </table>
            <hr />
            <table class="FormTable" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <p>
                            <a id="btnSaveNewsletter" href="javascript:void(0);" class="Button BigBlue"><span>
                                <%= Html.Term("SaveNewsletterCampaign", "Save Campaign Info")%></span></a> <a id="back"
                                    href="/Communication/NewsletterCampaigns" class="Button"><span><%= Html.Term("Cancel", "Cancel")%></span></a>
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="FR splitCol60">
            <h3>
                Campaign Emails</h3>
            <% var unsupportedEmailTemplateTypes = new List<short>() { (short)Constants.EmailTemplateType.Autoresponder, (short)Constants.EmailTemplateType.Campaign };
               Html.PaginatedGrid("~/Communication/NewsletterCampaigns/GetNewsletterActions")
                   .AddColumn(Html.Term("NewsletterName", "Name Newsletter"), "Name", true, true, Constants.SortDirection.Ascending)
                   .AddColumn(Html.Term("SendDateTime", "Send Date/Time"), "NextRunDateUTC", true)
                   .AddColumn(Html.Term("Active"), "Active", true)
                   .AddColumn("", "", false, false)
                   .AddOption("addCampaignActionLink", Html.Term("AddNewsletter", "Add Newsletter"))
                   .AddData("campaignID", Model.CampaignID)
				   .AddData("campaignActionType", Constants.CampaignActionType.Email)
                   .Render(); %>
            <!--/ End Data Grid -->
        </div>
		<div class="FR splitCol60">&nbsp;</div>
		 <div class="FR splitCol60">
            <h3>
                Campaign Alerts</h3>
            <% 
               Html.PaginatedGrid("~/Communication/NewsletterCampaigns/GetNewsletterActions", id:"grdCampaignAlerts")
				   .AddColumn(Html.Term("AlertCampaignName", "Alert Campaign Name"), "Name", true, true, Constants.SortDirection.Ascending)
                   .AddColumn(Html.Term("SendDateTime", "Send Date/Time"), "NextRunDateUTC", true)
                   .AddColumn(Html.Term("Active"), "Active", true)
				   //.AddColumn("", "", false, false)
				   //.AddOption("addCampaignActionLink", Html.Term("AddNewsletter", "Add Newsletter"))
                   .AddData("campaignID", Model.CampaignID)
				   .AddData("campaignActionType",Constants.CampaignActionType.Alert)
                   .Render(); %>
            <!--/ End Data Grid -->
        </div>
    </div>
    <input type="hidden" id="campaignID" value="<%: Model.CampaignID %>" />
    <div id="newsletterModal" class="jqmWindow LModal">
    </div>
</asp:Content>
