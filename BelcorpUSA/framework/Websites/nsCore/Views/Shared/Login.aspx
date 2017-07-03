<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="NetSteps.Web.Mvc.Controls.Analytics" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%: Html.Partial("ServerInfo") %>
    <meta name="googlebot" content="noarchive" />
    <meta name="robots" content="noindex,nofollow" />
    <title>ENCORE - Global Management Portal - Powered by NetSteps</title>
    <link rel="icon" type="image/x-icon" href='@Url.Content("~/Content/Images/favicon.ico")' />
    
	<%: Styles.Render("~/Content/stylebundles/login")%>
	<%: Scripts.Render("~/scriptbundles/jquery")%>
    <%: Scripts.Render("~/scriptbundles/jqueryval")%>
    <%: Scripts.Render("~/scriptbundles/knockout")%>
    <%: Scripts.Render("~/scriptbundles/site")%>    
	<script type="text/javascript">
        $(function () {
            // setup async exception handling
            $(document).ajaxComplete(function (e, request, settings) {
                var isErrorPage = /<input *type *= *"hidden" *value *= *"ErrorPage" *\/>/i.test(request.responseText);
                if (isErrorPage) {
                    window.location = '/Error/';
                }
            });

            $('#LoginForm').setupRequiredFields();
            $('#username').watermark('username');
            $('#password').watermark('password');
            $('input').keyup(function (e) {
                if (e.keyCode == 13)
                    $('#btnLogin').click();
            });
            $('#btnLogin').click(function () {
                if (!$('#LoginForm').checkRequiredFields()) {
                    return false;
                }

                $('form').submit();
                var t = $(this);
                showLoading(t);
            });

            if (/^true$/i.test('<%= ViewData["InvalidLogin"] %>')) {
                $('#password').focus();
            } else {
                $('#username').focus();
            }

            if ($.browser.msie) {
                $('body').addClass('IE' + parseInt($.browser.version));
            }
        });


        function showLoading(element, css) {
            if (!element.jquery)
                element = $(element);
            var loading = $('<img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="<%= Html.Term("Loading") %>" />');
            element.after(loading).hide();
        }

        function hideLoading(element) {
            if (!element.jquery)
                element = $(element);
            element.show().next().remove();
        }
    </script>
    <%= StackExchange.Profiling.MiniProfiler.RenderIncludes()%>
</head>
<body>
    <div id="Container">
        <div id="Login">
            <div id="LoginForm">
                <h1><%=Html.Term("GlobalManagement-LoginTitle","Global Management Portal") %></h1>
                <div class="Fields">
                    <form action="<%= ResolveUrl("~/Login") %>" method="post">
                    <p>
                        <input id="username" name="<%= Html.Term("UsernameIsRequired", "Username is required.")%>" type="text" value="<%= Request.IsLocal ? NetSteps.Data.Entities.ApplicationContext.Instance.DevelopmentCorpHelperLogin.Username : "" %>" class="TextInput required" title="<%=Html.Term("Username")%>" />
                    </p>
                    <p>
                        <input id="password" name="<%= Html.Term("PasswordIsRequired", "Password is required.")%>" type="password" class="TextInput required" value="<%= Request.IsLocal ? NetSteps.Data.Entities.ApplicationContext.Instance.DevelopmentCorpHelperLogin.Password : "" %>" title="<%=Html.Term("Password")%>" />
                    </p>
                    </form>
                    
                    <img id="loading" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." style="display: none;" />
              
                </div>
                <a id="btnLogin" class="LoginButton" href="javascript:void(0);"><span>
                    <%= Html.Term("ManagementPortalLoginButton", "Sign In")%></span></a>
                <span class="clr"></span>
            </div>
            
            <%string message = "";
              if (ViewData["InvalidLogin"] != null && (bool)ViewData["InvalidLogin"])
              {
                  if (NetSteps.Web.WebContext.IsLocalHost && ViewData["ErrorMessage"] != null)
                  {
                      message = ViewData["ErrorMessage"].ToString();
                  }
                  else
                  {
                      message = Html.Term("InvalidLogin", "Invalid login.  Please try again.");
                  }
              }
              if (ViewData["SessionExpired"] != null && (bool)ViewData["SessionExpired"])
              {
                  message = Html.Term("YourSessionExpired", "Your session expired. Please login again.");
              }
              if (TempData["NoMarkets"] != null && (bool)TempData["NoMarkets"])
              {
                  message = Html.Term("YouDoNotHaveSufficientRightsToAccessThisSite", "You do not have sufficient rights to access this site.");
              }
              if (!string.IsNullOrEmpty(message))
              { %>
            <div class="error_message_block">
                <div class="error_message">
                    <span class="UI-icon icon-exclamation"></span>
                    <%= message %></div>
            </div>
            <%} %>
        </div>       
    </div>
    <%this.Html.RenderPartial("_GoogleAnalytics", new AnalyticsModel(new HttpRequestWrapper(this.Request))); %>
</body>
</html>
