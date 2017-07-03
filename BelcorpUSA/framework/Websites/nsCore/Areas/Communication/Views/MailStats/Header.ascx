<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Communication.Models.MailStats.HeaderModel>" %>

<div class="clr newsLetterStats brdr1 brdrAll" style="background-color: White;">
    <div class="SectionHeader">
        <%: Html.Term("NewsletterStatistics", "Newsletter Statistics") %>
    </div>
    <div class="newsletterStats">
        <div id="newsletterStatsContainer">
            <div id="newsletterStatistics" class="NewsletterStatisticsNav">
                <ul class="inlineNav pad5">
                    <li>
                        <span class="UI-lightBg">
                            <span class="label"><%: Html.Term("MailingDate", "Mailing Date") %>:</span>
                            <span class="statValue"><%: Model.MailingDate.HasValue ? Model.MailingDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) : "N/A" %></span>
                            <br class="clr" />
                        </span>
                    </li>
                    <li>
                        <span class="UI-lightBg">
                            <span class="label"><%: Html.Term("EmailsSent", "Emails Sent") %>:</span>
                            <span class="statValue"><%: Model.Totals.SentCount.ToString("N0", CoreContext.CurrentCultureInfo) %></span>
                            <br class="clr" />
                        </span>
                    </li>
                    <li>
                        <span class="UI-lightBg">
                            <span class="label"><%: Html.Term("EmailsBounced", "Emails Bounced") %>:</span>
                            <span class="statValue"><%: Model.Totals.FailedCount.ToString("N0", CoreContext.CurrentCultureInfo) %></span>
                            <br class="clr" />
                        </span>
                    </li>
                    <li>
                        <span class="UI-lightBg">
                            <span class="label"><%: Html.Term("EmailsOpened", "Emails Opened") %>:</span>
                            <span class="statValue"><%: Model.Totals.OpenedCount.ToString("N0", CoreContext.CurrentCultureInfo) %></span>
                            <br class="clr" />
                        </span>
                    </li>
                    <li>
                        <span class="UI-lightBg">
                            <span class="label"><%: Html.Term("UniqueClicks", "Unique Clicks") %>:</span>
                            <span class="statValue"><%: Model.Totals.ClickedCount.ToString("N0", CoreContext.CurrentCultureInfo) %></span>
                            <br class="clr" />
                        </span>
                    </li>
                </ul>
                <span class="clr"></span>
            </div>
            <p>
                <a href="javascript:void(0);" class="Button jqmClose"><%: Html.Term("Close", "Close")%></a>
            </p>
        </div>
    </div>
</div>
